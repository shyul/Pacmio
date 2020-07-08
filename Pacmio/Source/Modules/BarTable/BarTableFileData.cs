/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BarTableFileData : IEquatable<BarTableFileData>, IEquatable<BarTable>, IDisposable
    {
        public BarTableFileData(Contract c, BarFreq freq, BarType type)
        {
            Contract = c.Info;
            BarFreq = freq;
            Type = type;

            EarliestTime = c.MarketData is HistoricalData sd ? sd.BarTableEarliestTime : DateTime.MinValue; // c.BarTableEarliestTime;
            //DataSourceSegments = new MultiPeriod<DataSource>();
        }

        public BarTableFileData(BarTable bt)
        {
            Contract = bt.Contract.Info;
            BarFreq = bt.BarFreq;
            Type = bt.Type;

            EarliestTime = bt.Contract.MarketData is HistoricalData sd ? sd.BarTableEarliestTime : DateTime.MinValue; //  bt.Contract.BarTableEarliestTime;
            LastUpdateTime = bt.LastDownloadRequestTime;
            //DataSourceSegments = bt.DataSourceSegments;
        }

        public void Dispose()
        {
            Bars.Clear();
            GC.Collect();
        }

        [DataMember]
        public (string name, Exchange exchange, string typeName) Contract { get; set; }

        [DataMember]
        public BarFreq BarFreq { get; set; }

        [DataMember]
        public BarType Type { get; set; }

        [DataMember]
        public DateTime EarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public MultiPeriod<DataSource> DataSourceSegments { get; private set; } = new MultiPeriod<DataSource>();

        [DataMember]
        public Dictionary<DateTime, (DataSource SRC, double O, double H, double L, double C, double V)> Bars { get; set; }
            = new Dictionary<DateTime, (DataSource SRC, double O, double H, double L, double C, double V)>();

        #region File Operation

        [IgnoreDataMember]
        public string FileName => GetFileName((Contract, BarFreq, Type));

        public static string GetFileName(((string name, Exchange exchange, string typeName) Contract, BarFreq BarFreq, BarType Type) info)
        {
            string prefix = "$";
            if (info.Contract.typeName == "INDEX") prefix = "^";
            string path = Root.ResourcePath + "HistoricalData\\" + info.Contract.typeName.ToString() + "\\" + info.Contract.exchange.ToString() + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path + prefix + info.Contract.name + ".json";
        }

        #endregion File Operation

        #region Equality
        public bool Equals(BarTableFileData other) => (Contract == other.Contract) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarTableFileData left, BarTableFileData right) => left.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTableFileData right) => !left.Equals(right);
        public bool Equals(BarTable other) => (Contract, BarFreq, Type) == (other.Contract.Info, other.BarFreq, other.Type);
        public static bool operator ==(BarTableFileData left, BarTable right) => left is BarTableFileData btd && btd.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTable right) => !(left == right);
        public bool Equals(Contract other) => other is Contract c && c.Info == Contract;
        public static bool operator ==(BarTableFileData left, Contract right) => left is BarTableFileData btd && btd.Equals(right);
        public static bool operator !=(BarTableFileData left, Contract right) => !(left == right);
        public override bool Equals(object other)
        {
            if (other is null)
                return false;
            else if (other is Contract c)
                return Equals(c);
            else if (other is BarTableFileData bi)
                return Equals(bi);
            else if (other is BarTable bt)
                return Equals(bt);
            else
                return false;
        }
        public override int GetHashCode() => Contract.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
