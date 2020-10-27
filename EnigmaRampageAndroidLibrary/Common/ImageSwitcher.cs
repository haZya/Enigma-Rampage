using Android.App;
using Android.Graphics;
using Android.Widget;
using System;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles switching of puzzle pieces
    /// </summary>
    public static class ImageSwitcher
    {
        public static event EventHandler OnPuzzleComplete;
        public static event EventHandler OnSwapComplete;

        /// <summary>
        /// Method for switching images between two imageViews
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <param name="images"></param>
        /// <param name="currentLvl"></param>
        /// <param name="picBoxes"></param>
        public static void SwitchImages(MyImageView box1, MyImageView box2, Bitmap[] images, int currentLvl, ImageView[] picBoxes)
        {
            int tmp = box2.ImageIndex; // Temporary holder for box2 imageIndex
            box2.SetImageBitmap(images[box1.ImageIndex]); // Change the box2 image to box1 image
            box2.ImageIndex = box1.ImageIndex; // Change the box2 imageIndex to box1 imageIndex
            box1.SetImageBitmap(images[tmp]); // Change the box1 image to box2 image
            box1.ImageIndex = tmp; // Change the box1 imageIndex to box2 imageIndex

            // Send the result each time when a puzzle piece is switched
            OnSwapComplete.Invoke(Application.Context, new EventArgs());

            // Check if the puzzle is fixed
            if (SuccessChecker.IsSuccessful(currentLvl, picBoxes))
            {
                // If true, send the result
                OnPuzzleComplete.Invoke(Application.Context, new EventArgs());
            }
        }
    }
}