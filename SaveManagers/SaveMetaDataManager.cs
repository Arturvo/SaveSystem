using Project.Core.SaveSystem.DataClasses;
using UnityEngine;

namespace Project.Core.SaveSystem.SaveManagers
{
    [RequireComponent(typeof(PlayTimeRecorder))]
    public class SaveMetaDataManager : BaseSaveManager<SaveMetaDataManager>
    {
        private PlayTimeRecorder playTimeRecorder;

        private void Start()
        {
            playTimeRecorder = GetComponent<PlayTimeRecorder>();
        }

        public override void NewSave(int slot)
        {
            playTimeRecorder.ChangeSaveSlot(slot, true);
            SaveData();
        }

        public override void SaveData(int slot)
        {
            base.SaveData();
            LoadDataToSaveSystemUsers();
        }

        public override void LoadData(int slot)
        {
            if (slot >= 0)
            {
                playTimeRecorder.ChangeSaveSlot(slot, false);
            }
            base.LoadData();
        }
    }
}