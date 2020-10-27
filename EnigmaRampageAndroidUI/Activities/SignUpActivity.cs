using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageAndroidUI.Utils;
using EnigmaRampageLibrary.Helper;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of SignUp Activity
    /// </summary>
    [Activity(Label = "Sign Up", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SignUpActivity : AppCompatActivity
    {       
        private TextInputEditText mTxtUsername, mTxtFullName, mTxtEmail, mTxtDOB, mTxtPwd, mTxtConfPwd;
        private ImageView mUserPic;
        private ProgressBar mProgressBar;
        private Button mBtnUserPicBrowse, mBtnSignUp;
        private Bitmap mUserImg;
        private LinearLayout mSignUpPanel;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sign_up);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            #region Initialization
            mTxtUsername = FindViewById<TextInputEditText>(Resource.Id.signUpUsername);
            mTxtFullName = FindViewById<TextInputEditText>(Resource.Id.signUpFullName);
            mTxtEmail = FindViewById<TextInputEditText>(Resource.Id.signUpEmail);
            mTxtDOB = FindViewById<TextInputEditText>(Resource.Id.signUpDOB);
            mTxtPwd = FindViewById<TextInputEditText>(Resource.Id.signUpPassword);
            mTxtConfPwd = FindViewById<TextInputEditText>(Resource.Id.signUpConfPassword);
            mUserPic = FindViewById<ImageView>(Resource.Id.userPic);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.signUpProgressBar);
            mBtnUserPicBrowse = FindViewById<Button>(Resource.Id.btnUserPicBrowse);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mSignUpPanel = FindViewById<LinearLayout>(Resource.Id.signUpPanel);

            mBtnUserPicBrowse.Click += BtnUserPicBrowse_Click;
            mBtnSignUp.Click += BtnSignUp_Click;
            #endregion

            mSignUpPanel.Click += (o, e) => KeyboardManager.CloseKeyboard(mSignUpPanel, this); // Close soft keyboard when background is clicked

            mTxtDOB.Click += (object sender, EventArgs e) =>
            {
                // Bring up the datepicker dialog
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    mTxtDOB.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
        }

        /// <summary>
        /// Handles BtnUserPicBrowse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUserPicBrowse_Click(object sender, EventArgs e)
        {
            // Create an intent for browsing images
            using (Intent intent = new Intent())
            {
                intent.SetType("image/*"); // Set file type to only images
                intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(intent, "Select an Image"), 0);
            }
        }

        /// <summary>
        /// Override OnActivityResult function
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                LoadImage(data);
            }
        }

        /// <summary>
        /// Method for loading the image asynchronously
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        private async void LoadImage(Intent data)
        {
            try
            {
                // Call BitmapMaker to draw the selected image into a resampled bitmap
                Bitmap result = await BitmapMaker.CreateBitmapImage(240, 240, mUserPic.Width, mUserPic.Height, data, this);
                if (result != null)
                {
                    mUserImg = result;
                }
            }
            catch
            {
                string error = "There was a problem trying to load the image.";
                AlertGenerator.ShowError(error, this);
            }

            mUserPic.SetImageBitmap(mUserImg); // Set the bitmap into the userPic imageView
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
                mTxtUsername.Enabled = true;
                mTxtFullName.Enabled = true;
                mTxtEmail.Enabled = true;
                mTxtDOB.Enabled = true;
                mTxtPwd.Enabled = true;
                mTxtConfPwd.Enabled = true;
                mBtnUserPicBrowse.Enabled = true;
                mBtnSignUp.Enabled = true;
            }
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                mTxtUsername.Enabled = false;
                mTxtFullName.Enabled = false;
                mTxtEmail.Enabled = false;
                mTxtDOB.Enabled = false;
                mTxtPwd.Enabled = false;
                mTxtConfPwd.Enabled = false;
                mBtnUserPicBrowse.Enabled = false;
                mBtnSignUp.Enabled = false;
            }
        }

        /// <summary>
        /// Handles BtnSignUp click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            string username = mTxtUsername.Text.Trim();
            string fullname = mTxtFullName.Text.Trim();
            string email = mTxtEmail.Text.Trim();

            #region Input validation
            if (username.Length < 1 || fullname.Length < 1 || email.Length < 1 || mTxtDOB.Text.Length < 1 ||
                mTxtPwd.Text.Length < 1 || mTxtConfPwd.Text.Length < 1)
            {
                string error = "Please fill all the fields.";
                AlertGenerator.ShowError(error, this);
                return;
            }
            else if (mTxtPwd.Text.Length < 8)
            {
                string error = "Password must contain at least 8 characters.";
                AlertGenerator.ShowError(error, this);
                return;
            }
            else if (mTxtPwd.Text != mTxtConfPwd.Text)
            {
                string error = "Password and Confirm Password do not match.";
                AlertGenerator.ShowError(error, this);
                return;
            }
            #endregion

            // Disabling controls while saving the record
            DisableEnableControls(false);

            Task.Factory.StartNew(() =>
            {
                if (DbConnector.OpenSQLConnection())
                {
                    // Connection opened
                    UserDA userDA = new UserDA();
                    if (userDA.IsUsernameExists(username))
                    {
                        string error = "The Username entered already exist.";
                        RunOnUiThread(() =>
                        {
                            //Enabling the controls back
                            DisableEnableControls(true);
                            AlertGenerator.ShowError(error, this);
                        });
                    }
                    else
                    {
                        byte[] imgBytes = null;
                        if (mUserImg != null)
                        {
                            // Converting the bitmap into a byte array
                            MemoryStream memStream = new MemoryStream();
                            mUserImg.Compress(Bitmap.CompressFormat.Webp, 100, memStream);
                            imgBytes = memStream.ToArray();
                        }

                        User user = new User()
                        {
                            Username = username,
                            FullName = fullname,
                            Email = email,
                            DOB = DateTime.Parse(mTxtDOB.Text),
                            Password = CryptoHasher.Hash(mTxtPwd.Text),
                            Pic = imgBytes
                        };

                        DbConnector.CloseSQLConnection(); // Close connection to the database
                        if (DbConnector.OpenSQLConnection())
                        {
                            if (userDA.InsertUser(user))
                            {
                                // User was added successfully
                                // Add a record to leaderboard
                                PlayerStats stats = new PlayerStats()
                                {
                                    Username = username,
                                    XP = 0,
                                    SR = 0,
                                    PlayTime = TimeSpan.FromSeconds(0),
                                    Golds = 0,
                                    Silvers = 0,
                                    Bronzes = 0
                                };

                                DbConnector.CloseSQLConnection(); // Close connection to the database
                                PlayerStatsDA statsDA = new PlayerStatsDA();
                                if (DbConnector.OpenSQLConnection())
                                {
                                    if (!statsDA.InsertStats(stats))
                                    {
                                        string error = "There was a problem updating your stats.";
                                        RunOnUiThread(() =>
                                        {
                                            AlertGenerator.ShowError(error, this);
                                        });
                                    }
                                }

                                // Add achievements
                                List<AchievementsCompleted> achievementsCompleted = new List<AchievementsCompleted>();
                                for (int i = 1; i < 20; i++)
                                {
                                    achievementsCompleted.Add(new AchievementsCompleted()
                                    {
                                        Username = username,
                                        Progress = 0,
                                        Status = false
                                    });
                                }

                                DbConnector.CloseSQLConnection(); // Close connection to the database
                                if (DbConnector.OpenSQLConnection())
                                {
                                    AchievementsDA achievementsDA = new AchievementsDA();
                                    if (!achievementsDA.InsertAchievements(achievementsCompleted))
                                    {
                                        string error = "There was a problem updating your stats.";
                                        RunOnUiThread(() =>
                                        {
                                            AlertGenerator.ShowError(error, this);
                                        });
                                    }
                                }

                                // Navigate to Login activity
                                Intent intent = new Intent(this, typeof(LoginActivity));
                                StartActivity(intent);
                                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                                Finish(); // Close this activity
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

        /// <summary>
        /// Override OnBackPressed method
        /// </summary>
        public override void OnBackPressed()
        {
            if (mBtnSignUp.Enabled)
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
                    if (mBtnSignUp.Enabled)
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