using Newtonsoft.Json;
using Project.Core.SaveSystem.DataClasses;
using UnityEngine.Assertions;

namespace Project.Core.SaveSystem.Serializers
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize(SaveData saveData)
        {
            Assert.IsFalse(saveData.Equals(default(SaveData)));
            string serializedData = JsonConvert.SerializeObject(saveData, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Assert.IsFalse(string.IsNullOrEmpty(serializedData));

            return serializedData;
        }

        public SaveData Deserialize(string serializedData)
        {
            Assert.IsFalse(string.IsNullOrEmpty(serializedData));
            SaveData dataObject = JsonConvert.DeserializeObject<SaveData>(serializedData, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
            Assert.IsFalse(dataObject.Equals(default(SaveData)));

            return dataObject;
        }
    }
}
    
