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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddTaskLayout);//

            int position = Intent.GetIntExtra("position", -1);
            if (position == -1) throw new InvalidOperationException("position not passed");
            TaskTD task = StaticData.StockList[position];

            FindViewById<EditText>(Resource.Id.AddTaskNameText).Text = task.Name;
            FindViewById<EditText>(Resource.Id.AddTaskValueText).Text = task.Value.ToString();

            Button DoneButton = FindViewById<Button>(Resource.Id.AddTaskDoneButton);
            DoneButton.Click += DoneAction;
        }
        void DoneAction(object sender, EventArgs args)
        {
            int position = Intent.GetIntExtra("position", -1);
            if (position == -1) throw new InvalidOperationException("position not passed");
            TaskTD task = StaticData.StockList[position];

            task.Name = FindViewById<EditText>(Resource.Id.AddTaskNameText).Text;
            task.Value = Int32.Parse(FindViewById<EditText>(Resource.Id.AddTaskValueText).Text);
            
            this.Finish();
        }
    }
}