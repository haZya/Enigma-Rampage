using System;
using System.Timers;
using Android.Content;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles triggering of events
    /// </summary>
    public static class MyEventHandler
    {
        /// <summary>
        /// Custom EventArgs nested class for holding data
        /// </summary>
        public class OnLoginEventArgs : EventArgs
        {
            public string Username { get; set; }
            public string FullName { get; set; }
            public byte[] Pic { get; set; }
            public bool IsLogged { get; set; }

            /// <summary>
            /// Initialization
            /// </summary>
            /// <param name="username"></param>
            /// <param name="fullName"></param>
            /// <param name="pic"></param>
            /// <param name="isLogged"></param>
            public OnLoginEventArgs(string username, string fullName, byte[] pic, bool isLogged) : base()
            {
                Username = username;
                FullName = fullName;
                Pic = pic;
                IsLogged = isLogged;
            }
        }

        public static event EventHandler<OnLoginEventArgs> OnLoginComplete;
        public static event EventHandler OnDeleteComplete;

        /// <summary>
        /// Method for invoking login event
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="firstname"></param>
        /// <param name="pic"></param>
        /// <param name="isLogged"></param>
        public static void Trigger(Context context, string username, string firstname, byte[] pic, bool isLogged)
        {
            // Set a timer to delay the execution til the flip is triggered
            Timer delayedTimer = new Timer
            {
                Interval = 1500,
                Enabled = true
            };
            delayedTimer.Start();

            delayedTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                delayedTimer.Stop();
                OnLoginComplete.Invoke(context, new OnLoginEventArgs(username, firstname, pic, isLogged));
                // Dispose time since it will no longer be used.
                delayedTimer.Dispose();
            };
        }

        /// <summary>
        /// Method for invoking delete user event
        /// </summary>
        public static void Trigger(Context context)
        {
            // Set a timer to delay the execution til the flip is triggered
            Timer delayedTimer = new Timer
            {
                Interval = 1,
                Enabled = true
            };
            delayedTimer.Start();

            delayedTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                delayedTimer.Stop();
                OnDeleteComplete.Invoke(context, new EventArgs());
                // Dispose time since it will no longer be used.
                delayedTimer.Dispose();
            };
        }
    }
}