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
    public class DataFilterDialogEventArgs : EventArgs
    {
        public DataStorage.DataCommand Command { get; set; }
        public DataFilterDialogEventArgs()
        {
            Command = new DataStorage.DataCommand();
        }
    }
    public class DataFilterDialog : Android.Support.V4.App.DialogFragment
    {
        private CheckBox filterByDateCB;
        private Button dateStartBtn;
        private Button dateEndBtn;
        private TextView dateStartTV;
        private TextView dateEndTV;

        private CheckBox filterByValueCB;
        private SeekBar minValueSB;
        private SeekBar maxValueSB;
        private TextView minValueTV;
        private TextView maxValueTV;

        private Button doneBtn;

        public event EventHandler<DataFilterDialogEventArgs> OnFiliteringComplete;

        private DateTime startDate;
        private DateTime endDate;
        private int minValue;
        private int maxValue;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            SetStyle(Android.Support.V4.App.DialogFragment.StyleNormal, Android.Resource.Style.AnimationDialog);

            var dialogView = inflater.Inflate(Resource.Layout.PgsFilterDialogLayout, container, false);
            filterByDateCB = dialogView.FindViewById<CheckBox>(Resource.Id.DialSBDateCB);
            dateStartBtn = dialogView.FindViewById<Button>(Resource.Id.DialStartDateBtn);
            dateEndBtn = dialogView.FindViewById<Button>(Resource.Id.DialEndDateBtn);
            dateStartTV = dialogView.FindViewById<TextView>(Resource.Id.DialStartDateTV);
            dateEndTV = dialogView.FindViewById<TextView>(Resource.Id.DialEndDateTV);

            filterByValueCB = dialogView.FindViewById<CheckBox>(Resource.Id.DialSBValueCB);
            minValueSB = dialogView.FindViewById<SeekBar>(Resource.Id.DialMinValueSB);
            maxValueSB = dialogView.FindViewById<SeekBar>(Resource.Id.DialMaxValueSB);
            minValueTV = dialogView.FindViewById<TextView>(Resource.Id.DialMinValueTV);
            maxValueTV = dialogView.FindViewById<TextView>(Resource.Id.DialMaxValueTV);

            doneBtn = dialogView.FindViewById<Button>(Resource.Id.DialDoneButton);


            this.Dialog.Window.SetLayout(3000, 3000);//qwe

            startDate = DateTime.Now;
            endDate = DateTime.Now;

            dateStartBtn.Click += ChooseStartDate;
            dateEndBtn.Click += ChooseEndDate;
            dateStartTV.Text = startDate.ToShortDateString();
            dateEndTV.Text = endDate.ToShortDateString();

            minValueSB.ProgressChanged += MinValueSB_ProgressChanged;
            maxValueSB.ProgressChanged += MaxValueSB_ProgressChanged;

            minValue = 0;
            maxValue = 0;

            minValueSB.Progress = minValue + StaticData.ValueDiff;
            maxValueSB.Progress = maxValue + StaticData.ValueDiff;


            doneBtn.Click += FinishFiltering;

            return dialogView;
        }

        private void MinValueSB_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
                filterByValueCB.Checked = true;
            minValue = e.Progress - StaticData.ValueDiff;
            minValueTV.Text = minValue.ToString();
            if (minValue > maxValue)
            {
                maxValue = minValue;
                maxValueSB.Progress = e.Progress;
            }
        }

        private void MaxValueSB_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
                filterByValueCB.Checked = true;
            maxValue = e.Progress - StaticData.ValueDiff;
            maxValueTV.Text = maxValue.ToString();
            if (minValue > maxValue)
            {
                minValue = maxValue;
                minValueSB.Progress = e.Progress;
            }
        }


        private void ChooseStartDate(object sender, EventArgs e)
        {
            DatePickerFragment frag = new DatePickerFragment(GetPickedStartTime, startDate, "start");
            frag.Show(Activity.FragmentManager, frag.Tag);
        }
        private void ChooseEndDate(object sender, EventArgs e)
        {
            DatePickerFragment frag = new DatePickerFragment(GetPickedEndTime, endDate, "end");
            frag.Show(Activity.FragmentManager, frag.Tag);
        }
        private void GetPickedStartTime(DateTime time)
        {
            filterByDateCB.Checked = true;
            startDate = time;
            if (startDate > endDate)
                endDate = startDate;
            dateStartTV.Text = startDate.ToShortDateString();

        }
        private void GetPickedEndTime(DateTime time)
        {
            filterByDateCB.Checked = true;
            endDate = time;
            if (startDate > endDate)
                startDate = endDate;
            dateEndTV.Text = endDate.ToShortDateString();

        }
        private void FinishFiltering(object sender, EventArgs e)
        {
            DataFilterDialogEventArgs dfdea = new DataFilterDialogEventArgs();
            if (filterByDateCB.Checked)
                dfdea.Command.FilterByDate(startDate, endDate);
            if (filterByValueCB.Checked)
                dfdea.Command.FilterByValue(minValue, maxValue);
            OnFiliteringComplete.Invoke(this, dfdea);
            this.Dismiss();
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//NOTE: sets the title bar invisible
            base.OnActivityCreated(savedInstanceState);
        }
    }
}