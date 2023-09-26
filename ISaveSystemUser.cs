using Project.Core.SaveSystem.DataClasses;

namespace Project.Core.SaveSystem
{
    /// <summary>
    /// SaveSystemUser interface used by classes willing to save data
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface ISaveSystemUser<T> : ISaveSystemUser where T : ISavableData
	{
        /// <summary>
        /// Load function used internally by the interface. Should not be used directly by save system users.
        /// </summary>
        void ISaveSystemUser.LoadData(SaveData saveData)
		{
			LoadData(saveData.GetData<T>());
		}
        /// <summary>
        /// Loads data from save system. Data is null if it was not yet initialized.
        /// </summary>
        void LoadData(T saveData);

        /// <summary>
        /// Save function used internally by the interface. Should not be used directly by save system users.
        /// </summary>
        ISavableData ISaveSystemUser.SaveData(SaveData saveData)
		{
			var data = SaveData();
			if (data != null) // data is null when ISaveSystemUser doesn't want to save data
            {
				saveData.SetData(data);
			}
			return data;
		}
        /// <summary>
        /// Caches data into the save system to be saved later. Returning null means data won't be modified.
        /// </summary>
        T SaveData();
	}

    /// <summary>
    /// SaveSystemUser interface used by save managers
    /// </summary>
    public interface ISaveSystemUser
	{
        void LoadData(SaveData saveData);
        ISavableData SaveData(SaveData saveData);
    }
}