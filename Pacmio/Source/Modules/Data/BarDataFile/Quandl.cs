/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Quandl Web Client and Utilities
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public static class Quandl
    {
        private const string URL = "https://www.quandl.com";
        private static string Key => Root.Settings is PacSettings pst ? pst.QuandlKey : null;
        public static bool Connected => (!string.IsNullOrEmpty(Key)) && HttpClientTools.Connected(URL);
        private static WebClient Client { get; } = new WebClient();

        #region URLs

        //private static string EOD_FULL_URL => "https://www.quandl.com/api/v3/databases/EOD/data?api_key=" + Key;
        //private static string EOD_LAST_DAY_URL => "https://www.quandl.com/api/v3/databases/EOD/data?download_type=partial&api_key=" + Key;
        private static string DailyBarURL(string symbol) => URL + "/api/v3/datasets/EOD/" + symbol.ToUpper() + ".csv?api_key=" + Key;
        private static string DailyBarURL(string symbol, Period period) => URL + "/api/v3/datasets/EOD/" + symbol.ToUpper() + ".csv?start_date=" + period.Start.ToString("yyyy-MM-dd") + "&end_date=" + period.Stop.ToString("yyyy-MM-dd") + "&api_key=" + Key;

        #endregion URLs

        public static object DownloadLockObject { get; } = new object();

        public static DateTime LastFetchTime { get; private set; } = DateTime.MinValue;

        public static bool Fetch(BarDataFile bdf)
        {
            bool use_quandl = bdf.Contract is Stock && bdf.Contract.Country == "US" && bdf.BarFreq == BarFreq.Daily && bdf.Type == DataType.Trades;

            if (Connected && use_quandl)
            {
                Contract c = bdf.Contract;

                DateTime startTime = bdf.LastTimeBy(DataSourceType.Quandl);
                DateTime stopTime = c.LatestClosingDateTime;

                Console.WriteLine(bdf.Contract + " | startTime = " + startTime + " | stopTime = " + stopTime);

                if (startTime > stopTime)
                {
                    Console.WriteLine("There is no new data from Quandl yet.");
                    return true;
                }

                bool getAll = bdf.Count == 0 || startTime < new DateTime(1982, 1, 1);

                string url = getAll ?
                    DailyBarURL(ConvertToQuandlName(bdf.Contract.Name)) :
                    DailyBarURL(ConvertToQuandlName(bdf.Contract.Name), new Period(startTime.AddDays(-7), stopTime));

                byte[] result = null;

                lock (DownloadLockObject)
                {
                    while ((DateTime.Now - LastFetchTime).TotalMilliseconds < 90)
                    {
                        Thread.Sleep(20);
                    }

                    LastFetchTime = DateTime.Now;

                    try
                    {
                        Console.WriteLine("Quandl Requesting: " + url);
                        lock (Client)
                        {
                            result = Client.DownloadData(url);
                        }
                    }
                    catch (Exception e) when (e is WebException || e is ArgumentException)
                    {
                        Console.WriteLine(c + " | Quandl download failed" + e.ToString());
                        c.Status = ContractStatus.Error;
                        return false;
                    }
                }

                if (result is not null)
                {
                    FundamentalData fd = bdf.Contract.GetOrCreateFundamentalData();
                    if (getAll) fd.Remove(DataSourceType.Quandl);
                    Period data_pd = new();

                    var rows = new List<(DateTime time, double O, double H, double L, double C, double V)>();
                    using (MemoryStream stream = new(result))
                    using (StreamReader sr = new(stream))
                    {
                        string[] headers = sr.CsvReadFields();
                        if (headers.Length == 13)
                            while (!sr.EndOfStream)
                            {
                                string[] fields = sr.CsvReadFields();
                                if (fields.Length == 13)
                                {
                                    double close = fields[4].ToDouble(0);
                                    if (close > 0)
                                    {
                                        DateTime time = DateTime.Parse(fields[0]);
                                        double open = fields[1].ToDouble();
                                        double high = fields[2].ToDouble();
                                        double low = fields[3].ToDouble();
                                        double volume = fields[5].ToDouble();

                                        rows.Add((time, open, high, low, close, volume));
                                        data_pd.Insert(time);

                                        //// Add Split and dividend to FundamentalData Table in FD
                                        double dividend = fields[6].ToDouble(0);
                                        fd.SetDividend(time, close, dividend, DataSourceType.Quandl);

                                        double split = fields[7].ToDouble(1);
                                        fd.SetSplit(time, close, split, DataSourceType.Quandl);
                                    }
                                }
                                else
                                    Console.WriteLine(fields);
                            }
                    }

                    Console.WriteLine("The data segment has: " + data_pd);

                    if ((getAll && rows.Count == 0) || (DateTime.Now - data_pd.Stop).TotalDays > 30)
                    {
                        c.Status = ContractStatus.Delist;
                    }

                    data_pd.Insert(data_pd.Stop + bdf.Frequency.Span);
                    bdf.AddRows(rows, DataSourceType.Quandl, data_pd);

                    if (getAll && rows.Count > 0)
                        bdf.HistoricalHeadTime = data_pd.Start;

                    bdf.SaveFile();
                    fd.SaveFile();

                    return true;
                }
            }
            return false;
        }

        private static void SaveFile(BarDataFile bdf_save, FundamentalData currentFd, IEnumerable<(DateTime time, double O, double H, double L, double C, double V)> rows, Period data_pd)
        {
            var rowsToSave = rows.ToArray();
            Period pd_to_save = new(data_pd);
            pd_to_save.Insert(data_pd.Stop + bdf_save.Frequency.Span);

            Task.Run(() =>
            {
                bdf_save.AddRows(rowsToSave, DataSourceType.Quandl, pd_to_save);
                bdf_save.SaveFile();
                currentFd.SaveFile();

                MarketData md = bdf_save.Contract.MarketData;
                if (double.IsNaN(md.LastPrice) && rowsToSave.Count() > 0)
                {
                    md.LastPrice = rowsToSave.Last().C;
                }
            });
        }

        public static void ImportEOD(string fileName, IProgress<float> progress, CancellationTokenSource cts)
        {
            long bytesRead = 0;

            string currentSymbolName = string.Empty;
            string lastSymbolName = string.Empty;
            Contract currentContract = null;

            FundamentalData currentFd = null;
            BarDataFile currentBtd = null;
            var rows = new List<(DateTime time, double O, double H, double L, double C, double V)>();

            ContractManager.RemoveDuplicateUSStock("US", cts);
            Dictionary<string, Contract> symbolLUT = new();
            ContractManager.Values.AsParallel().Where(n => n is Stock s && s.Country == "US").RunEach(n => symbolLUT[n.Name] = n);

            //Dictionary<string, Contract> symbolLUT = ContractManager.Values.AsParallel().Where(n => n is Stock s && s.Country == "US").ToDictionary(n => n.Name, n => n);
            HashSet<string> Unknown = new();

            if (File.Exists(fileName))
            {
                long totalSize = new FileInfo(fileName).Length;

                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                string line = sr.ReadLine();
                string[] headers = line.CsvReadFields();
                Period data_pd = new();

                if (headers.Length == 14)
                    while (!sr.EndOfStream && !cts.IsCancellationRequested)
                    {
                        line = sr.ReadLine();
                        bytesRead += line.Length + 1;
                        float percent = bytesRead * 100.0f / totalSize;

                        string[] fields = line.CsvReadFields();
                        if (fields.Length == 14)
                        {
                            currentSymbolName = ConvertToPacmioOrIbSymbolName(fields[0]);

                            // If sis is empty, or the current symbol is different (a new one.)
                            if (lastSymbolName != currentSymbolName)
                            {
                                /// Save File Now
                                if (currentBtd is BarDataFile bdf_save)
                                {
                                    //SaveFile(bdf_save, currentFd, rows, data_pd);

                                    data_pd.Insert(data_pd.Stop + bdf_save.Frequency.Span);
                                    bdf_save.AddRows(rows, DataSourceType.Quandl, data_pd);
                                    bdf_save.SaveFile();
                                    currentFd.SaveFile();

                                    MarketData md = bdf_save.Contract.MarketData;
                                    if (double.IsNaN(md.LastPrice) && rows.Count() > 0)
                                    {
                                        md.LastPrice = rows.Last().C;
                                    }

                                    rows.Clear();
                                    data_pd.Reset();
                                    currentContract = null;
                                    currentFd = null;
                                    currentBtd = null;
                                }

                                if (symbolLUT.ContainsKey(currentSymbolName))
                                {
                                    currentContract = symbolLUT[currentSymbolName];
                                    currentFd = currentContract.GetOrCreateFundamentalData();

                                    currentFd.Remove(DataSourceType.Quandl);
                                    currentBtd = BarDataFile.LoadFile((currentContract.Key, BarFreq.Daily, DataType.Trades));
                                }
                                else if (currentSymbolName.Contains("ATEST ") ||
                                    currentSymbolName.Contains("CTEST ") ||
                                    currentSymbolName == "ZVZZT" ||
                                    currentSymbolName == "ZWZZT")
                                {
                                    currentContract = null;
                                    currentBtd = null;
                                }
                                else if (ContractManager.GetOrFetch(currentSymbolName, "US").FirstOrDefault() is Contract c)
                                {
                                    currentContract = c;
                                    currentFd = currentContract.GetOrCreateFundamentalData();

                                    currentFd.Remove(DataSourceType.Quandl);
                                    currentBtd = BarDataFile.LoadFile((currentContract.Key, BarFreq.Daily, DataType.Trades));
                                }
                                else
                                {
                                    UnknownContractList.CheckIn(currentSymbolName);
                                    currentContract = null;
                                    currentBtd = null;
                                }
                            }

                            if (currentBtd is BarDataFile bdf0)
                            {
                                double close = fields[5].ToDouble(0);
                                if (close > 0)
                                {
                                    DateTime time = DateTime.Parse(fields[1]);
                                    double open = fields[2].ToDouble(0);
                                    double high = fields[3].ToDouble(0);
                                    double low = fields[4].ToDouble(0);
                                    double volume = fields[6].ToDouble(0);

                                    rows.Add((time, open, high, low, close, volume));
                                    data_pd.Insert(time);

                                    //// Add Split and dividend to FundamentalData Table in FD
                                    double dividend = fields[7].ToDouble(0);
                                    currentFd.SetDividend(time, close, dividend, DataSourceType.Quandl);

                                    double split = fields[8].ToDouble(1);
                                    currentFd.SetSplit(time, close, split, DataSourceType.Quandl);
                                }
                            }

                            lastSymbolName = currentSymbolName;
                        }
                        else
                        {
                            Console.WriteLine("\n" + CollectionTool.ToString(fields) + "\n"); //throw new Exception("Error loading QuandlEOD");

                        }

                        progress.Report(percent);
                    }
            }

            Console.WriteLine("Job done!! Hooray!\n" + CollectionTool.ToString(Unknown) + "\n");
        }

        public static void MergeEODFiles(IEnumerable<string> EODFiles, string mergedFile, CancellationTokenSource cts, IProgress<float> progress)
        {
            Dictionary<(string symbol, DateTime time), string> Lines = new();
            HashSet<string> Symbols = new();

            long totalSize = 0;

            foreach (string file in EODFiles)
                totalSize += new FileInfo(file).Length;

            long byteread = 0;
            long pt = 0;

            foreach (string file in EODFiles)
            {
                if (cts is CancellationTokenSource cs && cs.IsCancellationRequested) break;
                Console.WriteLine("loading: " + file);
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        if (cts is CancellationTokenSource cs2 && cs2.IsCancellationRequested) break;
                        string line = sr.ReadLine();
                        try
                        {
                            string[] fields = line.Split(',');

                            if (fields.Length == 14)
                            {
                                string symbol = fields[0];
                                Symbols.CheckAdd(symbol);

                                DateTime time = DateTime.Parse(fields[1]);

                                if (!Lines.ContainsKey((symbol, time)))
                                {
                                    //Symbols.CheckAdd(symbol);
                                    Lines[(symbol, time)] = line;
                                }
                            }
                        }
                        catch (Exception e) when (e is IOException || e is FormatException)
                        {
                            Console.WriteLine("line error: " + line + " | " + e.ToString());
                        }

                        byteread += line.Length + 1;
                        progress.Report(byteread * 100.0f / totalSize);
                    }
                }

                GC.Collect();
            }

            Console.WriteLine("Start Exported Merged File! \n");

            var sorted = Lines.AsParallel().OrderBy(n => n.Key.symbol).ThenBy(n => n.Key.time);

            totalSize = Lines.Count();
            pt = 0;

            using (var fs = new FileStream(mergedFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter file = new(fs))
            {
                foreach (var line in sorted)
                {
                    pt++;
                    float percent = pt * 100.0f / totalSize;
                    progress.Report(percent);
                    file.WriteLine(line.Value);
                }
            }

            using (var fs = new FileStream(mergedFile.Replace(".csv", "_Symbols.csv"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter file = new(fs))
            {
                //StringBuilder sb = new StringBuilder();
                foreach (string s in Symbols) file.WriteLine(s);//sb.Append(s + ",");
                //file.WriteLine(sb);
            }

            Console.WriteLine("Job Completed.");

            Console.WriteLine("\n\nPress any key to continue.");
            Console.ReadKey();
        }

        public static HashSet<string> ImportSymbols(string EODFile, CancellationTokenSource cts, IProgress<float> progress)
        {
            HashSet<string> Symbols = new();

            long byteread = 0;
            if (File.Exists(EODFile))
            {
                long totalSize = new FileInfo(EODFile).Length;

                using var fs = new FileStream(EODFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                string line = sr.ReadLine();
                string[] headers = line.CsvReadFields();

                if (headers.Length == 14)
                    while (!sr.EndOfStream && !cts.IsCancellationRequested)
                    {
                        if (cts.IsCancellationRequested) break;
                        line = sr.ReadLine();
                        byteread += line.Length + 1;
                        float percent = byteread * 100.0f / totalSize;
                        string[] fields = line.CsvReadFields();
                        if (fields.Length == 14)
                        {
                            string symbol = ConvertToPacmioOrIbSymbolName(fields[0]);
                            if (Regex.IsMatch(symbol, @"^[a-zA-Z0-9_]+$"))
                            {
                                if (Symbols.CheckAdd(symbol))
                                {
                                    Console.Write(symbol + ". ");
                                    ContractManager.GetOrFetch(symbol, "US");
                                }

                            }
                        }
                        progress?.Report(percent);
                    }
            }
            return Symbols;
        }

        public static string ConvertToQuandlName(string input)
        {
            input = input.Trim();

            if (input.Contains(" PR"))
            {
                input = input.Replace(" PR", "_P_");

                if (input.EndsWith("CL"))
                {
                    input = input.ReplaceEnd("CL", "_CL");
                }
            }
            else if (input.EndsWith("CL"))
            {
                input = input.ReplaceEnd("CL", "_CL");
            }

            return input.Replace(" ", "_");
        }

        public static string ConvertToPacmioOrIbSymbolName(string input)
        {
            if (input.Contains("_P_"))
            {
                input = input.Replace("_P_", " PR");

                if (input.EndsWith("_CL"))
                {
                    input = input.Replace("_CL", "CL");
                }

            }
            else if (input.EndsWith("_CL"))
            {
                input = input.Replace("_CL", " CL");
            }

            return input.Replace("_", " ").Trim();
        }
    }
}
