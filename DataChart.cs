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

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TDNoPV
{
    public static class DataChart
    {
        //static OxyColor[] colors = { OxyColors.Aqua, OxyColors.BlueViolet, OxyColors.Crimson, OxyColors.Violet, OxyColors.LawnGreen, OxyColors.LightSeaGreen, OxyColors.MediumSeaGreen, OxyColors.Orange, OxyColors.Peru, OxyColors.YellowGreen, OxyColors.SkyBlue };

        public static PlotModel CreatePlotModel(DateTime start, DateTime end)
        {
            PlotModel modelP1 = new PlotModel { Title = "" };

            modelP1.PlotMargins = new OxyThickness(30, 10, 30, 10);

            PieSeries seriesP1 = new PieSeries { StrokeThickness = 1.5, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };


            DataStorage.DataCommand dc = new DataStorage.DataCommand();
            List<DataStorage.DataCell> cells = DataStorage.GetProgress(dc.FilterByDate(start, end).Build());
            cells = cells.OrderByDescending(cell => cell.Time).ToList();

            char index = 'A';
            for (int i = 0; i < cells.Count; i++)
                seriesP1.Slices.Add(new PieSlice(index++.ToString(), Convert.ToDouble(cells[i].Time))
                {
                    IsExploded = false,
                    Fill = OxyColor.FromRgb(Convert.ToByte((cells[i].Value + 5) * 25), Convert.ToByte((5 - cells[i].Value) * 25), 0)
                });

            seriesP1.Stroke = OxyColors.DarkGray;

            //TODO: use it or delete it
            //seriesP1.InsideLabelPosition = 0.0;
            //seriesP1.InsideLabelFormat = null;


            //NOTE: to get rid of prcentage values
            // seriesP1.OutsideLabelFormat = "";
            seriesP1.TickHorizontalLength = 0.00;
            seriesP1.TickRadialLength = 10.00;

            modelP1.Series.Add(seriesP1);

            return modelP1;
        }
    }
}