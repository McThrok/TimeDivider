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
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            ProgressAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as ProgressAdapterViewHolder;

            if (holder == null)
            {
                holder = new ProgressAdapterViewHolder();
                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                //view = inflater.Inflate(Resource.Layout.item, parent, false);
                //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
                view.Tag = holder;
            }


            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

    }

    class ProgressAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}