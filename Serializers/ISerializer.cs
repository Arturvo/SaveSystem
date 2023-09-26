using Project.Core.SaveSystem.DataClasses;

namespace Project.Core.SaveSystem.Serializers
{
    public interface ISerializer
    {
        string Serialize(SaveData saveData);
        SaveData Deserialize(string serializedData);
    }
}