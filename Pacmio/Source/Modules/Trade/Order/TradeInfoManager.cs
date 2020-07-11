/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    public static class TradeInfoManager
    {
        private static readonly Dictionary<string, TradeInfo> List = new Dictionary<string, TradeInfo>();

        public static TradeInfo GetOrAdd(string execId)
        {
            lock (List)
            {
                if (!List.ContainsKey(execId)) List.Add(execId, new TradeInfo(execId));
                return List[execId];
            }
        }

        //public static void Add(TradeInfo ti) => List.Add(ti.ExecId, ti);

        public static TradeInfo Get(string execId)
        {
            if (List.ContainsKey(execId))
                return List[execId];
            else
                return null;
        }

        public static IEnumerable<TradeInfo> Get(Contract c)
        {
            return List.Select(n => n.Value).Where(n => (n.Contract is Stock) && c == n.Contract).OrderBy(n => n.ExecuteTime);
        }

        public static int Count => List.Count;

        public static double TotalPnL() => TotalPnL(List.Values);

        public static double TotalPnL(this IEnumerable<TradeInfo> log) => (log.Count() < 1) ? 0 : log.Select(n => n.RealizedPnL).Sum();

        #region Data Requests

        public static void Request_Log() => IB.Client.SendRequest_ExecutionData();

        #endregion Data Requests

        #region Events

        public static DateTime UpdatedTime { get; set; }

        public static event StatusEventHandler UpdatedHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdatedTime = DateTime.Now;
            UpdatedHandler?.Invoke(statusCode, UpdatedTime, message);
        }

        #endregion Events

        #region File system

        private static string FileName => Root.ResourcePath + "TradeLog.Json";

        public static void Save()
        {
            lock (List)
                List.Values.ToList().SerializeJsonFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                List<TradeInfo> data = Serialization.DeserializeJsonFile<List<TradeInfo>>(FileName);
                data.AsParallel().ForAll(ti =>
                {
                    lock (List) List[ti.ExecId] = ti;

                    if (ti.Contract is Contract c)
                    {
                        c.TradeData.Add(ti);
                    }
                });
            }
        }

        public static void ExportTradeLog(string fileName) => List.Values.ExportTradeLog(fileName);

        public static void ExportTradeLog(this IEnumerable<TradeInfo> log, string fileName)
        {
            StringBuilder sb = new StringBuilder("ACCOUNT_INFORMATION\n");
            sb.AppendLine("ACT_INF|DU332281|Xu Li|Individual|3581 Greenlee Dr. Apt 133 San Jose CA 95117 United States");
            sb.AppendLine("\n\nSTOCK_TRANSACTIONS");

            foreach (TradeInfo ti in log)
            {
                string action = (ti.Quantity > 0) ? "BUYTO" : "SELLTO";
                if (ti.LastLiquidity == LiquidityType.Added)
                {
                    action += "OPEN|O";
                }
                else
                {
                    action += "CLOSE|C";
                }

                if (ti.Contract is Contract)

                    sb.AppendLine("STK_TRD|" + ti.PermId + "|" + ti.Contract.Name + "|" + ti.Contract.FullName + "|" + ti.Contract.Exchange + "|" + action + "|" +
                        ti.ExecuteTime.ToString("yyyyMMdd") + "|" + ti.ExecuteTime.ToString("HH:mm:ss") + "|USD|" + ti.Quantity + "|1.00|" + ti.Price + "|" + ti.Quantity * ti.Price + "|" + (-ti.Commissions) + "|1.00");
            }
            sb.AppendLine("\n\nEOF");
            sb.ToFile(fileName);
        }

        #endregion File system
    }
}
