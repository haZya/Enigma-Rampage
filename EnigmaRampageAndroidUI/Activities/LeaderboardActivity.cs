using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageAndroidUI.Adapters;
using EnigmaRampageAndroidUI.Utils;
using EnigmaRampageLibrary.Helper;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of Leaderboard Activity
    /// </summary>
    [Activity(Label = "Leaderboard", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class LeaderboardActivity : AppCompatActivity
    {
        private List<PlayerStats> mItems;
        private SwipeRefreshLayout mSwipeLayout;
        private ProgressBar mProgressBar;
        private ListView mListView;
        private MyListViewAdapter mAdapter;
        private TextView mTvRank, mTvUsername, mTvLvl, mTvXP, mTvSR, mTvPlayTime, mTvGolds, mTvSilvers, mTvBronzes;
        private bool mRankASC, mUsernameASC, mLvlASC, mXpASC, mSRASC, mPlayTimeASC, mGoldsASC, mSilversASC, mBronzesASC;
        private Android.Support.V7.Widget.SearchView mSearchView;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_leaderboard);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            mListView = FindViewById<ListView>(Resource.Id.leaderboardListView);

            mItems = new List<PlayerStats>();
            LoadData();

            mAdapter = new MyListViewAdapter(this, mItems);
            mListView.Adapter = mAdapter;
            View header = LayoutInflater.Inflate(Resource.Layout.listview_leaderboard, null, false);
            mListView.AddHeaderView(header);

            #region Initialization            
            mSwipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.leaderboardSwipeLayout);
            mTvRank = FindViewById<TextView>(Resource.Id.lbTvRank);
            mTvUsername = FindViewById<TextView>(Resource.Id.lbTvUsername);
            mTvLvl = FindViewById<TextView>(Resource.Id.lbTvLvl);
            mTvXP = FindViewById<TextView>(Resource.Id.lbTvXP);
            mTvSR = FindViewById<TextView>(Resource.Id.lbTvSR);
            mTvPlayTime = FindViewById<TextView>(Resource.Id.lbTvPlayTime);
            mTvGolds = FindViewById<TextView>(Resource.Id.lbTvGolds);
            mTvSilvers = FindViewById<TextView>(Resource.Id.lbTvSilvers);
            mTvBronzes = FindViewById<TextView>(Resource.Id.lbTvBronzes);

            mTvRank.Click += MTvRank_Click;
            mTvUsername.Click += MTvUsername_Click;
            mTvLvl.Click += MTvLvl_Click;
            mTvXP.Click += MTvXP_Click;
            mTvSR.Click += MTvSR_Click;
            mTvPlayTime.Click += MTvPlayTime_Click;
            mTvGolds.Click += MTvGolds_Click;
            mTvSilvers.Click += MTvSilvers_Click;
            mTvBronzes.Click += MTvBronzes_Click;
            #endregion

            mSwipeLayout.Refresh += (o, e) =>
            {
                LoadData();
                mSwipeLayout.Refreshing = false;
            };
        }

        /// <summary>
        /// Method for loading data into the listview
        /// </summary>
        private void LoadData()
        {
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.leaderboardProgressBar);            

            Task.Factory.StartNew(() =>
            {
                RunOnUiThread(() => mProgressBar.Visibility = ViewStates.Visible);
                if (DbConnector.OpenSQLConnection())
                {
                    // Connection opened
                    PlayerStatsDA statsDA = new PlayerStatsDA();
                    mItems = statsDA.GetAllStats();

                    RunOnUiThread(() =>
                    {
                        // Refresh ListView
                        mAdapter = new MyListViewAdapter(this, mItems);
                        mListView.Adapter = mAdapter;
                    });
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
                RunOnUiThread(() => mProgressBar.Visibility = ViewStates.Gone);
            });                 
        }

        /// <summary>
        /// TvRank TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvRank_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mRankASC)
            {
                sortedItems = (from item in mItems orderby item.Rank select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Rank descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mRankASC = !mRankASC;
        }

        /// <summary>
        /// TvUsername TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvUsername_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mUsernameASC)
            {
                sortedItems = (from item in mItems orderby item.Username select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Username descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mUsernameASC = !mUsernameASC;
        }

        /// <summary>
        /// TvLvl TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvLvl_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mLvlASC)
            {
                sortedItems = (from item in mItems orderby item.Level select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Level descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mLvlASC = !mLvlASC;
        }

        /// <summary>
        /// TvXP TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvXP_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mXpASC)
            {
                sortedItems = (from item in mItems orderby item.XP select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.XP descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mXpASC = !mXpASC;
        }

        /// <summary>
        /// TvSR TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvSR_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mSRASC)
            {
                sortedItems = (from item in mItems orderby item.SR select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.SR descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mSRASC = !mSRASC;
        }

        /// <summary>
        /// TvPlayTime TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvPlayTime_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mPlayTimeASC)
            {
                sortedItems = (from item in mItems orderby item.PlayTime select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.PlayTime descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mPlayTimeASC = !mPlayTimeASC;
        }

        /// <summary>
        /// TvGolds TextView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvGolds_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mGoldsASC)
            {
                sortedItems = (from item in mItems orderby item.Golds select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Golds descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mGoldsASC = !mGoldsASC;
        }

        /// <summary>
        /// TvSilvers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvSilvers_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mSilversASC)
            {
                sortedItems = (from item in mItems orderby item.Silvers select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Silvers descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mSilversASC = !mSilversASC;
        }

        /// <summary>
        /// TvBronze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTvBronzes_Click(object sender, EventArgs e)
        {
            List<PlayerStats> sortedItems;

            if (!mBronzesASC)
            {
                sortedItems = (from item in mItems orderby item.Bronzes select item).ToList();
            }
            else
            {
                sortedItems = (from item in mItems orderby item.Bronzes descending select item).ToList();
            }

            // Refresh ListView
            mAdapter = new MyListViewAdapter(this, sortedItems);
            mListView.Adapter = mAdapter;

            mBronzesASC = !mBronzesASC;
        }

        /// <summary>
        /// Override OnBackPressed method
        /// </summary>
        public override void OnBackPressed()
        {
            Finish(); // Call Finish method when back button is pressed
            OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

            base.OnBackPressed();
        }

        /// <summary>
        /// Override OnCreateOptionsMenu function
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the MenuInflater using the XML resource
            MenuInflater.Inflate(Resource.Menu.menu_leaderboard, menu);
            return true;
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
                case Resource.Id.leaderboardSearch:
                    // Clicked on Search button
                    using (View searchView = item.ActionView)
                    {
                        using (mSearchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
                        {
                            mSearchView.QueryHint = "Search by Username..."; // Set SearchView Hint (Placeholder) text
                            mSearchView.MaxWidth = int.MaxValue; // Set the SearchView max width

                            // Get the value of the SearchView
                            mSearchView.QueryTextChange += (s, e) =>
                            {
                                List<PlayerStats> searchedItems = (from stat in mItems
                                                                   where stat.Username.Contains(e.NewText, StringComparison.OrdinalIgnoreCase)
                                                                   select stat).ToList();

                                // Refresh ListView
                                mAdapter = new MyListViewAdapter(this, searchedItems);
                                mListView.Adapter = mAdapter;

                                e.Handled = true;
                            };
                        }
                    }
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}