/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    using UnknownItemTable = Dictionary<(string TypeCode, string SymbolName, string ExchangeCode),
       (ContractStatus Status, DateTime LastCheckedTime, int ConId, string ExSuffix, string BusinessName, string Suffix, string ISIN, string CUSIP)>;

    /// <summary>
    /// Master List of all unknown symbols
    /// </summary>
    public static class UnknownItemList
    {
        /// <summary>
        /// Stores previously found invalid symbol names here to prevent any further repeating query.
        /// </summary>
        private static UnknownItemTable List { get; set; } = new UnknownItemTable();

        public static bool Add(DateTime lastCheckedTime, string symbolName, string typeCode = "_NO_SEC_TYPE_", string businessName = "", string suffix = "", int conId = 0, string ISIN = "", string CUSIP = "", string exchangeCode = "", string exSuffix = "")
        {
            var key = (typeCode, symbolName, exchangeCode);
            var value = (ContractStatus.Unknown, lastCheckedTime, conId, exSuffix, businessName, suffix, ISIN, CUSIP);

            lock (List)
            {
                if (!List.ContainsKey(key))
                {
                    List.Add(key, value);
                    return true;
                }
                else
                {
                    List[key] = value;
                    return false;
                }
            }
        }

        public static bool Contains(string symbolName) { lock (List) return List.Where(n => n.Key.SymbolName == symbolName).Count() > 0; }

        public static void Rescan(IProgress<int> progress)
        {
            int total = List.Count;

            UnknownItemTable temp = new UnknownItemTable();

            lock (List)
            {
                for (int i = 0; i < total; i++)
                {
                    (string TypeCode, string SymbolName, string ExchangeCode) = List.ElementAt(i).Key;

                    var value = List.ElementAt(i).Value;
                    //(ContractStatus Status, DateTime LastCheckedTime, int ConId, string ExSuffix, string BusinessName, string Suffix, string ISIN, string CUSIP) = value;

                    if (value.ConId > 0 || ContractList.GetList(SymbolName).Count() == 0)
                    {
                        temp[(TypeCode, Quandl.ConvertToPacmioOrIbSymbolName(SymbolName), ExchangeCode)] = value;
                    }

                    Console.Write(SymbolName + ". ");

                    float percent = i * 100 / total;
                    if (percent % 1 == 0) progress.Report((int)percent);
                }

                List.Clear();
            }

            foreach (var item in temp)
            {
                List[item.Key] = item.Value;
            }
        }

        #region File system

        private static string FileName => Root.ResourcePath + "UnknownItems.csv";

        public static void Save()
        {
            StringBuilder sb = new StringBuilder("Status,Type,Contract ID,Symbol,Exchange,ExSuffix,Business Name,Suffix,ISIN,CUSIP,LastCheckedTime\n");

            lock (List) 
            {
                var sorted = List.OrderBy(n => n.Key.SymbolName).ThenBy(n => n.Value.LastCheckedTime);
                foreach (var item in sorted)
                {
                    sb.AppendLine(
                        item.Value.Status + "," +
                        item.Key.TypeCode.CsvEncode() + "," +
                        item.Value.ConId + "," +
                        item.Key.SymbolName.CsvEncode() + "," +
                        item.Key.ExchangeCode.CsvEncode() + "," +
                        item.Value.ExSuffix.CsvEncode() + "," +
                        item.Value.BusinessName.CsvEncode() + "," +
                        item.Value.Suffix.CsvEncode() + "," +
                        item.Value.ISIN.CsvEncode() + "," +
                        item.Value.CUSIP.CsvEncode() + "," +
                        item.Value.LastCheckedTime.ToString());
                }
            }

            sb.ToFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                lock (List)
                    using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string[] headers = sr.CsvReadFields();

                        List.Clear();

                        while (!sr.EndOfStream)
                        {
                            string[] fields = sr.CsvReadFields();

                            if (fields.Length == 11)
                            {
                                ContractStatus status = fields[0].ParseEnum<ContractStatus>();
                                int conId = fields[2].Trim().ToInt32(0);
                                string symbolName = fields[3].TrimCsvValueField();
                                string typeCode = fields[1].TrimCsvValueField().ToUpper();
                                string exchangeCode = fields[4].TrimCsvValueField();
                                string exSuffix = fields[5].TrimCsvValueField();
                                string businessName = fields[6].TrimCsvValueField();
                                string suffix = fields[7].TrimCsvValueField();
                                string ISIN = fields[8].TrimCsvValueField();
                                string CUSIP = fields[9].TrimCsvValueField();
                                DateTime lastCheckedTime = (fields[10].TrimCsvValueField() is string timeString && !string.IsNullOrEmpty(timeString)) ? DateTime.Parse(timeString) : DateTime.MinValue;

                                if (typeCode.Length > 0 && (exchangeCode.Length > 0 && !exchangeCode.Contains("_")))
                                {
                                    try
                                    {
                                        switch (typeCode)
                                        {
                                            case ("STOCK"):
                                                Exchange exchange = exchangeCode.ParseEnum<Exchange>();
                                                Stock stk = ContractList.GetOrAdd(new Stock(symbolName, exchange));
                                                stk.Status = status;
                                                stk.ConId = conId;
                                                stk.ExchangeSuffix = exSuffix;

                                                if (stk is IBusiness ib)
                                                {
                                                    ib.ISIN = ISIN;

                                                    if (ib.BusinessInfo is BusinessInfo bi)
                                                    {
                                                        bi.FullName = businessName;
                                                        bi.FullNameLocked = true;
                                                        if (bi.CUSIP.Length == 0) bi.CUSIP = CUSIP;
                                                    }
                                                }

                                                stk.FullName = businessName;

                                                CollectionTool.FromString(stk.NameSuffix, suffix);

                                                Console.WriteLine("Added new symbol from Unknown" + stk.ToString());
                                                break;

                                            default:

                                                break;
                                        }
                                    }
                                    catch (Exception e) when (e is IOException || e is FormatException)
                                    {
                                        Add(lastCheckedTime, symbolName, typeCode, businessName, suffix, conId, ISIN, CUSIP, exchangeCode, exSuffix);
                                    }
                                }
                                else 
                                {
                                    if (typeCode == "_STK")
                                        typeCode = "STK";
                                    else if (string.IsNullOrWhiteSpace(typeCode))
                                        typeCode = "_NO_SEC_TYPE_";

                                    Add(lastCheckedTime, symbolName, typeCode, businessName, suffix, conId, ISIN, CUSIP, exchangeCode, exSuffix);
                                }
                            }
                        }
                    }
            }
        }

        #endregion File system
    }
}
