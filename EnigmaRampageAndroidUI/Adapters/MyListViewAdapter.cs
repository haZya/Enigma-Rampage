using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Adapters
{
    /// <summary>
    /// Custom ListViewAdapter for handling the Leaderboard
    /// </summary>
    public class MyListViewAdapter : BaseAdapter<PlayerStats>
    {
        private readonly Context mContext;
        private List<PlayerStats> mItems;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="items"></param>
        public MyListViewAdapter(Context context, List<PlayerStats> items)
        {
            mContext = context;
            mItems = items;
        }

        /// <summary>
        /// Override Count
        /// </summary>
        public override int Count
        {
            get
            {
                try
                {
                    return mItems.Count;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Override GetItemId
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Getting ItemId
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override PlayerStats this[int position]
        {
            get { return mItems[position]; }
        }

        /// <summary>
        /// Override GetView
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_leaderboard, null, false);
            }

            #region Initializing the ListView
            TextView tvRank = row.FindViewById<TextView>(Resource.Id.lbTvRank);
            tvRank.Text = mItems[position].Rank.ToString();
            tvRank.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvUsername = row.FindViewById<TextView>(Resource.Id.lbTvUsername);
            tvUsername.Text = mItems[position].Username;
            tvUsername.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvLevel = row.FindViewById<TextView>(Resource.Id.lbTvLvl);
            tvLevel.Text = mItems[position].Level.ToString();
            tvLevel.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvXP = row.FindViewById<TextView>(Resource.Id.lbTvXP);
            tvXP.Text = mItems[position].XP.ToString();
            tvXP.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvSR = row.FindViewById<TextView>(Resource.Id.lbTvSR);
            tvSR.Text = mItems[position].SR.ToString();
            tvSR.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvPlayTime = row.FindViewById<TextView>(Resource.Id.lbTvPlayTime);
            tvPlayTime.Text = mItems[position].PlayTime.ToString();
            tvPlayTime.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvGolds = row.FindViewById<TextView>(Resource.Id.lbTvGolds);
            tvGolds.Text = mItems[position].Golds.ToString();
            tvGolds.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvSilvers = row.FindViewById<TextView>(Resource.Id.lbTvSilvers);
            tvSilvers.Text = mItems[position].Silvers.ToString();
            tvSilvers.SetTextColor(Color.ParseColor("#3498db"));

            TextView tvBronzes = row.FindViewById<TextView>(Resource.Id.lbTvBronzes);
            tvBronzes.Text = mItems[position].Bronzes.ToString();
            tvBronzes.SetTextColor(Color.ParseColor("#3498db"));
            #endregion

            return row;
        }
    }
}