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

using ActionBar = Android.Support.V7.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Fragment = Android.Support.V4.App.Fragment;
using com.refractored.fab;

namespace TDNoPV
{
    public class StockFragment : Fragment, IScrollDirectorListener, AbsListView.IOnScrollListener//2 last to fab
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.StkFragLayout, container, false);

            var list = root.FindViewById<ListView>(Resource.Id.StkFragLV);
            var adapter = new StockAdapter(Activity, StaticData.StockList);
            list.Adapter = adapter;

            var fab = root.FindViewById<FloatingActionButton>(Resource.Id.StkFragFab);
            fab.AttachToListView(list, this, this);
            fab.Click += (s, e) => { StartActivity(new Intent(Activity, typeof(AddTaskActivity))); };

            return root;
        }

        //to fab
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) { }
        public void OnScrollDown() { }
        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState) { }
        public void OnScrollUp() { }
        //to fab
    }
}