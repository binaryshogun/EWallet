using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EWallet.Helpers
{
    /// <summary>
    /// Статический класс, содержащий 
    /// методы для шифрования и дешифрования строк.
    /// </summary>
    public static class EncryptionHelper
    {
        #region Constants
        const string encryptionKey = "wpf-pet";
        #endregion

        #region Methods
        /// <summary>
        /// Шифрует строку, используя <see cref="Rfc2898DeriveBytes"/>.
        /// </summary>
        /// <param name="clearText">Строка для шифрования.</param>
        /// <returns>Зашифрованный экземпляр <see cref="string"/>.</returns>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, 
                    new byte[] 
                    { 
                        0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                        0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 
                        0x76 
                    });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        /// <summary>
        /// Расшифровывает строку, используя <see cref="Rfc2898DeriveBytes"/>.
        /// </summary>
        /// <param name="cipherText">Зашифрованный текст.</param>
        /// <returns>Расшифрованный экземпляр <see cref="string"/>.</returns>
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, 
                    new byte[] 
                    { 
                        0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                        0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 
                        0x76 
                    });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}
