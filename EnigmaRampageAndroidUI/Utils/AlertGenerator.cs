using Android.App;
using Android.Content;

namespace EnigmaRampageAndroidUI.Utils
{
    /// <summary>
    /// Handles dialog alert generation
    /// </summary>
    public static class AlertGenerator
    {
        /// <summary>
        /// Method for showing error message alert
        /// </summary>
        /// <param name="error"></param>
        /// <param name="context"></param>
        public static void ShowError(string error, Context context)
        {
            // Create an alert for showing the error message
            using (AlertDialog.Builder alert = new AlertDialog.Builder(context))
            {
                alert.SetTitle("Error");
                alert.SetMessage(error);
                alert.SetIcon(Resource.Drawable.ic_error_red);
                alert.SetNeutralButton("OK", delegate { alert.Dispose(); });
                try
                {
                    alert.Show();
                }
                catch { }
            }
        }
    }
}