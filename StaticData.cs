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
    public static class StaticData
    {
        public static Activity Context;
        public static List<TaskTD> StockList;
        public static List<TaskTD> ActionList;
        public static List<DataStorage.DataCell> ProgressCells;//!!
        public static string DataFile = "Tasks.xml";

        public static void InitStaticData(Activity context)
        {
            Context = context;
            ActionList = new List<TaskTD>();
            StockList = new List<TaskTD>();

            ProgressCells = new List<DataStorage.DataCell>();//!!

            DataStorage.DataStorageOnAppStart();
            StockList = DataStorage.GetStockList();
            ActionList = DataStorage.GetActionList(StaticData.StockList);
        }
        public static void OnClose()
        {
            DataStorage.SaveActionList(StaticData.ActionList);
        }
    }
}