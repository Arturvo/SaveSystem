using UnityEngine.Assertions;

namespace Project.Core.SaveSystem.Encryptors
{
    public class XOREncryptor : IEncryptor
    {
        private readonly string encryptionKey;

        public XOREncryptor(string encryptionKey)
        {
            Assert.IsFalse(string.IsNullOrEmpty(encryptionKey));
            this.encryptionKey = encryptionKey;
        }

        public string Encrypt(string data)
        {
            return EncryptDecrypt(data);
        }

        public string Decrypt(string data)
        {
            return EncryptDecrypt(data);
        }

        private string EncryptDecrypt(string data)
        {
            string encryptedData = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                encryptedData += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
            }
            return encryptedData;
        }
    }
}