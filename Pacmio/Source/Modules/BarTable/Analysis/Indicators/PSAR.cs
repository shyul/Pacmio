/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
/// 
/// Rising SAR
/// ----------
///
/// Prior SAR: The SAR value for the previous period.
///
/// Extreme Point(EP): The highest high of the current uptrend.
///
/// Acceleration Factor(AF): Starting at .02, AF increases by .02 each time the extreme point makes a new high.AF can reach a maximum of .20, 
/// no matter how long the uptrend extends.
///
/// Current SAR = Prior SAR + Prior AF(Prior EP - Prior SAR)
/// 13-Apr-10 SAR = 48.28 = 48.13 + .14(49.20 - 48.13)
///
/// The Acceleration Factor is multiplied by the difference between the Extreme Point and the prior period's SAR. 
/// This is then added to the prior period's SAR.
/// Note however that SAR can never be above the prior two periods' lows. 
/// Should SAR be above one of those lows, use the lowest of the two for SAR. 
///
///
/// Falling SAR
/// -----------
///
/// Prior SAR: The SAR value for the previous period.
///
/// Extreme Point (EP): The lowest low of the current downtrend.
///
/// Acceleration Factor (AF): Starting at .02, AF increases by .02 each time the extreme point makes a new low.AF can reach a maximum of .20, 
/// no matter how long the downtrend extends.
///
/// Current SAR = Prior SAR - Prior AF(Prior SAR - Prior EP)
/// 9-Feb-10 SAR = 43.56 = 43.84 - .16(43.84 - 42.07)
///
/// The Acceleration Factor is multiplied by the difference between the Prior period's SAR and the Extreme Point. 
/// This is then subtracted from the prior period's SAR.
/// Note that SAR can never be below the prior two periods' highs. 
/// Should SAR be below one of those highs, use the highest of the two for SAR.
/// 
/// Ref 1: https://en.wikipedia.org/wiki/Parabolic_SAR
/// Ref 2: https://school.stockcharts.com/doku.php?id=technical_indicators:parabolic_sar
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class PSAR : BarAnalysis, ISingleData, IChartSeries
    {
        public PSAR(double af = 0.02, double max_af = 0.2)
        {
            AF = af;
            MaxAF = max_af;

            string label = "(" + AF + "," + MaxAF + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Parabolic SAR " + label;

            EP_Column = new NumericColumn(Name + "_EP");
            AFS_Column = new NumericColumn(Name + "_AFS");
            Rising_Column = new NumericColumn(Name + "_Rising");
            Result_Column = new NumericColumn(Name);

            DotSeries = new DotSeries(Result_Column, Color.Brown, 1.5f)
            {
                Name = Name,
                Label = label,
                LegendName = GroupName,
                IsAntialiasing = true,
            };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ AF.GetHashCode() ^ MaxAF.GetHashCode();

        public double AF { get; set; }

        public double MaxAF { get; set; }

        #endregion Parameters

        #region Calculation

        public NumericColumn EP_Column { get; }

        public NumericColumn AFS_Column { get; }

        public NumericColumn Rising_Column { get; }

        public NumericColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double current_low = bt[i].Low;
                double current_high = bt[i].High;

                if (i > 0)
                {
                    double prior_sar = bt[i - 1][Result_Column];
                    double prior_ep = bt[i - 1][EP_Column];
                    double prior_af = bt[i - 1][AFS_Column];
                    double current_sar;
                    bool isRising = bt[i - 1][Rising_Column] > 0;

                    if (isRising)
                    {
                        /// Rising SAR
                        /// ----------

                        // Current SAR = Prior SAR + Prior AF(Prior EP - Prior SAR)
                        current_sar = prior_sar + prior_af * (prior_ep - prior_sar);

                        // If the next period's SAR value is inside (or beyond) the next period's price range, 
                        // a new trend direction is then signaled. The SAR must then switch sides.
                        if (current_sar > current_low)
                        {
                            // Put the flag on
                            isRising = false;

                            // Upon a trend switch, the first SAR value for this new trend is set to the last EP recorded on the prior trend,
                            current_sar = prior_ep;

                            // EP is then reset accordingly to this period's maximum,
                            prior_ep = current_low;

                            // and the acceleration factor is reset to its initial value of 0.02.
                            prior_af = AF;
                        }
                        else
                        {
                            if (i > 1)
                            {
                                double prior_low = bt[i - 1].Low;
                                double prior_low_2 = bt[i - 2].Low;

                                if (current_sar > prior_low || current_sar > prior_low_2)
                                {
                                    current_sar = Math.Min(prior_low, prior_low_2);
                                }
                            }

                            // New high point is established
                            if (current_high > prior_ep)
                            {
                                prior_ep = current_high;
                                prior_af += AF;
                                if (prior_af > MaxAF) prior_af = MaxAF;
                            }
                        }
                    }
                    else
                    {
                        /// Falling SAR
                        /// -----------

                        // Current SAR = Prior SAR - Prior AF(Prior SAR - Prior EP)
                        current_sar = prior_sar - prior_af * (prior_sar - prior_ep);

                        // If the next period's SAR value is inside (or beyond) the next period's price range, 
                        // a new trend direction is then signaled. The SAR must then switch sides.
                        if (current_sar < current_high)
                        {
                            // Put the flag on
                            isRising = true;

                            // Upon a trend switch, the first SAR value for this new trend is set to the last EP recorded on the prior trend,
                            current_sar = prior_ep;

                            // EP is then reset accordingly to this period's maximum,
                            prior_ep = current_high;

                            // and the acceleration factor is reset to its initial value of 0.02.
                            prior_af = AF;
                        }
                        else
                        {
                            if (i > 1)
                            {
                                double prior_high = bt[i - 1].High;
                                double prior_high_2 = bt[i - 2].High;

                                if (current_sar < prior_high || current_sar < prior_high_2)
                                {
                                    current_sar = Math.Max(prior_high, prior_high_2);
                                }
                            }

                            if (current_low < prior_ep)
                            {
                                prior_ep = current_low;
                                prior_af += AF;
                                if (prior_af > MaxAF) prior_af = MaxAF;
                            }
                        }
                    }

                    bt[i][EP_Column] = prior_ep;
                    bt[i][AFS_Column] = prior_af;
                    bt[i][Result_Column] = current_sar;
                    bt[i][Rising_Column] = isRising ? 1 : -1;
                }
                else
                {
                    bt[0][EP_Column] = current_high;
                    bt[0][AFS_Column] = AF;
                    bt[0][Result_Column] = current_low;
                    bt[0][Rising_Column] = 1;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => DotSeries.Color; set => DotSeries.Color = value; }

        public float DotWidth { get => DotSeries.Width; set => DotSeries.Width = value; }

        public DotSeries DotSeries { get; }

        public bool ChartEnabled { get => Enabled && DotSeries.Enabled; set => DotSeries.Enabled = value; }

        public int ChartOrder { get => DotSeries.Order; set => DotSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; private set; } = MainArea.DefaultName;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, 10)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(DotSeries);
            }
        }

        #endregion Series
    }
}
