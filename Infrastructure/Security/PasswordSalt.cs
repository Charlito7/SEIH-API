using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public static  class PasswordSalt
{
    public static byte[] GenerateSalt()
    {
        // Use a cryptographically secure random number generator (CSPRNG)
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[16]; // Adjust size as needed
            rng.GetBytes(salt);
            return salt;
        }
    }
}
