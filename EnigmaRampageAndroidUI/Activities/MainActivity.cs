using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
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
    /// Contains the events of Main Activity
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        #region Global variables
        private const int LVL_1_NUM = 4;
        private const int LVL_2_NUM = 9;
        private const int LVL_3_NUM = 16;
        private const int LVL_4_NUM = 25;
        private const int LOAD_IMG_REQUEST = 1;
        private DrawerLayout mDrawer;
        private static int sCurrentLvl;
        private Button mBtnPlay, mBtnBrowse;
        private RelativeLayout mPuzzlePanel;
        private Bitmap mImage;
        private static TextView sTvProgress;
        private LinearLayout mLayoutProgress, mLayoutMessage;
        private ImageView mPicBoxWhole;
        public GestureDetector gestureDetector;
        private bool mShowingBack;
        private static bool sIsFixed;
        private string mLvl, mMode, mStatus, mTime;
        private string mCurrentMode;
        private int mSwaps;
        private Timer mStatusTimer;
        private int mMin, mSec, mTimeInSeconds;
        private string mUsername;
        private ImageView mProfilePic;
        private TextView mTvUername, mTvFullname;
        private FrontPanelDialog mDialogFrag;
        private IMenuItem navLogin, navSignUp, navProfile, navLogout, navLeaderBoard, navAchievements, navReports, navFeedback;
        #endregion

        /// <summary>
        /// Override OnCreate function
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Set-up the custom toolbar as the action bar
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar); // Set toolbar as actionBar

            // Set-up the navigation drawer
            mDrawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, mDrawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            mDrawer.AddDrawerListener(toggle);
            toggle.SyncState();

            // Set-up the navigation view
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_casual);
            View headerView = navigationView.GetHeaderView(0);

            gestureDetector = new GestureDetector(this, new MyGestureListener(this));
            if (savedInstanceState == null)
            {
                // If no saved instance state is found, add new StatusCardFrontFragment to container
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                transaction.Add(Resource.Id.container, new StatusCardFrontFragment());
                transaction.Commit();
            }

            #region Initialization
            mProfilePic = headerView.FindViewById<ImageView>(Resource.Id.mainProfilePic);
            mTvUername = headerView.FindViewById<TextView>(Resource.Id.mainTxtUsername);
            mTvFullname = headerView.FindViewById<TextView>(Resource.Id.mainTxtFullName);
            mPuzzlePanel = FindViewById<RelativeLayout>(Resource.Id.puzzlePanel);
            sTvProgress = FindViewById<TextView>(Resource.Id.txtPercentage);
            mLayoutProgress = FindViewById<LinearLayout>(Resource.Id.puzzlePanelLoading);
            mLayoutMessage = FindViewById<LinearLayout>(Resource.Id.puzzlePanelMessage);
            mBtnBrowse = FindViewById<Button>(Resource.Id.btnBrowse);
            mBtnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            navLogin = navigationView.Menu.FindItem(Resource.Id.nav_login);
            navSignUp = navigationView.Menu.FindItem(Resource.Id.nav_signup);
            navProfile = navigationView.Menu.FindItem(Resource.Id.nav_profile);
            navLogout = navigationView.Menu.FindItem(Resource.Id.nav_logout);
            navLeaderBoard = navigationView.Menu.FindItem(Resource.Id.nav_leaderboard);
            navAchievements = navigationView.Menu.FindItem(Resource.Id.nav_achievements);
            navReports = navigationView.Menu.FindItem(Resource.Id.nav_reports);
            navFeedback = navigationView.Menu.FindItem(Resource.Id.nav_feedback);

            mUsername = String.Empty;
            mProfilePic.Click += (o, e) =>
            {
                // Navigate to the Profile activity
                if (mUsername != String.Empty)
                {
                    Intent intent = new Intent(this, typeof(ProfileActivity));
                    intent.PutExtra("Username", mUsername);
                    StartActivity(intent);
                    OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
                }
            };

            mBtnPlay.Click += MBtnPlay_Click;
            mBtnBrowse.Click += BtnBrowse_Click;
            EnigmaRampageAndroidLibrary.Common.ImageSwitcher.OnPuzzleComplete += ImageSwitcher_OnPuzzleComplete;
            EnigmaRampageAndroidLibrary.Common.ImageSwitcher.OnSwapComplete += ImageSwitcher_OnSwapComplete;
            MyEventHandler.OnLoginComplete += MyEventHandler_OnLoginComplete;
            MyEventHandler.OnDeleteComplete += MyEventHandler_OnDeleteComplete;

            mDialogFrag = new FrontPanelDialog();
            mDialogFrag.OnSignUpNavComplete += DialogFrag_OnSignUpNavComplete;
            mDialogFrag.OnLoginNavComplete += DialogFrag_OnLoginNavComplete;

            mPicBoxWhole = new ImageView(this);
            sCurrentLvl = LVL_1_NUM;
            sIsFixed = false;
            mMode = "Casual";
            mCurrentMode = mMode;
            mStatus = "Idle";
            mSwaps = -1;
            mTime = "∞";
            mStatusTimer = new Timer
            {
                Interval = 1000
            };
            mStatusTimer.Elapsed += OnTimedEvent;
            #endregion

            if (!Intent.GetBooleanExtra("LoggingStatus", false))
            {
                // Pull up Front Panel dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                mDialogFrag.Show(transaction, "dialog fragment");
            }
        }

        /// <summary>
        /// Set profile information and navigation after login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyEventHandler_OnLoginComplete(object sender, MyEventHandler.OnLoginEventArgs e)
        {
            Login(e.Username, e.FullName, e.Pic, e.IsLogged);
        }

        /// <summary>
        /// Method for handling login
        /// </summary>
        /// <param name="isLogged"></param>
        /// <param name="username"></param>
        /// <param name="fullName"></param>
        /// <param name="pic"></param>
        public void Login(string username, string fullName, byte[] pic, bool isLogged)
        {
            //Set profile information on nav header
            if (isLogged)
            {
                this.mUsername = username;
                RunOnUiThread(() =>
                {
                    // Execute on UI thread                    
                    mTvUername.Text = username;
                    mTvFullname.Text = fullName;
                    byte[] picData = pic;
                    if (picData != null)
                    {
                        Bitmap bmp = BitmapFactory.DecodeByteArray(picData, 0, picData.Length);
                        mProfilePic.SetImageBitmap(bmp);
                    }
                    else
                    {
                        mProfilePic.SetImageResource(Resource.Drawable.default_pic);
                    }

                    //Alter navigation items
                    navLogin.SetVisible(false);
                    navSignUp.SetVisible(false);
                    navProfile.SetVisible(true);
                    navLogout.SetVisible(true);
                    navReports.SetVisible(true);
                    navFeedback.SetVisible(true);
                    navLeaderBoard.SetVisible(true);
                    navAchievements.SetVisible(true);
                });
            }
            else
            {
                this.mUsername = String.Empty;
                RunOnUiThread(() =>
                {
                    // Execute on UI thread
                    mTvUername.Text = username;
                    mTvFullname.Text = fullName;
                    byte[] picData = pic;
                    mProfilePic.SetImageResource(Resource.Drawable.default_pic);

                    // Alter navigation items
                    navLogin.SetVisible(true);
                    navSignUp.SetVisible(true);
                    navProfile.SetVisible(false);
                    navLogout.SetVisible(false);
                    navReports.SetVisible(false);
                    navFeedback.SetVisible(false);
                    navLeaderBoard.SetVisible(false);
                    navAchievements.SetVisible(false);
                });
            }
        }

        /// <summary>
        /// Handle navigation to SignUp Activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogFrag_OnSignUpNavComplete(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
        }

        /// <summary>
        /// Handle navigation to Login Activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogFrag_OnLoginNavComplete(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
        }

        /// <summary>
        /// Handles the Timer Elipsed event of statusTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            mTimeInSeconds++;
            if (mMode == "Competitive")
            {
                mSec++;
                if (mSec == 60)
                {
                    mSec = 0;
                    mMin++;
                }

                mTime = $"{ mMin.ToString("00") }:{ mSec.ToString("00") }";
                RunOnUiThread(() => StatusCardFrontFragment.TimeControl(mTime));

                // Basic validation
                if (mMin == 59 && mSec == 59)
                {
                    mStatusTimer.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Method for flipping status card fragments
        /// </summary>
        public void FlipCard()
        {
            if (sIsFixed)
            {
                // Check whether the puzzle is fixed
                if (mShowingBack)
                {
                    // Pop back to the last fragment on the stack (the front side fragment)
                    FragmentManager.PopBackStack();
                    mShowingBack = false;

                    // Set a timer to delay the execution til the flip is triggered
                    Timer delayedTimer = new Timer
                    {
                        Interval = 1,
                        Enabled = true
                    };
                    delayedTimer.Start();

                    delayedTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                    {
                        delayedTimer.Stop();
                        RunOnUiThread(() =>
                        {
                            // Execute on UI thread
                            if (mStatus != "Paused")
                            {
                                StatusCardFrontFragment.StatusVisibility(true);
                            }
                            StatusCardFrontFragment.StatusControl(mLvl, mCurrentMode, mStatus);
                            StatusCardFrontFragment.SwapsControl(mSwaps);
                            StatusCardFrontFragment.TimeControl(mTime);
                        });
                        // Dispose time since it will no longer be used.
                        delayedTimer.Dispose();
                    };
                }
                else
                {
                    // The front is showing, therefore flip to back
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();

                    // Set the custom animations
                    transaction.SetCustomAnimations(Resource.Animation.card_flip_right_in, Resource.Animation.card_flip_right_out,
                                                    Resource.Animation.card_flip_left_in, Resource.Animation.card_flip_left_out);

                    // Replace the fragment
                    transaction.Replace(Resource.Id.container, new StatusCardBackFragment());
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                    mShowingBack = true;

                    // Set a timer to delay the execution til the flip is triggered
                    Timer delayedTimer = new Timer
                    {
                        Interval = 1,
                        Enabled = true
                    };
                    delayedTimer.Start();

                    delayedTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                    {
                        delayedTimer.Stop();
                        RunOnUiThread(() =>
                        {
                            // Execute on UI thread
                            StatusCardBackFragment.LvlControl(sCurrentLvl, mTimeInSeconds, mSwaps, mMode);
                        });
                        // Dispose time since it will no longer be used.
                        delayedTimer.Dispose();
                    };
                }
            }
        }

        /// <summary>
        /// Handles BtnBrowse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            // Create an intent for browsing images
            using (Intent intent = new Intent())
            {
                intent.SetType("image/*"); // Set file type to only images
                intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(intent, "Select an Image"), LOAD_IMG_REQUEST);
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

            switch (requestCode)
            {
                // Getting image
                case (LOAD_IMG_REQUEST):
                    {
                        if (resultCode == Result.Ok)
                        {
                            LoadImage(data, null);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Method for loading the image asynchronously
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        private async void LoadImage(Intent data, string url)
        {
            mPuzzlePanel.RemoveView(mPicBoxWhole);
            mLayoutMessage.Visibility = ViewStates.Gone;
            mLayoutProgress.Visibility = ViewStates.Visible;
            sTvProgress.Text = "0%";
            GC.Collect(); // Must call Garbage Collector

            try
            {
                // Call BitmapMaker to draw the selected image into a resampled bitmap
                if (url == null)
                {
                    mImage = await BitmapMaker.CreateBitmapImage(640, 360, mPuzzlePanel.Width, mPuzzlePanel.Height - 2, data, this);
                    sCurrentLvl = LVL_1_NUM;
                    mBtnPlay.Text = "Play";
                    Drawable img = ContextCompat.GetDrawable(this, Resource.Drawable.ic_play_white);
                    mBtnPlay.SetCompoundDrawablesWithIntrinsicBounds(img, null, null, null); // Set button icon
                }
                else
                {
                    // Check for network availability
                    if (ConnectionChecker.IsNetworkAvailable(this))
                    {
                        Bitmap result = await BitmapMaker.CreateBitmapImage(640, 360, mPuzzlePanel.Width, mPuzzlePanel.Height - 2, url, sTvProgress, this);
                        if (result != null)
                        {
                            mImage = result;
                            sCurrentLvl = LVL_1_NUM;
                            mBtnPlay.Text = "Play";
                            Drawable img = ContextCompat.GetDrawable(this, Resource.Drawable.ic_play_white);
                            mBtnPlay.SetCompoundDrawablesWithIntrinsicBounds(img, null, null, null); // Set button icon
                        }
                        else
                        {
                            string error = "Couldn't load the image. Please check the URL.";
                            AlertGenerator.ShowError(error, this);
                        }
                    }
                    else
                    {
                        string error = "Device is not connected to a network.";
                        AlertGenerator.ShowError(error, this);
                    }
                }
            }
            catch
            {
                string error = "There was a problem trying to load the image.";
                AlertGenerator.ShowError(error, this);
            }

            mPuzzlePanel.AddView(mPicBoxWhole); // Add picBoxWhole to puzzlePanel
            mPicBoxWhole.SetImageBitmap(mImage); // Set the bitmap into the picBoxWhole imageView           

            mLayoutProgress.Visibility = ViewStates.Gone;
            mLayoutMessage.Visibility = ViewStates.Visible;

            if (mImage != null)
            {
                // Make btnPlay clickable
                mBtnPlay.Enabled = true;
                mBtnPlay.SetBackgroundResource(Resource.Drawable.btn_accent_style);
            }
        }

        /// <summary>
        /// Handles BtnPlay click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtnPlay_Click(object sender, EventArgs e)
        {
            PuzzlePlayer player = new PuzzlePlayer(mPuzzlePanel, sCurrentLvl);

            if (mBtnPlay.Text == "Play")
            {
                #region Play button click event
                player.Play(mImage, this);
                if (sCurrentLvl == LVL_1_NUM)
                    mLvl = "Level 1";
                else if (sCurrentLvl == LVL_2_NUM)
                    mLvl = "Level 2";
                else if (sCurrentLvl == LVL_3_NUM)
                    mLvl = "Level 3";
                else
                    mLvl = "Level 4";
                mStatus = "In-Game";
                mBtnPlay.Text = "Pause";
                Drawable img = ContextCompat.GetDrawable(this, Resource.Drawable.ic_pause_white);
                mBtnPlay.SetCompoundDrawablesWithIntrinsicBounds(img, null, null, null); // Set button icon
                mBtnBrowse.Enabled = false;
                mBtnBrowse.SetBackgroundResource(Resource.Drawable.btn_disabled_style);
                mStatusTimer.Enabled = true; // Start the status timer
                if (mMode == "Competitive")
                {
                    mSwaps = 0;
                    mMin = mSec = mTimeInSeconds = 0;
                    mTime = "00:00";
                }
                else
                {
                    mSwaps = -1;
                }
                StatusCardFrontFragment.StatusVisibility(true);
                mDrawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed); // Lock the navigation drawer
                #endregion
            }
            else if (mBtnPlay.Text == "Pause")
            {
                #region Pause button click event
                mPuzzlePanel.RemoveAllViews();
                mPuzzlePanel.AddView(mLayoutProgress);
                mPuzzlePanel.AddView(mLayoutMessage);
                mPuzzlePanel.AddView(mPicBoxWhole);
                mStatus = "Paused";
                mBtnPlay.Text = "Retry";
                Drawable img = ContextCompat.GetDrawable(this, Resource.Drawable.ic_replay_white);
                mBtnPlay.SetCompoundDrawablesWithIntrinsicBounds(img, null, null, null); // Set button icon
                mBtnBrowse.Enabled = true;
                mBtnBrowse.SetBackgroundResource(Resource.Drawable.btn_orange_style);
                if (mShowingBack)
                {
                    FlipCard();
                }
                sIsFixed = false;
                mStatusTimer.Enabled = false;
                StatusCardFrontFragment.StatusVisibility(false);
                mDrawer.SetDrawerLockMode(DrawerLayout.LockModeUnlocked); // Unlock the navigation drawer
                #endregion
            }
            else if (mBtnPlay.Text == "Retry")
            {
                #region Retry button click event
                player.Play(mImage, this);
                if (sCurrentLvl == LVL_1_NUM)
                    mLvl = "Level 1";
                else if (sCurrentLvl == LVL_2_NUM)
                    mLvl = "Level 2";
                else if (sCurrentLvl == LVL_3_NUM)
                    mLvl = "Level 3";
                else
                    mLvl = "Level 4";
                mStatus = "In-Game";
                mBtnPlay.Text = "Pause";
                Drawable img = ContextCompat.GetDrawable(this, Resource.Drawable.ic_pause_white);
                mBtnPlay.SetCompoundDrawablesWithIntrinsicBounds(img, null, null, null); // Set button icon
                mBtnBrowse.Enabled = false;
                mBtnBrowse.SetBackgroundResource(Resource.Drawable.btn_disabled_style);
                mStatusTimer.Enabled = true; // Start the status timer
                if (mMode == "Competitive")
                {
                    mSwaps = 0;
                    mMin = mSec = mTimeInSeconds = 0;
                    mTime = "00:00";
                }
                else
                {
                    mSwaps = -1;
                }
                StatusCardFrontFragment.StatusVisibility(true);
                mDrawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed); // Lock the navigation drawer
                #endregion
            }

            mCurrentMode = mMode;
            StatusCardFrontFragment.StatusControl(mLvl, mCurrentMode, mStatus);
            StatusCardFrontFragment.SwapsControl(mSwaps);
            StatusCardFrontFragment.TimeControl(mTime);
        }

        /// <summary>
        /// Method for handling common code for playing levels
        /// </summary>
        private void PlayLevels()
        {
            PuzzlePlayer player = new PuzzlePlayer(mPuzzlePanel, sCurrentLvl);
            player.Play(mImage, this);
            mStatus = "In-Game";
            mBtnPlay.Text = "Pause";
            mBtnBrowse.Enabled = false;
            FlipCard();
            sIsFixed = false;
            mCurrentMode = mMode;

            if (mMode == "Competitive")
            {
                mSwaps = 0;
                mTime = "00:00";
            }
            else
            {
                mSwaps = -1;
                mTime = "∞";
            }

            if (mMode == "Competitive")
            {
                mMin = mSec = mTimeInSeconds = 0;
                mTime = "00:00";
            }
            mStatusTimer.Enabled = true; // Start the status timer
            StatusCardFrontFragment.TimeControl(mTime);
            mDrawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed); // Lock the navigation drawer
        }

        /// <summary>
        /// Method for setting up the next level
        /// </summary>
        public void NextLvl()
        {
            if (sCurrentLvl == LVL_1_NUM)
            {
                sCurrentLvl = LVL_2_NUM;
                mLvl = "Level 2";
            }
            else if (sCurrentLvl == LVL_2_NUM)
            {
                sCurrentLvl = LVL_3_NUM;
                mLvl = "Level 3";
            }
            else if (sCurrentLvl == LVL_3_NUM)
            {
                sCurrentLvl = LVL_4_NUM;
                mLvl = "Level 4";
            }
            PlayLevels();
        }

        /// <summary>
        /// Method for setting up the current level for replay
        /// </summary>
        public void ReplayLvl()
        {
            PlayLevels();
        }

        /// <summary>
        /// Method for setting up the previous level
        /// </summary>
        public void PrevLvl()
        {
            if (sCurrentLvl == LVL_2_NUM)
            {
                sCurrentLvl = LVL_1_NUM;
                mLvl = "Level 1";
            }
            else if (sCurrentLvl == LVL_3_NUM)
            {
                sCurrentLvl = LVL_2_NUM;
                mLvl = "Level 2";
            }
            else if (sCurrentLvl == LVL_4_NUM)
            {
                sCurrentLvl = LVL_3_NUM;
                mLvl = "Level 3";
            }
            PlayLevels();
        }

        /// <summary>
        /// Triggers when a two puzzle pieces have swited positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageSwitcher_OnSwapComplete(object sender, EventArgs e)
        {
            if (mCurrentMode == "Competitive")
            {
                mSwaps += 1;
                StatusCardFrontFragment.SwapsControl(mSwaps);
            }
        }

        /// <summary>
        /// Triggers when a puzzle is successfully fixed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageSwitcher_OnPuzzleComplete(object sender, EventArgs e)
        {
            sIsFixed = true;
            mStatus = "Finished";
            mStatusTimer.Enabled = false;
            mDrawer.SetDrawerLockMode(DrawerLayout.LockModeUnlocked); // Unlock the navigation drawer
            FlipCard();
            UpdatePlayerStats(ScoreCalculator.Calculate(sCurrentLvl, mSwaps, mTimeInSeconds, mMode));
        }

        /// <summary>
        /// Method for updating player stats and achievements once a puzzle is fixed
        /// </summary>
        /// <param name="scores"></param>
        private void UpdatePlayerStats(PlayerStats scores)
        {
            if (mUsername != String.Empty)
            {
                Task.Factory.StartNew(() =>
                {
                    PlayerStats stats = new PlayerStats();

                    #region Update Player Stats
                    if (DbConnector.OpenSQLConnection())
                    {
                        // Connection opened
                        PlayerStatsDA statsDA = new PlayerStatsDA();
                        if (statsDA.IsStatsExist(mUsername))
                        {
                            // Stats exist, therefore update                            
                            stats = statsDA.FindStats(mUsername);
                            stats.XP += scores.XP;
                            stats.SR += scores.SR;
                            stats.PlayTime += TimeSpan.FromSeconds(mTimeInSeconds);
                            stats.Golds += scores.Golds;
                            stats.Silvers += scores.Silvers;
                            stats.Bronzes += scores.Bronzes;

                            DbConnector.CloseSQLConnection(); // Close connection to the database
                            if (DbConnector.OpenSQLConnection())
                            {
                                if (!statsDA.UpdateStats(stats))
                                {
                                    string error = "There was a problem updating your stats.";
                                    RunOnUiThread(() =>
                                    {
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
                                    AlertGenerator.ShowError(error, this);
                                });
                            }
                        }
                        else
                        {
                            // Stats does not exist, therefore insert
                            stats.Username = mUsername;
                            stats.XP = scores.XP;
                            stats.SR = scores.SR;
                            stats.PlayTime = TimeSpan.FromSeconds(mTimeInSeconds);
                            stats.Golds = scores.Golds;
                            stats.Silvers = scores.Silvers;
                            stats.Bronzes = scores.Bronzes;

                            if (!statsDA.InsertStats(stats))
                            {
                                string error = "There was a problem updating your stats.";
                                RunOnUiThread(() =>
                                {
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
                            AlertGenerator.ShowError(error, this);
                        });
                    }

                    DbConnector.CloseSQLConnection(); // Close connection to the database
                    #endregion

                    #region Update Achievements
                    if (DbConnector.OpenSQLConnection())
                    {
                        // Connection opened
                        AchievementsDA achievementsDA = new AchievementsDA();
                        List<AchievementsCompleted> achievementsCompleted = new List<AchievementsCompleted>();
                        achievementsCompleted = achievementsDA.GetAllAchievements(mUsername);
                        DbConnector.CloseSQLConnection(); // Close connection to the database

                        if (achievementsCompleted != null)
                        {
                            // Achievements exist, therefore update
                            achievementsCompleted = AchievementsTracker.UpdateAchievements(achievementsCompleted, mUsername, stats, mCurrentMode, sCurrentLvl);
                            for (int i = 0; i < 19; i++)
                            {
                                if (achievementsCompleted[i].Title != String.Empty)
                                {
                                    string title = achievementsCompleted[i].Title;
                                    string description = achievementsCompleted[i].Description;
                                    string image = achievementsCompleted[i].Image;

                                    // Pull up Achievement dialog
                                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                                    AchDialog dialogFrag = new AchDialog(title, description, image);
                                    dialogFrag.Show(transaction, "dialog fragment");
                                }
                            }
                            if (DbConnector.OpenSQLConnection())
                            {
                                if (!achievementsDA.UpdateAchievements(achievementsCompleted))
                                {
                                    string error = "There was a problem updating your stats.";
                                    RunOnUiThread(() =>
                                    {
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
                                    AlertGenerator.ShowError(error, this);
                                });
                            }
                        }
                        else
                        {
                            // Achievements do not exist, therefore insert
                            achievementsCompleted = AchievementsTracker.InsertAchievements(achievementsCompleted, mUsername, mCurrentMode);

                            if (DbConnector.OpenSQLConnection())
                            {
                                if (!achievementsDA.InsertAchievements(achievementsCompleted))
                                {
                                    string error = "There was a problem updating your stats.";
                                    RunOnUiThread(() =>
                                    {
                                        AlertGenerator.ShowError(error, this);
                                    });
                                }
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
                            AlertGenerator.ShowError(error, this);
                        });
                    }

                    DbConnector.CloseSQLConnection(); // Close connection to the database
                    #endregion
                });
            }
        }

        /// <summary>
        /// Handles post deletion event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyEventHandler_OnDeleteComplete(object sender, EventArgs e)
        {
            Logout();
        }

        /// <summary>
        /// Method for handling logout function
        /// </summary>
        private void Logout()
        {
            // Clear shared preferences
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.Remove("Username");
            edit.Remove("Password");
            edit.Apply();

            // Pull up Front Panel dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            mDialogFrag.Show(transaction, "dialog fragment");

            // Alter profile information on nav header
            Login("Player", "Player One", null, false);
        }

        /// <summary>
        /// Override OnBackPressed function
        /// </summary>
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                // Close the drawer if it's already opened
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        /// <summary>
        /// Override OnCreateOptionsMenu function
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the MenuInflater using the XML resource
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        /// <summary>
        /// Override OnOptionsItemSelected function
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                // Clicked on Settings
                return true;
            }
            else if (id == Resource.Id.image_from_url)
            {
                // Clicked on ImageSearch 
                using (View searchView = item.ActionView)
                {
                    using (var _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
                    {
                        _searchView.QueryHint = "Add image from url..."; // Set SearchView Hint (Placeholder) text
                        _searchView.MaxWidth = int.MaxValue; // Set the SearchView max width

                        // Get the value of the SearchView
                        _searchView.QueryTextSubmit += (s, e) =>
                        {
                            if (mBtnBrowse.Enabled)
                            {
                                LoadImage(null, e.Query);
                            }
                            else
                            {
                                string error = "Please pause the current puzzle before adding a new image.";
                                AlertGenerator.ShowError(error, this);
                            }
                            e.Handled = true;
                            mBtnPlay.RequestFocus(); // Change focus

                            // Close the keyboard
                            View view = CurrentFocus;
                            if (view != null)
                            {
                                KeyboardManager.CloseKeyboard(view, this); // Closing the keyboard on focus change
                            }
                        };
                    }
                }
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Handles the navigation item selected event
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_casual)
            {
                // Select casual mode
                mMode = "Casual";
            }
            else if (id == Resource.Id.nav_comp)
            {
                // Select comp mode
                mMode = "Competitive";
            }
            else if (id == Resource.Id.nav_login)
            {
                // Navigate to the Login activity
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_signup)
            {
                // Navigate to the SignUp activity
                Intent intent = new Intent(this, typeof(SignUpActivity));
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_leaderboard)
            {
                // Navigate to the Leaderboard activity
                Intent intent = new Intent(this, typeof(LeaderboardActivity));
                intent.PutExtra("Username", mUsername);
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_achievements)
            {
                // Navigate to the Achievements activity
                Intent intent = new Intent(this, typeof(AchievementsActivity));
                intent.PutExtra("Username", mUsername);
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_profile)
            {
                // Navigate to the Profile activity
                Intent intent = new Intent(this, typeof(ProfileActivity));
                intent.PutExtra("Username", mUsername);
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_logout)
            {
                Logout();
            }
            else if (id == Resource.Id.nav_reports)
            {
                // Navigate to the Feedback activity
                Intent intent = new Intent(this, typeof(ReportsActivity));
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_feedback)
            {
                // Navigate to the Feedback activity
                Intent intent = new Intent(this, typeof(FeedbackActivity));
                intent.PutExtra("Username", mUsername);
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
            }
            else if (id == Resource.Id.nav_about)
            {
                // Pull up About dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                AboutDialog dialogFrag = new AboutDialog();
                dialogFrag.Show(transaction, "dialog fragment");
            }

            // Close out the navigation drawer
            mDrawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

