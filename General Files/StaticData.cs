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
        public static string DataFile = "Tasks.xml";

        public static int ValueRange { get; } = 11;
        public static int ValueDiff
        { get { return ValueRange / 2 ; } }

        public static void InitStaticData(Activity context)
        {
            Context = context;
            ActionList = new List<TaskTD>();
            StockList = new List<TaskTD>();

            DataStorage.DataStorageOnAppStart();
            StockList = DataStorage.GetStockList();
            ActionList = DataStorage.GetActionList(StaticData.StockList);
        }
        public static string GetTimeFromSeconds(int seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            StringBuilder sb = new StringBuilder();

            if (t.TotalDays >= 1)
                sb.Append($"{(int)t.TotalDays} d  ");
            if (t.Hours > 0)
                sb.Append($"{t.Hours} h  ");
            if (t.Minutes > 0)
                sb.Append($"{t.Minutes} m  ");
            sb.Append($"{t.Seconds} s  ");

            return sb.ToString();
        }
        public static void OnClose()
        {
            DataStorage.SaveActionList(StaticData.ActionList);
        }
    }
}