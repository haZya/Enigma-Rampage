using System;

using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles launching of DatePicker Fragment
    /// </summary>
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        private Action<DateTime> mDateSelectedHandler = delegate { };

        /// <summary>
        /// Get an instance of the class
        /// </summary>
        /// <param name="onDateSelected"></param>
        /// <returns></returns>
        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment
            {
                mDateSelectedHandler = onDateSelected
            };
            return frag;
        }

        /// <summary>
        /// Override OnCreateDialog method
        /// </summary>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            return dialog;
        }

        /// <summary>
        /// Handles setting of date on the DatePicker
        /// </summary>
        /// <param name="view"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dayOfMonth"></param>
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            // Note: month is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, month + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            mDateSelectedHandler(selectedDate);
        }
    }
}