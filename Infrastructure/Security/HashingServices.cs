using Core.Application.Interface;
using Core.Domain.Commons.ValueObjects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class HashingServices : IHashingServices
{
    public HashSalt HashText(string plainText)
    {
        try
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(

                password: plainText,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return new HashSalt { Hash = hashedPassword, Salt = salt };
        }
        catch { throw; }
    }

    public bool VerifyText(string enteredText, byte[] salt, string storedHashPassword)
    {
        try
        {
            string hashedUserInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(

                password: enteredText,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return hashedUserInput == storedHashPassword;
        }
        catch { throw; }
    }
}
