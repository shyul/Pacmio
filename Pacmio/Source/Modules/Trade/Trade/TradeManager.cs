/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public static class TradeManager
    {
        public static void Request_Log() => IB.Client.SendRequest_ExecutionData();

        public static int Count => ExecIdToTradeLUT.Count;

        public static double TotalPnL() => TotalPnL(ExecIdToTradeLUT.Values);

        public static double TotalPnL(this IEnumerable<TradeInfo> log) => (log.Count() < 1) ? 0 : log.Select(n => n.RealizedPnL).Sum();

        #region Data Operation

        public static IEnumerable<TradeInfo> QueryForTrades(this OrderInfo od)
        {
            lock (ExecIdToTradeLUT)
                return ExecIdToTradeLUT.Values
                    .Where(n => n.ConId == od.ConId && n.PermId > 0 && n.PermId == od.PermId)
                    .OrderBy(n => n.ExecuteTime);
        }

        public static IEnumerable<TradeInfo> QueryForTrades(this Contract c)
        {
            lock (ExecIdToTradeLUT)
                return ExecIdToTradeLUT.Values
                    .Where(n => n.Contract == c)
                    .OrderBy(n => n.ExecuteTime);
        }

        public static TradeInfo GetOrCreateTradeByExecId(string execId)
        {
            if (string.IsNullOrWhiteSpace(execId))
                throw new Exception("The execId has to be valid.");

            lock (ExecIdToTradeLUT)
                if (!ExecIdToTradeLUT.ContainsKey(execId))
                {
                    TradeInfo ti = new TradeInfo(execId);
                    ExecIdToTradeLUT[execId] = ti;
                    return ti;
                }
                else
                    return ExecIdToTradeLUT[execId];
        }

        public static TradeInfo GetTradeByExecId(string execId)
        {
            if (string.IsNullOrWhiteSpace(execId))
                throw new Exception("The execId has to be valid.");

            lock (ExecIdToTradeLUT)
                if (ExecIdToTradeLUT.ContainsKey(execId))
                    return ExecIdToTradeLUT[execId];
                else
                    return null;
        }

        private static Dictionary<string, TradeInfo> ExecIdToTradeLUT { get; } = new Dictionary<string, TradeInfo>();

        #endregion Data Operation

        #region Data Events

        public static DateTime UpdatedTime { get; set; }

        public static event StatusEventHandler UpdatedHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdatedTime = DateTime.Now;
            UpdatedHandler?.Invoke(statusCode, UpdatedTime, message);
        }

        #endregion Data Events

        #region File system

        private static string FileName => Root.ResourcePath + "TradeLog.Json";

        public static void Save()
        {
            lock (ExecIdToTradeLUT)
                ExecIdToTradeLUT.Values.ToList().SerializeJsonFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                List<TradeInfo> data = Serialization.DeserializeJsonFile<List<TradeInfo>>(FileName);
                lock (ExecIdToTradeLUT)
                {
                    data.AsParallel().ForAll(ti =>
                    {
                        ExecIdToTradeLUT[ti.ExecId] = ti;
                    });
                }
            }
        }

        #endregion File system

        public static void ExportTradeLog(string fileName) => ExecIdToTradeLUT.Values.ExportTradeLog(fileName);

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

    }
}
