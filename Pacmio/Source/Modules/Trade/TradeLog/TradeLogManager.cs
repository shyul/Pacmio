/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using Xu;

namespace Pacmio
{
    public static class TradeLogManager
    {
        private static readonly ConcurrentDictionary<string, TradeLogDatum> List = new ConcurrentDictionary<string, TradeLogDatum>();

        public static TradeLogDatum GetOrAdd(string execId)
        {
            if (!List.ContainsKey(execId)) List.TryAdd(execId, new TradeLogDatum(execId));
            return List[execId];
        }

        public static void Add(TradeLogDatum ti) => List.TryAdd(ti.ExecId, ti);

        public static TradeLogDatum Get(string execId)
        {
            if (List.ContainsKey(execId))
                return List[execId];
            else
                return null;
        }

        public static IEnumerable<TradeLogDatum> Get(Contract c)
        {
            return List.Select(n => n.Value).Where(n => (n.Contract is Stock) && c == n.Contract).OrderBy(n => n.ExecuteTime);
        }

        public static int Count => List.Count;

        public static double TotalPnL() => TotalPnL(List.Values);

        public static double TotalPnL(this IEnumerable<TradeLogDatum> log) => (log.Count() < 1) ? 0 : log.Select(n => n.RealizedPnL).Sum();

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
            List<TradeLogDatum> trades = List.Values.ToList();
            trades.SerializeJsonFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(FileName)) 
            {
                List<TradeLogDatum> trades = Serialization.DeserializeJsonFile<List<TradeLogDatum>>(FileName);
                trades.ForEach(n => Add(n));
            }
        }

        public static void ExportTradeLog(string fileName) => List.Values.ExportTradeLog(fileName);

        public static void ExportTradeLog(this IEnumerable<TradeLogDatum> log, string fileName)
        {
            StringBuilder sb = new StringBuilder("ACCOUNT_INFORMATION\n");
            sb.AppendLine("ACT_INF|DU332281|Xu Li|Individual|3581 Greenlee Dr. Apt 133 San Jose CA 95117 United States");
            sb.AppendLine("\n\nSTOCK_TRANSACTIONS");

            foreach (TradeLogDatum ti in log)
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

                if(ti.Contract is Contract)

                sb.AppendLine("STK_TRD|" + ti.PermId + "|" + ti.Contract.Name + "|" + ti.Contract.FullName + "|" + ti.Contract.Exchange + "|" + action + "|" +
                    ti.ExecuteTime.ToString("yyyyMMdd") + "|" + ti.ExecuteTime.ToString("HH:mm:ss") + "|USD|" + ti.Quantity + "|1.00|" + ti.Price + "|" + ti.Quantity * ti.Price + "|" + (-ti.Commissions) + "|1.00");
            }
            sb.AppendLine("\n\nEOF");
            sb.ToFile(fileName);
        }

        #endregion File system
    }
}
