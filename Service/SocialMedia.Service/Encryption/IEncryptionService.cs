using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Encryption
{
    public interface IEncryptionService
    {
        (string EncryptedText, string IV) Encrypt(string clearText);
        string Decrypt(string cipherText, string iv);
    }
}
