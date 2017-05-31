
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using ActionBar = Android.Support.V7.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Fragment = Android.Support.V4.App.Fragment;

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Collections.Generic;

namespace TDNoPV
{
    public class ActionAdapter : BaseAdapter
    {
        private static List<TaskTD> _tasks;
        private Context _context;

        public ActionAdapter(Context context, List<TaskTD> tasks)
        {
            _tasks = tasks;
            _context = context;
        }

        public override int Count
        {
            get { return _tasks.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TaskTD task = _tasks[position];
            bool NewRow = convertView == null;

            var row = convertView ?? LayoutInflater.From(_context).Inflate(Resource.Layout.ActRowLayout, parent, false);

            //NOTE: it lets reset button.click to make it simpler but lower an efficiency
            //var row = LayoutInflater.From(_context).Inflate(Resource.Layout.Task2ActionFragment, parent, false);

            TextView NameTV = row.FindViewById<TextView>(Resource.Id.ActRowNameTV);
            NameTV.Text = task.Name;

            TextView ValueTV = row.FindViewById<TextView>(Resource.Id.ActRowValueTV);
            ValueTV.Text = task.Value.ToString();

            TextView StopwatchTV = row.FindViewById<TextView>(Resource.Id.ActRowStopwatchTV);
            ImageButton FinishBtn = row.FindViewById<ImageButton>(Resource.Id.ActRowRemoveBtn);
            ImageButton RunBtn = row.FindViewById<ImageButton>(Resource.Id.ActRowRunBtn);

            task.TVActTimer = StopwatchTV;
            task.RunActBtn = RunBtn;
            task.FinishActBtn = FinishBtn;

            task.UpdateActRunBtn();
            task.UpdateActTimer();

            if (NewRow)
            {
                RunBtn.Click += RunStopAction;
                FinishBtn.Click += FinishAction;

                //FinishBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.no_iconSmall, 0, 0);
                FinishBtn.SetImageResource( Resource.Drawable.no_icon);
            }

            return row;
        }

        private void FinishAction(object sender, EventArgs args)
        {
            ImageButton btn = sender as ImageButton;
            int position = -1;
            while (btn != _tasks[++position].FinishActBtn) ;
            TaskTD task = _tasks[position];

            _tasks[position].RemoveAction();

            _tasks.RemoveAt(position);
            NotifyDataSetChanged();
        }
        private void RunStopAction(object sender, EventArgs args)
        {
            ImageButton btn = sender as ImageButton;
            int position = -1;
            while (btn != _tasks[++position].RunActBtn) ;
            TaskTD task = _tasks[position];

            if (task.Stopwatch.IsRunning)
                task.StopAction();
            else
            {
                foreach (var t in _tasks)
                    if (t != task)
                        t.StopAction();
                task.RunAction();

            }
        }
    }
}

