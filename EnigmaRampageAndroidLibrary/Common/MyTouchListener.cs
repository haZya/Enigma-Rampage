using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Custom TouchListner for handling touch events of puzzle pieces
    /// </summary>
    public class MyTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private MyImageView mFirstBox;
        private readonly Bitmap[] mImages;
        private readonly int mCurrentLvl;
        private readonly ImageView[] mPicBoxes;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="images"></param>
        /// <param name="currentLvl"></param>
        /// <param name="picBoxes"></param>
        public MyTouchListener(Bitmap[] images, int currentLvl, ImageView[] picBoxes)
        {
            mImages = images;
            mCurrentLvl = currentLvl;
            mPicBoxes = picBoxes;
        }

        /// <summary>
        /// Handles OnTouch event for picBoxes imageViews
        /// </summary>
        /// <param name="v"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnTouch(View v, MotionEvent e)
        {
            if (!SuccessChecker.IsSuccessful(mCurrentLvl, mPicBoxes))
            {
                int x = (int)v.GetX(); // Get X position of picBox imageView
                int y = (int)v.GetY(); // Get Y position of picBox imageView

                if (e.Action == MotionEventActions.Down)
                {
                    // Touch down event
                    mFirstBox = (MyImageView)v; // Get the imageView which is touched
                    return true;
                }
                if (e.Action == MotionEventActions.Up)
                {
                    // Touch up event
                    int positionX = (int)e.GetX() + x; // Get X position where the touch released relative to the original X position
                    int positionY = (int)e.GetY() + y; // Get Y position where the touch released relative to the original Y position
                    int picBox = 0;

                    for (int i = 0; i < mCurrentLvl; i++)
                    {
                        // Get the bounds of each imageView
                        int xStart = (int)mPicBoxes[i].GetX(); // Get the X position of the imageView top left bound
                        int xEnd = (int)mPicBoxes[i].GetX() + mPicBoxes[i].Width; // Get the X position of the imageView top right bound
                        int yStart = (int)mPicBoxes[i].GetY(); // Get the Y position of the imageView bottom left bound
                        int yEnd = (int)mPicBoxes[i].GetY() + mPicBoxes[i].Height; // Get the Y position of the imageView bottom right bound

                        // Check if the touch released position is within the imageView bounds
                        if (positionX > xStart && positionX < xEnd && positionY > yStart && positionY < yEnd)
                        {
                            picBox = i; // If true, get the index of the imageView
                        }
                    }
                    if (mFirstBox != mPicBoxes[picBox])
                    {
                        // Call ImageSwitcher if a swap is triggered
                        ImageSwitcher.SwitchImages(mFirstBox, (MyImageView)mPicBoxes[picBox], mImages, mCurrentLvl, mPicBoxes);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}