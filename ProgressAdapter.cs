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

using System.Collections.Generic;

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

            //if (view != null)
            //    holder = view.Tag as ProgressAdapterViewHolder;

            //if (holder == null)
            //{
            //    holder = new ProgressAdapterViewHolder();
            //    var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            //    //replace with your item and your holder items
            //    //comment back in
            //    view = inflater.Inflate(Resource.Layout.PgsRowLayout, parent, false);
            //   // view = LayoutInflater.From(_context).Inflate(Resource.Layout.PgsRowLayout, parent, false);
            //    holder.Index = view.FindViewById<TextView>(Resource.Id.PgsRowIndexTV);
            //    holder.Name = view.FindViewById<TextView>(Resource.Id.PgsRowNameTV);
            //    holder.Time = view.FindViewById<TextView>(Resource.Id.PgsRowTimeTV);
            //    holder.Percentage = view.FindViewById<TextView>(Resource.Id.PgsRowPercentageTV);
            //    view.Tag = holder;
            //}


            ////fill in your items
            //holder.Index.Text = Convert.ToChar('A' + position).ToString();
            //holder.Name.Text = _cells[position].Name;
            //holder.Time.Text = _cells[position].Time.ToString();
            //holder.Percentage.Text = "0";
            ////holder.Title.Text = "new text here";

            view = LayoutInflater.From(_context).Inflate(Resource.Layout.PgsRowLayout, parent, false);

            view.FindViewById<TextView>(Resource.Id.PgsRowIndexTV).Text = Convert.ToChar('A' + position).ToString();
            view.FindViewById<TextView>(Resource.Id.PgsRowNameTV).Text = _cells[position].Name;
            view.FindViewById<TextView>(Resource.Id.PgsRowTimeTV).Text = _cells[position].Time.ToString();
            view.FindViewById<TextView>(Resource.Id.PgsRowPercentageTV).Text = "0";

            //holder.Index.Text = Convert.ToChar('A' + position).ToString();
            //holder.Name.Text = _cells[position].Name;
            //holder.Time.Text = _cells[position].Time.ToString();
            //holder.Percentage.Text = "0";
            return view;
        }

    }

    class ProgressAdapterViewHolder : Java.Lang.Object
    {
        public TextView Index { get; set; }
        public TextView Name { get; set; }
        public TextView Time { get; set; }
        public TextView Percentage { get; set; }

    }
}