using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of FrontPanel DialogFragment
    /// </summary>
    public class FrontPanelDialog : DialogFragment
    {
        private Button mBtnSignUp, mBtnLogin, mBtnGuest;
        public event EventHandler OnSignUpNavComplete, OnLoginNavComplete;

        /// <summary>
        /// Override OnCreateView method
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.dialog_front_panel, container, false);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.diaFPnlBtnSignUp);
            mBtnLogin = view.FindViewById<Button>(Resource.Id.diaFPnlBtnLogin);
            mBtnGuest = view.FindViewById<Button>(Resource.Id.diaFPnlBtnGuest);

            mBtnSignUp.Click += (o, e) =>
            {
                // User clicked on SignUp button
                OnSignUpNavComplete.Invoke(this, new EventArgs());
                Dismiss();
            };

            mBtnLogin.Click += (o, e) =>
            {
                // User clicked on Login button
                OnLoginNavComplete.Invoke(this, new EventArgs());
                Dismiss();
            };

            mBtnGuest.Click += (o, e) =>
            {
                // User clicked on Guest button
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