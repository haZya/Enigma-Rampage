using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading.Views;

namespace EnigmaRampageAndroidUI.Utils
{
    /// <summary>
    /// Contains the properties of RecyclerView ViewHolder
    /// </summary>
    public class CardView : RecyclerView.ViewHolder
    {
        public View MainView { get; set; }
        public TextView Title { get; set; }
        public TextView Description { get; set; }
        public ImageViewAsync Image { get; set; }
        public TextView Status { get; set; }
        public ImageView ImgStatus { get; set; }

        public CardView(View view) : base(view)
        {
            MainView = view;
        }
    }
}