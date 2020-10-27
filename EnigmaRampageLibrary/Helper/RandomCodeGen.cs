using System;
using System.Linq;

namespace EnigmaRampageLibrary.Helper
{
    /// <summary>
    /// Handles random code generation
    /// </summary>
    public static class RandomCodeGen
    {
        /// <summary>
        /// Method for generating random strings with specific lengths
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateCode(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}