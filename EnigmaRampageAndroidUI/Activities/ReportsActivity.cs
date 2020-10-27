using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EnigmaRampageAndroidUI.Utils;

namespace EnigmaRampageAndroidUI.Activities
{
    [Activity(Label = "Reports", Theme = "@style/AppTheme.NoActionBar")]
    public class ReportsActivity : AppCompatActivity
    {
        private ProgressBar mProgressBar;
        private TabLayout mTabLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reports);

            // Set-up the custom toolbar as the action bar
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.reportToolbar);
            SetSupportActionBar(toolbar); // Set toolbar as actionBar

            // Set the activity back button on the toolbar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Initialization
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.reportsProgressBar);
            mTabLayout = FindViewById<TabLayout>(Resource.Id.reportTablayout);
            TabItem tabAge = FindViewById<TabItem>(Resource.Id.reportTabAge);
            TabItem tabPlayTime = FindViewById<TabItem>(Resource.Id.reportTabPlayTime);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            // Set up the lablayout
            Adapters.PagerAdapter pagerAdapter = new Adapters.PagerAdapter(SupportFragmentManager, mTabLayout.TabCount);
            viewPager.Adapter = pagerAdapter;
            mTabLayout.AddOnTabSelectedListener(new TabLayout.ViewPagerOnTabSelectedListener(viewPager));
            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(mTabLayout));

            // Set icons to the tabs
            mTabLayout.GetTabAt(0).SetIcon(Resource.Drawable.ic_age_white);
            mTabLayout.GetTabAt(1).SetIcon(Resource.Drawable.ic_playtime_white);

            // Alter the color of the icons
            mTabLayout.GetTabAt(1).Icon.SetColorFilter(Color.Rgb(224, 224, 224), PorterDuff.Mode.SrcIn);

            // Change icon color on tab selected
            mTabLayout.TabSelected += (o, e) => e.Tab.Icon.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);

            // Change icon color on tab unselected
            mTabLayout.TabUnselected += (obj, ev) => ev.Tab.Icon.SetColorFilter(Color.Rgb(224, 224, 224), PorterDuff.Mode.SrcIn);

            RunOnUiThread(() =>
            {
                mProgressBar.Visibility = ViewStates.Visible;
                ChartDataRetriever.RetrieveData(this);
                mProgressBar.Visibility = ViewStates.Gone;
            });
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