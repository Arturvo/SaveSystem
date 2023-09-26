using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Core.SaveSystem.FileHandlers
{
    public class DefaultFileHandler : IFileHandler
    {
        public bool SaveFileExists(string saveDirectoryPath, string saveName)
        {
            return File.Exists(GetFullPath(saveDirectoryPath, saveName));
        }

        public void SaveData(string dataToSave, string saveDirectoryPath, string saveName)
        {
            Assert.IsFalse(string.IsNullOrEmpty(dataToSave));
            string savePath = GetFullPath(saveDirectoryPath, saveName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToSave);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogError("Error occured when trying to save data to file: " + savePath + "\n" + exception);
            }
        }

        public string LoadData(string saveDirectoryPath, string saveName)
        {
            Assert.IsTrue(SaveFileExists(saveDirectoryPath, saveName));
            string loadedData = string.Empty;
            string savePath = GetFullPath(saveDirectoryPath, saveName);

            try
            {
                using (FileStream stream = new FileStream(savePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        loadedData = reader.ReadToEnd();
                        Assert.IsFalse(string.IsNullOrEmpty(loadedData));
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogError("Error occured when trying to load data from file: " + savePath + "\n" + exception);
            }

            return loadedData;
        }

        private string GetFullPath(string directoryPath, string fileName)
        {
            Assert.IsFalse(string.IsNullOrEmpty(directoryPath));
            Assert.IsFalse(string.IsNullOrEmpty(fileName));
            return Path.Combine(directoryPath, fileName);
        }
    }
}
