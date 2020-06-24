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
        public static IEnumerable<Contract> Values => List.Values;

        public static T GetOrAdd<T>(T c) where T : Contract
        {
            if (!List.ContainsKey(c.Info))
                List.TryAdd(c.Info, c);

            return (T)List[c.Info];
        }

        //private static (string name, Exchange exchange, ContractType type) GetKey(int pt) => List.ElementAt(pt).Key;

        public static IEnumerable<Contract> GetList(string value)
            => Values.Where(val => val.Name == value.ToUpper());
        //private static bool Contains(string value) => (GetList(value).Count() > 0 || UnknownItemList.Contains(value.ToUpper()));

        private static IEnumerable<Contract> GetList(string value, IEnumerable<Exchange> exchanges)
            => Values.Where(val => val.Name == value.ToUpper() && exchanges.Contains(val.Exchange));
        private static IEnumerable<Contract> GetList(IEnumerable<string> values, IEnumerable<Exchange> exchanges)
            => Values.Where(val => values.Contains(val.Name) && exchanges.Contains(val.Exchange));

        //private static bool Contains(string value, ICollection<Exchange> exchanges) => GetList(value, exchanges).Count() > 0;
        private static IEnumerable<Contract> GetList(string value, string countryCode)
            => GetList(value, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == countryCode));

        public static IEnumerable<Contract> GetList(IEnumerable<string> values, string countryCode)
            => GetList(values, Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == countryCode));

        public static IEnumerable<Contract> GetList(int conId) => Values.Where(val => val.ConId == conId);

        public static bool Remove((string name, Exchange exchange, string typeName) info)
        {
            return List.TryRemove(info, out _);
        }

        #region Fetch / Update

        public static Contract[] Fetch(string symbol, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractSamples(symbol, cts);

        public static void Fetch(Contract c, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(c, cts);

        public static Contract Fetch(int conId, CancellationTokenSource cts = null) => IB.Client.Fetch_ContractData(conId, cts);

        public static Contract GetOrFetch(int conId, CancellationTokenSource cts = null)
        {
            if (conId > 0)
            {
                var list = GetList(conId);

                if (list.Count() == 1)
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

        public static IEnumerable<Contract> GetOrFetch(string symbol, string countryCode, CancellationTokenSource cts = null)
        {
            var list = GetList(symbol, countryCode).Where(n => n.ConId > 0);

            if (list.Count() == 0 && Root.IBConnected)
            {
                Fetch(symbol, cts);
                return GetList(symbol, countryCode).Where(n => n.ConId > 0);
            }
            else
                return list;
        }

        public static IEnumerable<Contract> GetOrFetch(IEnumerable<string> symbols, string countryCode, CancellationTokenSource cts, IProgress<float> progress)
        {
            var existing_symbols = GetList(symbols, countryCode).Select(n => n.Name);
            var non_existing_symbols = symbols.Where(n => !existing_symbols.Contains(n));

            if (non_existing_symbols.Count() > 0 && Root.IBConnected)
            {
                int i = 0, count = non_existing_symbols.Count();
                foreach (string symbol in non_existing_symbols)
                {
                    progress?.Report(100 * i / count);
                    Thread.Sleep(10);
                    Fetch(symbol, cts);
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
        public static void UpdateContractData(CancellationTokenSource cts, IProgress<int> progress)
        {
            var cList = Values.Where(n => n.FullName.Length < 2 || (n is ITradable it && it.ISIN.Length < 2));

            int count = cList.Count();
            int pt = 0;

            foreach (Contract c in cList)
            {
                if (cts.IsCancellationRequested) return;
                Thread.Sleep(10);
                Fetch(c, cts);
                pt++;
                progress.Report(pt * 100 / count);
            }
        }

        #endregion Database Tools

        #region Background Task

        public static void StartTask(CancellationTokenSource cts)
        {
            SymbolTask = new Task(() => SymbolTaskWorker(), cts.Token);
            SymbolTask.Start();
        }

        public static void StopTask()
        {
            while (SymbolTask != null && SymbolTask?.Status == TaskStatus.Running) { Thread.Sleep(1); }
            SymbolTask?.Dispose();
        }

        #region Symbol Task
        /*
        private static void RequestFetchSymbolContracts(IEnumerable<string> symbols)
        {
            foreach (string symbol in symbols)
                RequestFetchSymbolContract(symbol);
        }

        private static void RequestFetchSymbolContract(string symbol)
        {
            if (CheckedSymbolList.ContainsKey(symbol) || SymbolTaskList.Contains(symbol)) return;
            SymbolTaskList.Enqueue(symbol);
        }
        */
        //public static CancellationTokenSource DownloadCancellationTokenSource { get; private set; }

        //public static bool IsCancellationRequested => !(DownloadCancellationTokenSource is null) && DownloadCancellationTokenSource.IsCancellationRequested;

        //public static IProgress<float> DownloadProgress { get; private set; }

        private static int DownloadTimeout { get; set; } = 1000; // 5 minutes

        private static Task SymbolTask { get; set; }

        public static bool SymbolTaskIsBusy => SymbolTaskList.Count > 0 || ContractDataTaskList.Count > 0;

        private static void WaitSymbolTaskBusy() { while (SymbolTaskIsBusy) { Thread.Sleep(100); } }

        private static readonly ConcurrentQueue<string> SymbolTaskList = new ConcurrentQueue<string>();

        private static readonly ConcurrentQueue<string> ContractDataTaskList = new ConcurrentQueue<string>();

        private static readonly ConcurrentDictionary<string, DateTime> CheckedSymbolList = new ConcurrentDictionary<string, DateTime>();

        private static void SymbolTaskWorker()
        {
            int time = 0;
            while (!Root.RequestWorkerCancel)
            {
                if (Root.IBConnected)
                {
                Start:
                    if (IsCancellationRequested)
                    {
                        while (SymbolTaskList.Count > 0)
                            SymbolTaskList.TryDequeue(out _);

                        while (ContractDataTaskList.Count > 0)
                            ContractDataTaskList.TryDequeue(out _);
                    }
                    else if (SymbolTaskList.Count > 0 && IB.Client.IsReady_ContractSamples)
                    {
                        SymbolTaskList.TryDequeue(out string symbol);

                    ContractSamples:
                        IB.Client.SendRequest_ContractSamples(symbol);

                        time = 0;
                        while (!IB.Client.IsReady_ContractSamples)
                        {
                            time++;
                            Thread.Sleep(60);
                            if (time > DownloadTimeout) // Handle Time out here.
                            {
                                IB.Client.SendCancel_ContractSamples();
                                Thread.Sleep(100);
                                goto ContractSamples;
                            }
                            else if (IsCancellationRequested)
                            {
                                IB.Client.SendCancel_ContractSamples();
                                goto Start;
                            }
                        }

                        ContractDataTaskList.Enqueue(symbol);
                        CheckedSymbolList.TryAdd(symbol, DateTime.Now);
                    }
                    else if (SymbolTaskList.Count == 0 && ContractDataTaskList.Count > 0 && IB.Client.IsReady_ContractSamples && IB.Client.IsReady_ContractData)
                    {
                        List<string> symbols = new List<string>();

                        while (ContractDataTaskList.Count > 0)
                        {
                            ContractDataTaskList.TryDequeue(out string s);
                            symbols.Add(s);
                        }

                        var cList = Values.Where(c => symbols.Contains(c.Name) && c.ConId > 0 && (DateTime.Now - c.UpdateTime).TotalDays > 1);
                        foreach (Contract c in cList)
                        {

                        ContractData:
                            IB.Client.SendRequest_ContractData(c);

                            time = 0;
                            while (!IB.Client.IsReady_ContractData)
                            {
                                time++;
                                Thread.Sleep(60);
                                if (time > DownloadTimeout) // Handle Time out here.
                                {
                                    IB.Client.SendCancel_ContractData();
                                    Thread.Sleep(100);
                                    goto ContractData;
                                }
                                else if (IsCancellationRequested)
                                {
                                    IB.Client.SendCancel_ContractSamples();
                                    goto Start;
                                }
                            }
                        }
                    }
                    else
                    {
                        var list = CheckedSymbolList.Where(n => (DateTime.Now - n.Value).TotalDays > 1).Select(n => n.Key);
                        foreach (string sym in list) CheckedSymbolList.TryRemove(sym, out _);
                    }

                    DownloadProgress?.Report(SymbolTaskList.Count + ContractDataTaskList.Count);
                    Thread.Sleep(15);
                }
                else
                    Thread.Sleep(100);
            }
        }

        #endregion Symbol Task

        #endregion Background Task

        #region File Operation

        private static string FileName => Root.ResourcePath + "Symbols.Json";

        public static void Save()
        {
            if (!IsEmpty)
            {
                Contract[] list = Values.ToArray();
                list.SerializeJsonFile(FileName);
            }
        }

        public static void Load()
        {
            var list = Serialization.DeserializeJsonFile<Contract[]>(FileName);
            if (list is null) return;
            Parallel.ForEach(list, c => {
                c.MarketData = new MarketData(c);
                GetOrAdd(c);
            });
        }

        #endregion File Operation

        #region Export / Import

        public static bool ExportCSV(string fileName, IProgress<int> progress)
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
                switch (c)
                {
                    case Stock si:
                        (bool ciValid, BusinessInfo ci) = BusinessInfoList.GetOrAdd(si);
                        string cusip = (ciValid) ? ci.CUSIP : string.Empty;
                        string[] items = {
                            si.Status.ToString(),
                            si.TypeName.ToString(),
                            si.ConId.ToString(),
                            si.Name.CsvEncode(),
                            si.Exchange.ToString(),
                            si.ExchangeSuffix.CsvEncode(),
                            si.FullName.CsvEncode(),
                            CollectionTool.ToString(si.NameSuffix),
                            si.ISIN.CsvEncode(),
                            cusip.CsvEncode(),
                        };
                        sb.AppendLine(string.Join(",", items));
                        break;
                }

                pt++;
                float percent = pt * 100 / Count;
                if (percent % 1 == 0 && percent <= 100) progress.Report((int)percent);
            }

            return sb.ToFile(fileName);
        }

        public static void ImportCSV(string fileName, IProgress<int> progress)
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

                    string[] fields = line.CsvReadFields();
                    if (fields.Length == 10)
                    {
                        string typeName = fields[1].ToUpper();

                        switch (typeName)
                        {
                            case ("STOCK"):
                                if (fields[4] != "VALUE")
                                {
                                    Exchange exchange = fields[4].ParseEnum<Exchange>(); //(Exchange)Enum.Parse(typeof(Exchange), fields[4]);

                                    Stock si = GetOrAdd(new Stock(fields[3].TrimCsvValueField(), exchange));

                                    Console.Write(si.Name + ". ");

                                    if (si.Status != ContractStatus.Alive)
                                    {
                                        si.Status = fields[0].ParseEnum<ContractStatus>(); // ContractStatus.Unknown;
                                        si.ConId = fields[2].TrimCsvValueField().ToInt32(0);
                                        si.ExchangeSuffix = fields[5].TrimCsvValueField();
                                        si.ISIN = fields[8].TrimCsvValueField();

                                        if (si.ISIN.Length != 12 && si.ISIN.Length > 0)
                                            throw new Exception(si.ISIN);

                                        string fullName = fields[6].TrimCsvValueField();
                                        CollectionTool.FromString(si.NameSuffix, fields[7].TrimCsvValueField());

                                        (bool ciValid, BusinessInfo bi) = BusinessInfoList.GetOrAdd(si);

                                        if (ciValid)
                                        {
                                            bi.FullName = fullName;
                                            bi.CUSIP = fields[9].TrimCsvValueField();
                                            bi.IsModified = true;

                                            if (bi.CUSIP.Length != 9 && bi.CUSIP.Length > 0)
                                                throw new Exception("Invalid CUSIP: " + bi.CUSIP);
                                        }
                                        else
                                        {
                                            si.FullName = fullName;
                                        }
                                        si.IsModified = true;
                                    }
                                    else
                                    {
                                        if (si.ISIN.Length < 2) si.ISIN = fields[8].TrimCsvValueField();
                                        if (si.FullName.Length < 4) si.FullName = fields[6].TrimCsvValueField();
                                    }

                                }
                                break;
                        }
                    }
                    if (percent % 1 == 0 && percent <= 100) progress.Report((int)percent);
                }
            }
        }

        #endregion Export / Import
    }
}
