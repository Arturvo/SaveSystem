namespace Project.Core.SaveSystem.Encryptors
{
    public interface IEncryptor
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}