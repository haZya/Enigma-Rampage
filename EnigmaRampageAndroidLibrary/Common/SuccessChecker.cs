using Android.Widget;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Contains the function for checking puzzle progress
    /// </summary>
    public static class SuccessChecker
    {
        /// <summary>
        /// Method for checking if the puzzle is fixed correctly
        /// </summary>
        /// <param name="currentLvl"></param>
        /// <param name="picBoxes"></param>
        /// <returns></returns>
        public static bool IsSuccessful(int currentLvl, ImageView[] picBoxes)
        {            
            for (int i = 0; i < currentLvl; i++)
            {
                // Check if the puzzle pieces are matching the original order
                if (((MyImageView)picBoxes[i]).Index != ((MyImageView)picBoxes[i]).ImageIndex)
                    return false;
            }
            return true;
        }
    }
}