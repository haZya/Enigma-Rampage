using Android.Content;
using Android.Net;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Performs network connection checking
    /// </summary>
    public static class ConnectionChecker
    {
        /// <summary>
        /// Method for getting the network status
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsNetworkAvailable(Context context)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting;
        }
    }
}