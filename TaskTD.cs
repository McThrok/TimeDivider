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

using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

using System.Xml.Serialization;

namespace TDNoPV
{
    public class TaskTD
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int Time { get; set; }
        public int TimeThisDay { get; set; }
        public Stopwatch Stopwatch { get; set; }

        private int TimeToLastStop;
        public static int IdCounter { get; set; } = 1;

        public ImageButton RunActBtn { get; set; }
        public ImageButton FinishActBtn { get; set; }
        public TextView TVActTimer { get; set; }

        public ImageButton AddStkBtn { get; set; }
        public ImageButton DeleteStkBtn { get; set; }
        public ImageButton EditStkBtn { get; set; }

        public TaskTD(string name = "Noname", int value = 0, int time = 0, long id = -1)
        {
            //TODO: fix it, get IdCounter after closing
            Id = id != -1 ? id : IdCounter++;
            Name = name;
            Value = value;
            Time = time;
            Stopwatch = new Stopwatch();
            TimeToLastStop = 0;
        }

        public void RunAction()
        {
            Stopwatch.Start();
            SetBtnStateClickToStop();
            ShowRunningTime();
        }
        public void StopAction()
        {
            Stopwatch.Stop();
            SetBtnStateClickToRun();
            TimeThisDay += TimeFromLastStop;
            Time += TimeFromLastStop;
            TimeToLastStop = TimeOnStopwatch;
            DataStorage.SaveProgress(this);
        }
        public void RemoveAction()
        {
            StopAction();
            Stopwatch.Reset();
            TimeToLastStop = 0;
        }

        public void UpdateActRunBtn()
        {
            if (Stopwatch.IsRunning)
                SetBtnStateClickToStop();
            else
                SetBtnStateClickToRun();
        }
        public void UpdateActTimer()
        {
            TVActTimer.Text = TimeOnStopwatch.ToString();
        }
        public void ShowRunningTime()
        {
            StaticData.Context.RunOnUiThread(UpdateActTimer);
            Timer timer = new Timer(1000);
            timer.Elapsed += (s, e) =>
            {
                if (!Stopwatch.IsRunning)
                    timer.Close();
                else
                    StaticData.Context.RunOnUiThread(UpdateActTimer);
            };
            timer.Start();
        }

        private void SetBtnStateClickToRun()
        {
            RunActBtn.SetImageResource(Resource.Drawable.start_iconSmall);
            //RunActBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.start_iconSmall, 0, 0);

        }
        private void SetBtnStateClickToStop()
        {
            RunActBtn.SetImageResource(Resource.Drawable.stop_iconSmall);

           // RunActBtn.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.stop_iconSmall, 0, 0);

        }

        public int TimeFromLastStop
        {
            get { return TimeOnStopwatch - TimeToLastStop; }
        }
        public int TimeOnStopwatch
        {
            get { return Convert.ToInt32(Stopwatch.Elapsed.TotalSeconds); }
        }


    }
}