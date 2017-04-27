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

using Fragment = Android.Support.V4.App.Fragment;

namespace TDNoPV
{
    public class OptionsFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.OptFragLayout, container, false);
            root.FindViewById<Button>(Resource.Id.OptFragGenerateDataBtn).Click += GenerateData;

            return root;
        }

        private void GenerateData(object sender, EventArgs e)
        {
            Random rd = new Random();
            StaticData.StockList = new List<TaskTD>();
            string[] tab = { "bieganie", "programowanie", "sprzątanie", "oglądanie seriali", "nauka języków obcych", "granie w gry", "czytanie książek", "siłownia", "nauka gotowania", "pranie/prasowanie", "spacerowanie", "karmienie kaczek" };

            for (int i = 0; i < tab.Length; i++)
                StaticData.StockList.Add(new TaskTD(tab[i], rd.Next(-5, 6), 0));
            DataStorage.SaveStockList(StaticData.StockList);

            DateTime Date = DateTime.Now;
            for (int i = 0; i < 20; i++)
            {
                int taskId = rd.Next(1, tab.Length);
                int time = rd.Next(1, 2);
                DataStorage.InsertIntoProgress(taskId, time, Date.Day, Date.Month, Date.Year);
                

                Date = Date.AddDays(-1);
                Console.WriteLine(i);
            }

            DataStorage.FillStock(StaticData.StockList);
        }
    }
}