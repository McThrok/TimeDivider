//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
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
using System.Linq;

using System.Collections.Generic;
using Android.Graphics;

namespace TDNoPV
{
    class ProgressAdapter : BaseAdapter
    {

        Context _context;
        List<DataStorage.DataCell> _cells;
        public ProgressAdapter(Context context, List<DataStorage.DataCell> cells)
        {
            _context = context;
            _cells = cells;
        }


        public override int Count
        {
            get { return _cells.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            ProgressAdapterViewHolder holder = null;

            if (holder == null)
            {
                holder = new ProgressAdapterViewHolder();
                //replace with your item and your holder items
                //comment back in
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.PgsRowLayout, parent, false);
                holder.Index = view.FindViewById<TextView>(Resource.Id.PgsRowIndexTV);
                holder.Value = view.FindViewById<View>(Resource.Id.PgsRowValueV);
                holder.Name = view.FindViewById<TextView>(Resource.Id.PgsRowNameTV);
                holder.Time = view.FindViewById<TextView>(Resource.Id.PgsRowTimeTV);
                holder.Percentage = view.FindViewById<TextView>(Resource.Id.PgsRowPercentageTV);

                view.Tag = holder;
            }

            holder.Index.Text = Convert.ToChar('A' + position).ToString();
            holder.Value.SetBackgroundColor(Color.Rgb(Convert.ToByte(Math.Min(5 - _cells[position].Value, 5) * 50), Convert.ToByte(Math.Min(_cells[position].Value + 5, 5) * 50), 0));
            holder.Name.Text = _cells[position].Name;
            holder.Time.Text = StaticData.GetTimeFromSeconds(_cells[position].Time);
            holder.Percentage.Text = Math.Round((100.0 * _cells[position].Time / _cells.Sum(c => c.Time)), 2).ToString() + "%";

            return view;
        }

    }

    class ProgressAdapterViewHolder : Java.Lang.Object
    {
        public TextView Index { get; set; }
        public View Value { get; set; }
        public TextView Name { get; set; }
        public TextView Time { get; set; }
        public TextView Percentage { get; set; }

    }
}