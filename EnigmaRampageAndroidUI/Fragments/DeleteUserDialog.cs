using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace EnigmaRampageAndroidUI.Fragments
{
    /// <summary>
    /// Contains the events of DeleteUser DialogFragment
    /// </summary>
    public class DeleteUserDialog : DialogFragment
    {
        /// <summary>
        /// Custom EventArgs nested class for holding password entered
        /// </summary>
        public class OnDeleteEventArgs : EventArgs
        {
            public string Password { get; set; }

            /// <summary>
            /// Initialization
            /// </summary>
            /// <param name="password"></param>
            public OnDeleteEventArgs(string password) : base()
            {
                Password = password;
            }
        }

        private TextInputEditText mTxtPwd;
        private Button mBtnDel;

        public event EventHandler<OnDeleteEventArgs> OnDeleteComplete;

        /// <summary>
        /// Override OnCreateView method
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.dialog_delete_user, container, false);
            mTxtPwd = view.FindViewById<TextInputEditText>(Resource.Id.diaDelTxtPwd);
            mBtnDel = view.FindViewById<Button>(Resource.Id.diaDelBtnDel);

            mBtnDel.Click += (o, e) =>
            {
                // User clicked on Delete button
                OnDeleteComplete.Invoke(this, new OnDeleteEventArgs(mTxtPwd.Text));
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