using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TDNoPV
{
    [Activity(Label = "AddTaskActivity")]
    public class AddTaskActivity : Activity
    {

        SeekBar _valueSeekBar;
        TextView _valueTextView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddTaskLayout);

            _valueSeekBar = FindViewById<SeekBar>(Resource.Id.AddTaskValueSB);
            _valueTextView = FindViewById<TextView>(Resource.Id.AddTaskValueTV);
            _valueSeekBar.ProgressChanged += _valueSeekBar_ProgressChanged;
            _valueSeekBar.Progress = 5;

            Button DoneButton = FindViewById<Button>(Resource.Id.AddTaskDoneButton);
            DoneButton.Click += DoneAction;
        }

        private void _valueSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            _valueTextView.Text = (_valueSeekBar.Progress - StaticData.ValueDiff).ToString();
        }

        void DoneAction(object sender, EventArgs args)
        {
            string name = FindViewById<EditText>(Resource.Id.AddTaskNameText).Text;

            int value = _valueSeekBar.Progress - StaticData.ValueDiff;
            TaskTD newTask = new TaskTD(name, value);
            StaticData.StockList.Add(newTask);
            DataStorage.InsertOrReplaceIntoStock(newTask);
            this.Finish();            
        }
    }
}