using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Projeto.Data.Utils
{
    public class Criptografia
    {
        public static string GetMD5Hash(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            string result = string.Empty;
            foreach(var pos in hash)
            {
                result += pos.ToString("X2").ToUpper();
            }

            return result;
        }
    }
}
