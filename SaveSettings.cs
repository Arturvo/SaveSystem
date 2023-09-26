using Project.Core.SaveSystem.Enums;
using UnityEngine;

namespace Project.Core.SaveSystem
{
    [CreateAssetMenu]
    public class SaveSettings : ScriptableObject
    {
        [field: SerializeField]
        public SaveFileFormat SaveFileFormat { get; private set; }
        [field: SerializeField]
        public EncryptionMethod EncryptionMethod { get; private set; }
        [field: SerializeField]
        public SaveDirectory SaveDirectory { get; private set; }
    }
}

