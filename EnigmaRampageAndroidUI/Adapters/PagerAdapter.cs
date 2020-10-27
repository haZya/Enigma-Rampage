using Android.Support.V4.App;
using EnigmaRampageAndroidUI.Fragments;

namespace EnigmaRampageAndroidUI.Adapters
{
    /// <summary>
    /// Contains the methods of Pager Adapter
    /// </summary>
    public class PagerAdapter : FragmentPagerAdapter
    {
        private readonly int mNumOfTabs;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="numOfTabs"></param>
        public PagerAdapter(FragmentManager fm, int numOfTabs) : base(fm)
        {
            mNumOfTabs = numOfTabs;
        }

        /// <summary>
        /// Return the number of tabs
        /// </summary>
        public override int Count
        {
            get
            {
                return mNumOfTabs;
            }
        }

        /// <summary>
        /// Override GetItem method
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return new AgeTabFragment();
                case 1:
                    return new PlayTimeTabFragment();
                default:
                    return null;
            }
        }
    }
}