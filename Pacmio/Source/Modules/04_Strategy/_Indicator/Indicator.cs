/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// https://support.stockcharts.com/doku.php?id=scans:library:sample_scans
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Passive: Only yield signal score
    /// </summary>
    public abstract class Indicator : BarAnalysis, IChartSeries, IEquatable<Indicator>
    {
        protected Indicator(BarFreq barFreq, DataType type)
        {
            DataType = type;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;

            SignalSeries = new(this);
        }

        public DataType DataType { get; set; } = DataType.Trades;

        public BarFreq BarFreq { get; set; } = BarFreq.Daily;

        public Frequency Frequency { get; }

        public BarAnalysisSet BarAnalysisSet { get; protected set; }

        public abstract IEnumerable<SignalColumn> SignalColumns { get; }

        public TimePeriod TimeInForce { get; set; } = TimePeriod.Full;

        public double BullishPointLimit { get; set; } = 1;

        public double BearishPointLimit { get; set; } = -1;

        public (IEnumerable<Bar> BullishBars, IEnumerable<Bar> BearishBars) RunScan(BarTableSet bts, Period pd)
        {
            BarTable bt = bts[BarFreq, DataType];

            BarAnalysisSet bas = BarAnalysisSet;
            bt.CalculateRefresh(bas);

            var BullishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(this).Bullish >= BullishPointLimit);
            var BearishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(this).Bearish <= BearishPointLimit);

            return (BullishBars, BearishBars);
        }

        public IndicatorScanResult RunScanResult(BarTableSet bts, Period pd)
        {
            var (BullishBars, BearishBars) = RunScan(bts, pd);
            IndicatorScanResult result = new(bts.Contract);

            BullishBars.RunEach(n => result.BullishPeriods.Add(ToDailyPeriod(n.Period)));
            BearishBars.RunEach(n => result.BearishPeriods.Add(ToDailyPeriod(n.Period)));
            result.TotalCount = bts[BarFreq, DataType].Count;
            result.BullishCount = BullishBars.Count();
            result.BearishCount = BearishBars.Count();

            return result;
        }

        public Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);

        //public Period ToDailyPeriod(Period pd) => Frequency.AlignPeriod(pd);

        #region Series

        public Color Color { get => SignalSeries.Color; set => SignalSeries.Color = value; }

        public Series MainSeries => SignalSeries;

        public SignalSeries SignalSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && SignalSeries.Enabled; set => SignalSeries.Enabled = value; }

        public int DrawOrder { get => SignalSeries.Order; set => SignalSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                SignalArea a = bc.AddArea(new SignalArea(bc, SignalSeries)
                {
                    Order = AreaOrder,
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(SignalSeries);
            }
        }

        #endregion Series

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ BarFreq.GetHashCode() ^ DataType.GetHashCode();
        public bool Equals(Indicator other) => GetType() == other.GetType() && Name == other.Name && BarFreq == other.BarFreq && DataType == other.DataType;
        public static bool operator !=(Indicator s1, Indicator s2) => !s1.Equals(s2);
        public static bool operator ==(Indicator s1, Indicator s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is Indicator ba && Equals(ba);

        #endregion Equality
    }
}
