using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Threading;

//CLASS NOT USED IN THIS PROJECT

//namespace TDNoPV
//{
//    public static class DataSaver
//    {
//        async public static Task SaveDataAsync(List<TaskTD> list, string FileName)
//        {
//            await Task.Run(() =>
//            {
//                long time = DateTime.Now.Ticks;
//                string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FileName);
//               FileStream fs = new FileStream(FilePath, FileMode.Create);
//               XmlSerializer xs = new XmlSerializer(typeof(TaskTD[]));
//               xs.Serialize(fs, list.ToArray());
//               fs.Close();
//                time = DateTime.Now.Ticks - time;
//                Console.WriteLine(time);
//            }).ConfigureAwait(false);//strange but without it wait() never ends
//        }

//        async public static Task<List<TaskTD>> LoadDataAsync(string FileName)
//        {
//            return await Task.Run(() =>
//            {
//                List<TaskTD> list = null;
//                string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FileName);
//                if (!File.Exists(FilePath))
//                    return new List<TaskTD>();
//                FileStream fs = new FileStream(FilePath, FileMode.Open);
//                XmlSerializer xs = new XmlSerializer(typeof(TaskTD[]));
//                list = new List<TaskTD>(((TaskTD[])xs.Deserialize(fs)));
//                fs.Close();

//                return list;
//            }).ConfigureAwait(false);//strange but without it wait() never ends
//        }
//    }
//}