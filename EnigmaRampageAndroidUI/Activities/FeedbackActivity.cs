using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageAndroidUI.Utils;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of Feedback activity
    /// </summary>
    [Activity(Label = "Feedback", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class FeedbackActivity : AppCompatActivity
    {
        private LinearLayout mFeedbackPanel;
        private ProgressBar mProgressBar;
        private RatingBar mRatingBar;
        private TextInputEditText mTxtFeedback;
        private Button mBtnSend;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_feedback);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            mFeedbackPanel = FindViewById<LinearLayout>(Resource.Id.feedbackPanel);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.feedbackProgressBar);
            mRatingBar = FindViewById<RatingBar>(Resource.Id.feedbackRating);
            mTxtFeedback = FindViewById<TextInputEditText>(Resource.Id.txtFeedback);
            mBtnSend = FindViewById<Button>(Resource.Id.btnFeedback);

            mBtnSend.Click += BtnSend_Click;

            mFeedbackPanel.Click += (o, e) => KeyboardManager.CloseKeyboard(mFeedbackPanel, this); // Close soft keyboard when background is clicked
        }

        /// <summary>
        /// Method for disabling and enabling controls
        /// </summary>
        /// <param name="enable"></param>
        private void DisableEnableControls(bool enable)
        {
            if (enable)
            {
                mProgressBar.Visibility = ViewStates.Gone;
                mRatingBar.Enabled = true;
                mTxtFeedback.Enabled = true;
                mBtnSend.Enabled = true;
            }
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                mRatingBar.Enabled = false;
                mTxtFeedback.Enabled = false;
                mBtnSend.Enabled = false;
            }
        }

        /// <summary>
        /// Handles BtnSend event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, EventArgs e)
        {
            string message = mTxtFeedback.Text.Trim();
            if (message.Length > 255)
            {
                string error = "Sorry, the feedback message is too long.";
                AlertGenerator.ShowError(error, this);
            }
            else
            {
                DisableEnableControls(false);

                Task.Factory.StartNew(() =>
                {
                    if (DbConnector.OpenSQLConnection())
                    {
                        // Connection opened
                        Feedback feedback = new Feedback()
                        {
                            Username = Intent.GetStringExtra("Username"),
                            Rating = mRatingBar.Rating,
                            Message = message,
                            Date = DateTime.Now.Date
                        };

                        FeedbackDA feedbackDA = new FeedbackDA();
                        if (feedbackDA.InsertFeedback(feedback))
                        {
                            // Feedback was added successfully
                            Finish(); // Close this activity
                            OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                        }
                        else
                        {
                            string error = "There was a problem saving the record.";
                            RunOnUiThread(() =>
                            {
                                //Enabling the controls back
                                DisableEnableControls(true);
                                AlertGenerator.ShowError(error, this);
                            });
                        }
                    }
                    else
                    {
                        // Connection could not be opened
                        string error = "Connection to the database could not be established.";
                        RunOnUiThread(() =>
                        {
                            //Enabling the controls back
                            DisableEnableControls(true);
                            AlertGenerator.ShowError(error, this);
                        });
                    }

                    DbConnector.CloseSQLConnection(); // Close connection to the database
                });
            }
        }

        /// <summary>
        /// Override OnBackPressed method
        /// </summary>
        public override void OnBackPressed()
        {
            if (mBtnSend.Enabled)
            {                
                Finish(); // Call Finish method when back button is pressed
                OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
            }

            base.OnBackPressed();
        }

        /// <summary>
        /// Override OnOptionsItemSelected method
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (mBtnSend.Enabled)
                    {
                        Finish(); // Call Finish method when back button is pressed
                        OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                    }
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}