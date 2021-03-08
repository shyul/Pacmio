/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BarDataFile : IDataFile, IEquatable<BarDataFile>, IEquatable<BarTable>, IDisposable
    {
        public BarDataFile(BarTable bt) : this(bt.Contract, bt.BarFreq, bt.Type) { }

        public BarDataFile(Contract c, BarFreq freq, BarType type)
        {
            Contract = c;
            BarFreq = freq;
            Type = type;
            //EarliestTime = c.GetOrCreateFundamentalData().EarliestTime;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
        }

        public void Dispose()
        {
            Rows.Clear();
            GC.Collect();
        }

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; private set; }

        [IgnoreDataMember]
        public Contract Contract
        {
            get
            {
                if (m_Contract is null || m_FundamentalData is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
                }
                return m_Contract;

            }
            private set
            {
                m_Contract = value;
                ContractKey = value.Key;
                m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        [IgnoreDataMember]
        private FundamentalData FundamentalData
        {
            get
            {
                if (m_Contract is null || FundamentalData is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
                }
                return m_FundamentalData;
            }
        }

        [IgnoreDataMember]
        private FundamentalData m_FundamentalData = null;

        [DataMember]
        public BarFreq BarFreq { get; set; }

        [DataMember]
        public BarType Type { get; set; }

        [IgnoreDataMember]
        public BarTable GetBarTable() => new BarTable(Contract, BarFreq, Type);

        [IgnoreDataMember]
        private Frequency Frequency { get; set; } //=> BarFreq.GetAttribute<BarFreqInfo>().Frequency;

        [IgnoreDataMember]
        public DateTime EarliestDataTime => DataSourceSegments.Start;

        [IgnoreDataMember]
        public DateTime HistoricalHeadTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime LastUpdateTime { get; private set; } = DateTime.MinValue;

        [DataMember]
        private MultiPeriod<DataSourceType> DataSourceSegments { get; set; } = new MultiPeriod<DataSourceType>();

        [DataMember]
        private Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)> Rows { get; set; }
            = new Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)>();

        [IgnoreDataMember]
        private object DataLockObject { get; set; } = new object();

        [IgnoreDataMember]
        public int Count => Rows.Count;

        public DateTime LastTimeBy(DataSourceType source)
        {
            lock (DataLockObject)
            {
                if (DataSourceSegments.Where(n => n.Value == source).OrderBy(n => n.Key).Select(n => n.Key).LastOrDefault() is Period pd)
                {
                    return pd.Stop;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            //var res = Bars.Where(n => n.Source <= source).OrderBy(n => n.Time);
            //return (res.Count() > 0) ? res.Last().Time : DateTime.MinValue.AddYears(500);
        }

        /*
        private void Adjust(bool forwardAdjust = true, bool adjustdividend = false)
        {
            //Sort();
            //if (Contract.MarketData is StockData sd)
            //{
            MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjustdividend); //sd.BarTableAdjust(AdjustDividend);

            // Please notice b.Time is the start time of the Bar
            // When the adjust event (split or dividend) happens at d 
            // The adjust will happen in d-1, which belongs to the
            // prior adjust segment.
            //                    S
            // ---------------------------------------
            //                   AD
            // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
            for (int i = 0; i < Count; i++)
            {
                Bar b = this[i];

                var (adj_price, adj_vol) = barTableAdjust[b.Time];
                b.Adjust(adj_price, adj_vol, forwardAdjust);
            }
            //}
            //ResetCalculationPointer();
        }

        #region Adjusted Calculation

        public void Adjust(double adj_price, double adj_vol, bool forwardAdjust = true)
        {
            if (forwardAdjust) // Adjust Part
            {
                if (Actual_Open > 0 && Actual_High > 0 && Actual_Low > 0 && Actual_Close > 0 && Actual_Volume >= 0)
                {
                    Open = Actual_Open * adj_price;
                    High = Actual_High * adj_price;
                    Low = Actual_Low * adj_price;
                    Close = Actual_Close * adj_price;
                    Volume = Actual_Volume / adj_vol;
                }
            }
            else // CounterAdjust Part
            {
                if (Open > 0 && High > 0 && Low > 0 && Close > 0 && Volume >= 0)
                {
                    Actual_Open = Open / adj_price;
                    Actual_High = High / adj_price;
                    Actual_Low = Low / adj_price;
                    Actual_Close = Close / adj_price;
                    Actual_Volume = Volume * adj_vol;
                }
            }
        }

        #endregion Adjusted Calculation
        */

        public void AddRows(
            IEnumerable<(DateTime time, double O, double H, double L, double C, double V)> rows,
            DataSourceType sourceType,
            Period segmentPeriod,
            bool counterAdjust = false,
            bool adjustDividend = false)
        {
            lock (rows)
                if (rows.Count() > 0)
                {
                    var sortedList = rows.OrderBy(n => n.time).ToList();

                    if (counterAdjust)
                    {
                        MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjustDividend);

                        // Please notice b.Time is the start time of the Bar
                        // When the adjust event (split or dividend) happens at d 
                        // The adjust will happen in d-1, which belongs to the
                        // prior adjust segment.
                        //                    S
                        // ---------------------------------------
                        //                   AD
                        // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
                        for (int i = 0; i < sortedList.Count; i++)
                        {
                            var row = sortedList[i];
                            var (adj_price, adj_vol) = barTableAdjust[row.time];
                            sortedList[i] = (row.time, row.O / adj_price, row.H / adj_price, row.L / adj_price, row.C / adj_price, row.V * adj_vol);
                        }
                    }

                    Period pd = segmentPeriod is null ? new Period(sortedList.First().time, sortedList.Last().time + Frequency.Span) : segmentPeriod;

                    lock (DataLockObject)
                    {
                        DataSourceSegments.Add(pd, sourceType);
                        foreach (var row in sortedList)
                        {
                            Rows[row.time] = (sourceType, row.O, row.H, row.L, row.C, row.V);
                        }
                    }

                    LastUpdateTime = DateTime.Now;
                    IsModified = true;
                }
        }

        public List<Bar> LoadBars(BarTable bt, Period pd, bool adjustDividend = false)
        {
            if (this != bt) throw new Exception("BarTable must match!");

            List<Bar> sortedList = null;

            lock (DataLockObject)
            {
                sortedList = Rows.
                    Where(n => pd.Contains(n.Key)).
                    OrderBy(n => n.Key).
                    Select(n => new Bar(bt, n.Key, n.Value.O, n.Value.H, n.Value.L, n.Value.C, n.Value.V, n.Value.SRC)).ToList();
            }

            return AdjustBars(bt, sortedList, adjustDividend);
        }

        public List<Bar> LoadBars(BarTable bt, bool adjustDividend = false)
        {
            if (this != bt) throw new Exception("BarTable must match!");

            List<Bar> sortedList = null;

            lock (DataLockObject)
            {
                sortedList = Rows.
                    OrderBy(n => n.Key).
                    Select(n => new Bar(bt, n.Key, n.Value.O, n.Value.H, n.Value.L, n.Value.C, n.Value.V, n.Value.SRC)).ToList();
            }

            return AdjustBars(bt, sortedList, adjustDividend);
        }

        private List<Bar> AdjustBars(BarTable bt, List<Bar> sortedList, bool adjustDividend)
        {
            if (sortedList is not null && sortedList.Count > 0)
            {
                MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjustDividend);
                // Please notice b.Time is the start time of the Bar
                // When the adjust event (split or dividend) happens at d 
                // The adjust will happen in d-1, which belongs to the
                // prior adjust segment.
                //                    S
                // ---------------------------------------
                //                   AD
                // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
                for (int i = 0; i < sortedList.Count; i++)
                {
                    Bar b = sortedList[i];
                    var (adj_price, adj_vol) = barTableAdjust[b.Time];
                    b.Open *= adj_price;
                    b.High *= adj_price;
                    b.Low *= adj_price;
                    b.Close *= adj_price;
                    b.Volume /= adj_vol;
                }
            }

            return sortedList;

            //bt.LoadBars(sortedList);
        }

        public void Clear(Period pd)
        {
            if (pd.Span >= Frequency.Span)
            {
                lock (DataLockObject)
                {
                    DataSourceSegments.Remove(pd);
                    var listToRemove = Rows.Select(n => n.Key).Where(n => pd.Contains(n)).ToList();
                    listToRemove.ForEach(n => Rows.Remove(n));

                    if (listToRemove.Count() > 0)
                        IsModified = true;
                }
            }
        }

        public void Clear()
        {
            lock (DataLockObject)
            {
                IsModified = !(DataSourceSegments.Count == 0 && Rows.Count == 0);

                DataSourceSegments.Clear();
                Rows.Clear();
            }
        }


        #region Download / Fetch Operation

        public void Fetch(Period period, CancellationTokenSource cts)
        {
            if (cts.IsContinue() && Enabled)
                lock (DataLockObject)
                {
                    Status = TableStatus.Downloading;

                    IsLive = period.IsCurrent;
                    //if (IsLive) Contract.Request_MarketTicks();
                    if (IsLive) Contract.MarketData.Start();

                    ResetCalculationPointer();
                    SyncFile(period);
                    Sort();
                    Adjust();

                    if (BarFreq == BarFreq.Daily)
                    {
                        Fetch_Daily(this, period, cts);
                        SaveFile();
                    }
                    else if (BarFreq > BarFreq.Daily)
                    {
                        Period download_time_period = new Period(Frequency.Align(period.Start, -1), Frequency.Align(period.Stop, 1));

                        using BarTable referenceTable = new BarTable(Contract, BarFreq.Daily, Type);
                        referenceTable.Load(download_time_period); // Then add the Bar to the Data Object
                        Fetch_Daily(referenceTable, download_time_period, cts); // sorted, adjusted, and saved as well // Forward Adjust, Getting the adjusted OHLC from Actual OHLC
                                                                                // Fetch_Daily will sort the reference table

                        download_time_period = new Period(Frequency.Align(LastTime, -1), Frequency.Align(referenceTable.LastTimeBound, 1));
                        Remove(download_time_period); // Remove the updating period from this table, becuase it is obsolete!! Remove the tail end

                        //ReferenceTable[download_time_period].AsParallel().ForAll(b => MergeFromSmallerBar(b));
                        referenceTable[download_time_period].ToList().ForEach(b => MergeFromSmallerBar(b));
                        AddDataSourceSegment(download_time_period, DataSourceType.Consolidated); // update the period segment

                        referenceTable.Save(); // Blocking the process and save...

                        Sort();
                        Adjust(false);

                        SaveFile();
                    }
                    else if (BarFreq > BarFreq.Minute && BarFreq < BarFreq.Daily) // TODO: TEST intraday BarFreq from 1 minute bars
                    {
                        MultiPeriod missing_period_list = new MultiPeriod(period);
                        foreach (Period existingPd in DataSourceSegments.Keys.Where(n => DataSourceSegments[n] <= DataSourceType.IB))
                        {
                            missing_period_list.Remove(existingPd);
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + "(BarFreq > BarFreq.Minute || BarFreq < BarFreq.Daily) | Already Existing: " + existingPd);
                        }

                        // Now get the missing periods from reference table
                        foreach (Period missing_period in missing_period_list)
                        {
                            Period transfer_reference_time_period = new Period(Frequency.Align(missing_period.Start, -1), Frequency.Align(missing_period.Stop, 1));

                            using BarTable referenceTable = new BarTable(Contract, BarFreq.Minute, Type);
                            referenceTable.Load(transfer_reference_time_period); // Then add the Bar to the Data Object
                            referenceTable.Sort();

                            if (referenceTable.Count > 0)
                            {
                                // Remove bar yielding partial result!!
                                DateTime first_valid_time_in_reference_table = Frequency.Align(referenceTable.FirstTime, 1);
                                DateTime last_valid_time_in_reference_table = Frequency.Align(referenceTable.LastTime, -1);

                                if (last_valid_time_in_reference_table > first_valid_time_in_reference_table)
                                {
                                    transfer_reference_time_period = new Period(first_valid_time_in_reference_table, last_valid_time_in_reference_table);
                                    Remove(transfer_reference_time_period); // Remove the updating period from this table, becuase it is obsolete!! Remove the tail end
                                    referenceTable[transfer_reference_time_period].ToList().ForEach(b => MergeFromSmallerBar(b));
                                    AddDataSourceSegment(transfer_reference_time_period, DataSourceType.Consolidated);
                                }
                            }
                        }

                        if (missing_period_list.Count() > 0)
                        {
                            Sort();
                            Adjust(false);
                        }

                        // Use IB to download the rest
                        Fetch_IB(this, period, cts);
                        Sort();
                        Adjust(false);
                        SaveFile();
                    }
                    else if (BarFreq <= BarFreq.Minute)
                    {
                        Fetch_IB(this, period, cts);
                        Sort();
                        Adjust(false);
                        SaveFile();
                    }

                    Status = TableStatus.LoadFinished;
                }
        }

        public bool Remove(Period pd)
        {
            var list = this[pd].ToList();
            bool isMod = list.Count > 0;

            if (isMod)
            {
                foreach (Bar b in list)
                    Rows.Remove(b);

                Sort();
            }

            return isMod;
        }

        private static bool Fetch_Daily(BarTable bt, Period period, CancellationTokenSource cts)
        {
            // The table is loaded but not sorted, with only Actual Values
            // Do not sort the table, becasuse it is going to be refreshed by quandl with only actual values later.

            bool success = false;

            DateTime quandlTime = bt.LastTimeBy(DataSourceType.Quandl); //period.Start;

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
                    success = quandl_is_available = Quandl.Download(bt, download_time_period);
                }
                else
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + "Quandl: We already have the latest data, no need to download.");

                if (success)
                {
                    bt.Save();
                }

                if (period.Start > quandlTime)
                    bt.Remove(new Period(DateTime.MinValue, period.Start.AddDays(-1))); // Trim away extra downloaded bars

                bt.Sort();
                bt.Adjust(); // With all the quandl bars loaded (or not... we still have bars loaded from the file system), we can do Sort and Forward Adjust now.

                // Load IB Daily if Quandle fails
                if (!quandl_is_available && IB.Client.Connected)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + "Quandl is not available, try getting the Daily Bars from IB!");
                    Fetch_IB(bt, period, cts);
                    bt.Sort();
                    bt.Adjust(false);
                }
            }
            else
            {
                bt.Sort();
                bt.Adjust();
            }

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
        private static void Fetch_IB(BarTable bt, Period period, CancellationTokenSource cts)
        {
            if (bt.Contract.Status != ContractStatus.Error)
            {
                if (bt.BarFreq.GetAttribute<BarFreqInfo>() is BarFreqInfo bfi && IB.Client.Connected && IB.Client.HistoricalData_Connected) // && HistoricalData_Connected)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Initial Request: " + period);

                    DateTime upToDate = bt.Contract.CurrentTime.AddMinutes(30);
                    //if (period.IsCurrent) period = new Period(period.Start, DateTime.Now.AddDays(1));
                    if (period.IsCurrent) period = new Period(period.Start, upToDate);

                    MultiPeriod missing_period_list = new MultiPeriod(period);

                    foreach (Period existingPd in bt.DataSourceSegments.Keys.Where(n => bt.DataSourceSegments[n] <= DataSourceType.IB))
                    {
                        missing_period_list.Remove(existingPd);
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Already Existing: " + existingPd);
                    }

                    //If EarliestTime is unset, then request it here.
                    if (bt.EarliestTime == DateTime.MaxValue)
                    {
                        if (cts.Cancelled()) goto End;

                        IB.Client.Fetch_HistoricalDataHeadTimestamp(bt, cts);
                    }

                    // https://interactivebrokers.github.io/tws-api/historical_limitations.html
                    DateTime earliestTime = (bt.BarFreq < BarFreq.Minute) ? DateTime.Now.AddMonths(-6) : bt.EarliestTime;

                    if (bt.Contract.Status != ContractStatus.Error && earliestTime < DateTime.Now)
                    {
                        List<Period> api_request_pd_list = new List<Period>();

                        foreach (Period missing_period in missing_period_list)
                        {
                            if (cts.Cancelled()) goto End;

                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | This is what we miss: " + missing_period);

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

                        foreach (Period api_request_pd in api_request_pd_list.OrderBy(n => n.Start))
                        {
                            if (cts.Cancelled())
                                goto End;
                            else
                                Thread.Sleep(2000);

                            Console.WriteLine("\n" + MethodBase.GetCurrentMethod().Name + " | Sending Api Request: " + api_request_pd);
                            IB.Client.Fetch_HistoricalData(bt, api_request_pd, cts);
                        }
                    }
                }
            }

            End:
            return;
        }

        public static void Download(IEnumerable<Contract> contracts, IEnumerable<(BarFreq freq, BarType type, Period period)> settings, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<(BarFreq freq, BarType type, Period period)> settings_list = new List<(BarFreq freq, BarType type, Period period)>() { (BarFreq.Daily, BarType.Trades, new Period(new DateTime(1000, 1, 1), DateTime.Now)) };

            var priority_settings = settings.Where(n => n.type == BarType.Trades && (n.freq == BarFreq.Hourly || n.freq == BarFreq.Minute)).OrderByDescending(n => n.freq);
            settings_list.AddRange(priority_settings);

            var remaining_settings = settings.Where(n => !priority_settings.Contains(n) && !(n.freq == BarFreq.Daily && n.type == BarType.Trades)).OrderByDescending(n => n.freq);
            settings_list.AddRange(remaining_settings);

            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count() * settings_list.Count();
            Parallel.ForEach(contracts, po, c =>
            {
                if (cts.IsContinue())
                {
                    foreach (var (freq, type, period) in settings_list)
                    {
                        if (cts.IsContinue())
                        {
                            BarTable bt = new BarTable(c, freq, type);
                            bt.Fetch(period, cts);
                            i++;
                            if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            });
        }

        #endregion Download / Fetch Operation


        #region File Operation




        public void AddDataSourceSegment(Period pd, DataSourceType source)
        {
            DataSourceSegments.Add(pd, source);
        }

        public DateTime GetDataSourceStartTime(DateTime endTime, DataSourceType source)
        {
            var res = DataSourceSegments.Where(n => n.Value <= source && n.Key.Contains(endTime));
            if (res.Count() > 0) return res.Last().Key.Start;
            else return endTime;
        }

        //string fileName = BarTableFileData.GetDataFileName((Contract.Key, BarFreq, Type));
        //BarTableFileData btd = Serialization.DeserializeJsonFile<BarTableFileData>(fileName);
        //private BarDataFile BarTableFileData => BarDataFile.LoadFile(this) is BarDataFile btd && btd == this ? btd : new BarDataFile(this);

        public void LoadFile() => LoadFile(Period.Full);

        public void LoadFile(Period pd, bool adjustDividend = false)
        {
            BarDataFile bdf = this.GetOrCreateBarDataFile();
            var sortedList = bdf.GetRows(pd, adjustDividend);



        }



        private void Sort()
        {
            TimeToRows.Clear();
            Rows.Sort((t1, t2) => t1.Time.CompareTo(t2.Time));
            for (int i = 0; i < Count; i++)
            {
                Bar b = Rows[i];
                TimeToRows[b.Time] = i;
                b.Index = i;
            }
            //ResetCalculationPointer();
            //Console.WriteLine("Sorted Table " + ToString() + " | Count: " + Count + " | Period: " + Period.ToString());
        }

        private void LoadFile(BarDataFile btd, Period pd)
        {
            ResetCalculationPointer();
            TimeToRows.Clear();
            Rows.Clear();

            var bars = btd.Rows.Where(n => pd.Contains(n.Key)).OrderBy(n => n.Key);
            Range<DateTime> Invalid_Period = null;

            foreach (var pb in bars)
            {
                if (pb.Value.O < 0 || pb.Value.H < 0 || pb.Value.L < 0 || pb.Value.C < 0 || pb.Value.V < 0)
                {
                    if (Invalid_Period is null) Invalid_Period = new Range<DateTime>(pb.Key, Frequency.Align(pb.Key, 0));
                    else
                    {
                        Invalid_Period.Insert(pb.Key);
                        Invalid_Period.Insert(Frequency.Align(pb.Key, 0));
                    }
                }
                else
                {
                    if (pb.Value.SRC < DataSourceType.Tick)
                    {
                        Bar b = GetOrAdd(pb.Key);
                        b.Source = pb.Value.SRC;
                        b.Actual_Open = pb.Value.O;
                        b.Actual_High = pb.Value.H;
                        b.Actual_Low = pb.Value.L;
                        b.Actual_Close = pb.Value.C;
                        b.Actual_Volume = pb.Value.V;
                    }
                }
            }

            if (Invalid_Period is Range<DateTime> rgt)
            {
                btd.DataSourceSegments.Remove(new Period(rgt.Minimum, rgt.Maximum));
                btd.Rows.Where(n => rgt.Contains(n.Key)).ToList().ForEach(n => btd.Rows.Remove(n.Key));

                btd.SaveFile();
                //btd.SerializeJsonFile(BarTableFileData.GetDataFileName((Contract.Key, BarFreq, Type)));
            }

            DataSourceSegments.Clear();
            DataSourceSegments.Merge(btd.DataSourceSegments);
        }

        public void Save()
        {
            lock (DataLockObject)
            {
                TableStatus last_status = Status;
                Status = TableStatus.Saving;
                SaveFile();
                Status = last_status;
            }
        }

        private void SaveFile()
        {
            using BarDataFile btd = BarTableFileData;
            SaveFile(btd);
        }

        private void SaveFile(BarDataFile btd)
        {
            if (Count > 0)
            {
                Range<DateTime> Invalid_Period = null;

                lock (Rows)
                    foreach (var b in Rows.Where(n => n.Source < DataSourceType.Tick))
                    {
                        if (b.Actual_Open < 0 || b.Actual_High < 0 || b.Actual_Low < 0 || b.Actual_Close < 0 || b.Actual_Volume < 0)
                        {
                            if (Invalid_Period is null) Invalid_Period = new Range<DateTime>(b.Period.Start, b.Period.Stop);
                            else
                            {
                                Invalid_Period.Insert(b.Period.Start);
                                Invalid_Period.Insert(b.Period.Stop);
                            }
                        }
                        else if (!btd.Rows.ContainsKey(b.Time) || b.Source < btd.Rows[b.Time].SRC)
                            btd.Rows[b.Time] = (b.Source, b.Actual_Open, b.Actual_High, b.Actual_Low, b.Actual_Close, b.Actual_Volume);
                    }

                btd.DataSourceSegments.Merge(DataSourceSegments);
                if (Invalid_Period is Range<DateTime> rgt) btd.DataSourceSegments.Remove(new Period(rgt.Minimum, rgt.Maximum));

                if (btd.EarliestTime < EarliestTime)
                    btd.EarliestTime = EarliestTime;

                if (btd.LastUpdateTime < LastDownloadRequestTime)
                    btd.LastUpdateTime = LastDownloadRequestTime;

                btd.SaveFile();
                //btd.SerializeJsonFile(BarTableFileData.GetDataFileName((Contract.Key, BarFreq, Type)));
            }
        }

        public void SyncFile(Period pd)
        {
            using BarDataFile btd = BarTableFileData;
            SaveFile(btd);
            LoadFile(btd, pd);
        }

        public void ClearFile(Period pd)
        {
            using BarDataFile btd = BarTableFileData;
            btd.DataSourceSegments.Remove(pd);
            var to_remove_list = btd.Rows.Where(n => pd.Contains(n.Key)).ToList();
            to_remove_list.ForEach(n => btd.Rows.Remove(n.Key));

            btd.SaveFile();
            //btd.SerializeJsonFile(BarTableFileData.GetDataFileName((Contract.Key, BarFreq, Type)));

            Clear();

        }

        public void ClearFile()
        {
            using BarDataFile btd = BarTableFileData;
            btd.DataSourceSegments.Clear();
            btd.Rows.Clear();
            btd.SaveFile();
            //btd.SerializeJsonFile(BarTableFileData.GetDataFileName((Contract.Key, BarFreq, Type)));
            Clear();
        }

        #endregion File Operation


        #region File Operation

        private static string GetDataFileName(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            string dir = Root.HistoricalDataPath(info.ContractKey) + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir + (info.ContractKey.typeName == "INDEX" ? "^" : "$") + info.ContractKey.name + ".json";
        }

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName((ContractKey, BarFreq, Type));

        // TODO: Handle this
        [IgnoreDataMember]
        public bool IsModified { get; private set; } = false;

        public void SaveFile()
        {
            lock (DataLockObject)
                this.SerializeJsonFile(DataFileName);
        }

        public static BarDataFile LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            var bdf = Serialization.DeserializeJsonFile<BarDataFile>(GetDataFileName(info));
            bdf.DataLockObject = new object();
            bdf.Frequency = bdf.BarFreq.GetAttribute<BarFreqInfo>().Frequency;
            return bdf;
        }

        #endregion File Operation

        #region Export CSV

        public void ExportCSV(string fileName)
        {
            lock (DataLockObject)
            {
                var list = Rows.OrderByDescending(n => n.Key);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Time,Data Source,Open,High,Low,Close,Volume,Event");

                var fd = ContractKey.GetOrCreateFundamentalData();

                FundamentalDatum[] fdlist = null;

                DateTime date = DateTime.MaxValue.Date;
                string fd_event = string.Empty;

                foreach (var row in list)
                {
                    DateTime newDate = row.Key.Date;

                    if (date > newDate)
                    {
                        fdlist = fd.GetList(newDate);
                        foreach (var fdm in fdlist)
                        {
                            fd_event += fdm.TypeName + " = " + fdm.Value + " | ";
                        }
                        fd_event = fd_event.Trim(new char[] { ' ', '|' }).Trim();
                        date = newDate;
                    }

                    sb.AppendLine(string.Join(",", new string[] {
                    row.Key.ToString(),
                    row.Value.SRC.ToString(),
                    row.Value.O.ToString(),
                    row.Value.H.ToString(),
                    row.Value.L.ToString(),
                    row.Value.C.ToString(),
                    row.Value.V.ToString(),
                    fd_event
                }));

                    fd_event = string.Empty;
                }

                sb.ToFile(fileName);
            }
        }

        #endregion Export CSV

        #region Equality

        public bool Equals(BarDataFile other) => (ContractKey == other.ContractKey) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarDataFile left, BarDataFile right) => left.Equals(right);
        public static bool operator !=(BarDataFile left, BarDataFile right) => !left.Equals(right);

        public bool Equals(BarTable other) => (ContractKey, BarFreq, Type) == (other.Contract.Key, other.BarFreq, other.Type);
        public static bool operator ==(BarDataFile left, BarTable right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, BarTable right) => !(left == right);

        public bool Equals(Contract other) => other is Contract c && c.Key == ContractKey;
        public static bool operator ==(BarDataFile left, Contract right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, Contract right) => !(left == right);

        public override bool Equals(object other)
        {
            if (other is BarDataFile btd)
                return Equals(btd);
            else if (other is BarTable bt)
                return Equals(bt);
            else if (other is Contract c)
                return Equals(c);
            else
                return false;
        }

        public override int GetHashCode() => ContractKey.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
