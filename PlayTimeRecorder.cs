using Project.Core.SaveSystem.DataClasses;
using Project.Core.SaveSystem.SaveManagers;
using UnityEngine;
using System.Diagnostics;

namespace Project.Core.SaveSystem
{
    public class PlayTimeRecorder : MonoBehaviour, ISaveSystemUser<SavesMetaData>
    {
        private Stopwatch stopwatch;
        private SaveMetaDataManager saveMetaDataManager;
        private int currentSaveSlot;
        private bool isNewGame;
        private SavesMetaData savesMetaData;

        private void Awake()
        {
            stopwatch = new Stopwatch();
        }
        
        private void Start()
        {
            saveMetaDataManager = (SaveMetaDataManager)SaveMetaDataManager.Instance;
            saveMetaDataManager.SubscribeUser(this);
        }

        private void OnDestroy()
        {
            if(saveMetaDataManager)
            {
                saveMetaDataManager.UnsubscribeUser(this);
            }
        }

        public void ChangeSaveSlot(int currentSaveSlot, bool isNewGame)
        {
            this.currentSaveSlot = currentSaveSlot;
            this.isNewGame = isNewGame;
            stopwatch.Restart();
        }

        public void PauseRecording()
        {
            stopwatch.Stop();
        }

        public void UnpauseRecording()
        {
            stopwatch.Start();
        }

        public void LoadData(SavesMetaData savesMetaData)
        {
            savesMetaData ??= new SavesMetaData();
            this.savesMetaData = savesMetaData;
        }

        public SavesMetaData SaveData()
        {
            SaveSlotMetaData saveSlotMetaData = GetSaveSlotMetaData();
            long elapsedTime = Mathf.RoundToInt((float)stopwatch.Elapsed.TotalSeconds);
            if (isNewGame)
            {
                saveSlotMetaData.totalPlaytime = elapsedTime;
                isNewGame = false;
            }
            else
            {
                saveSlotMetaData.totalPlaytime += elapsedTime;
            }
            stopwatch.Restart();
            return savesMetaData;
        }

        private SaveSlotMetaData GetSaveSlotMetaData()
        {
            savesMetaData.saveSlotsMetaData.TryGetValue(currentSaveSlot, out SaveSlotMetaData saveSlotMetaData);
            if (saveSlotMetaData == null)
            {
                saveSlotMetaData = new SaveSlotMetaData();
                savesMetaData.saveSlotsMetaData.Add(currentSaveSlot, saveSlotMetaData);
            }
            return saveSlotMetaData;
        }
	}
}