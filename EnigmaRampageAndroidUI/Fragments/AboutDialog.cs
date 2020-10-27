using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of About DialogFragment
    /// </summary>
    public class AboutDialog : DialogFragment
    {
        private Button mTtnClose;

        /// <summary>
        /// Override OnCreateView method
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.dialog_about, container, false);
            mTtnClose = view.FindViewById<Button>(Resource.Id.diaAboutBtnClose);

            mTtnClose.Click += (o, e) =>
            {
                // User clicked on Close button
                Dismiss();
            };

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