using Core.Domain.Entities;
using Core.Domain.Entity.SEIH;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public static class MyPasswordHasher 
{
    private static readonly int _workFactor = 12; 

    public static string HashPassword(UserEntity user, string password)
    {
        // Use Argon2id for best security and resistance to GPU-based attacks
        var hashedPassword =  Argon2.Hash(Encoding.UTF8.GetBytes(password), user.Salt, _workFactor);
        return hashedPassword;
    }

    public static bool VerifyHashedPassword(UserEntity user, string hashedPassword, string providedPassword) =>
         // Use Argon2id for verification
         Argon2.Verify(hashedPassword, Encoding.UTF8.GetBytes(providedPassword), user.Salt);

    public static string HashPassword(UsersEntity user, string password)
    {
        // Use Argon2id for best security and resistance to GPU-based attacks
        var hashedPassword = Argon2.Hash(Encoding.UTF8.GetBytes(password), user.Salt, _workFactor);
        return hashedPassword;
    }

    public static bool VerifyHashedPassword(UsersEntity user, string hashedPassword, string providedPassword) =>
         // Use Argon2id for verification
         Argon2.Verify(hashedPassword, Encoding.UTF8.GetBytes(providedPassword), user.Salt);
}
