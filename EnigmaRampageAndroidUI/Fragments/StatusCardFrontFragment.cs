using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidUI.Activities;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of StatusCardFront Fragment
    /// </summary>
    public class StatusCardFrontFragment : Fragment
    {
        private MainActivity mParentActivity;
        private static TextView sTvLvl, sTvMode, sTvStatus, sTvSwaps, sTvTime;
        private static LinearLayout sLayoutSwaps, sLayoutTime;

        /// <summary>
        /// Override OnCreateView function
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Initialization
            mParentActivity = Activity as MainActivity;
            View statusCardFront = inflater.Inflate(Resource.Layout.status_card_front, container, false);
            statusCardFront.Touch += StatusCardFront_Touch;

            sTvLvl = statusCardFront.FindViewById<TextView>(Resource.Id.txtLvl);
            sTvMode = statusCardFront.FindViewById<TextView>(Resource.Id.txtMode);
            sTvStatus = statusCardFront.FindViewById<TextView>(Resource.Id.txtStatus);
            sTvSwaps = statusCardFront.FindViewById<TextView>(Resource.Id.txtSwaps);
            sTvTime = statusCardFront.FindViewById<TextView>(Resource.Id.txtTime);
            sLayoutSwaps = statusCardFront.FindViewById<LinearLayout>(Resource.Id.layoutSwaps);
            sLayoutTime = statusCardFront.FindViewById<LinearLayout>(Resource.Id.layoutTime);

            return statusCardFront;
        }

        /// <summary>
        /// Handles the changes to the controls in the fragment
        /// </summary>
        /// <param name="lvl"></param>
        /// <param name="mode"></param>
        /// <param name="status"></param>
        public static void StatusControl(string lvl, string mode, string status)
        {
            sTvLvl.Text = lvl;
            sTvMode.Text = mode;
            sTvStatus.Text = status;

            if (mode == "Casual")
                sTvMode.SetTextColor(Color.Rgb(0, 167, 102));
            else
                sTvMode.SetTextColor(Color.Rgb(238, 26, 64));
        }

        /// <summary>
        /// Handles the number of swaps
        /// </summary>
        /// <param name="swaps"></param>
        public static void SwapsControl(int swaps)
        {
            if (swaps == -1)
            {
                sTvSwaps.Text = "∞";
            }
            else
            {
                sTvSwaps.Text = swaps.ToString();
            }
        }

        /// <summary>
        /// Handles the puzzling time
        /// </summary>
        /// <param name="time"></param>
        public static void TimeControl(string time)
        {
            sTvTime.Text = time;
        }

        /// <summary>
        /// Handles visibility of the status controls
        /// </summary>
        /// <param name="visible"></param>
        public static void StatusVisibility(bool visible)
        {
            if (visible)
            {
                sTvLvl.Visibility = ViewStates.Visible;
                sTvMode.Visibility = ViewStates.Visible;
                sLayoutSwaps.Visibility = ViewStates.Visible;
                sLayoutTime.Visibility = ViewStates.Visible;
            }
            else
            {
                sTvLvl.Visibility = ViewStates.Invisible;
                sTvMode.Visibility = ViewStates.Invisible;
                sLayoutSwaps.Visibility = ViewStates.Invisible;
                sLayoutTime.Visibility = ViewStates.Invisible;
            }
        }

        /// <summary>
        /// Handles the touch event of the fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusCardFront_Touch(object sender, View.TouchEventArgs e)
        {
            mParentActivity.gestureDetector.OnTouchEvent(e.Event);
        }
    }
}