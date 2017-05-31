using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TDNoPV
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public new string Tag { get; }//NOTE:not needed
        private Action<DateTime> _returnDate = delegate { };
        private DateTime _current;

        public DatePickerFragment(Action<DateTime> returnDate, DateTime current, string tag)
        {
            Tag = tag;
            _returnDate = returnDate;
            _current = current;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            //NOTE: in DateTime month is 1-12 in DatePickerDialog is 0-11
            return new DatePickerDialog(Activity, this, _current.Year, _current.Month-1, _current.Day);
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            //NOTE: in DateTime month is 1-12 in DatePickerDialog is 0-11
            DateTime selectedDate = new DateTime(year, month+1, dayOfMonth);
            _returnDate(selectedDate);
        }
    }
}