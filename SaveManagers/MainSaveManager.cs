namespace Project.Core.SaveSystem.SaveManagers
{
    public class MainSaveManager : BaseSaveManager<MainSaveManager>
    {
        private SaveMetaDataManager saveMetaDataManager;

        private void Start()
        {
            saveMetaDataManager = (SaveMetaDataManager)SaveMetaDataManager.Instance;
        }

        public override void NewSave(int slot)
        {
            base.NewSave(slot);
            saveMetaDataManager.NewSave(slot);
        }

        public override void SaveData(int slot)
        {
            base.SaveData(slot);
            saveMetaDataManager.SaveData(slot);
        }

        public override void LoadData(int slot)
        {
            base.LoadData(slot);
            saveMetaDataManager.LoadData(slot);
        }
    }
}