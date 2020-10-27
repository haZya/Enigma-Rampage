using Android.Content;
using Android.Util;
using Android.Widget;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Custom ImageView for indexing puzzle pieces
    /// </summary>
    public class MyImageView : ImageView
    {
        public MyImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            
        }

        /// <summary>
        /// Holds imageView index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Holds image index
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        /// Holds value for matching indexes
        /// </summary>
        /// <returns></returns>
        public bool IsMatch()
        {
            return (Index == ImageIndex);
        }
    }
}