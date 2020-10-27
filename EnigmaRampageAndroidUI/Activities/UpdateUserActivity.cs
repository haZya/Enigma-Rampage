using System;
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
    /// Contains the events of Update User Activity 
    /// </summary>
    [Activity(Label = "Update User", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class UpdateUserActivity : AppCompatActivity
    {
        private TextInputEditText mTxtUsername, mTxtFullName, mTxtEmail, mTxtDOB, mTxtCurrentPwd, mTxtNewPwd, mTxtNewConfPwd;
        private ImageView mUserPic;
        private ProgressBar mProgressBar;
        private Button mBtnUserPicBrowse, mBtnUpdate;
        private Bitmap mUserImg;
        private LinearLayout mUpdatePanel;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_update_user);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            #region Initialization
            mTxtUsername = FindViewById<TextInputEditText>(Resource.Id.updateUserUsername);
            mTxtFullName = FindViewById<TextInputEditText>(Resource.Id.updateUserFullName);
            mTxtEmail = FindViewById<TextInputEditText>(Resource.Id.updateUserEmail);
            mTxtDOB = FindViewById<TextInputEditText>(Resource.Id.updateUserDOB);
            mTxtCurrentPwd = FindViewById<TextInputEditText>(Resource.Id.updateUserCurentPwd);
            mTxtNewPwd = FindViewById<TextInputEditText>(Resource.Id.updateUserPwd);
            mTxtNewConfPwd = FindViewById<TextInputEditText>(Resource.Id.updateUserConfPwd);
            mUserPic = FindViewById<ImageView>(Resource.Id.updateUserPic);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.updateUserProgressBar);
            mBtnUserPicBrowse = FindViewById<Button>(Resource.Id.updateUserBtnBrowse);
            mBtnUpdate = FindViewById<Button>(Resource.Id.updateUserBtnUpdate);
            mUpdatePanel = FindViewById<LinearLayout>(Resource.Id.updateUserPanel);

            mBtnUserPicBrowse.Click += MBtnUserPicBrowse_Click;
            mBtnUpdate.Click += MBtnUpdate_Click;
            #endregion

            mUpdatePanel.Click += (o, e) => KeyboardManager.CloseKeyboard(mUpdatePanel, this); // Close soft keyboard when background is clicked

            mTxtDOB.Click += (object sender, EventArgs e) =>
            {
                // Bring up the datepicker dialog
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    mTxtDOB.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

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
                                mUserImg = BitmapFactory.DecodeByteArray(resultUser.Pic, 0, resultUser.Pic.Length);
                                mUserPic.SetImageBitmap(mUserImg);
                            }
                            else
                            {
                                mUserPic.SetImageResource(Resource.Drawable.default_pic);
                            }
                            mTxtUsername.Text = resultUser.Username;
                            mTxtFullName.Text = resultUser.FullName;
                            mTxtEmail.Text = resultUser.Email;
                            mTxtDOB.Text = resultUser.DOB.ToShortDateString();
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
        /// Handles BtnUserPicBrowse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtnUserPicBrowse_Click(object sender, EventArgs e)
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
                mTxtFullName.Enabled = true;
                mTxtEmail.Enabled = true;
                mTxtDOB.Enabled = true;
                mTxtCurrentPwd.Enabled = true;
                mTxtNewPwd.Enabled = true;
                mTxtNewConfPwd.Enabled = true;
                mBtnUserPicBrowse.Enabled = true;
                mBtnUpdate.Enabled = true;
            }
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                mTxtFullName.Enabled = false;
                mTxtEmail.Enabled = false;
                mTxtDOB.Enabled = false;
                mTxtCurrentPwd.Enabled = false;
                mTxtNewPwd.Enabled = false;
                mTxtNewConfPwd.Enabled = false;
                mBtnUserPicBrowse.Enabled = false;
                mBtnUpdate.Enabled = false;
            }
        }

        /// <summary>
        /// Handles BtnUpdate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtnUpdate_Click(object sender, EventArgs e)
        {
            string fullname = mTxtFullName.Text.Trim();
            string email = mTxtEmail.Text.Trim();

            #region Input validation
            if (fullname.Length < 1 || email.Length < 1 || mTxtDOB.Text.Length < 1 ||
                mTxtCurrentPwd.Text.Length < 1 || mTxtNewPwd.Text.Length < 1 || mTxtNewConfPwd.Text.Length < 1)
            {
                string error = "Please fill all the fields.";
                AlertGenerator.ShowError(error, this);
                return;
            }
            else if (mTxtNewPwd.Text.Length < 8)
            {
                string error = "Password must contain at least 8 characters.";
                AlertGenerator.ShowError(error, this);
                return;
            }
            else if (mTxtNewPwd.Text != mTxtNewConfPwd.Text)
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
                        Username = mTxtUsername.Text.Trim(),
                        FullName = fullname,
                        Email = email,
                        DOB = DateTime.Parse(mTxtDOB.Text),
                        Password = CryptoHasher.Hash(mTxtNewPwd.Text),
                        Pic = imgBytes
                    };

                    UserDA userDA = new UserDA();
                    if (userDA.UpdateUser(user, Intent.GetStringExtra("Username"), CryptoHasher.Hash(mTxtCurrentPwd.Text)))
                    {
                        // User details updated successfully
                        Intent intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                        OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                        Finish(); // Close this activity
                    }
                    else
                    {
                        string error = "Invalid Password.";
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

        /// <summary>
        /// Override OnBackPressed method
        /// </summary>
        public override void OnBackPressed()
        {
            if (mBtnUpdate.Enabled)
            {
                Intent intent = new Intent(this, typeof(ProfileActivity));
                intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                Finish(); // Call Finish method when back button is pressed                
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
                    if (mBtnUpdate.Enabled)
                    {
                        Intent intent = new Intent(this, typeof(ProfileActivity));
                        intent.PutExtra("Username", Intent.GetStringExtra("Username"));
                        StartActivity(intent);
                        OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                        Finish(); // Call Finish method when back button is pressed
                    }
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}