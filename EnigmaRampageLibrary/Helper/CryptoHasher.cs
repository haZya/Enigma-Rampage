using System;
using System.Text;

namespace EnigmaRampageLibrary.Helper
{
    /// <summary>
    /// Handles cryptographic hashing of values
    /// </summary>
    public static class CryptoHasher
    {
        /// <summary>
        /// Method for hashing
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}