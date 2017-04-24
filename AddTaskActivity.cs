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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddTaskLayout);
            Button DoneButton = FindViewById<Button>(Resource.Id.AddTaskDoneButton);
            DoneButton.Click += DoneAction;
        }
        void DoneAction(object sender, EventArgs args)
        {
            string name = FindViewById<EditText>(Resource.Id.AddTaskNameText).Text;
            int value = 0;
            Int32.TryParse(FindViewById<EditText>(Resource.Id.AddTaskValueText).Text, out value);
            TaskTD newTask = new TaskTD(name, value);
            StaticData.StockList.Add(newTask);
            DataStorage.InsertOrReplaceIntoStock(newTask);
            this.Finish();            
        }
    }
}