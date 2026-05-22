namespace Core.Application.Interface.Security
{
    public interface ICryptoService
    {
        (byte[] encryptedPayload, byte[] encryptedSessionKey, byte[] iv)
            EncryptData(string data, string publicKey);

        string DecryptData(
            byte[] encryptedPayload,
            byte[] encryptedSessionKey,
            byte[] iv,
            string privateKey);
    }
}
