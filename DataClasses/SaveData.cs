using System;
using System.Collections.Generic;

namespace Project.Core.SaveSystem.DataClasses
{
	public class SaveData
	{
		public Dictionary<Type, ISavableData> savableData;

		public T GetData<T>()
		{
			savableData ??= new Dictionary<Type, ISavableData>();
			savableData.TryGetValue(typeof(T), out var data);
			return (T)data;
		}

		public void SetData(ISavableData data)
		{
			bool dataAddedToDictionary = savableData.TryAdd(data.GetType(), data);
			if (!dataAddedToDictionary)
			{
				savableData[data.GetType()] = data;
            }
		}
	}
}