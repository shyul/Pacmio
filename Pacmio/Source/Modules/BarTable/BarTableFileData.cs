/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
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
            Contract = c.Key;
            BarFreq = freq;
            Type = type;

            EarliestTime = c.MarketData is StockData sd ? sd.BarTableEarliestTime : DateTime.MinValue; // c.BarTableEarliestTime;
            //DataSourceSegments = new MultiPeriod<DataSource>();
        }

        public BarTableFileData(BarTable bt)
        {
            Contract = bt.Contract.Key;
            BarFreq = bt.BarFreq;
            Type = bt.Type;

            EarliestTime = bt.Contract.MarketData is StockData sd ? sd.BarTableEarliestTime : DateTime.MinValue; //  bt.Contract.BarTableEarliestTime;
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
        public MultiPeriod<DataSourceType> DataSourceSegments { get; private set; } = new MultiPeriod<DataSourceType>();

        [DataMember]
        public Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)> Bars { get; set; }
            = new Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)>();

        #region File Operation

        public static string GetDataFileName(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            string prefix = "$";
            if (info.ContractKey.typeName == "INDEX") prefix = "^";
            //string path = Root.ResourcePath + "HistoricalData\\" + info.Contract.typeName.ToString() + "\\" + info.Contract.exchange.ToString() + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            string path = Root.HistoricalDataPath(info.ContractKey) + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path + prefix + info.ContractKey.name + ".json";
        }

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName((Contract, BarFreq, Type));

        public void SaveFile()
        {
            //lock (Bars)
                this.SerializeJsonFile(DataFileName);
        }

        public static BarTableFileData LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
            => Serialization.DeserializeJsonFile<BarTableFileData>(GetDataFileName(info));

        public static BarTableFileData LoadFile(BarTable bt)
            => Serialization.DeserializeJsonFile<BarTableFileData>(GetDataFileName(bt.Key));


        #endregion File Operation

        #region Equality
        public bool Equals(BarTableFileData other) => (Contract == other.Contract) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarTableFileData left, BarTableFileData right) => left.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTableFileData right) => !left.Equals(right);
        public bool Equals(BarTable other) => (Contract, BarFreq, Type) == (other.Contract.Key, other.BarFreq, other.Type);
        public static bool operator ==(BarTableFileData left, BarTable right) => left is BarTableFileData btd && btd.Equals(right);
        public static bool operator !=(BarTableFileData left, BarTable right) => !(left == right);
        public bool Equals(Contract other) => other is Contract c && c.Key == Contract;
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

        public override int GetHashCode() => Contract.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
