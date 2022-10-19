using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Helpers
{
    public class HashHelper
    {
        /// <summary>
        /// Хэширует строку, используя криптографический алгоритм SHA-1.
        /// </summary>
        /// <param name="stringToHash">Строка для хэширования.</param>
        /// <param name="length">Длина возвращаемой строки.</param>
        /// <returns>Зашифрованная строка <see cref="string"/></returns>
        public static string GetHash(string stringToHash, int length)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)).Select(x => x.ToString("X2"))).Substring(0, length);
            }
        }
    }
}
