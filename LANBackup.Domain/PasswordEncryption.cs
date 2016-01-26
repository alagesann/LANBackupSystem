using System;
using System.Security.Cryptography;
using System.Text;

namespace LANBackup.Domain
{
   public  static class PasswordEncryption
    {
        public static string Encrypt(string plainText)
        {     
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);         
            return Convert.ToBase64String(encrypted);
        }
        public static string Decrypt(string cipher)
        {                  
            byte[] data = Convert.FromBase64String(cipher);            
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }
    }
}
