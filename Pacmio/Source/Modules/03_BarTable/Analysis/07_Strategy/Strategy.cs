/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class Strategy : SignalAnalysis, IEquatable<Strategy>, IChartOverlay
    {
        protected Strategy(
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq barFreq,
            PriceType priceType)
            : base(tif, barFreq, priceType)
        {
            MinimumRiskRewardRatio = minRiskRewardRatio;
            HoldingPeriod = holdingPeriod;
            HoldingMaxSpan = holdingMaxSpan;
        }

        #region Analysis

        public FilterAnalysis Filter { get; protected set; }

        public BarAnalysisSet AnalysisSet { get; protected set; }

        #endregion Entry Signals

        #region Analysis

        public TimePeriod HoldingPeriod { get; }

        public TimeSpan HoldingMaxSpan { get; }

        public double MinimumRiskRewardRatio { get; }

        #endregion Exit Signals

        #region Draw Chart Overlay

        public int DrawOrder { get; set; } = 0;

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; protected set; } = MainBarChartArea.DefaultName;

        public virtual void DrawOverlay(Graphics g, BarChart bc)
        {
            Console.WriteLine("Start drawing Strategy Overlay, AreaName = " + AreaName);
            if (bc[AreaName] is Area a) 
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                int pix_index = 0;
                for (int i = bc.StartPt; i <= bc.StopPt; i++)
                {

                    if (i >= 0 && i < bc.BarTable.Count)
                    {
                        int x = a.IndexToPixel(pix_index);
                        int y = a.AxisY(AlignType.Right).ValueToPixel(bc.BarTable[i].Close);

                        Point pt1 = new(x - a.AxisX.HalfTickWidth, y);
                        Point pt3 = new(x + a.AxisX.HalfTickWidth, y);

                        g.DrawLine(new Pen(Color.Green, 3), pt1, pt3);
                    }
                    //Console.WriteLine("index = " + i);

                    pix_index++;
                }

                g.SmoothingMode = SmoothingMode.Default;
                g.ResetClip();
            }
        }

        #endregion Draw Chart Overlay

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ BarFreq.GetHashCode() ^ PriceType.GetHashCode();
        public bool Equals(Strategy other) => GetType() == other.GetType() && Name == other.Name && BarFreq == other.BarFreq && PriceType == other.PriceType;
        public static bool operator !=(Strategy s1, Strategy s2) => !s1.Equals(s2);
        public static bool operator ==(Strategy s1, Strategy s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is Strategy s && Equals(s);

        #endregion Equality
    }
}
