
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
            TimeTV.Text = task.Time.ToString();

            task.AddStkBtn = row.FindViewById<Button>(Resource.Id.StkRowAddBtn);
            task.DeleteStkBtn = row.FindViewById<Button>(Resource.Id.StkRowDeleteBtn);
            task.EditStkBtn = row.FindViewById<Button>(Resource.Id.StkRowEditBtn);

            if (newRow)
            {
                //TODO: change it into icons
                task.AddStkBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.plus_iconSmall, 0, 0);
                task.AddStkBtn.Click += AddToAction;

                task.DeleteStkBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.no_iconSmall, 0, 0);

                task.DeleteStkBtn.Click += DeleteFromStk;

                task.EditStkBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.edit_iconSmall, 0, 0);
                task.EditStkBtn.Click += EditStk;
            }

            return row;
        }

        private void AddToAction(object sender, EventArgs args)
        {
            Button btn = sender as Button;
            int position = -1;
            while (btn != _tasks[++position].AddStkBtn) ;
            TaskTD task = _tasks[position];

            if (!StaticData.ActionList.Contains(task))
                StaticData.ActionList.Add(task);
            //TODO: add to database
        }
        private void DeleteFromStk(object sender, EventArgs args)
        {
            Button btn = sender as Button;
            int position = -1;
            while (btn != _tasks[++position].DeleteStkBtn) ;
            TaskTD task = _tasks[position];

            if (task.FinishActBtn != null)
                task.FinishActBtn.CallOnClick();
            DataStorage.DeleteFromStock(task);
            _tasks.RemoveAt(position);
            NotifyDataSetChanged();
        }
        private void EditStk(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int position = -1;
            while (btn != _tasks[++position].EditStkBtn) ;

            Intent intent = new Intent(_context, typeof(EditTaskActivity));
            intent.PutExtra("position", position);
            _context.StartActivity(intent);

            DataStorage.UpdateStock(StaticData.StockList[position]);
        }
    }
}