using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Fragment = Android.Support.V4.App.Fragment;
using OxyPlot.Xamarin.Android;

namespace TDNoPV
{
    public class ProgressFragment : Fragment
    {
        //View _root;
        PlotView ProgressChart;
        public DateTime FilteredStart;
        DateTime FilteredEnd;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.PgsFragLayout, container, false);

            FilteredStart = DateTime.Now;
            FilteredEnd = DateTime.Now;
            ProgressChart = root.FindViewById<PlotView>(Resource.Id.plot_view);

            PrintChart(DateTime.Now, DateTime.Now);

            root.FindViewById<Button>(Resource.Id.PgsFragTodayBtn).Click += TodayPgs;
            root.FindViewById<Button>(Resource.Id.PgsFragYesterdayBtn).Click += YesterdayPgs;
            root.FindViewById<Button>(Resource.Id.PgsFragThisWeekBtn).Click += ThisWeekPgs;
            root.FindViewById<Button>(Resource.Id.PgsFragLastWeekBtn).Click += LastWeekPgs;
            root.FindViewById<Button>(Resource.Id.PgsFragFilterBtn).Click += ShowFilterDataDialog;

            return root;
        }
        async private Task PrintChart(DateTime start, DateTime end)
        {
           await Task.Run(()=>  ProgressChart.Model = DataChart.CreatePlotModel(start, end));
        }
        private void TodayPgs(object sender, EventArgs args)
        {
            PrintChart(DateTime.Now, DateTime.Now);
        }
        private void YesterdayPgs(object sender, EventArgs args)
        {
            DateTime dt = DateTime.Now.AddDays(-1);
            PrintChart(dt, dt);
        }
        private void ThisWeekPgs(object sender, EventArgs args)
        {
            DateTime dt = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            PrintChart(dt, DateTime.Now);
        }
        private void LastWeekPgs(object sender, EventArgs args)
        {
            DateTime start = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 7 + 1);
            DateTime end = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            PrintChart(start, end);
        }
        private void ShowFilterDataDialog(object sender, EventArgs args)
        {
            Android.Support.V4.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DataFilterDialog dataFilterDialog = new DataFilterDialog();
            dataFilterDialog.Show(transaction, "DataFilterDialog");

            dataFilterDialog.OnFiliteringComplete += PrintFilteredChart;
        }
        private void PrintFilteredChart(object sender, DataFilterDialogEventArgs data)
        {
            if (data.FilterByDate)
                ProgressChart.Model = DataChart.CreatePlotModel(data.StartDate, data.EndDate);
            else
                PrintChart(DateTime.MinValue, DateTime.Now);//total

        }
    }
}