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
using com.refractored.fab;

using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;



namespace TDNoPV
{
    public class ActionFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.ActFragLayout, container, false);

            var list = root.FindViewById<ListView>(Resource.Id.ActFragLV);
            var adapter = new ActionAdapter(Activity, StaticData.ActionList);
            list.Adapter = adapter;

            return root;
        }
    }
}
