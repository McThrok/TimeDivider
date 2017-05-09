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
    [Activity(Label = "EditTaskActivity")]
    public class EditTaskActivity : Activity
    {
        //TODO: make it safer, add its own layout with cancel button
        EditText _nameEditText;
        SeekBar _valueSeekBar;
        TextView _valueTextView;
        TaskTD task;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddTaskLayout);//

            int position = Intent.GetIntExtra("position", -1);
            if (position == -1) throw new InvalidOperationException("position not found");
            task = StaticData.StockList[position];

            _nameEditText = FindViewById<EditText>(Resource.Id.AddTaskNameText);
            _valueSeekBar = FindViewById<SeekBar>(Resource.Id.AddTaskValueSB);
            _valueTextView = FindViewById<TextView>(Resource.Id.AddTaskValueTV);

            _valueSeekBar.ProgressChanged += _valueSeekBar_ProgressChanged;
            _valueSeekBar.Progress = task.Value + StaticData.ValueDiff;

            _nameEditText.Text = task.Name;

            Button DoneButton = FindViewById<Button>(Resource.Id.AddTaskDoneButton);
            DoneButton.Click += DoneAction;
        }
        private void _valueSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            _valueTextView.Text = (_valueSeekBar.Progress - StaticData.ValueDiff).ToString();
        }
        void DoneAction(object sender, EventArgs args)
        {

            task.Name = _nameEditText.Text;
            task.Value = _valueSeekBar.Progress - StaticData.ValueDiff;

            this.Finish();
        }
    }
}