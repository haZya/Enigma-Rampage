using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageAndroidUI.Fragments;
using EnigmaRampageAndroidUI.Utils;
using EnigmaRampageLibrary.Helper;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of Profile Activity
    /// </summary>
    [Activity(Label = "Player Profile", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ProfileActivity : AppCompatActivity
    {
        private ProgressBar mProgressBar;
        private ImageView mProfPic;
        private TextView mTvUsername, mTvFullName, mTvEmail, mTvDOB;
        private Button mBtnUpdate, mBtnDel;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            #region Initialization
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.profProgressBar);
            mProfPic = FindViewById<ImageView>(Resource.Id.profPic);
            mTvUsername = FindViewById<TextView>(Resource.Id.profTxtUsername);
            mTvFullName = FindViewById<TextView>(Resource.Id.profTxtFullName);
            mTvEmail = FindViewById<TextView>(Resource.Id.profTxtEmail);
            mTvDOB = FindViewById<TextView>(Resource.Id.profTxtDOB);
            mBtnUpdate = FindViewById<Button>(Resource.Id.profBtnUpdate);
            mBtnDel = FindViewById<Button>(Resource.Id.profBtnDel);

            mBtnUpdate.Click += MBtnUpdate_Click;
            mBtnDel.Click += MBtnDel_Click;
            #endregion

            SetData();
        }

        /// <summary>
        /// Method for setting the user data into the fields
        /// </summary>
        private void SetData()
        {
            mProgressBar.Visibility = ViewStates.Visible;

            Task.Factory.StartNew(() =>
            {
                if (DbConnector.OpenSQLConnection())
                {
                    // Connection opened
                    UserDA userDA = new UserDA();
                    User resultUser = userDA.FindUser(Intent.GetStringExtra("Username"));
                    if (resultUser != null)
                    {
                        // User found
                        RunOnUiThread(() =>
                        {
                            if (resultUser.Pic != null)
                            {
                                GC.Collect(); // Must call Garbage Collector
                                mProfPic.SetImageBitmap(BitmapFactory.DecodeByteArray(resultUser.Pic, 0, resultUser.Pic.Length));                                
                            }
                            else
                            {
                                mProfPic.SetImageResource(Resource.Drawable.default_pic);
                            }
                            mTvUsername.Text = resultUser.Username;
                            mTvFullName.Text = resultUser.FullName;
                            mTvEmail.Text = resultUser.Email;
                            mTvDOB.Text = resultUser.DOB.ToLongDateString();
                            mProgressBar.Visibility = ViewStates.Gone;
                        });
                    }
                    else
                    {
                        // User not found
                        string error = "There was a problem when trying to retrieve data.";
                        RunOnUiThread(() =>
                        {
                            mProgressBar.Visibility = ViewStates.Gone;
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
                        mProgressBar.Visibility = ViewStates.Gone;
                        AlertGenerator.ShowError(error, this);
                    });
                }

                DbConnector.CloseSQLConnection(); // Close connection to the database
            });
        }

        /// <summary>
        /// Handles BtnUpdate click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtnUpdate_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(UpdateUserActivity));
            intent.PutExtra("Username", Intent.GetStringExtra("Username"));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            Finish();
        }

        /// <summary>
        /// Handles BtnDel click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtnDel_Click(object sender, EventArgs e)
        {
            // Pull up delete dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DeleteUserDialog dialogFrag = new DeleteUserDialog();
            dialogFrag.Show(transaction, "dialog fragment");

            dialogFrag.OnDeleteComplete += DialogFrag_OnDeleteComplete;             
        }

        /// <summary>
        /// Handles deletion of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogFrag_OnDeleteComplete(object sender, DeleteUserDialog.OnDeleteEventArgs e)
        {
            mProgressBar.Visibility = ViewStates.Visible;
            Task.Factory.StartNew(() =>
            {
                if (DbConnector.OpenSQLConnection())
                {
                    // Connection opened
                    User user = new User()
                    {
                        Username = Intent.GetStringExtra("Username"),
                        Password = CryptoHasher.Hash(e.Password)
                    };

                    UserDA userDA = new UserDA();
                    if (userDA.DeleteUser(user))
                    {
                        // User deleted
                        RunOnUiThread(() => MyEventHandler.Trigger(this));
                        Finish(); // Close the activity
                        OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                    }
                    else
                    {
                        string error = "Invalid password.";
                        RunOnUiThread(() =>
                        {
                            //Enabling the controls back
                            mProgressBar.Visibility = ViewStates.Gone;
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
                        mProgressBar.Visibility = ViewStates.Gone;
                        AlertGenerator.ShowError(error, this);
                    });
                }

                DbConnector.CloseSQLConnection(); // Close connection to the database
            });
        }

        /// <summary>
        /// Override OnBackPressed method
        /// </summary>
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
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
                    Finish(); // Call Finish method when back button is pressed
                    OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}