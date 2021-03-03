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
    public class BarDataFile : IDataFile, IEquatable<BarDataFile>, IEquatable<BarTable>, IDisposable
    {
        public BarDataFile(Contract c, BarFreq freq, BarType type)
        {
            ContractKey = c.Key;
            BarFreq = freq;
            Type = type;
            EarliestTime = c.GetOrCreateFundamentalData().EarliestTime;
        }

        public BarDataFile(BarTable bt)
        {
            ContractKey = bt.Contract.Key;
            BarFreq = bt.BarFreq;
            Type = bt.Type;
            EarliestTime = bt.Contract.GetOrCreateFundamentalData().EarliestTime;
            LastUpdateTime = bt.LastDownloadRequestTime;
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
            string dir = Root.HistoricalDataPath(info.ContractKey) + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir + (info.ContractKey.typeName == "INDEX" ? "^" : "$") + info.ContractKey.name + ".json";
        }

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName((ContractKey, BarFreq, Type));

        // TODO: Handle this
        [IgnoreDataMember]
        public bool IsModified { get; set; } = false;

        public void SaveFile()
        {
            lock (Bars)
                this.SerializeJsonFile(DataFileName);
        }

        public static BarDataFile LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
            => Serialization.DeserializeJsonFile<BarDataFile>(GetDataFileName(info));

        public static BarDataFile LoadFile(BarTable bt) => LoadFile(bt.Key);

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
                    foreach (var fdm in fdlist)
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

        public bool Equals(BarDataFile other) => (ContractKey == other.ContractKey) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarDataFile left, BarDataFile right) => left.Equals(right);
        public static bool operator !=(BarDataFile left, BarDataFile right) => !left.Equals(right);
        
        public bool Equals(BarTable other) => (ContractKey, BarFreq, Type) == (other.Contract.Key, other.BarFreq, other.Type);
        public static bool operator ==(BarDataFile left, BarTable right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, BarTable right) => !(left == right);
        
        public bool Equals(Contract other) => other is Contract c && c.Key == ContractKey;
        public static bool operator ==(BarDataFile left, Contract right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, Contract right) => !(left == right);
        
        public override bool Equals(object other)
        {
            if (other is BarDataFile btd)
                return Equals(btd);
            else if (other is BarTable bt)
                return Equals(bt);
            else if (other is Contract c)
                return Equals(c);
            else
                return false;
        }

        public override int GetHashCode() => ContractKey.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
