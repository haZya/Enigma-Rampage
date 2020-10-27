using Android.Content;
using Android.Graphics;
using Android.Widget;
using System.Net;
using System.Threading.Tasks;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles bitmap drawing
    /// </summary>
    public static class BitmapMaker
    {
        /// <summary>
        /// Method for drawing image pieces
        /// </summary>
        /// <param name="image"></param>
        /// <param name="images"></param>
        /// <param name="index"></param>
        /// <param name="numRow"></param>
        /// <param name="numCol"></param>
        /// <param name="unitX"></param>
        /// <param name="unitY"></param>
        public static void CreateBitmapImage(Bitmap image, Bitmap[] images, int index, int numRow, int numCol, int unitX, int unitY)
        {
            if (images[index] == null)
            {
                // Initialize if not already initialized
                images[index] = Bitmap.CreateBitmap(unitX, unitY - 2, Bitmap.Config.Argb8888);
            }

            if (image != null)
            {
                // Draw image piece into the respective bitmap
                using (Canvas can = new Canvas(images[index]))
                {
                    can.DrawColor(Color.Black); // Set the color

                    // Draw image piece according to the bounds
                    can.DrawBitmap(image,
                        new Rect(unitX * (index % numCol), unitY * (index / numCol), unitX * ((index % numCol) + 1), unitY * ((index / numCol) + 1)),
                        new Rect(0, 0, unitX, unitY),
                        null);
                }
            }
        }

        /// <summary>
        /// Method for drawing whole image from Intent data
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<Bitmap> CreateBitmapImage(int reqWidth, int reqHeight, int width, int height, Intent data, Context context)
        {
            Bitmap image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            // Draw image into bitmap
            using (Canvas can = new Canvas(image))
            {
                can.DrawColor(Color.Black); // Set the color

                // Get the image from the stream scaled and resampled 
                Bitmap bmp = Bitmap.CreateScaledBitmap(await BitmapResampler.DecodeBitmapFromStreamAsync(data.Data, reqWidth, reqHeight, context), width, height, true);
                can.DrawBitmap(bmp, new Rect(0, 0, width, height), new Rect(0, 0, width, height), null);
                bmp.Recycle();
            }
            return image;
        }

        /// <summary>
        /// Method for drawing whole image from url asynchronously
        /// </summary>
        /// <param name="reqWidth"></param>
        /// <param name="reqHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        /// <param name="txtProgress"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<Bitmap> CreateBitmapImage(int reqWidth, int reqHeight, int width, int height, string url, TextView txtProgress, Context context)
        {
            Bitmap image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            byte[] imageBytes = null;

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    // Show download progress
                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        txtProgress.Text = e.ProgressPercentage.ToString() + "%";
                    };

                    // Download the image into a byte array                    
                    imageBytes = await webClient.DownloadDataTaskAsync(url);
                }
                catch
                {                    
                    return null;
                }                              
            }

            if (imageBytes != null && imageBytes.Length > 0)
            {
                // Draw image into bitmap
                using (Canvas can = new Canvas(image))
                {
                    can.DrawColor(Color.Black); // Set the color

                    // Get the image from the byte array scaled and resampled 
                    Bitmap bmp = Bitmap.CreateScaledBitmap(await BitmapResampler.DecodeBitmapFromStreamAsync(imageBytes, reqWidth, reqHeight, context), width, height, true);
                    can.DrawBitmap(bmp, new Rect(0, 0, width, height), new Rect(0, 0, width, height), null);
                    bmp.Recycle();
                }
            }
            return image;
        }
    }
}