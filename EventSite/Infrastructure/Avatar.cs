using System;
using System.Security.Cryptography;
using System.Text;

namespace EventSite.Infrastructure {
    public static class Avatar {
        public static string GetUrl(string email, int size = 32) {
            var emailHash = CalculateEmailHash(email);
            return string.Format("https://secure.gravatar.com/avatar/{0}?s={1}&d={2}", emailHash, size, "mm");
        }

        static string CalculateEmailHash(string email) {
            var trimmed = email.Trim();
            var lowered = trimmed.ToLower();
            return ComputeHash(lowered).ToLower();
        }

        static string ComputeHash(string input) {
            using(var md5 = new MD5CryptoServiceProvider()) {
                var inputArray = Encoding.ASCII.GetBytes(input);
                var hashedArray = md5.ComputeHash(inputArray);
                return BitConverter.ToString(hashedArray).Replace("-", "");
            }
        }
    }
}