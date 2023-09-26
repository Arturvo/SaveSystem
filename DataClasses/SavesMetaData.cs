using System.Collections.Generic;

namespace Project.Core.SaveSystem.DataClasses
{
    public class SavesMetaData : ISavableData
    {
        public Dictionary<int, SaveSlotMetaData> saveSlotsMetaData;

		public SavesMetaData()
		{
			saveSlotsMetaData = new Dictionary<int, SaveSlotMetaData>();
		}
	}
}