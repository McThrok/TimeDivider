using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Mono.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;

namespace TDNoPV
{
    public static class DataStorage
    {
        //TODO:try to use data format
        static string DbName = "TimideDividerDatabase.db";
        static string ActionTable = "ActionTable";
        static string StockTable = "StockTable";
        static string ProgressTable = "ProgressTable";
        static string DbPath;
        static string ConName;


        public static void DataStorageOnAppStart()
        {
            DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DbName);
            ConName = string.Format("Data Source={0};Version=3;", DbPath);

            if (!File.Exists(DbPath))
            {
                SqliteConnection.CreateFile(DbPath);
                string sql = string.Format("CREATE TABLE {0} (Id INTEGER PRIMARY KEY, Time INT)", ActionTable);
                Execute(sql);
                sql = string.Format("CREATE TABLE {0} (Id INTEGER PRIMARY KEY , Name TEXT, Value INT, Deleted INT)", StockTable);
                Execute(sql);
                sql = string.Format("CREATE TABLE {0} (Task_id INTEGER, Time INT, Day INT, Month INT, Year INT, UNIQUE(Task_id, Day, Month, Year))", ProgressTable);
                Execute(sql);
            }
            TaskTD.IdCounter = GetIdCounter();
        }

        private static int Execute(string sql)
        {
            int result;
            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = sql;
                result = command.ExecuteNonQuery();
                Con.Close();
            }
            return result;
        }
        private static int GetIdCounter()
        {
            int result;
            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = string.Format("SELECT MAX(Id) as CounterId FROM {0}", StockTable);

                SqliteDataReader reader = command.ExecuteReader();
                result = 1;
                if (reader.Read())
                    if (!(reader["CounterId"] is DBNull))
                        result = Convert.ToInt32(reader["CounterId"]);

                Con.Close();
            }
            return result;
        }

        public static void InsertOrReplaceIntoAction(TaskTD task)
        {
            string sql = string.Format("INSERT OR REPLACE INTO {0} (Id,Time) VALUES ({1},{2}) ", ActionTable, task.Id, task.TimeOnStopwatch);
            Console.Write(Execute(sql));
        }
        public static void DeleteFromAction(TaskTD task)
        {
            string sql = string.Format("DELETE FROM {0} WHERE Id = {1}", ActionTable, task.Id);
            Execute(sql);
        }
        public static void UpdateAction(TaskTD task)
        {
            string sql = string.Format("UPDATE {0} Time = {1} WHERE Id = {2}", StockTable, task.Time, task.Id);
            Execute(sql);
        }
        public static void SaveActionList(List<TaskTD> list)
        {
            foreach (var t in list)
                InsertOrReplaceIntoAction(t);
        }
        public static List<TaskTD> GetActionList(List<TaskTD> stockList)
        {
            List<TaskTD> list = new List<TaskTD>();
            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = string.Format("SELECT * FROM {0} ORDER BY Id", ActionTable);

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TaskTD task = stockList.Find((TaskTD t) => t.Id == (long)reader["Id"]);
                    task.Stopwatch.Elapsed.Add(new TimeSpan(0, 0, (int)reader["Time"]));
                    list.Add(task);
                }
                Con.Close();
            }
            return list;
        }


        public static void InsertOrReplaceIntoStock(TaskTD task)
        {
            string sql = string.Format("INSERT OR REPLACE INTO {0} (Id,Name,Value,Deleted) VALUES ({1},'{2}',{3},0) ", StockTable, task.Id, task.Name, task.Value);
            Console.WriteLine(Execute(sql));//DEBUG
        }
        public static void DeleteFromStock(TaskTD task)
        {
            string sql = string.Format("UPDATE {0} SET Deleted = 1 WHERE Id = {1}", StockTable, task.Id);
            Execute(sql);
        }
        public static void UpdateStock(TaskTD task)
        {
            string sql = string.Format("UPDATE {0} SET Name = '{1}', Value = {2} WHERE Id = {3}", StockTable, task.Name, task.Value, task.Id);
            Execute(sql);
        }
        public static void SaveStockList(List<TaskTD> list)
        {
            foreach (var t in list)
                InsertOrReplaceIntoStock(t);
        }
        public static List<TaskTD> GetStockList()
        {
            List<TaskTD> list = new List<TaskTD>();
            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = string.Format(
                    @"SELECT st.*, progress.TaskTime FROM {0} st join (SELECT pt.Task_id, SUM(Time) as TaskTime FROM {1} pt GROUP BY pt.Task_id) progress on st.Id = progress.Task_id"
                    , StockTable, ProgressTable);

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //NOTE: use convert on object instead of (int)
                    if ((int)reader["Deleted"] == 0)
                        list.Add(new TaskTD((string)reader["Name"], (int)reader["Value"], Convert.ToInt32(reader["TaskTime"]), (long)reader["Id"]));
                }

                Con.Close();
            }
            return list;
        }


        public static void SaveProgress(TaskTD task)
        {
            DateTime DT = DateTime.Now;
            string sql = string.Format("INSERT OR REPLACE INTO {0} (Task_id,Time,Day,Month,Year) VALUES ({1},{2},{3},{4},{5}) ", ProgressTable, task.Id, task.TimeThisDay, DT.Day, DT.Month, DT.Year);
            Console.WriteLine(Execute(sql));//DEBUG
        }

        //generate data
        public static void FillStock(List<TaskTD> list)
        {
            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = string.Format("SELECT Task_id,Time FROM {0} ORDER BY Task_id", ProgressTable);

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TaskTD task = list.Find((TaskTD t) => t.Id == (long)reader["Task_id"]);
                    task.Time += (int)reader["Time"];
                }
                Con.Close();
            }
        }
        public static void InsertIntoProgress(long taskId, int time, int day, int month, int year)
        {
            string sql = string.Format("INSERT OR REPLACE INTO {0} (Task_id,Time,Day,Month,Year) VALUES ({1},{2},{3},{4},{5}) ", ProgressTable, taskId, time, day, month, year);
            Execute(sql);
        }
        //generate data

        //TODO: make its own connections and datachart async
        public static List<DataCell> GetProgress(string sql)
        {
            List<DataCell> list = new List<DataCell>();

            using (SqliteConnection Con = new SqliteConnection(ConName))
            using (SqliteCommand command = Con.CreateCommand())
            {
                Con.Open();
                command.CommandText = sql;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DataCell cell = new DataCell((long)reader["Id"]);
                    cell.Name = (string)reader["Name"];
                    cell.Value = (int)reader["Value"];
                    cell.Time = Convert.ToInt32(reader["Time"]);
                    list.Add(cell);
                }
                Con.Close();
            }
            return list;
        }

        public class DataCell
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }
            public int Time { get; set; }
            public DataCell(long id)
            {
                Id = id;
            }
        }
        public class DataCommand
        {
            private StringBuilder _command;

            private bool _byDate;
            private DateTime _startDate;
            private DateTime _endDate;
            private bool _byValue;
            private int _minValue;
            private int _maxValue;

            public DataCommand()
            {
                _command = new StringBuilder("SELECT stock.Id, stock.Name, stock.Value, progress.Time FROM ");
            }
            public DataCommand FilterByDate(DateTime startDate, DateTime endDate)
            {
                _startDate = startDate;
                _endDate = endDate;
                _byDate = true;
                return this;
            }
            public DataCommand FilterByValue(int minValue, int maxValue)
            {
                _minValue = minValue;
                _maxValue = maxValue;
                _byValue = true;
                return this;
            }
            public string Build()
            {
                if (!_byValue)
                    _command.AppendFormat("{0} stock ", StockTable);
                else
                    _command.AppendFormat(
                        @"(SELECT st.Id, st.Name, st.Value FROM  {0} st where st.Value>={1} and st.Value<={2}) "
                        , StockTable, _minValue, _maxValue);

                _command.AppendFormat(@"join (SELECT Task_id, SUM(Time) as Time FROM {0} pt ", ProgressTable);

                if (_byDate)
                    _command.AppendFormat(@"WHERE ((Year>{3} OR (Year={3} AND (Month>{2} OR (Month={2} AND Day>={1})))) AND (Year<{6} OR (Year={6} AND (Month<{5} OR (Month={5} AND Day<={4}))))) "
                         , ProgressTable, _startDate.Day, _startDate.Month, _startDate.Year, _endDate.Day, _endDate.Month, _endDate.Year);

                _command.Append("GROUP BY pt.Task_id) progress on progress.Task_id=stock.Id");

                Console.Write("qwe  " + _command.ToString());//DEBUG
                return _command.ToString();


            }
        }
    }
}