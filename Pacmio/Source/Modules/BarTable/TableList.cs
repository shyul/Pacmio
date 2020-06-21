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
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// Master List of all symbols
    /// </summary>
    public static class TableList
    {
        private static readonly ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType type), BarTable> List
            = new ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType type), BarTable>();

        private static int Count => List.Count;
        public static bool IsEmpty => Count == 0;
        public static IEnumerable<BarTable> Values => List.Values;

        /// <summary>
        /// Use Download Worker for this one please....
        /// </summary>
        /// <param name="c"></param>
        /// <param name="barFreq"></param>
        /// <param name="type"></param>
        public static BarTable GetTable(this Contract c, BarFreq barFreq, BarType type)
        {
            if (!List.ContainsKey((c, barFreq, type)))
                List.TryAdd((c, barFreq, type), new BarTable(c, barFreq, type));

            BarTable bt = List[(c, barFreq, type)];

            Console.WriteLine("TableList Get Table: " + bt.Name);

            return bt;
        }

        public static BarTable GetTable(this Contract c, BarFreq barFreq, BarType type, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            BarTable bt = GetTable(c, barFreq, type);
            bt.Reset(period, cts, progress);
            return bt;
        }

        public static void Save(this BarTable bt)
        {
            LocalRequestAction.Enqueue(new Action(() => {
                bt.LoadJsonFileToFileData(); // Blocking the process first
                // Then add the Bar to the Data Object
                InboundDataActions.Enqueue(new Action(() => {
                    bt.TransferActualValuesFromBarsToFileData();
                }));
                // Blocking the process and save...
                bt.SaveFileDataToJsonFile();
            }));
        }

        public static void Reset(BarFreq barFreq, BarType type, Period pd, CancellationTokenSource cts, IProgress<float> progress)
        {
            int i = 0;
            List.Values.Where(n => n.BarFreq == barFreq && n.Type == type).ToList().ForEach(bt =>
            {
                if (!cts.IsCancellationRequested)
                {
                    bt.Reset(pd, cts, progress);
                }
                i++;
            });
        }

        public static void Reset(this BarTable bt, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            Console.WriteLine("Reset Table: " + bt.Name + " Period = " + period);

            bt.ReadyForTickCalculation = false;
            DownloadCancellationTokenSource = cts;
            DownloadProgress = progress;

            if (period.IsCurrent && IB.Client.Connected)
                bt.Contract.Request_MarketTicks("236,375");

            LocalRequestAction.Enqueue(new Action(() => {
                bt.LoadJsonFileToFileData(); // Blocking the process first
                // Then add the Bar to the Data Object
                InboundDataActions.Enqueue(new Action(() => {
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.TransferActualValuesFromFileDataToBars(period);
                    // The table is loaded but not sorted, with only Actual Values
                    LocalRequestAction.Enqueue(new BarDataRequest(bt, period));
                }));
            }));
        }

        #region Download Task

        public static CancellationTokenSource DownloadCancellationTokenSource { get; private set; }

        public static bool IsCancellationRequested => !(DownloadCancellationTokenSource is null) && DownloadCancellationTokenSource.IsCancellationRequested;

        public static IProgress<float> DownloadProgress { get; private set; }

        private static int DownloadTimeout { get; set; } = 3000; // 5 minutes

        private static readonly ConcurrentQueue<object> LocalRequestAction = new ConcurrentQueue<object>();

        private static Task RequestTask { get; set; }

        private static void RequestTaskWorker()
        {
            while (!Root.RequestWorkerCancel)
            {
                if (IsCancellationRequested)
                {
                    while (LocalRequestAction.Count > 0)
                        LocalRequestAction.TryDequeue(out _);
                }
                else if (LocalRequestAction.Count > 0)
                {
                    LocalRequestAction.TryDequeue(out object data);

                    switch (data)
                    {
                        case BarDataRequest bdr:
                            Fetch(bdr.Table, bdr.Period);
                            break;

                        case Action ax: // blocking.
                            ax?.Invoke();
                            break;

                        case Task task: // Non blocking
                            task?.Start(); // https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
                            break;
                    }
                }
                else
                    Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Assume there is no way Two Fetch can happen at the same time.
        /// </summary>
        private static BarTable ReferenceTable { get; set; }

        private static void Fetch(BarTable bt, Period period)
        {
            bt.ReadyForTickCalculation = false;

            if (bt.BarFreq == BarFreq.Daily)
            {
                InboundDataActions.Enqueue(new Action(() => {
                    bt.SuspendCharts();
                }));
                // The table is loaded but not sorted, with only Actual Values
                // Do not sort the table, becasuse it is going to be refreshed by quandl with only actual values later.
                Fetch_Daily_No_Calculation(bt, period); // The table is downloaded with new candles, sorted and adjusted, but not calculated here
                Inbound_CalculateOnly(bt);
            }
            else if (bt.BarFreq > BarFreq.Daily)
            {
                // Load active values first
                Inbound_SortThenAdjustOnly(bt);
                Period download_time_period = new Period(bt.Frequency.Align(period.Start, -1), bt.Frequency.Align(period.Stop, 1));

                // Step 1: Get the Daily Chart from Quandl
                ReferenceTable = new BarTable(bt.Contract, BarFreq.Daily, bt.Type);

                // Step 2: Download to a separate Table !!! There could be a daily table already existing (for some other period) in the List!!
                ReferenceTable.LoadJsonFileToFileData();
                ReferenceTable.TransferActualValuesFromFileDataToBars(download_time_period); // Then add the Bar to the Data Object
                Fetch_Daily_No_Calculation(ReferenceTable, download_time_period); // sorted, adjusted, and saved as well // Forward Adjust, Getting the adjusted OHLC from Actual OHLC

                InboundDataActions.Enqueue(new Action(() =>
                {
                    // Fetch daily should be redeisgned!!!
                    download_time_period = new Period(bt.Frequency.Align(bt.LastTime, -1), bt.Frequency.Align(ReferenceTable.LastTimeBound, 1));
                    bt.Remove(download_time_period); // Remove the updating period from this table, becuase it is obsolete!! Remove the tail end
                    /*
                    ReferenceTable[download_time_period]
                        .Select(b => new BarData(bt, b.Source, b.Time, ReferenceTable.Frequency.Span, b.Open, b.High, b.Low, b.Close, b.Volume, true))
                        .ToList().ForEach(bd => Inbound(bd));*/

                    ReferenceTable[download_time_period].ToList().ForEach(b => InboundDataActions.Enqueue((bt, b)));

                    // Here we have a cascaded Inbound call, to make sure it happens after all bars are handled.
                    InboundDataActions.Enqueue(new Action(() =>
                    {
                        bt.DataSourceSegments.Add(download_time_period, DataSource.Consolidated); // update the period segment
                        bt.ReverseAdjustThenCalculate();
                        bt.RefreshChartToEnd();
                        ReferenceTable.TransferActualValuesFromBarsToFileData();
                        ReferenceTable.SaveFileDataToJsonFile(); // Blocking the process and save...
                    }));
                }));
            }
            else if (bt.BarFreq < BarFreq.Daily)
            {
                // Load active values first
                Inbound_SortThenAdjustOnly(bt);
                if (!Fetch_IB(bt, period))
                    Inbound_CalculateOnly(bt);
                else
                    Inbound_ReverseAdjustThenCalculate(bt);
            }
        }

        private static bool Fetch_Daily_No_Calculation(BarTable bt, Period period)
        {
            // The table is loaded but not sorted, with only Actual Values
            // Do not sort the table, becasuse it is going to be refreshed by quandl with only actual values later.

            bool success = false;

            DateTime quandlTime = bt.LastTimeBy(DataSource.Quandl); //period.Start;

            //if (quandlTime < bt.LastTimeBy(DataSource.Quandl)) quandlTime = bt.LastTimeBy(DataSource.Quandl);

            if (period.Stop > quandlTime) // The requested time is later than the Quandl time
            {
                bool quandl_is_available = bt.Contract.Country == "US" && bt.BarFreq == BarFreq.Daily && bt.Type == BarType.Trades && bt.Contract is Stock;

                DateTime now = DateTime.Now.Date;
                while (!bt.Contract.WorkHours.IsWorkDate(now)) now = now.AddDays(-1);

                Period download_time_period = new Period(quandlTime, now); // Get the missing part

                if (quandl_is_available && (now - quandlTime).TotalHours >= 24) // After 4:00 PM the next day.
                {
                    // If Quandl fails, please still try IB
                    success = quandl_is_available = Quandl.Download(bt, download_time_period); //, bt.Count == 0);
                }
                else
                    Console.WriteLine("Quandl: We already have the latest data, no need to download.");

                InboundDataActions.Enqueue(new Action(() => {
                    if (success)
                    {
                        bt.TransferActualValuesFromBarsToFileData();
                        bt.SaveFileDataToJsonFile();
                    }

                    if (period.Start > quandlTime)
                        bt.Remove(new Period(DateTime.MinValue, period.Start.AddDays(-1))); // Trim away extra downloaded bars

                    bt.SortThenAdjustOnly(); // With all the quandl bars loaded, we can do Sort and Forward Adjust now.
                }));

                // Load IB Daily 
                if (!quandl_is_available && Root.IBConnected && !IsCancellationRequested)
                {
                    Console.WriteLine("Quandl is not available, try getting the Daily Bars from IB!");
                    success = Fetch_IB(bt, period);
                    if (success) Inbound_SortThenReverseAdjustOnly(bt);
                }
            }
            else
                Inbound_SortThenAdjustOnly(bt);

            return success;

            // The table is downloaded with new candles, sorted and adjusted, but not calculated
            // The problem of calculate is because of the candle stick takes a long time!
        }

        /// <summary>
        /// If a request requires more than several minutes to return data, it would be best to cancel the request using the IBApi.EClient.cancelHistoricalData function.
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private static bool Fetch_IB(BarTable bt, Period period)
        {
            int time = 0;

            bool isModified = false;

            var (bfi_valid, bfi) = bt.BarFreq.GetAttribute<BarFreqInfo>();

            if (bfi_valid && Root.IBConnected && IB.Client.HistoricalData_Connected) // && HistoricalData_Connected)
            {
                Console.WriteLine("RequestHistoricalData | Initial Request: " + period);

                DateTime upToDate = bt.Contract.CurrentTime.AddMinutes(30);
                //if (period.IsCurrent) period = new Period(period.Start, DateTime.Now.AddDays(1));
                if (period.IsCurrent) period = new Period(period.Start, upToDate);

                MultiPeriod missing_period_list = new MultiPeriod(period);

                foreach (Period existingPd in bt.DataSourceSegments.Keys.Where(n => bt.DataSourceSegments[n] <= DataSource.IB))
                {
                    missing_period_list.Remove(existingPd);
                    Console.WriteLine("RequestHistoricalData | Already Existing: " + existingPd);
                }

                //If EarliestTime is unset, then request it here.
                if (bt.EarliestTime == DateTime.MinValue)
                {

                Download_HistoricalDataHeadTimestamp:
                    IB.Client.SendRequest_HistoricalDataHeadTimestamp(bt);

                    time = 0;
                    while (!IB.Client.IsReady_HistoricalDataHeadTimestamp)
                    {
                        time++;
                        Thread.Sleep(100);

                        // Check Cancellation Token Here in the if statament

                        if (time > DownloadTimeout) // Handle Time out here.
                        {
                            IB.Client.SendCancel_HistoricalHeadDataTimestamp();
                            Thread.Sleep(100);
                            goto Download_HistoricalDataHeadTimestamp;
                        }
                        else if (IsCancellationRequested)
                        {
                            IB.Client.SendCancel_HistoricalHeadDataTimestamp();
                            goto End;
                        }
                    }
                }

                // https://interactivebrokers.github.io/tws-api/historical_limitations.html
                DateTime earliestTime = (bt.BarFreq < BarFreq.Minute) ? DateTime.Now.AddMonths(-6) : bt.EarliestTime;
                List<Period> api_request_pd_list = new List<Period>();

                foreach (Period missing_period in missing_period_list)
                {
                    Console.WriteLine("RequestHistoricalData | This is what we miss: " + missing_period);

                    DateTime endTimeBound = DateTime.Now.AddDays(1);

                    if (missing_period.Start < earliestTime)
                        if (missing_period.Stop > earliestTime)
                            missing_period.SetStart(earliestTime); // Get Head time please, and reduce the period to limit
                        else
                            continue;
                    else if (missing_period.Stop > endTimeBound)
                        if (missing_period.Start < endTimeBound)
                            missing_period.SetStop(endTimeBound);
                        else
                            continue;

                    api_request_pd_list.AddRange(missing_period.Split(bfi.Duration));
                }

                time = 0;
                while (!IB.Client.IsReady_HistoricalData)
                {
                    time++;
                    Thread.Sleep(100);
                    if (time > DownloadTimeout) // Handle Time out here.
                    {
                        IB.Client.SendCancel_HistoricalData();
                        Thread.Sleep(100);
                        break;
                    }
                    else if (IsCancellationRequested)
                    {
                        IB.Client.SendCancel_HistoricalData();
                        goto End;
                    }
                }

                int pt = 0;
                foreach (Period api_request_pd in api_request_pd_list.OrderBy(n => n.Start))
                {
                    Console.WriteLine("RequestHistoricalData: | Sending Api Request: " + api_request_pd);

                    IB.Client.LastRequestedHistoricalDataPeriod = api_request_pd;

                    // We will download, but won't log the period if the stop may extended to the future.
                    IB.Client.IsLoggingLastRequestedHistoricalDataPeriod = api_request_pd.Stop < DateTime.Now.AddDays(-1);

                Download_HistoricalData:
                    isModified = IB.Client.SendRequest_HistoricalData(bt, bfi.DurationString, api_request_pd.Stop);

                    time = 0; // Wait the last transmit is over.
                    while (!IB.Client.IsReady_HistoricalData)
                    {
                        time++;
                        Thread.Sleep(100);

                        // Check Cancellation Token Here in the if statament

                        if (time > DownloadTimeout) // Handle Time out here.
                        {
                            IB.Client.SendCancel_HistoricalData();
                            Thread.Sleep(100);
                            goto Download_HistoricalData;
                        }
                        else if (IsCancellationRequested)
                        {
                            IB.Client.SendCancel_HistoricalData();
                            goto End;
                        }
                    }

                    Thread.Sleep(2000);

                    pt++;
                    DownloadProgress?.Report(100 * pt / api_request_pd_list.Count);
                }
            }

        End:
            return isModified;
        }

        /// <summary>
        /// By default, only load last valid segement
        /// </summary>
        //public void Fetch() => Fetch(BarTableFileData.DataSourceSegments.LastStreak);

        #endregion Download Task

        #region Data Task

        public static void Inbound(BarData bd) => InboundDataActions.Enqueue(bd);

        public static void Inbound(MarketTick mt) => InboundDataActions.Enqueue(mt);

        private static readonly ConcurrentQueue<object> InboundDataActions = new ConcurrentQueue<object>();

        public static void Inbound_CalculateOnly(BarTable bt)
        {
            InboundDataActions.Enqueue(new Action(() => {
                bt.CalculateOnly();
                bt.ReadyForTickCalculation = true;
                bt.RefreshChartToEnd();
            }));
        }

        private static void Inbound_SortThenAdjustOnly(BarTable bt)
        {
            InboundDataActions.Enqueue(new Action(() => {
                bt.SuspendCharts();
                bt.SortThenAdjustOnly();
            }));
        }

        private static void Inbound_SortThenReverseAdjustOnly(BarTable bt)
        {
            InboundDataActions.Enqueue(new Action(() => {
                bt.SuspendCharts();
                bt.SortThenReverseAdjustOnly();
            }));
        }

        private static void Inbound_ReverseAdjustThenCalculate(BarTable bt)
        {
            InboundDataActions.Enqueue(new Action(() => {
                bt.SuspendCharts();
                bt.ReverseAdjustThenCalculate();
                bt.ReadyForTickCalculation = true;
                bt.RefreshChartToEnd();
            }));
        }

        private static Task InboundTask { get; set; }

        private static void InboundTaskWorker()
        {
            while (!Root.RequestWorkerCancel)
            {
                if (InboundDataActions.Count > 0)
                {
                    InboundDataActions.TryDequeue(out object data);

                    switch (data)
                    {
                        case MarketTick mt:

                            var listTickAdd = Values.Where(n => n.Contract == mt.Contract && n.IsAcceptingTicks); // The last Bar must be current...

                            Parallel.ForEach(listTickAdd, bt =>
                            {
                                Task.Run(() =>
                                {
                                    lock (bt)
                                    {
                                        bt.AddPriceTick(mt.Time, mt.Price, mt.Size);
                                        if (bt.ReadyForTickCalculation)
                                        {
                                            bt.SetCalculationPointer(bt.LatestCalculatePointer - 3); // This is to set the analysis pointer at least 2 Bars behind.
                                            bt.CalculateOnly();
                                            /*
                                            if (ObsoleteStrategyMaster.Enabled)
                                            {
                                                Contract c = bt.Contract;
                                                if (ObsoleteStrategyMaster.TradeContract.ContainsKey(c))
                                                {
                                                    foreach (ObsoleteStrategy s in ObsoleteStrategyMaster.TradeContract[c])
                                                    {
                                                        s.RunLiveTrade(c);
                                                    }
                                                }
                                            }*/

                                            bt.RefreshChartNextTick();
                                        }
                                    }
                                });
                            });

                            break;

                        case BarData bd:

                            if (bd.Span == bd.Table.Frequency.Span)
                            {
                                Bar b = bd.Table.GetOrAdd(bd.Time);

                                if (b.Source >= bd.Source)
                                {
                                    b.Source = bd.Source;

                                    if (bd.IsAdjusted)
                                    {
                                        b.Open = bd.Open;
                                        b.High = bd.High;
                                        b.Low = bd.Low;
                                        b.Close = bd.Close;
                                        b.Volume = bd.Volume;
                                    }
                                    else
                                    {
                                        b.Actual_Open = bd.Open;
                                        b.Actual_High = bd.High;
                                        b.Actual_Low = bd.Low;
                                        b.Actual_Close = bd.Close;
                                        b.Actual_Volume = bd.Volume;
                                    }
                                }
                            }
                            break;

                        case (BarTable bt, Bar b):
                            bt.MergeFromSmallerBar(b);
                            break;

                        case Action ax:
                            ax?.Invoke();
                            break;

                        case Task task:
                            task?.Start(); // https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
                            break;

                        default: break;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        #endregion Data Task

        public static void StartTask(CancellationTokenSource cts)
        {
            InboundTask = new Task(() => InboundTaskWorker(), cts.Token);
            InboundTask.Start();

            RequestTask = new Task(() => RequestTaskWorker(), cts.Token);
            RequestTask.Start();
        }

        public static void StopTask()
        {
            while (RequestTask != null && RequestTask?.Status == TaskStatus.Running) { Thread.Sleep(1); }
            RequestTask?.Dispose();

            while (InboundTask != null && InboundTask?.Status == TaskStatus.Running) { Thread.Sleep(1); }
            InboundTask?.Dispose();
        }

        public static void Save()
        {
            // This is happening when the inbound task is over.

            if (!IsEmpty)
                Parallel.ForEach(Values, bt => {
                    bt.LoadJsonFileToFileData();
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.SaveFileDataToJsonFile();
                });
        }
    }
}
