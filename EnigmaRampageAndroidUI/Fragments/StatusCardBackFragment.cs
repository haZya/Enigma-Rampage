using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidUI.Activities;
using FFImageLoading;
using FFImageLoading.Views;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of StatusCardBack Fragment
    /// </summary>
    public class StatusCardBackFragment : Fragment
    {
        private static MainActivity sParentActivity;
        private static ImageButton sBtnPrev, sBtnReplay, sBtnNext;
        private static ImageViewAsync sRewardGif;
        private static TextView sTvReward;

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
            sParentActivity = Activity as MainActivity;
            View statusCardBack = inflater.Inflate(Resource.Layout.status_card_back, container, false);
            statusCardBack.Touch += StatusCardFront_Touch;

            sBtnPrev = statusCardBack.FindViewById<ImageButton>(Resource.Id.btnPrev);
            sBtnReplay = statusCardBack.FindViewById<ImageButton>(Resource.Id.btnReplay);
            sBtnNext = statusCardBack.FindViewById<ImageButton>(Resource.Id.btnNext);
            sRewardGif = statusCardBack.FindViewById<ImageViewAsync>(Resource.Id.rewardGif);
            sTvReward = statusCardBack.FindViewById<TextView>(Resource.Id.txtReward);

            sBtnPrev.Click += BtnPrev_Click;
            sBtnReplay.Click += BtnReplay_Click;
            sBtnNext.Click += BtnNext_Click;

            return statusCardBack;
        }

        /// <summary>
        /// Handles the BtnNext clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            sParentActivity.NextLvl();
        }

        /// <summary>
        /// Handles the BtnReplay clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReplay_Click(object sender, EventArgs e)
        {
            sParentActivity.ReplayLvl();
        }

        /// <summary>
        /// Handles the BtnPrev clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            sParentActivity.PrevLvl();
        }

        /// <summary>
        /// Handles the changes to the controls in the fragment
        /// </summary>
        /// <param name="currentLvl"></param>
        public static void LvlControl(int currentLvl, int time, int swaps, string mode)
        {
            sBtnPrev.Enabled = true;
            sBtnPrev.SetImageResource(Resource.Drawable.ic_arrow_back_blue);
            sBtnNext.Enabled = true;
            sBtnNext.SetImageResource(Resource.Drawable.ic_arrow_forward_blue);

            sTvReward.Text = "Good Game!";
            if (currentLvl == 4)
            {
                if (mode == "Competitive")
                {
                    if (swaps > 5 || time > 10)
                    {
                        ImageService.Instance.LoadCompiledResource("bronze_cup").Into(sRewardGif);
                    }
                    else if (swaps > 3 || time > 7)
                    {
                        ImageService.Instance.LoadCompiledResource("silver_cup").Into(sRewardGif);
                    }
                    else
                    {
                        ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                    }
                }
                else
                {
                    ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                }
                sBtnPrev.Enabled = false;
                sBtnPrev.SetImageResource(Resource.Drawable.ic_arrow_back_grey);
            }
            else if (currentLvl == 9)
            {
                if (mode == "Competitive")
                {
                    if (swaps > 16 || time > 30)
                    {
                        ImageService.Instance.LoadCompiledResource("bronze_cup").Into(sRewardGif);
                    }
                    else if (swaps > 11 || time > 18)
                    {
                        ImageService.Instance.LoadCompiledResource("silver_cup").Into(sRewardGif);
                    }
                    else
                    {
                        ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                    }
                }
                else
                {
                    ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                }
            }
            else if (currentLvl == 16)
            {
                if (mode == "Competitive")
                {
                    if (swaps > 28 || time > 52)
                    {
                        ImageService.Instance.LoadCompiledResource("bronze_cup").Into(sRewardGif);
                    }
                    else if (swaps > 19 || time > 35)
                    {
                        ImageService.Instance.LoadCompiledResource("silver_cup").Into(sRewardGif);
                    }
                    else
                    {
                        ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                    }
                }
                else
                {
                    ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                }
            }
            else if (currentLvl == 25)
            {
                if (mode == "Competitive")
                {
                    if (swaps > 48 || time > 120)
                    {
                        ImageService.Instance.LoadCompiledResource("bronze_cup").Into(sRewardGif);
                    }
                    else if (swaps > 33 || time > 80)
                    {
                        ImageService.Instance.LoadCompiledResource("silver_cup").Into(sRewardGif);
                    }
                    else
                    {
                        ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                    }
                }
                else
                {
                    ImageService.Instance.LoadCompiledResource("gold_cup").Into(sRewardGif);
                }
                sBtnNext.Enabled = false;
                sBtnNext.SetImageResource(Resource.Drawable.ic_arrow_forward_grey);
            }
        }

        /// <summary>
        /// Handles the touch event of the fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusCardFront_Touch(object sender, View.TouchEventArgs e)
        {
            sParentActivity.gestureDetector.OnTouchEvent(e.Event);
        }
    }
}