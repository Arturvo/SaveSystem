namespace Project.Core.SaveSystem.FileHandlers
{
    public interface IFileHandler
    {
        bool SaveFileExists(string saveDirectoryPath, string saveName);
        void SaveData(string dataToSave, string saveDirectoryPath, string saveName);
        string LoadData(string saveDirectoryPath, string saveName);
    }
}
