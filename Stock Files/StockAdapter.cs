
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

using System.Collections.Generic;

namespace TDNoPV

{
    public class StockAdapter : BaseAdapter
    {
        private List<TaskTD> _tasks;
        private Context _context;

        public StockAdapter(Context context, List<TaskTD> tasks)
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
            bool newRow = convertView == null;

            var row = convertView ?? LayoutInflater.From(_context).Inflate(Resource.Layout.StkRowLayout, parent, false);

            TextView NameTV = row.FindViewById<TextView>(Resource.Id.StkRowNameTV);
            NameTV.Text = task.Name;

            TextView ValueTV = row.FindViewById<TextView>(Resource.Id.StkRowValueTV);
            ValueTV.Text = task.Value.ToString();

            TextView TimeTV = row.FindViewById<TextView>(Resource.Id.StkRowTimeTV);
            TimeTV.Text = StaticData.GetTimeFromSeconds(task.Time);

            task.AddStkBtn = row.FindViewById<ImageButton>(Resource.Id.StkRowAddBtn);
            task.DeleteStkBtn = row.FindViewById<ImageButton>(Resource.Id.StkRowDeleteBtn);
            task.EditStkBtn = row.FindViewById<ImageButton>(Resource.Id.StkRowEditBtn);

            if (newRow)
            {
                //TODO: change it into icons
                task.AddStkBtn.SetImageResource( Resource.Drawable.plus_icon);
                task.AddStkBtn.Click += AddToAction;

                task.DeleteStkBtn.SetImageResource(Resource.Drawable.no_icon);
                task.DeleteStkBtn.Click += DeleteFromStk;

                task.EditStkBtn.SetImageResource(Resource.Drawable.edit_icon);
                task.EditStkBtn.Click += EditStk;
            }

            return row;
        }

        private void AddToAction(object sender, EventArgs args)
        {
            ImageButton btn = sender as ImageButton;
            TaskTD task = _tasks.Find(t => t.AddStkBtn == btn);

            if (!StaticData.ActionList.Contains(task))
                StaticData.ActionList.Add(task);
        }
        private void DeleteFromStk(object sender, EventArgs args)
        {
            ImageButton btn = sender as ImageButton;
            TaskTD task = _tasks.Find(t => t.DeleteStkBtn == btn);

            //if (task.FinishActBtn != null)
            //    task.FinishActBtn?.CallOnClick();
            task.FinishActBtn?.CallOnClick();

            DataStorage.DeleteFromStock(task);
            _tasks.Remove(task);
            NotifyDataSetChanged();
        }
        private void EditStk(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            int position = _tasks.FindIndex(t => t.EditStkBtn == btn);

            Intent intent = new Intent(_context, typeof(EditTaskActivity));
            intent.PutExtra("position", position);
            _context.StartActivity(intent);

            DataStorage.UpdateStock(StaticData.StockList[position]);
        }
    }
}