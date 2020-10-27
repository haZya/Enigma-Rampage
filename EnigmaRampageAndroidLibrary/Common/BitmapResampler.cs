using Android.Content;
using Android.Graphics;
using Android.Net;
using System.IO;
using System.Threading.Tasks;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles decoding and resampling of bitmaps asynchronously
    /// </summary>
    public static class BitmapResampler
    {
        /// <summary>
        /// Method for decoding and resampling a bitmap from stream asynchronously
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestedWidth"></param>
        /// <param name="requestedHeight"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<Bitmap> DecodeBitmapFromStreamAsync(Uri data, int requestedWidth, int requestedHeight, Context context)
        {
            // Decode with InJustDecodeBounds = true to check dimensions
            Stream stream = context.ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            await BitmapFactory.DecodeStreamAsync(stream, null, options); // Load the image to see its size

            // Calculate InSampleSize
            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            // Decode Bitmap with InSampleSize set
            stream = context.ContentResolver.OpenInputStream(data); // Read again
            options.InJustDecodeBounds = false;
            Bitmap bitmap = await BitmapFactory.DecodeStreamAsync(stream, null, options); // Get the resampled bitmap
            return bitmap;
        }

        /// <summary>
        /// Method for decoding and resampling a bitmap from byte array asynchronously
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestedWidth"></param>
        /// <param name="requestedHeight"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<Bitmap> DecodeBitmapFromStreamAsync(byte[] imageBytes, int requestedWidth, int requestedHeight, Context context)
        {
            // Decode with InJustDecodeBounds = true to check dimensions
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length, options); // Load the image to see its size

            // Calculate InSampleSize
            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            // Decode Bitmap with InSampleSize set
            options.InJustDecodeBounds = false;
            Bitmap bitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length, options); // Get the resampled bitmap
            return bitmap;
        }

        /// <summary>
        /// Method for calculating the InSampleSize value for image resampling
        /// </summary>
        /// <param name="options"></param>
        /// <param name="requestedWidth"></param>
        /// <param name="requestedHeight"></param>
        /// <returns></returns>
        public static int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            // Raw Height and Width of the Image
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > requestedHeight || width > requestedWidth)
            {
                // Image is larger than the optimal
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                // Execute until the height and width becomes what is requested
                while ((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
                {
                    inSampleSize *= 2;
                }
            }
            return inSampleSize;
        }
    }
}