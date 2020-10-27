using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;

namespace EnigmaRampageAndroidUI.Fragments
{
    public class AchDialog : DialogFragment
    {
        private ImageViewAsync mAchImg;
        private ImageView mAchStatusImg;
        private TextView mTvTtle, mTvDescription, mTvStatus;
        private readonly string mTitle, mDescription, mImage;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public AchDialog(string title, string description, string image)
        {
            mTitle = title;
            mDescription = description;
            mImage = image;
        }

        /// <summary>
        /// Override OnCreateView method
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Set dialog fragment gravity top center
            Dialog.Window.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.Top);
            WindowManagerLayoutParams p = Dialog.Window.Attributes;
            p.SoftInputMode = SoftInput.StateAlwaysVisible;
            Dialog.Window.Attributes = p;

            View view = inflater.Inflate(Resource.Layout.card_view_ach_gold, container, false);
            mAchImg = view.FindViewById<ImageViewAsync>(Resource.Id.achGoldImg);
            mTvTtle = view.FindViewById<TextView>(Resource.Id.tvGoldTitle);
            mTvDescription = view.FindViewById<TextView>(Resource.Id.tvGoldDescription);
            mTvStatus = view.FindViewById<TextView>(Resource.Id.tvAchGoldStatus);
            mAchStatusImg = view.FindViewById<ImageView>(Resource.Id.imgAchGoldStatus);
            
            mTvTtle.SetTextColor(Color.Rgb(52, 152, 219));
            mTvDescription.SetTextColor(Color.Rgb(247, 139, 31));
            mTvStatus.SetTextColor(Color.Rgb(238, 26, 64));
            mAchStatusImg.SetImageResource(Resource.Drawable.ic_achieved_green);

            if (mImage == "Bronze")
            {
                ImageService.Instance.LoadCompiledResource("bronze_medal").Into(mAchImg);
            }
            else if (mImage == "Silver")
            {
                ImageService.Instance.LoadCompiledResource("silver_medal").Into(mAchImg);
            }
            else
            {
                ImageService.Instance.LoadCompiledResource("gold_medal").Into(mAchImg);
            }
            mTvTtle.Text = mTitle;
            mTvDescription.Text = mDescription;
            mTvStatus.Text = "Completed";
            return view;
        }

        /// <summary>
        /// Override OnActivityCreated
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); // Set the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAnimation; // Set the animation
        }
    }
}