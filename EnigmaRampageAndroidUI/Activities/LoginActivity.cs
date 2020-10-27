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
using EnigmaRampageLibrary.Helper;
using EnigmaRampageLibrary.Models;
using System;
using System.Threading.Tasks;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of Login Activity
    /// </summary>
    [Activity(Label = "Login", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class LoginActivity : AppCompatActivity
    {
        private TextInputEditText mTxtUsername, mTxtPwd;
        private CheckBox mRememberMe;
        private TextView mForgotPwd;
        private ProgressBar mProgressBar;
        private Button mBtnLogin;
        private LinearLayout mLoginPanel;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            #region Initialization
            mTxtUsername = FindViewById<TextInputEditText>(Resource.Id.loginUsername);
            mTxtPwd = FindViewById<TextInputEditText>(Resource.Id.loginPassword);
            mRememberMe = FindViewById<CheckBox>(Resource.Id.loginRememberMe);
            mForgotPwd = FindViewById<TextView>(Resource.Id.txtForgotPass);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.loginProgressBar);
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            mLoginPanel = FindViewById<LinearLayout>(Resource.Id.loginPanel);
            #endregion

            mLoginPanel.Click += (o, e) =>
                {
                    KeyboardManager.CloseKeyboard(mLoginPanel, this); // Close soft keyboard when background is clicked
                };

            mBtnLogin.Click += BtnLogin_Click;
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
                mTxtPwd.Enabled = true;
                mRememberMe.Enabled = true;
                mForgotPwd.Enabled = true;
                mBtnLogin.Enabled = true;
            }
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                mTxtUsername.Enabled = false;
                mTxtPwd.Enabled = false;
                mRememberMe.Enabled = false;
                mForgotPwd.Enabled = false;
                mBtnLogin.Enabled = false;
            }
        }

        /// <summary>
        /// Handles btnLogin click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validation
            string username = mTxtUsername.Text.Trim();
            if (username.Length < 1 || mTxtPwd.Text.Length < 1)
            {
                string error = "Please fill all the fields.";
                AlertGenerator.ShowError(error, this);
                return;
            }

            // Disabling controls while saving the record
            DisableEnableControls(false);

            Task.Factory.StartNew(() =>
            {
                if (DbConnector.OpenSQLConnection())
                {
                    // Connection opened
                    User user = new User()
                    {
                        Username = mTxtUsername.Text,
                        Password = CryptoHasher.Hash(mTxtPwd.Text)
                    };

                    UserDA userDA = new UserDA();
                    User resultUser = userDA.LoginUser(user);
                    if (resultUser != null)
                    {
                        // User found
                        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                        ISharedPreferencesEditor edit = pref.Edit();
                        if (mRememberMe.Checked)
                        {
                            // Save credentials in shared preferences
                            edit.PutString("Username", resultUser.Username);
                            edit.PutString("Password", resultUser.Password);
                            edit.Apply();
                        }
                        else
                        {
                            // Remove credentials from shared preferences
                            edit.Remove("Username");
                            edit.Remove("Password");
                            edit.Apply();
                        }

                        MyEventHandler.Trigger(this, resultUser.Username, resultUser.FullName, resultUser.Pic, true);
                        Finish(); // Close the activity
                        OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
                    }
                    else
                    {
                        string error = "Invalid Username or Password.";
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
            if (mBtnLogin.Enabled)
            {
                MyEventHandler.Trigger(this, "Player", "Player One", null, false);
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
                    if (mBtnLogin.Enabled)
                    {
                        MyEventHandler.Trigger(this, "Player", "Player One", null, false);
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