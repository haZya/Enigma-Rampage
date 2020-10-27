using System;

namespace EnigmaRampageLibrary.Helper
{
    /// <summary>
    /// Handles extensions of methods
    /// </summary>
    public static class ExtMethods
    {
        /// <summary>
        /// Method fpr comparing two string values with a specified comparison type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            return (source.IndexOf(toCheck, comparisonType) >= 0);
        }
    }
}