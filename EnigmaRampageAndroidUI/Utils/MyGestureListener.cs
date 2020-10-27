using Android.Views;
using EnigmaRampageAndroidUI.Activities;

namespace EnigmaRampageAndroidUI.Utils
{
    /// <summary>
    /// Custom GestureListner for handling StatusCard flip
    /// </summary>
    class MyGestureListener : GestureDetector.SimpleOnGestureListener
    {
        private MainActivity mActivity;
        public MyGestureListener(MainActivity activity)
        {
            // Initialization
            mActivity = activity;
        }

        /// <summary>
        /// Override OnDoubleTab motion event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnDoubleTap(MotionEvent e)
        {
            mActivity.FlipCard();
            return true;
        }

        /// <summary>
        /// Override OnFling motion event
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="velocityX"></param>
        /// <param name="velocityY"></param>
        /// <returns></returns>
        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            mActivity.FlipCard();
            return true;
        }
    }
}