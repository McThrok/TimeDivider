#pragma warning disable CS0618

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using System;

using ActionBar = Android.Support.V7.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Fragment = Android.Support.V4.App.Fragment;


namespace TDNoPV
{
    //TODO: add padding everywhere
    //TODO: add icons
    [Activity(Label = "TDNoPV", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ActionBarActivity, ActionBar.ITabListener
    {
        private int _tabCount = 4;
        private string[] _tabNames = { "Action", "Stock", "Progress", "Options" };//Act Stk Pgs Opt
        private ActionFragment _ActFrag = null;
        private StockFragment _StkFrag = null;
        private ProgressFragment _PgsFrag = null;
        private OptionsFragment _OptFrag = null;


        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            StaticData.InitStaticData(this);
            InitTabs();
            InitActionBar();
        }
        protected override void OnStop()
        {
            StaticData.OnClose();
            base.OnStop();
        }
        private void InitTabs()
        {
                _ActFrag = new ActionFragment();
                _StkFrag = new StockFragment();
                _PgsFrag = new ProgressFragment();
                _OptFrag = new OptionsFragment();
        }
        private void InitActionBar()
        {
            if (SupportActionBar == null)
                return;

            var actionBar = SupportActionBar;
            actionBar.NavigationMode = (int)ActionBarNavigationMode.Tabs;
            

            for (int i = 0; i < _tabCount; i++)
            {
                var tab = actionBar.NewTab();
                tab.SetTabListener(this);
                tab.SetText(_tabNames[i]);
                actionBar.AddTab(tab);
            }
        }
        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft) { }
        public void OnTabUnselected(ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft) { }
        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
        {
            switch (tab.Text)
            {
                case "Action":
                    ft.Replace(Android.Resource.Id.Content, _ActFrag);//content is a must
                    break;

                case "Stock":
                    ft.Replace(Android.Resource.Id.Content, _StkFrag);//content is a must
                    break;

                case "Progress":
                    ft.Replace(Android.Resource.Id.Content, _PgsFrag);//content is a must
                    break;

                case "Options":
                    ft.Replace(Android.Resource.Id.Content, _OptFrag);//content is a must
                    break;
            }
        }

        protected override void OnResume()
        {
            var listTasks = FindViewById<ListView>(Resource.Id.StkFragLV);
            if (listTasks != null)
                ((BaseAdapter)listTasks.Adapter).NotifyDataSetChanged();//T-H-I-S !!!!

            var actionTasks = FindViewById<ListView>(Resource.Id.ActFragLV);
            if (actionTasks != null)
                ((BaseAdapter)actionTasks.Adapter).NotifyDataSetChanged();//T-H-I-S !!!!

            base.OnResume();
        }
    }
}

