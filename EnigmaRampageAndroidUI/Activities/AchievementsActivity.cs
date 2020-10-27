using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;
using EnigmaRampageAndroidUI.Adapters;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Activities
{
    /// <summary>
    /// Contains the events of Achievements activity
    /// </summary>
    [Activity(Label = "Achievements", Theme = "@style/AppTheme")]
    public class AchievementsActivity : AppCompatActivity
    {
        private ProgressBar mProgressBar;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<AchievementsCompleted> mAchievementsCompl;

        /// <summary>
        /// Override OnCreate method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_achievements);

            // Set the activity back button on the actionbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            #region Initialization
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.achProgressBar);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            AchievementsDA achievementsDA = new AchievementsDA();
           
            Task.Factory.StartNew(() =>
            {
                RunOnUiThread(() => mProgressBar.Visibility = ViewStates.Visible);
                if (DbConnector.OpenSQLConnection())
                {
                    mAchievementsCompl = achievementsDA.GetAllAchievements(Intent.GetStringExtra("Username"));
                }
                DbConnector.CloseSQLConnection();

                // Handle columns on orientation change
                if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                {
                    mLayoutManager = new GridLayoutManager(this, 1);
                }
                else
                {
                    mLayoutManager = new GridLayoutManager(this, 2);
                }

                // Load achievements into CardViews
                RunOnUiThread(() =>
                {
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mAdapter = new RecyclerAdapter(mAchievementsCompl, this);
                    mRecyclerView.SetAdapter(mAdapter);
                    mProgressBar.Visibility = ViewStates.Gone;
                });
            });
            #endregion
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