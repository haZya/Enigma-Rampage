using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageLibrary.Models;
using FFImageLoading;
using FFImageLoading.Views;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains events of Splash Activity
    /// </summary>
    [Activity(Label = "Enigma Rampage", Theme = "@style/AppTheme.Launcher",  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);

            Task.Factory.StartNew(() =>
            {
                // Load splash loading image
                ImageViewAsync splashGif = FindViewById<ImageViewAsync>(Resource.Id.splashImageView);
                ImageService.Instance.LoadCompiledResource("loading").Into(splashGif);

                // Read UserInfo for saved credentials if available
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                string username = pref.GetString("Username", String.Empty);
                string password = pref.GetString("Password", String.Empty);

                if (username == String.Empty || password == String.Empty)
                {
                    // No saved credentials, take user to the access screen
                    DbConnector.OpenSQLConnection();
                    DbConnector.CloseSQLConnection();
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("LoggingStatus", false);
                    StartActivity(intent);
                    OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                    Finish(); // Close this activity
                    RunOnUiThread(() => MyEventHandler.Trigger(this, "Player", "Player One", null, false));
                }
                else
                {
                    // Saved credentials found
                    // Validate credentials
                    if (DbConnector.OpenSQLConnection())
                    {
                        // Connection opened
                        User user = new User()
                        {
                            Username = username,
                            Password = password
                        };

                        UserDA userDA = new UserDA();
                        User resultUser = userDA.LoginUser(user);
                        if (resultUser != null)
                        {
                            // User found
                            Intent resultIntent = new Intent(this, typeof(MainActivity));
                            resultIntent.PutExtra("LoggingStatus", true);
                            StartActivity(resultIntent);
                            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                            Finish(); // Close this activity
                            RunOnUiThread(() => MyEventHandler.Trigger(this, resultUser.Username, resultUser.FullName, resultUser.Pic, true));
                        }
                        else
                        {
                            // Saved credentials are incorrect, take the user to access screen
                            ISharedPreferencesEditor edit = pref.Edit();
                            edit.Remove("Username");
                            edit.Remove("Password");
                            edit.Apply();

                            Intent intent = new Intent(this, typeof(MainActivity));
                            intent.PutExtra("LoggingStatus", false);
                            StartActivity(intent);
                            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                            Finish(); // Close this activity
                            RunOnUiThread(() => MyEventHandler.Trigger(this, "Player", "Player One", null, false));
                        }
                    }
                    else
                    {
                        // Connection could not be opened, take the user to access screen
                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.PutExtra("LoggingStatus", false);
                        StartActivity(intent);
                        OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                        Finish(); // Close this activity
                        RunOnUiThread(() => MyEventHandler.Trigger(this, "Player", "Player One", null, false));
                    }

                    DbConnector.CloseSQLConnection(); // Close connection to the database
                    GC.Collect(); // Must call Garbage Collector
                }
            });
        }
    }
}