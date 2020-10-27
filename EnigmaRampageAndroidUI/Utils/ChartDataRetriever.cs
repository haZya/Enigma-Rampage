using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageAndroidLibrary.DataAccess;

namespace EnigmaRampageAndroidUI.Utils
{
    public static class ChartDataRetriever
    {
        /// <summary>
        /// Custom EventArgs nested class for holding data
        /// </summary>
        public class OnAgeEventArgs : EventArgs
        {
            public List<int> Age { get; set; }

            /// <summary>
            /// Initialization
            /// </summary>
            /// <param name="age"></param>
            public OnAgeEventArgs(List<int> age) : base()
            {
                Age = age;
            }
        }

        /// <summary>
        /// Custom EventArgs nested class for holding data
        /// </summary>
        public class OnPlayTimeEventArgs : EventArgs
        {
            public List<TimeSpan> PlayTime { get; set; }

            /// <summary>
            /// Initialization
            /// </summary>
            /// <param name="playtime"></param>
            public OnPlayTimeEventArgs(List<TimeSpan> playtime) : base()
            {
                PlayTime = playtime;
            }
        }

        public static event EventHandler<OnAgeEventArgs> OnAgeDataComplete;
        public static event EventHandler<OnPlayTimeEventArgs> OnPlayTimeDataComplete;

        public static void RetrieveData(Activity activity)
        {
            Task.Factory.StartNew(() =>
            {
                #region Get age data
                if (DbConnector.OpenSQLConnection())
                {
                    List<int> ageGroups = new List<int>();
                    UserDA userDA = new UserDA();
                    ageGroups = userDA.GetAllUsersAge();
                    activity.RunOnUiThread(() => OnAgeDataComplete.Invoke(activity, new OnAgeEventArgs(ageGroups)));
                }
                else
                {
                    // Connection could not be opened
                    string error = "Connection to the database could not be established.";
                    activity.RunOnUiThread(() =>
                    {
                        AlertGenerator.ShowError(error, activity);
                    });
                }
                DbConnector.CloseSQLConnection();
                #endregion

                #region Get play time data
                if (DbConnector.OpenSQLConnection())
                {
                    List<TimeSpan> playTimes = new List<TimeSpan>();
                    PlayerStatsDA playerDA = new PlayerStatsDA();
                    playTimes = playerDA.GetAllPlayTimes();
                    activity.RunOnUiThread(() => OnPlayTimeDataComplete.Invoke(activity, new OnPlayTimeEventArgs(playTimes)));
                }
                else
                {
                    // Connection could not be opened
                    string error = "Connection to the database could not be established.";
                    activity.RunOnUiThread(() =>
                    {
                        AlertGenerator.ShowError(error, activity);
                    });
                }
                DbConnector.CloseSQLConnection();
                #endregion
            });
        }
    }
}