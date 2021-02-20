/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class UnknownContract : IEquatable<UnknownContract>
    {
        [DataMember]
        public DateTime LastCheckedTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string ExchangeCode { get; set; } = string.Empty;

        [DataMember]
        public string ExchangeSuffix { get; set; } = string.Empty;

        [DataMember]
        public string TypeCode { get; set; } = string.Empty;

        [DataMember]
        public int ConId { get; set; } = 0;

        [DataMember]
        public string FullName { get; set; } = string.Empty;

        [DataMember]
        public string FullNameSuffix { get; set; } = string.Empty;

        [DataMember]
        public string ISIN { get; set; } = string.Empty;

        [DataMember]
        public string CUSIP { get; set; } = string.Empty;

        [IgnoreDataMember]
        public (string Name, string ExchangeCode, string TypeCode) Key { get => (Name, ExchangeCode, TypeCode); set { Name = value.Name; ExchangeCode = value.ExchangeCode; TypeCode = value.TypeCode; } }

        #region Equality

        public override int GetHashCode() => ConId > 0 ? ConId : (Name.GetHashCode() ^ ExchangeCode.GetHashCode() ^ TypeCode.GetHashCode());

        public virtual bool Equals(UnknownContract other) => ConId > 0 ? ConId == other.ConId : (Name == other.Name && ExchangeCode == other.ExchangeCode && TypeCode == other.TypeCode);

        public override bool Equals(object other)
        {
            if (other is UnknownContract dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(UnknownContract s1, UnknownContract s2) => !s1.Equals(s2);
        public static bool operator ==(UnknownContract s1, UnknownContract s2) => s1.Equals(s2);

        #endregion Equality
    }

    /// <summary>
    /// Master List of all unknown symbols
    /// </summary>
    public static class UnknownContractList
    {
        /// <summary>
        /// Stores previously found invalid symbol names here to prevent any further repeating query.
        /// </summary>
        private static Dictionary<(string Name, string ExchangeCode, string TypeCode), UnknownContract> List { get; set; } = new Dictionary<(string Name, string ExchangeCode, string TypeCode), UnknownContract>();

        public static UnknownContract CheckIn(string symbolName, string exchangeCode = "", string typeCode = "STK")
        {
            Console.WriteLine("Checking unknown item: " + symbolName);

            lock (List)
            {
                var key = (symbolName, exchangeCode, typeCode);

                if (!List.ContainsKey(key))
                {
                    UnknownContract uc = new UnknownContract() { Key = key }; //, LastCheckedTime = DateTime.Now };
                    List.Add(key, uc);
                    return uc;
                }
                else
                    return List[key];
            }
        }

        public static void Remove(UnknownContract uc)
        {
            lock (List)
            {
                if (List.ContainsKey(uc.Key))
                    List.Remove(uc.Key);
            }
        }

        public static void Remove(string symbol)
        {
            lock (List)
            {
                var key = (symbol, "", "STK");
                if (List.ContainsKey(key))
                {
                    List.Remove(key);
                }
            }
        }

        public static bool Contains(string symbol)
        {
            lock (List)
            {
                if (List.ContainsKey((symbol, "", "STK")))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        #region File system

        private static string FileName => Root.ResourcePath + "UnknownItems.json"; // Root.ResourcePath + "UnknownItems.csv";

        public static void Save()
        {
            UnknownContract[] list = List.Select(n => n.Value).ToArray();
            list.SerializeJsonFile(FileName);

            Export(Root.ResourcePath + "UnknownItems.csv");
        }

        public static void Load()
        {
            if (Serialization.DeserializeJsonFile<UnknownContract[]>(FileName) is UnknownContract[] list)
            {
                lock (List)
                {
                    foreach (var uc in list)
                    {
                        List.Add(uc.Key, uc);
                    }
                }
            }
        }

        public static void Export(string fileName)
        {
            StringBuilder sb = new StringBuilder("LastCheckedTime,Symbol,Exchange,ExSuffix,Type,Contract ID,Business Name,Suffix,ISIN,CUSIP\n");

            lock (List)
            {
                var sorted = List.OrderBy(n => n.Key.Name).ThenBy(n => n.Value.LastCheckedTime).Select(n => n.Value);
                foreach (var item in sorted)
                {
                    sb.AppendLine(
                        item.LastCheckedTime.ToString() + "," +
                        item.Name.CsvEncode() + "," +
                        item.ExchangeCode.CsvEncode() + "," +
                        item.ExchangeSuffix.CsvEncode() + "," +
                        item.TypeCode.CsvEncode() + "," +
                        item.ConId + "," +
                        item.FullName.CsvEncode() + "," +
                        item.FullNameSuffix.CsvEncode() + "," +
                        item.ISIN.CsvEncode() + "," +
                        item.CUSIP.CsvEncode());
                }
            }

            sb.ToFile(fileName);
        }

        public static void Import(string fileName)
        {
            if (File.Exists(fileName))
            {
                lock (List)
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string[] headers = sr.CsvReadFields();

                        List.Clear();

                        while (!sr.EndOfStream)
                        {
                            string[] fields = sr.CsvReadFields();

                            if (fields.Length == 10)
                            {
                                DateTime lastCheckedTime = (fields[0].TrimCsvValueField() is string timeString && !string.IsNullOrEmpty(timeString)) ? DateTime.Parse(timeString) : DateTime.MinValue;
                                string symbolName = fields[1].TrimCsvValueField();
                                string exchangeCode = fields[2].TrimCsvValueField();
                                string exchangeSuffix = fields[3].TrimCsvValueField();
                                string typeCode = fields[4].TrimCsvValueField().ToUpper();
                                int conId = fields[5].Trim().ToInt32(0);
                                string fullName = fields[6].TrimCsvValueField();
                                string fullNameSuffix = fields[7].TrimCsvValueField();
                                string ISIN = fields[8].TrimCsvValueField();
                                string CUSIP = fields[9].TrimCsvValueField();

                                if (typeCode.Length > 0 && (exchangeCode.Length > 0 && !exchangeCode.Contains("_")))
                                {
                                    try
                                    {
                                        switch (typeCode)
                                        {
                                            case ("STOCK"):
                                                Exchange exchange = exchangeCode.ParseEnum<Exchange>();
                                                Stock stk = ContractManager.GetOrAdd(new Stock(symbolName, exchange));
                                                stk.Status = ContractStatus.Unknown;
                                                stk.ConId = conId;
                                                stk.ExchangeSuffix = exchangeSuffix;

                                                if (stk is IBusiness ib)
                                                {
                                                    ib.ISIN = ISIN;

                                                    if (ib.BusinessInfo is BusinessInfo bi)
                                                    {
                                                        bi.FullName = fullName;
                                                        bi.FullNameLocked = true;
                                                        if (bi.CUSIP.Length == 0) bi.CUSIP = CUSIP;
                                                    }
                                                }

                                                stk.FullName = fullName;

                                                CollectionTool.FromString(stk.NameSuffix, fullNameSuffix);

                                                Console.WriteLine("Added new symbol from Unknown" + stk.ToString());
                                                break;

                                            default:

                                                break;
                                        }
                                    }
                                    catch (Exception e) when (e is IOException || e is FormatException)
                                    {
                                        var uc = CheckIn(symbolName, exchangeCode, typeCode);
                                        uc.LastCheckedTime = lastCheckedTime;
                                        uc.ExchangeSuffix = exchangeSuffix;
                                        uc.FullName = fullName;
                                        uc.FullNameSuffix = fullNameSuffix;
                                        uc.ConId = conId;
                                        uc.ISIN = ISIN;
                                        uc.CUSIP = CUSIP;

                                        //CheckIn(lastCheckedTime, symbolName, typeCode, businessName, suffix, conId, ISIN, CUSIP, exchangeCode, exSuffix);
                                    }
                                }
                                else
                                {
                                    var uc = CheckIn(symbolName, exchangeCode, typeCode);
                                    uc.LastCheckedTime = lastCheckedTime;
                                    uc.ExchangeSuffix = exchangeSuffix;
                                    uc.FullName = fullName;
                                    uc.FullNameSuffix = fullNameSuffix;
                                    uc.ConId = conId;
                                    uc.ISIN = ISIN;
                                    uc.CUSIP = CUSIP;
                                }
                            }
                        }
                    }
            }
        }

        #endregion File system
    }
}
