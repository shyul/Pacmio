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
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Xu;

namespace Pacmio
{
    using UnknownItemTable = Dictionary<(string TypeCode, string SymbolName, string ExchangeCode),
       (ContractStatus Status, int ConId, string ExSuffix, string BusinessName, string Suffix, string ISIN, string CUSIP)>;

    /// <summary>
    /// Master List of all unknown symbols
    /// </summary>
    public static class UnknownItemList
    {
        public static Dictionary<string, Dictionary<string, HashSet<string>>> Industry { get; private set; } = new Dictionary<string, Dictionary<string, HashSet<string>>>();


        /// <summary>
        /// Stores previously found invalid symbol names here to prevent any further repeating query.
        /// </summary>
        private static UnknownItemTable List { get; set; } = new UnknownItemTable();

        public static bool Add(string symbolName, string typeCode, string businessName = "", string suffix = "", int conId = 0, string ISIN = "", string CUSIP = "", string exchangeCode = "", string exSuffix = "")
        {
            (string TypeCode, string SymbolName, string ExchangeCode) key = (typeCode, symbolName, exchangeCode);

            (ContractStatus Status, int ConId, string ExSuffix, string BusinessName, string Suffix, string ISIN, string CUSIP) value = (ContractStatus.Unknown, conId, exSuffix, businessName, suffix, ISIN, CUSIP);

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

        public static bool Contains(string value) { lock (List) return List.Where(n => n.Key.SymbolName == value).Count() > 0; }

        public static void Rescan(IProgress<int> progress)
        {
            int total = List.Count;
            HashSet<string> symbols = new HashSet<string>();
            UnknownItemTable temp = new UnknownItemTable();

            lock (List)
            {
                for (int i = 0; i < total; i++)
                {
                    (string TypeCode, string SymbolName, string ExchangeCode) key = List.ElementAt(i).Key;
                    (ContractStatus Status, int ConId, string ExSuffix, string BusinessName, string Suffix, string ISIN, string CUSIP) value = List.ElementAt(i).Value;

                    if (value.ConId > 0)
                    {
                        temp[(key.TypeCode, Quandl.ConvertToPacmioOrIbSymbolName(key.SymbolName), key.ExchangeCode)] = value;
                        //temp[(key.TypeCode, Quandl.ConvertSymbolName(key.SymbolName), "_VALUE")] = value;
                    }
                    else if (ContractList.GetList(key.SymbolName).Count() == 0)
                    {

                        temp[(key.TypeCode, Quandl.ConvertToPacmioOrIbSymbolName(key.SymbolName), key.ExchangeCode)] = value;
                    }

                    Console.Write(key.SymbolName + ". ");
                    /*
                    if (key.ExchangeCode.Length == 0)
                        if (GetSymbols(key.SymbolName).Count == 0)
                        {
                            if (!symbols.Contains(key.SymbolName)) symbols.Add(key.SymbolName);
                        }
                        */
                    float percent = i * 100 / total;
                    if (percent % 1 == 0) progress.Report((int)percent);
                }

                List.Clear();
            }

            //IBClient.RequestMatchSymbols(progress, symbols.ToArray());
            foreach (var item in temp)
            {
                List[item.Key] = item.Value;
            }
        }

        #region File system

        private static string FileName => Root.ResourcePath + "UnknownItems.csv";

        public static void Save()
        {
            Industry.SerializeJsonFile(Root.ResourcePath + "Industry.Json");

            StringBuilder sb = new StringBuilder("Status,Type,Contract ID,Symbol,Exchange,ExSuffix,Business Name,Suffix,ISIN,CUSIP\n");

            var sorted = List.OrderBy(n => n.Key.SymbolName);

            lock (List)
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
                        item.Value.CUSIP.CsvEncode());
                }

            sb.ToFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(Root.ResourcePath + "Industry.Json"))
            {
                Industry = Serialization.DeserializeJsonFile<Dictionary<string, Dictionary<string, HashSet<string>>>>(Root.ResourcePath + "Industry.Json");
            }

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

                            if (fields.Length == 10)
                            {
                                /*
                                0 Status + "," +
                                1 TypeCode.CsvEncode() + "," +
                                2 ConId + "," +
                                3 SymbolName.CsvEncode() + "," +
                                4 ExchangeCode.CsvEncode() + "," +
                                5 ExSuffix.CsvEncode() + "," +
                                6 BusinessName.CsvEncode() + "," +
                                7 Suffix.CsvEncode() + "," +
                                8 ISIN.CsvEncode() + "," +
                                9 CUSIP.CsvEncode());
                                */

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

                                if (fields[1].Length > 0 && (fields[4].Length > 0 && !fields[4].Contains("_")))
                                {
                                    try
                                    {
                                        switch (typeCode)
                                        {
                                            case ("STOCK"):
                                                Exchange exchange = exchangeCode.ParseEnum<Exchange>();
                                                Stock c = ContractList.GetOrAdd(new Stock(symbolName, exchange));
                                                c.Status = status;
                                                c.ConId = conId;
                                                c.ExchangeSuffix = exSuffix;

                                                if (c is IBusiness ib)
                                                {
                                                    ib.ISIN = ISIN;

                                                    if (ib.BusinessInfo is BusinessInfo bi)
                                                    {
                                                        bi.FullName = businessName;
                                                        bi.FullNameLocked = true;
                                                        if (bi.CUSIP.Length == 0) bi.CUSIP = CUSIP;
                                                    }
                                                }

                                                c.FullName = businessName;

                                                CollectionTool.FromString(c.NameSuffix, suffix);

                                                Log.Print("Added new symbol from Unknown" + c.ToString());
                                                break;

                                            default:

                                                break;
                                        }
                                    }
                                    catch (Exception e) when (e is IOException || e is FormatException)
                                    {
                                        Add(symbolName, typeCode, businessName, suffix, conId, ISIN, CUSIP, exchangeCode, exSuffix);
                                    }
                                }
                                else
                                    Add(symbolName, typeCode, businessName, suffix, conId, ISIN, CUSIP, exchangeCode, exSuffix);
                            }
                        }
                    }
            }
        }

        #endregion File system
    }
}
