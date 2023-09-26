using OdinSerializer;
using Project.Core.SaveSystem.DataClasses;
using System;
using UnityEngine.Assertions;

namespace Project.Core.SaveSystem.Serializers
{
    public class BinarySerializer : ISerializer
    {
        public string Serialize(SaveData dataObject)
        {
            Assert.IsFalse(dataObject.Equals(default(SaveData)));
            byte[] serializedDataBytes = SerializationUtility.SerializeValue(dataObject, DataFormat.Binary);
            string serializedData = Convert.ToBase64String(serializedDataBytes);
            Assert.IsFalse(string.IsNullOrEmpty(serializedData));
            
            return serializedData;
        }

        public SaveData Deserialize(string serializedData)
        {
            Assert.IsFalse(string.IsNullOrEmpty(serializedData));
            byte[] serializedDataBytes = Convert.FromBase64String(serializedData);
            SaveData dataObject = SerializationUtility.DeserializeValue<SaveData>(serializedDataBytes, DataFormat.Binary);
            Assert.IsFalse(dataObject.Equals(default(SaveData)));

            return dataObject;
        }
    }
}
    
