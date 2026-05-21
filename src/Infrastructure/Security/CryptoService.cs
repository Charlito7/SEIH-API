using System.Security.Cryptography;
using System.Text;
using Core.Application.Interface.Security;

namespace Infrastructure.Security
{
    public class CryptoService : ICryptoService
    {
        public (byte[] encryptedPayload, byte[] encryptedSessionKey, byte[] iv)
            EncryptData(string data, string publicKey)
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor();

            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedPayload;

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
                encryptedPayload = ms.ToArray();
            }

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);

            var encryptedSessionKey =
                rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

            return (encryptedPayload, encryptedSessionKey, aes.IV);
        }

        public string DecryptData(
            byte[] encryptedPayload,
            byte[] encryptedSessionKey,
            byte[] iv,
            string privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);

            var aesKey =
                rsa.Decrypt(encryptedSessionKey, RSAEncryptionPadding.OaepSHA256);

            using var aes = Aes.Create();
            aes.Key = aesKey;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();

            using var ms = new MemoryStream(encryptedPayload);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
