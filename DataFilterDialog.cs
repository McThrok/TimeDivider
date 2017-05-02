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
        private Button doneBtn;
        private TextView dateStartTV;
        private TextView dateEndTV;

        public event EventHandler<DataFilterDialogEventArgs> OnFiliteringComplete;

        private DateTime startDate;
        private DateTime endDate;

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
            doneBtn = dialogView.FindViewById<Button>(Resource.Id.DialDoneButton);

            this.Dialog.Window.SetLayout(3000, 3000);

            startDate = DateTime.Now;
            endDate = DateTime.Now;

            doneBtn.Click += FinishFiltering;

            dateStartBtn.Click += ChooseStartDate;
            dateEndBtn.Click += ChooseEndDate;

            dateStartTV.Text = startDate.ToShortDateString();
            dateEndTV.Text = endDate.ToShortDateString();

            return dialogView;
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
            startDate = time;
            if (startDate > endDate)
                endDate = startDate;
            dateStartTV.Text = startDate.ToShortDateString();

        }
        private void GetPickedEndTime(DateTime time)
        {
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