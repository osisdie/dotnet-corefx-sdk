using System;
using System.Security.Cryptography;
using System.Text;

namespace CoreFX.Abstractions.Extensions
{
    public static class Crypto_Extension
    {
        public static string ToMD5(this string src)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(src);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }
    }
}
