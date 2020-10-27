using System;

namespace EnigmaRampageLibrary
{
    /// <summary>
    /// Handles random shuffling of items in an array
    /// </summary>
    public class Shuffler
    {
        /// <summary>
        /// Method for shuffling an array of integers
        /// </summary>
        /// <param name="array"></param>
        public static void Shuffle(ref int[] array)
        {
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n);
                n--;
                int temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}