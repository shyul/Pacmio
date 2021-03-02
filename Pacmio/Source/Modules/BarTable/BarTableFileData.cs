/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BarTableFileData : IDataFile, IEquatable<BarTableFileData>, IEquatable<BarTable>, IDisposable
    {
        public BarTableFileData(Contract c, BarFreq freq, BarType type)
        {
            ContractKey = c.Key;
            BarFreq = freq;
            Type = type;

            //EarliestTime = c.MarketData is StockData sd ? sd.BarTableEarliestTime : DateTime.MinValue; // c.BarTableEarliestTime;

            EarliestTime = c.GetOrCreateFundamentalData().EarliestTime;

            //DataSourceSegments = new MultiPeriod<DataSource>();
        }

        public BarTableFileData(BarTable bt)
        {
            ContractKey = bt.Contract.Key;
            BarFreq = bt.BarFreq;
            Type = bt.Type;

            //EarliestTime = bt.Contract.MarketData is StockData sd ? sd.BarTableEarliestTime : DateTime.MinValue; //  bt.Contract.BarTableEarliestTime;
            EarliestTime = bt.Contract.GetOrCreateFundamentalData().EarliestTime;

            LastUpdateTime = bt.LastDownloadRequestTime;
            //DataSourceSegments = bt.DataSourceSegments;
        }

        public void Dispose()
        {
            Bars.Clear();
            GC.Collect();
        }

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; set; }

        [DataMember]
        public BarFreq BarFreq { get; set; }

        [DataMember]
        public BarType Type { get; set; }

        [DataMember]
        public DateTime EarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public MultiPeriod<DataSourceType> DataSourceSegments { get; private set; } = new MultiPeriod<DataSourceType>();

        [DataMember]
        public Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)> Bars { get; set; }
            = new Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)>();

        #region File Operation

        private static string GetDataFileName(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            string prefix = "$";
            if (info.ContractKey.typeName == "INDEX") prefix = "^";
            //string path = Root.ResourcePath + "HistoricalData\\" + info.Contract.typeName.ToString() + "\\" + info.Contract.exchange.ToString() + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            string path = Root.HistoricalDataPath(info.ContractKey) + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path + prefix + info.ContractKey.name + ".json";
        }

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName((ContractKey, BarFreq, Type));

        public void SaveFile()
        {
            //lock (Bars)
                this.SerializeJsonFile(DataFileName);
        }

        public static BarTableFileData LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
            => Serialization.DeserializeJsonFile<BarTableFileData>(GetDataFileName(info));

        public static BarTableFileData LoadFile(BarTable bt) => LoadFile(bt.Key);
        //=> Serialization.DeserializeJsonFile<BarTableFileData>(GetDataFileName(bt.Key));

        #endregion File Operation

        #region Export CSV

        public void ExportCSV(string fileName)
        {
            var list = Bars.OrderByDescending(n => n.Key);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Time,Data Source,Open,High,Low,Close,Volume,Event");

            var fd = ContractKey.GetOrCreateFundamentalData();

            FundamentalDatum[] fdlist = null;

            DateTime date = DateTime.MaxValue.Date;
            string fd_event = string.Empty;

            foreach (var row in list)
            {
                DateTime newDate = row.Key.Date;

                if (date > newDate)
                {
                    fdlist = fd.GetList(newDate);
                    foreach(var fdm in fdlist) 
                    {
                        fd_event += fdm.TypeName + " = " + fdm.Value + " | ";
                    }
                    fd_event = fd_event.Trim(new char[] { ' ', '|' }).Trim();
                    date = newDate;
                }

                sb.AppendLine(string.Join(",", new string[] {
                    row.Key.ToString(),
                    row.Value.SRC.ToString(),
                    row.Value.O.ToString(),
                    row.Value.H.ToString(),
                    row.Value.L.ToString(),
                    row.Value.C.ToString(),
                    row.Value.V.ToString(),
                    fd_event
                }));

                fd_event = string.Empty;
            }

            sb.ToFile(fileName);
        }

        #endregion Export CSV

        #region Equality
        public bool Equals(BarTableFileData other) => (ContractKey == other.ContractKey) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarTableFileData left, BarTableFileData right) => left.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTableFileData right) => !left.Equals(right);
        public bool Equals(BarTable other) => (ContractKey, BarFreq, Type) == (other.Contract.Key, other.BarFreq, other.Type);
        public static bool operator ==(BarTableFileData left, BarTable right) => left is BarTableFileData btd && btd.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTable right) => !(left == right);
        public bool Equals(Contract other) => other is Contract c && c.Key == ContractKey;
        public static bool operator ==(BarTableFileData left, Contract right) => left is BarTableFileData btd && btd.Equals(right);
        public static bool operator !=(BarTableFileData left, Contract right) => !(left == right);
        public override bool Equals(object other)
        {
            if (other is Contract c)
                return Equals(c);
            else if (other is BarTableFileData bi)
                return Equals(bi);
            else if (other is BarTable bt)
                return Equals(bt);
            else
                return false;
        }

        public override int GetHashCode() => ContractKey.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
