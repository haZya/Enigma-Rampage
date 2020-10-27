using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handle the behaviour of the soft onscreen keyboard
    /// </summary>
    public static class KeyboardManager
    {
        /// <summary>
        /// Method for closing the onscreen keyboard
        /// </summary>
        /// <param name="view"></param>
        /// <param name="context"></param>
        public static void CloseKeyboard(View view, Context context)
        {          
            InputMethodManager imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
        }
    }
}