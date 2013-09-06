using System;
using System.Security.Cryptography;
using System.Text;
using CodeCamp.Domain;

namespace CodeCamp.Infrastructure {
    public static class ExtensionMethods {
        public static string GetUserAvatarLink(this IApplicationState state, int size = 32) {
            if(state.UserIsLoggedIn()) {
                var emailHash = CalculateEmailHash(state.User.Email);
                return string.Format("https://secure.gravatar.com/avatar/{0}?s={1}&d={2}", emailHash, size, "mm");
            }

            return "#";
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