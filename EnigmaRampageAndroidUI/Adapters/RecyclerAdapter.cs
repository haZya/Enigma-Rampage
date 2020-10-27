using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using EnigmaRampageLibrary.Models;

namespace EnigmaRampageAndroidUI.Adapters
{
    /// <summary>
    /// Contains the methods of RecyclerView Adapter
    /// </summary>
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<AchievementsCompleted> mAchievements;
        private readonly Context mContext;
        private int mCurrentPosition;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="achievements"></param>
        public RecyclerAdapter (List<AchievementsCompleted> achievements, Context context)
        {
            mAchievements = achievements;
            mContext = context;
            mCurrentPosition = -1;
        }

        /// <summary>
        /// Override OnCreateViewHolder method
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View card;
            TextView tvTitle;
            TextView tvDescription;
            ImageViewAsync image;
            TextView tvStatus;
            ImageView imgStatus;

            if (viewType == Resource.Layout.card_view_ach_silver)
            {
                // Inflate the view
                card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_ach_bronze, parent, false);

                tvTitle = card.FindViewById<TextView>(Resource.Id.tvBronzeTitle);
                tvDescription = card.FindViewById<TextView>(Resource.Id.tvBronzeDescription);
                image = card.FindViewById<ImageViewAsync>(Resource.Id.achBronzeImg);
                tvStatus = card.FindViewById<TextView>(Resource.Id.tvAchBronzeStatus);
                imgStatus = card.FindViewById<ImageView>(Resource.Id.imgAchBronzeStatus);

                Utils.CardView view = new Utils.CardView(card)
                {
                    Title = tvTitle,
                    Description = tvDescription,
                    Image = image,
                    Status = tvStatus,
                    ImgStatus = imgStatus
                };

                return view;
            }
            else if (viewType == Resource.Layout.card_view_ach_silver)
            {
                // Inflate the view
                card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_ach_silver, parent, false);

                tvTitle = card.FindViewById<TextView>(Resource.Id.tvSilTitle);
                tvDescription = card.FindViewById<TextView>(Resource.Id.tvSilDescription);
                image = card.FindViewById<ImageViewAsync>(Resource.Id.achSilImg);
                tvStatus = card.FindViewById<TextView>(Resource.Id.tvAchSilStatus);
                imgStatus = card.FindViewById<ImageView>(Resource.Id.imgAchSilStatus);

                Utils.CardView view = new Utils.CardView(card)
                {
                    Title = tvTitle,
                    Description = tvDescription,
                    Image = image,
                    Status = tvStatus,
                    ImgStatus = imgStatus
                };

                return view;
            }
            else
            {
                // Inflate the view
                card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_ach_gold, parent, false);

                tvTitle = card.FindViewById<TextView>(Resource.Id.tvGoldTitle);
                tvDescription = card.FindViewById<TextView>(Resource.Id.tvGoldDescription);
                image = card.FindViewById<ImageViewAsync>(Resource.Id.achGoldImg);
                tvStatus = card.FindViewById<TextView>(Resource.Id.tvAchGoldStatus);
                imgStatus = card.FindViewById<ImageView>(Resource.Id.imgAchGoldStatus);

                Utils.CardView view = new Utils.CardView(card)
                {
                    Title = tvTitle,
                    Description = tvDescription,
                    Image = image,
                    Status = tvStatus,
                    ImgStatus = imgStatus
                };

                return view;
            }
        }

        /// <summary>
        /// Override OnBindViewHolder method
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Utils.CardView cardHolder = holder as Utils.CardView;
            if (position == 18)
            {               
                ImageService.Instance.LoadCompiledResource("gold_medal").Into(cardHolder.Image);
            }
            else if (position % 3 == 0)
            {
                ImageService.Instance.LoadCompiledResource("bronze_medal").Into(cardHolder.Image);
            }
            else if (position % 3 == 1)
            {
                ImageService.Instance.LoadCompiledResource("silver_medal").Into(cardHolder.Image);
            }
            else if (position % 3 == 2)
            {
                ImageService.Instance.LoadCompiledResource("gold_medal").Into(cardHolder.Image);
            }

            // Set values to the views
            cardHolder.Title.Text = mAchievements[position].Title;
            cardHolder.Description.Text = mAchievements[position].Description;
            if (mAchievements[position].Status)
            {
                // Achievement completed
                cardHolder.Title.SetTextColor(Color.Rgb(52, 152, 219));
                cardHolder.Description.SetTextColor(Color.Rgb(247, 139, 31));
                cardHolder.Status.SetTextColor(Color.Rgb(238, 26, 64));
                cardHolder.Status.Text = "Completed";
                cardHolder.ImgStatus.SetImageResource(Resource.Drawable.ic_achieved_green);
            }
            else
            {
                // Achievement active
                cardHolder.Title.SetTextColor(Color.Rgb(128, 128, 128));
                cardHolder.Description.SetTextColor(Color.Rgb(128, 128, 128));
                cardHolder.Status.SetTextColor(Color.Rgb(128, 128, 128));
                cardHolder.Status.Text = "Active";
                cardHolder.ImgStatus.SetImageResource(Resource.Drawable.ic_open_green);
            }

            // Set the animation
            int currentAnim;
            if (position % 2 == 0)
            {
                currentAnim = Resource.Animation.card_slide_left_to_right;
            }
            else
            {
                currentAnim = Resource.Animation.card_slide_right_to_left;
            }

            if (position > mCurrentPosition)
            {
                SetAnimation(cardHolder.MainView, currentAnim);
                mCurrentPosition = position;
            }
        }

        /// <summary>
        /// Override GetItemViewType method
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            if (position == 18)
            {
                return Resource.Layout.card_view_ach_gold;
            }
            else if (position % 3 == 0)
            {
                return Resource.Layout.card_view_ach_bronze;
            }
            else if (position % 3 == 1)
            {
                return Resource.Layout.card_view_ach_silver;
            }
            else if (position % 3 == 2)
            {
                return Resource.Layout.card_view_ach_gold;
            }
            return Resource.Layout.card_view_ach_gold;
        }

        /// <summary>
        /// Override ItemCount method
        /// </summary>
        public override int ItemCount
        {
            get
            {
                try
                {
                    return mAchievements.Count;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Method for setting up card view slide up animation
        /// </summary>
        /// <param name="view"></param>
        private void SetAnimation(View view, int currentAnim)
        {
            Animation anim = AnimationUtils.LoadAnimation(mContext, currentAnim);
            view.StartAnimation(anim);
        }
    }
}