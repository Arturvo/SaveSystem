using Project.Common.Patterns;
using Project.Core.SaveSystem.Encryptors;
using Project.Core.SaveSystem.Enums;
using Project.Core.SaveSystem.FileHandlers;
using Project.Core.SaveSystem.DataClasses;
using Project.Core.SaveSystem.Serializers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Core.SaveSystem.SaveManagers
{
    public abstract class BaseSaveManager<T> : PersistentSingleton<BaseSaveManager<T>>
    {
        private const string encryptionKey = "nPfdRJX1E7TkJSIgKVRw";
        private const string saveFileNameEmptyMessage = "Save file name can't be empty";
        private const string forgotToUnsubscribeMessage = "Forgot to unsubscribe ISaveSystemUser";
        private const string platformNotSupportedMessage = "Save system doesn't support this platform";

        [SerializeField]
        private SaveSettings saveSettings;
        [SerializeField]
        private string saveFileName;

        private readonly Dictionary<RuntimePlatform, FileHandler> platformToFileHandlerMapping = new()
        {
            { RuntimePlatform.WindowsPlayer, FileHandler.DefaultFileHandler },
            { RuntimePlatform.WindowsEditor, FileHandler.DefaultFileHandler },
        };
        private SaveData saveData;
        private List<ISaveSystemUser> saveSystemUsers;
        private Dictionary<SaveFileFormat, ISerializer> serializers;
        private Dictionary<EncryptionMethod, IEncryptor> encryptors;
        private Dictionary<FileHandler, IFileHandler> fileHandlers;
        private Dictionary<SaveDirectory, string> saveDirectories;
        private IFileHandler fileHandler;

        protected override void Awake()
        {
            base.Awake();
            InitializeVariables();
        }

        public virtual void NewSave(int slot)
        {
            saveData = new SaveData();
            string saveName = GetSaveName(slot);
            SaveGameDataToFile(saveDirectories[saveSettings.SaveDirectory], saveName);
            LoadDataToSaveSystemUsers();
        }

        public virtual void SaveData(int slot)
        {
            string saveName = GetSaveName(slot);
            SaveDataFromSaveSystemUsers();
            SaveGameDataToFile(saveDirectories[saveSettings.SaveDirectory], saveName);
        }

        public virtual void LoadData(int slot)
        {
            string saveName = GetSaveName(slot);
            LoadGameDataFromFile(saveDirectories[saveSettings.SaveDirectory], saveName);
            LoadDataToSaveSystemUsers();
        }

        public virtual bool SaveFileExists(int slot)
        {
            string saveName = GetSaveName(slot);
            return fileHandler.SaveFileExists(saveDirectories[saveSettings.SaveDirectory], saveName);
        }

        /// <summary>
        /// SaveSystemUsers must subscribe in Start method and unsubscribe in OnDestroy method
        /// </summary>
        public void SubscribeUser(ISaveSystemUser saveSystemUser)
        {
            Assert.IsNotNull(saveSystemUser);
            saveSystemUsers.Add(saveSystemUser);
            LoadDataToSaveSystemUser(saveSystemUser);
        }

        /// <summary>
        /// SaveSystemUsers must subscribe in Start method and unsubscribe in OnDestroy method
        /// </summary>
        public void UnsubscribeUser(ISaveSystemUser saveSystemUser)
        {
            Assert.IsNotNull(saveSystemUser);
            Assert.IsTrue(saveSystemUsers.Contains(saveSystemUser));
            saveSystemUsers.Remove(saveSystemUser);
        }

        public void LoadDataToSaveSystemUsers()
        {
            foreach (ISaveSystemUser saveSystemUser in saveSystemUsers)
            {
                Assert.IsNotNull(saveSystemUser, forgotToUnsubscribeMessage);
                LoadDataToSaveSystemUser(saveSystemUser);
            }
        }

        public void SaveDataFromSaveSystemUsers()
        {
            foreach (ISaveSystemUser saveSystemUser in saveSystemUsers)
            {
                Assert.IsNotNull(saveSystemUser, forgotToUnsubscribeMessage);
                SaveDataFromSaveSystemUser(saveSystemUser);
            }
        }

        private void LoadDataToSaveSystemUser(ISaveSystemUser saveSystemUser)
        {
            saveSystemUser.LoadData(saveData);
        }

        private void SaveDataFromSaveSystemUser(ISaveSystemUser saveSystemUser)
        {
            saveSystemUser.SaveData(saveData);
        }

        private void InitializeVariables()
        {
            Assert.IsFalse(string.IsNullOrEmpty(saveFileName), saveFileNameEmptyMessage);

            saveData = new SaveData();
            saveSystemUsers = new List<ISaveSystemUser>();

            serializers = new Dictionary<SaveFileFormat, ISerializer>()
            {
                { SaveFileFormat.JSON, new JSONSerializer() },
                { SaveFileFormat.Binary, new BinarySerializer() }
            };
            encryptors = new Dictionary<EncryptionMethod, IEncryptor>()
            {
                {EncryptionMethod.XOR, new XOREncryptor(encryptionKey) }
            };
            fileHandlers = new Dictionary<FileHandler, IFileHandler>()
            {
                { FileHandler.DefaultFileHandler, new DefaultFileHandler()}
            };
            saveDirectories = new Dictionary<SaveDirectory, string>
            {
                { SaveDirectory.DataPath, Application.dataPath },
                { SaveDirectory.PersistentDataPath, Application.persistentDataPath }
            };

            bool platformSupported = platformToFileHandlerMapping.TryGetValue(Application.platform, out FileHandler fileHandlerType);
            Assert.IsTrue(platformSupported, platformNotSupportedMessage);
            fileHandler = fileHandlers[fileHandlerType];
        }

        private string GetSaveName(int slot)
        {
            Assert.IsFalse(string.IsNullOrEmpty(saveFileName), saveFileNameEmptyMessage);
            string saveName = saveFileName;
            if (slot >= 0) saveName += slot;
            return saveName;
        }

        private void SaveGameDataToFile(string saveDirectory, string saveName)
        {
            string serializedData = serializers[saveSettings.SaveFileFormat].Serialize(saveData);
            if (saveSettings.EncryptionMethod != EncryptionMethod.None)
            {
                serializedData = encryptors[saveSettings.EncryptionMethod].Encrypt(serializedData);
            }
            fileHandler.SaveData(serializedData, saveDirectory, saveName);
        }

        private void LoadGameDataFromFile(string saveDirectory, string saveName)
        {
            string serializedData = fileHandler.LoadData(saveDirectory, saveName);
            if (saveSettings.EncryptionMethod != EncryptionMethod.None)
            {
                serializedData = encryptors[saveSettings.EncryptionMethod].Decrypt(serializedData);
            }
            saveData = serializers[saveSettings.SaveFileFormat].Deserialize(serializedData);
        }
    }
}