using Core.Domain.Commons.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interface;

public interface IHashingServices
{
    HashSalt HashText(string plainText);
    bool VerifyText(string enteredText, byte[] salt, string storedHashPassword);
}
