/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Master List of all symbols
    /// </summary>
    public static class ContractManager
    {
        /// <summary>
        /// The Master List of all symbols are here
        /// </summary>
        private static ConcurrentDictionary<(string name, Exchange exchange, string typeName), Contract> ContractLUT { get; }
            = new ConcurrentDictionary<(string name, Exchange exchange, string typeName), Contract>();

        public static int Count => ContractLUT.Count;
        public static bool IsEmpty => Count == 0;
        public static IEnumerable<Contract> Values => ContractLUT.Values;

        public static bool Remove((string name, Exchange exchange, string typeName) info)
        {
            bool successful = ContractLUT.TryRemove(info, out _);
            return successful;
        }

        public static T GetOrAdd<T>(T c) where T : Contract
        {
            if (!ContractLUT.ContainsKey(c.Info))
                ContractLUT.TryAdd(c.Info, c);

            return (T)ContractLUT[c.Info];
        }

        public static IEnumerable<Contract> GetList(int conId)
            => Values.Where(val => val.ConId == conId);

        public static IEnumerable<Contract> GetList(string symbol)
            => Values.Where(val => val.Name == symbol.ToUpper());

        public static IEnumerable<Contract> GetList(string symbol, Exchange exchange)
            => Values.Where(val => val.Name == symbol.ToUpper() && val.Exchange == exchange);

        public static IEnumerable<Contract> GetList(IEnumerable<string> symbols)
            => Values.Where(val => symbols.Contains(val.Name));

        public static IEnumerable<Contract> GetList(string symbol, IEnumerable<Exchange> exchanges)
            => Values.Where(val => val.Name == symbol.ToUpper() && exchanges.Contains(val.Exchange));

        private static IEnumerable<Contract> GetList(IEnumerable<string> symbols, IEnumerable<Exchange> exchanges)
            => Values.Where(val => symbols.Contains(val.Name) && exchanges.Contains(val.Exchange));

        public static IEnumerable<Contract> GetList(string symbol, string countryCode)
            => GetList(symbol, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>()?.Region.Name == countryCode));

        public static IEnumerable<Contract> GetList(IEnumerable<string> symbols, string countryCode)
            => GetList(symbols, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>()?.Region.Name == countryCode));

        public static IEnumerable<Contract> GetListByCountry(string countryCode)
            => Values.AsParallel().Where(val => Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>()?.Region.Name == countryCode).Contains(val.Exchange));

        #region Fetch / Update

        public static Contract[] Fetch(string symbol, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractSamples(symbol, cts);








        public static Contract[] Fetch(string symbol, Exchange exchange, CancellationTokenSource cts = null)
        {
            if (exchange == Exchange.UNKNOWN) return new Contract[] { };

            var clist = GetList(symbol, exchange).Where(n => n.Status == ContractStatus.Alive && (DateTime.Now - n.UpdateTime).TotalDays < 100);

            if (clist.Count() == 0)
            {
                clist = Fetch(symbol, cts).Where(n => n.Name == symbol && n.Exchange == exchange);

                foreach (Contract c in clist)
                {
                    if (cts.IsCancellationRequested) break;
                    Fetch(c, cts);
                }
            }

            return clist.ToArray();
        }










        public static void Fetch(Contract c, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(c, cts);

        public static Contract Fetch(int conId, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(conId, cts);

        public static Contract GetOrFetch(int conId, bool forceUpdate = false, CancellationTokenSource cts = null)
        {
            if (conId > 0)
            {
                var list = GetList(conId);

                if (list.Count() == 1 && !forceUpdate)
                    return list.First();
                else if (IB.Client.Connected)
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

            if ((list.Count() == 0 || forceUpdate) && IB.Client.Connected)
            {
                var dataDownload = Fetch(symbol, cts).Where(n => n.Name == symbol);
                if (dataDownload.Count() > 0)
                    foreach (Contract c in dataDownload)
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
            GetList(symbols).Where(n => (DateTime.Now - n.UpdateTime).Days < 7).Select(n => n.Name).ToList().ForEach(n => existing_symbols.CheckAdd(n));

            var non_existing_symbols = symbols.AsParallel().Where(n => !existing_symbols.Contains(n));
            int count = non_existing_symbols.Count();

            if (count > 0 && IB.Client.Connected)
            {
                int i = 0;
                foreach (string symbol in non_existing_symbols)
                {
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested)
                        break;
                    else
                    {
                        Thread.Sleep(10);
                        if (Fetch(symbol, cts) is Contract[] cx)
                        {
                            var clist = cx.Where(n => (DateTime.Now - n.UpdateTime).Days > 6);
                            foreach (Contract c in clist)
                            {
                                if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested)
                                    break;
                                else
                                    Fetch(c, cts);
                            }

                        }
                        progress?.Report(100.0f * i / count);
                        i++;
                    }
                }
            }

            return GetList(symbols);
        }

        public static IEnumerable<Contract> GetOrFetch(IEnumerable<string> symbols, string countryCode, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<string> existing_symbols = new List<string>();
            GetList(symbols, countryCode).Where(n => (DateTime.Now - n.UpdateTime).Days < 7).Select(n => n.Name).ToList().ForEach(n => existing_symbols.CheckAdd(n));

            var non_existing_symbols = symbols.AsParallel().Where(n => !existing_symbols.Contains(n) && (!UnknownContractList.Contains(n)));
            int count = non_existing_symbols.Count();
            Console.WriteLine("non_existing_symbols.Count() = " + count);

            if (count > 0 && IB.Client.Connected)
            {
                int i = 0;
                foreach (string symbol in non_existing_symbols)
                {
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested)
                        break;
                    else
                    {
                        Thread.Sleep(10);
                        if (Fetch(symbol, cts) is Contract[] cs0)
                        {
                            var clist = cs0.Where(n => (DateTime.Now - n.UpdateTime).Days > 6);
                            foreach (Contract c in clist)
                            {
                                if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested)
                                    break;
                                else
                                    Fetch(c, cts);
                            }
                        }
                        progress?.Report(100.0f * i / count);
                        i++;
                    }
                }
            }

            return GetList(symbols, countryCode);
        }

        public static HashSet<string> GetSymbolList(ref string text)
        {
            string[] symbolFields = text.Replace('/', ',').CsvReadFields();
            HashSet<string> symbolItemList = new HashSet<string>();
            HashSet<string> symbolList = new HashSet<string>();
            foreach (string field in symbolFields)
            {
                string symbol = field.TrimCsvValueField().ToUpper();

                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    symbolItemList.CheckAdd("\"" + symbol + "\"");
                    symbolList.CheckAdd(symbol);
                }

            }
            text = symbolItemList.ToString(", ");
            return symbolList;
        }

        #endregion Fetch / Update

        #region Database Tools

        // Download Nasdaq traded list, Update Contract data, Remove duplicated stocks (and its BarTable maybe???)

        /// <summary>
        /// TODO: Test
        /// </summary>
        /// <param name="cts"></param>
        /// <param name="progress"></param>
        public static void UpdateContractData(string countryCode, Func<Contract, bool> searchFunc, CancellationTokenSource cts, IProgress<float> progress)
        {
            var c0List = string.IsNullOrWhiteSpace(countryCode) ? Values : GetListByCountry(countryCode);
            var cList = searchFunc is null ? c0List.AsParallel().Where(n => n.NeedUpdate) : c0List.AsParallel().Where(searchFunc);


            //var cList = GetListByCountry("US").Where(n => (DateTime.Now - n.UpdateTime).Minutes > 150);
            //var cList = GetListByCountry("US").Where(n => n is IBusiness ib && ib.Industry is null);// string.IsNullOrWhiteSpace( stk.Industry));
            //Console.WriteLine("Update time based count = " + tlist.Count() + "; Industry File Check count = " + t2list.Count());


            int count = cList.Count();

            Console.WriteLine("\nUpdate count = " + count + "\n");

            int i = 0;

            foreach (Contract c in cList)
            {
                if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                if (c is IBusiness ib) { ib.Industry = string.Empty; ib.Category = string.Empty; ib.Subcategory = string.Empty; }

                Thread.Sleep(10);
                //Fetch(c.Name, cts);
                Fetch(c, cts);
                i++;
                progress?.Report(i * 100.0f / count);
            }
        }

        public static IEnumerable<Stock> RemoveDuplicateStock(string countryCode, CancellationTokenSource cts)
        {
            IEnumerable<Stock> cList = Values.AsParallel().Where(n => n is Stock s && s.Country == countryCode).Select(n => (Stock)n); // && s.Exchange != Exchange.OTCMKT && s.Exchange != Exchange.OTCBB);

            Dictionary<string, int> duplicate = new Dictionary<string, int>();

            foreach (Stock c in cList)
            {
                string name = c.Name;

                if (!duplicate.ContainsKey(name))
                    duplicate.Add(name, 1);
                else
                    duplicate[name]++;
            }

            var result = duplicate.AsParallel().Where(n => n.Value > 1).SelectMany(n => cList.Where(c => c.Name == n.Key));

            foreach (Stock s in result)
            {
                if (cts is CancellationTokenSource cs1 && cs1.IsCancellationRequested)
                    break;
                else
                    Fetch(s, cts);

                Console.WriteLine(s.Status + " | " + s.ToString());
            }

            var toDelete = result.AsParallel().Where(n => n.Status != ContractStatus.Alive).ToList();

            foreach (Stock s in toDelete)
            {
                Remove(s.Info);
                Console.WriteLine("Removing: " + s.Status + " | " + s.ToString());
            }

            return result;
        }

        /// <summary>
        /// ftp://ftp.nasdaqtrader.com/symboldirectory/
        /// nasdaqtraded.txt
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cts"></param>
        /// <param name="progress"></param>
        public static void ImportNasdaq(string fileName, CancellationTokenSource cts, IProgress<float> progress)
        {
            if (File.Exists(fileName))
            {
                var regexItem = new System.Text.RegularExpressions.Regex("^[A-Z0-9 ]*$");

                long totalSize = new FileInfo(fileName).Length;
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs);
                string[] headers = sr.ReadLine().Split('|');

                long byteread = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    byteread += line.Length + 1;

                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                    progress?.Report(byteread * 100.0f / totalSize);

                    string[] fields = line.Split('|');

                    if (fields.Length == 12 && (fields[5] == "Y" || fields[5] == "N") && fields[1] != "ZEXIT" && fields[1] != "ZIEXT" && fields[1] != "ZXIET")
                    {
                        bool isETF = fields[5] == "Y";

                        string symbolName = fields[1];
                        string fullName = fields[2];

                        if (regexItem.IsMatch(symbolName))
                        {
                            fullName = fullName.Replace(" - Common Stock", "").Replace(" Common Stock", "");
                        }

                        symbolName = symbolName.Replace("$", " PR").Replace(".", " ");
                        Exchange exchange = fields[3] switch
                        {
                            "N" => Exchange.NYSE,
                            "P" => Exchange.ARCA,
                            "Q" => Exchange.NASDAQ,
                            "A" => Exchange.AMEX,
                            "Z" => Exchange.BATS,
                            "V" => Exchange.IEX,
                            _ => throw new Exception("Unknown exchange"),
                        };

                        var searchList = GetOrFetch(symbolName, "US", false, cts).Where(n => n is Stock && n.Exchange == exchange);

                        if (searchList.Count() > 0 && searchList.First() is Stock stk)
                        {
                            stk.FullName = fullName;
                            if (isETF && !stk.NameSuffix.Contains("ETN") && !stk.NameSuffix.Contains("ETF")) stk.NameSuffix.Add("ETF");

                            stk.UpdateTime = DateTime.Now;
                        }

                        //if (!regexItem.IsMatch(symbolName)) Console.WriteLine(symbolName + ",\t" + exchange + ",\t" + isETF + ",\t" + fullName);
                        //if(symbolName.Any(n => !char.IsLetterOrDigit(n))) Console.WriteLine(symbolName + ",\t" + exchange + ",\t" + isETF + ",\t" + fullName);
                    }
                }
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
                Parallel.ForEach(list, c => { c.SaveMarketData(); });
            }
        }

        public static void Load()
        {
            var list = Serialization.DeserializeJsonFile<Contract[]>(FileName);
            if (list is null) return;

            Parallel.ForEach(list, c =>
            {
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

            List<Contract> sorted = ContractLUT.Values.AsParallel()
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

                        /*
                        if (stk.Exchange == Exchange.NASDAQ)
                        {
                            if (stk.ExchangeSuffix != "NMS" && stk.ExchangeSuffix != "SCM")
                            {
                                stk.ExchangeSuffix = string.Empty;
                            }
                        }
                        else if (stk.Exchange == Exchange.OTCMKT)
                        {
                            if (stk.ExchangeSuffix != "CURRENT" && stk.ExchangeSuffix != "LIMITED" && stk.ExchangeSuffix != "NOINFO")
                            {
                                stk.ExchangeSuffix = string.Empty;
                            }
                        }
                        else
                        {
                            stk.ExchangeSuffix = string.Empty;
                        }

                        if (stk.NameSuffix.Contains(string.Empty)) stk.NameSuffix.Remove(string.Empty);
                        */

                        string[] name_fields = stk.Name.Split(' ');
                        if (name_fields.Length == 2 && name_fields[1].Length > 0)
                        {
                            stk.NameSuffix.CheckAdd(name_fields[1]);
                        }

                        if (stk.FullName.EndsWith(" ETF"))
                        {
                            stk.NameSuffix.CheckAdd("ETF");
                            stk.FullName = stk.FullName.ReplaceEnd(" ETF", "");
                            stk.NameSuffix.CheckRemove("ETN");
                        }

                        if (stk.FullName.EndsWith(" ETN"))
                        {
                            stk.NameSuffix.CheckAdd("ETN");
                            stk.FullName = stk.FullName.ReplaceEnd(" ETN", "");
                            stk.NameSuffix.CheckRemove("ETF");
                        }

                        if (stk.FullName.Contains("American Depository Receipt") || stk.FullName.Contains(" ADR "))
                        {
                            stk.NameSuffix.CheckAdd("ADR");
                        }

                        if (stk.FullName.EndsWith(" ADR"))
                        {
                            stk.FullName = stk.FullName.ReplaceEnd(" ADR", "");
                            stk.NameSuffix.CheckAdd("ADR");
                        }

                        string[] items = {
                            stk.Status.ToString(),
                            stk.TypeName.ToString(),
                            stk.ConId.ToString(),
                            stk.Name.CsvEncode(),
                            stk.Exchange.ToString(),
                            stk.ExchangeSuffix.ToString(),
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

                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                    progress?.Report(byteread * 100.0f / totalSize);

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

                                    if (stk.Status == ContractStatus.Unknown)
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
                                        //if (stk.Status == ContractStatus.Alive) stk.Status = ContractStatus.Unknown;
                                        if (stk.ISIN.Length < 2) stk.ISIN = fields[8].TrimCsvValueField();
                                        if (stk.FullName.Length < 4) stk.FullName = fields[6].TrimCsvValueField();
                                    }
                                }
                                break;
                        }
                    }

                }
            }
        }

        #endregion Export / Import

        #endregion Database Tools
    }
}
