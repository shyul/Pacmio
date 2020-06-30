/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Quandl Web Client and Utilities
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using Xu;

namespace Pacmio
{
    public static class Quandl
    {
        private static string Key => Root.Settings.QuandlKey;

        private static readonly WebClient Client = new WebClient();

        #region URLs

        //private static string EOD_FULL_URL => "https://www.quandl.com/api/v3/databases/EOD/data?api_key=" + Key;
        //private static string EOD_LAST_DAY_URL => "https://www.quandl.com/api/v3/databases/EOD/data?download_type=partial&api_key=" + Key;
        private static string DailyBarURL(string symbol) => "https://www.quandl.com/api/v3/datasets/EOD/" + symbol.ToUpper() + ".csv?api_key=" + Key;
        private static string DailyBarURL(string symbol, Period period) => "https://www.quandl.com/api/v3/datasets/EOD/" + symbol.ToUpper() + ".csv?start_date=" + period.Start.ToString("yyyy-MM-dd") + "&end_date=" + period.Stop.ToString("yyyy-MM-dd") + "&api_key=" + Key;

        #endregion URLs

        public static bool Download(BarTable bt, Period period, bool GetAll = false)
        {
            bool success = false;

            if (bt.LastTimeBy(DataSource.Quandl) >= Frequency.Daily.Align(period.Stop)) return true;

            if (bt.Contract is Stock c && Root.Settings.QuandlKey.Length > 0)
            {
                if (!GetAll)
                    Console.WriteLine("Quandl: Will only try to get bars up to date from Quandl. Starting: " + period.Start.ToString("yyyy-MM-dd"));
                else
                    Console.WriteLine("Quandl: Getting all bars from Quandl. You will have to reset all analysis pointers later! ");

                string url = (!GetAll) ? DailyBarURL(ConvertToQuandlName(bt.Contract.Name), period) : DailyBarURL(ConvertToQuandlName(bt.Contract.Name));
                Console.WriteLine("Quandl Requesting: " + url);

                if (GetAll) period = new Period(period.Stop); // From now and onward
                TimeSpan ts = bt.Frequency.Span;
                /*
                if (c.MarketData is StockData sd0) 
                {
                    sd0.DividendTable.Clear();
                    sd0.SplitTable.Clear();
                }*/
          
                try
                {
                    using (MemoryStream stream = new MemoryStream(Client.DownloadData(url)))
                    using (StreamReader sr = new StreamReader(stream))
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
                                        period.Insert(time);

                                        double open = fields[1].ToDouble(0);
                                        double high = fields[2].ToDouble(0);
                                        double low = fields[3].ToDouble(0);
                                        double volume = fields[5].ToDouble(0);

                                        //double dividend_percent = fields[6].ToDouble(0) / close;
                                        //double split = fields[7].ToDouble(1);

                                        bt.Add(new BarData(DataSource.Quandl, time, ts,
                                             open, high, low, close, volume, false));


                                        //// Add Split and dividend to FundamentalData Table in BusinessInfo
                                        if (c.MarketData is StockData sd)
                                        {
                                            double dividend = fields[6].ToDouble(0);
                                            if (dividend != 0)
                                            {
                                                //Console.WriteLine("Add Dividend: " + dividend);
                                                sd.DividendTable[time] = (DataSource.Quandl, close, dividend);
                                            }

                                            double split = fields[7].ToDouble(1);
                                            if (split != 1)
                                            {
                                                //Console.WriteLine("Add Split: " + split);
                                                sd.SplitTable[time] = (DataSource.Quandl, split);
                                            }
                                        }
                                    }
                                }
                                else
                                    Console.WriteLine(fields);
                            }
                    }

                    bt.LastDownloadRequestTime = DateTime.Now;
                    bt.AddDataSourceSegment(period, DataSource.Quandl);
                    Console.WriteLine("Quandl download finished");
                    success = true;
                }
                catch (Exception e) when (e is WebException || e is ArgumentException)
                {
                    Console.WriteLine("Quandl download failed" + e.ToString());
                }
            }
            return success;
        }

        public static void ImportEOD(string fileName, IProgress<float> progress, CancellationTokenSource cts)
        {
            long byteread = 0;

            string currentSymbolName = string.Empty;
            string lastSymbolName = string.Empty;

            BarTableFileData btd = null;
            Contract currentContract = null;
            bool btdIsValid = false;

            IEnumerable<Contract> cList = ContractList.Values.AsParallel().Where(n => n is Stock s && s.Country == "US");// && s.Exchange != Exchange.OTCMKT && s.Exchange != Exchange.OTCBB);

            Dictionary<string, Contract> symbolList = cList.ToDictionary(n => n.Name, n => n);

            HashSet<string> Unknown = new HashSet<string>();

            if (File.Exists(fileName))
            {
                long totalSize = new FileInfo(fileName).Length;

                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs);
                string line = sr.ReadLine();
                string[] headers = line.CsvReadFields();
                Period pd = new Period();
                pd.Reset();

                if (headers.Length == 14)
                    while (!sr.EndOfStream && !cts.IsCancellationRequested)
                    {
                        line = sr.ReadLine();
                        byteread += line.Length + 1;
                        float percent = byteread * 100.0f / totalSize;

                        string[] fields = line.CsvReadFields();
                        if (fields.Length == 14)
                        {
                            currentSymbolName = ConvertToPacmioOrIbSymbolName(fields[0]);

                            // If sis is empty, or the current symbol is different (a new one.)
                            if (lastSymbolName != currentSymbolName)
                            {
                                /// Save File Now
                                if (btdIsValid)
                                {
                                    btd.DataSourceSegments.Add(pd, DataSource.Quandl);

                                    // !! Please check if the file is locked or now before saving.
                                    btd.SerializeJsonFile(btd.FileName);

                                    if (currentContract is Stock stk) stk.SaveMarketData();

                                    Console.Write(btd.Contract.name + ". ");
                                    pd.Reset();
                                    currentContract = null;
                                    btd = null;
                                }

                                if (symbolList.ContainsKey(currentSymbolName))
                                {
                                    currentContract = symbolList[currentSymbolName];

                                    if (currentContract.MarketData is StockData sd0)
                                    {
                                        sd0.DividendTable.Clear();
                                        sd0.SplitTable.Clear();
                                    }

                                    btd = new BarTableFileData(currentContract, BarFreq.Daily, BarType.Trades);
                                    btdIsValid = true;
                                }
                                else
                                {
                                    if (!UnknownItemList.Contains(currentSymbolName))
                                    {
                                        UnknownItemList.Add(currentSymbolName, "STOCK");
                                    }
                                    btdIsValid = false;
                                }
                            }

                            if (btdIsValid)
                            {
                                double close = fields[5].ToDouble(0);
                                if (close > 0)
                                {
                                    DateTime time = DateTime.Parse(fields[1]);

                                    if (!btd.Bars.ContainsKey(time) || btd.Bars[time].SRC > DataSource.Quandl)
                                    {
                                        double open = fields[2].ToDouble(0);
                                        double high = fields[3].ToDouble(0);
                                        double low = fields[4].ToDouble(0);
                                        double volume = fields[6].ToDouble(0);

                                        pd.Insert(time);

                                        btd.Bars[time] = (DataSource.Quandl, open, high, low, close, volume);
                                    }

                                    //// Add Split and dividend to FundamentalData Table in BusinessInfo
                                    if (currentContract.MarketData is StockData sd)
                                    {
                                        double dividend = fields[7].ToDouble(0);
                                        if (dividend != 0)
                                        {
                                            if (dividend < 0) throw new Exception("Split can't be: " + dividend);
                                            //Console.WriteLine("Add Dividend: " + dividend);
                                            sd.DividendTable[time] = (DataSource.Quandl, close, dividend);
                                        }

                                        double split = fields[8].ToDouble(1);
                                        if (split != 1)
                                        {
                                            //Console.WriteLine("Add Split: " + split);

                                            if (split <= 0) throw new Exception("Split can't be: " + split);
                                            sd.SplitTable[time] = (DataSource.Quandl, split);
                                        }
                                    }
                                }
                            }

                            lastSymbolName = currentSymbolName;
                        }
                        else
                        {
                            Log.Print("\n" + CollectionTool.ToString(fields) + "\n"); //throw new Exception("Error loading QuandlEOD");

                        }

                        progress.Report(percent);
                    }
            }

            Log.Print("Job done!! Hooray!\n" + CollectionTool.ToString(Unknown) + "\n");
        }

        public static void MergeEODFiles(IEnumerable<string> EODFiles, string mergedFile, CancellationTokenSource cts, IProgress<float> progress)
        {
            Dictionary<(string symbol, DateTime time), string> Lines = new Dictionary<(string symbol, DateTime time), string>();
            HashSet<string> Symbols = new HashSet<string>();

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
                using (StreamReader sr = new StreamReader(fs))
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
            using (StreamWriter file = new StreamWriter(fs))
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
            using (StreamWriter file = new StreamWriter(fs))
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
            HashSet<string> Symbols = new HashSet<string>();

            long byteread = 0;
            if (File.Exists(EODFile))
            {
                long totalSize = new FileInfo(EODFile).Length;

                using var fs = new FileStream(EODFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs);
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
                                    ContractList.GetOrFetch(symbol, "US");
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

            return input.Replace("_", " ");
        }
    }
}
