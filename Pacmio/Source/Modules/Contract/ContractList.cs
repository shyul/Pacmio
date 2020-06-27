/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xu;
using System.Diagnostics.Contracts;

namespace Pacmio
{
    /// <summary>
    /// Master List of all symbols
    /// </summary>
    public static class ContractList
    {
        /// <summary>
        /// The Master List of all symbols are here
        /// </summary>
        private static readonly ConcurrentDictionary<(string name, Exchange exchange, string typeName), Contract> List
            = new ConcurrentDictionary<(string name, Exchange exchange, string typeName), Contract>();

        public static int Count => List.Count;
        public static bool IsEmpty => Count == 0;
        public static bool Remove((string name, Exchange exchange, string typeName) info) => List.TryRemove(info, out _);
        public static IEnumerable<Contract> Values => List.Values;
        public static T GetOrAdd<T>(T c) where T : Contract
        {
            if (!List.ContainsKey(c.Info))
                List.TryAdd(c.Info, c);

            return (T)List[c.Info];
        }

        public static IEnumerable<Contract> GetList(int conId)
            => Values.Where(val => val.ConId == conId);

        public static IEnumerable<Contract> GetList(string symbol)
            => Values.Where(val => val.Name == symbol.ToUpper());

        public static IEnumerable<Contract> GetList(IEnumerable<string> symbols)
            => Values.Where(val => symbols.Contains(val.Name));

        private static IEnumerable<Contract> GetList(string symbol, IEnumerable<Exchange> exchanges)
            => Values.Where(val => val.Name == symbol.ToUpper() && exchanges.Contains(val.Exchange));

        private static IEnumerable<Contract> GetList(IEnumerable<string> symbols, IEnumerable<Exchange> exchanges)
            => Values.Where(val => symbols.Contains(val.Name) && exchanges.Contains(val.Exchange));

        private static IEnumerable<Contract> GetList(string symbol, string countryCode)
            => GetList(symbol, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == countryCode));

        public static IEnumerable<Contract> GetList(IEnumerable<string> symbols, string countryCode)
            => GetList(symbols, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == countryCode));

        #region Fetch / Update

        public static Contract[] Fetch(string symbol, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractSamples(symbol, cts);

        public static void Fetch(Contract c, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(c, cts);

        public static Contract Fetch(int conId, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(conId, cts);

        public static Contract GetOrFetch(int conId, bool forceUpdate = false, CancellationTokenSource cts = null)
        {
            if (conId > 0)
            {
                var list = GetList(conId);

                if (list.Count() == 1 && !forceUpdate)
                    return list.First();
                else if (Root.IBConnected)
                {
                    if (list.Count() > 1)
                    {
                        Contract[] toRemove = list.ToArray();
                        foreach (Contract c in toRemove)
                            Remove(c.Info);
                    }

                    return Fetch(conId, cts);
                }
            }

            return null;
        }

        public static IEnumerable<Contract> GetOrFetch(string symbol, string countryCode, bool forceUpdate = false, CancellationTokenSource cts = null)
        {
            var list = GetList(symbol, countryCode).Where(n => n.ConId > 0);

            if ((list.Count() == 0 || forceUpdate) && Root.IBConnected)
            {
                Contract[] clist = Fetch(symbol, cts);
                foreach (Contract c in clist)
                {
                    if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested) break;
                    Fetch(c, cts);
                }
                return GetList(symbol, countryCode).Where(n => n.ConId > 0);
            }
            else
                return list;
        }

        public static IEnumerable<Contract> GetOrFetch(IEnumerable<string> symbols, CancellationTokenSource cts, IProgress<float> progress)
        {
            HashSet<string> existing_symbols = new HashSet<string>();
            GetList(symbols).Select(n => n.Name).ToList().ForEach(n => existing_symbols.Add(n));

            var non_existing_symbols = symbols.AsParallel().Where(n => !existing_symbols.Contains(n));
            int count = non_existing_symbols.Count();

            if (count > 0 && Root.IBConnected)
            {
                int i = 0;
                foreach (string symbol in non_existing_symbols)
                {
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                    progress?.Report(100.0f * i / count);
                    Thread.Sleep(10);
                    Contract[] clist = Fetch(symbol, cts);
                    foreach (Contract c in clist)
                    {
                        if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested) break;
                        Fetch(c, cts);
                    }
                    i++;
                }
            }

            return GetList(symbols);
        }

        public static IEnumerable<Contract> GetOrFetch(IEnumerable<string> symbols, string countryCode, CancellationTokenSource cts, IProgress<float> progress)
        {
            HashSet<string> existing_symbols = new HashSet<string>();
            GetList(symbols, countryCode).Select(n => n.Name).ToList().ForEach(n => existing_symbols.Add(n));

            var non_existing_symbols = symbols.AsParallel().Where(n => !existing_symbols.Contains(n));
            int count = non_existing_symbols.Count();

            if (count > 0 && Root.IBConnected)
            {
                int i = 0;
                foreach (string symbol in non_existing_symbols)
                {
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                    progress?.Report(100.0f * i / count);
                    Thread.Sleep(10);
                    Contract[] clist = Fetch(symbol, cts);
                    foreach(Contract c in clist)
                    {
                        if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested) break;
                        Fetch(c, cts);
                    }
                    i++;
                }
            }

            return GetList(symbols, countryCode);
        }

        #endregion Fetch / Update

        #region Database Tools

        /// <summary>
        /// TODO: Test
        /// </summary>
        /// <param name="cts"></param>
        /// <param name="progress"></param>
        public static void UpdateContractData(CancellationTokenSource cts, IProgress<float> progress)
        {
            var cList = Values.Where(n => n.NeedUpdate);

            int count = cList.Count();
            int i = 0;

            foreach (Contract c in cList)
            {
                if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                Thread.Sleep(10);
                Fetch(c.Name, cts);
                Fetch(c, cts);
                i++;
                progress?.Report(i * 100.0f / count);
            }
        }

        #region File Operation

        private static string FileName => Root.ResourcePath + "Symbols.Json";

        public static void Save()
        {
            if (!IsEmpty)
            {
                Contract[] list = Values.ToArray();
                list.SerializeJsonFile(FileName);
                Parallel.ForEach(list, c => c.SaveMarketData());
            }
        }

        public static void Load()
        {
            var list = Serialization.DeserializeJsonFile<Contract[]>(FileName);
            if (list is null) return;

            Parallel.ForEach(list, c => {
                //c.LoadMarketData();
               
                //c.MarketData.Status = MarketTickStatus.Unknown;
                /*
                c.DerivativeTypes = new HashSet<string>();
                c.ValidExchanges = new HashSet<string>();
                c.OrderTypes = new HashSet<string>();
                c.MarketRules = new HashSet<string>();
                if (c is ITradable it && it.TradingPeriods is null) 
                    it.TradingPeriods = new MultiPeriod();
                */
                /*
                if(c is IHistoricalBar ihb) 
                {
                    ihb.DividendTable = new Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)>();
                    ihb.SplitTable = new Dictionary<DateTime, (DataSource DataSource, double Split)>();
                }*/
                GetOrAdd(c);
            });
        }

        #endregion File Operation

        #region Export / Import

        public static bool ExportCSV(string fileName, CancellationTokenSource cts, IProgress<float> progress)
        {
            StringBuilder sb = new StringBuilder("Status,Type,Contract ID,Symbol,Exchange,ExSuffix,Business Name,Suffix,ISIN,CUSIP\n");

            Console.WriteLine("Start sorting the list!");

            List<Contract> sorted = List.Values.AsParallel()
                .OrderBy(n => n.TypeName)
                .ThenBy(n => n.Exchange)
                .ThenBy(n => n.Name)
                .ToList();

            int pt = 0;
            foreach (Contract c in sorted)
            {
                if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;

                switch (c)
                {
                    case Stock stk:
                        string cusip = (stk.BusinessInfo is BusinessInfo bi) ? bi.CUSIP : string.Empty;
                        string[] items = {
                            stk.Status.ToString(),
                            stk.TypeName.ToString(),
                            stk.ConId.ToString(),
                            stk.Name.CsvEncode(),
                            stk.Exchange.ToString(),
                            stk.ExchangeSuffix.CsvEncode(),
                            stk.FullName.CsvEncode(),
                            CollectionTool.ToString(stk.NameSuffix),
                            stk.ISIN.CsvEncode(),
                            cusip.CsvEncode(),
                        };
                        sb.AppendLine(string.Join(",", items));
                        break;
                }

                pt++;
                progress?.Report(pt * 100 / Count);
            }

            return sb.ToFile(fileName);
        }

        public static void ImportCSV(string fileName, CancellationTokenSource cts, IProgress<float> progress)
        {
            long byteread = 0;

            if (File.Exists(fileName))
            {
                long totalSize = new FileInfo(fileName).Length;

                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs);
                string[] headers = sr.ReadLine().Split(',');

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    byteread += line.Length + 1;
                    float percent = byteread * 100.0f / totalSize;

                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;

                    string[] fields = line.CsvReadFields();
                    if (fields.Length == 10)
                    {
                        string typeName = fields[1].ToUpper();

                        switch (typeName)
                        {
                            case ("STOCK"):
                                if (fields[4] != "VALUE")
                                {
                                    Exchange exchange = fields[4].ParseEnum<Exchange>();
                                    Stock stk = GetOrAdd(new Stock(fields[3].TrimCsvValueField(), exchange) { Status = ContractStatus.Unknown, UpdateTime = DateTime.MinValue });

                                    Console.Write(stk.Name + ". ");

                                    if (stk.Status != ContractStatus.Alive)
                                    {
                                        // stk.Status = fields[0].ParseEnum<ContractStatus>(); // ContractStatus.Unknown;
                                        stk.ConId = fields[2].TrimCsvValueField().ToInt32(0);
                                        stk.ExchangeSuffix = fields[5].TrimCsvValueField();
                                        stk.ISIN = fields[8].TrimCsvValueField();

                                        if (stk.ISIN.Length != 12 && stk.ISIN.Length > 0)
                                            throw new Exception(stk.ISIN);

                                        string fullName = fields[6].TrimCsvValueField();
                                        CollectionTool.FromString(stk.NameSuffix, fields[7].TrimCsvValueField());
                                        stk.FullName = fullName;

                                        if (stk.BusinessInfo is BusinessInfo bi)
                                        {
                                            bi.CUSIP = fields[9].TrimCsvValueField();
                                            bi.IsModified = true;

                                            if (bi.CUSIP.Length != 9 && bi.CUSIP.Length > 0)
                                                throw new Exception("Invalid CUSIP: " + bi.CUSIP);
                                        }
                                    }
                                    else
                                    {
                                        if (stk.ISIN.Length < 2) stk.ISIN = fields[8].TrimCsvValueField();
                                        if (stk.FullName.Length < 4) stk.FullName = fields[6].TrimCsvValueField();
                                    }
                                }
                                break;
                        }
                    }
                    progress?.Report(percent);
                }
            }
        }

        #endregion Export / Import

        #endregion Database Tools
    }
}
