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
        protected Indicator(BarFreq barFreq, PriceType type)
        {
            PriceType = type;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
        }

        public PriceType PriceType { get; set; } = PriceType.Trades;

        public BarFreq BarFreq { get; set; } = BarFreq.Daily;

        public Frequency Frequency { get; }

        public BarAnalysisSet BarAnalysisSet { get; protected set; }

        public abstract IEnumerable<SignalColumn> SignalColumns { get; }

        public double BullishPointLimit { get; set; } = 1;

        public double BearishPointLimit { get; set; } = -1;

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

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ BarFreq.GetHashCode() ^ PriceType.GetHashCode();
        public bool Equals(Indicator other) => GetType() == other.GetType() && Name == other.Name && BarFreq == other.BarFreq && PriceType == other.PriceType;
        public static bool operator !=(Indicator s1, Indicator s2) => !s1.Equals(s2);
        public static bool operator ==(Indicator s1, Indicator s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is Indicator ba && Equals(ba);

        #endregion Equality
    }
}
