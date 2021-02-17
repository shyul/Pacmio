/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Ultimate Oscillator
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:ultimate_oscillator
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;
using System.Windows.Forms;

namespace Pacmio.Analysis
{
    public class ULTO : BarAnalysis, IOscillator
    {
        public ULTO(int interval_fast = 7, int interval_middle = 14, int interval_slow = 28)
        {
            Interval_Fast = interval_fast;
            Interval_Middle = interval_middle;
            Interval_Slow = interval_slow;

            string label = "(" + Interval_Fast.ToString() + "," + Interval_Middle.ToString() + "," + Interval_Slow.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Ultimate Oscillator " + label;

            Column_BP = new NumericColumn(Name + "_BP");
            Column_TR = new NumericColumn(Name + "_TR");
            Column_Result = new NumericColumn(Name);

            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                Label = label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            Color = Color.FromArgb(255, 96, 96, 96);
        }

        #region Parameters

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 70;

        public double LowerLimit { get; set; } = 30;

        public int Interval_Fast { get; }

        public int Interval_Middle { get; }

        public int Interval_Slow { get; }

        #endregion Parameters

        #region Calculation

        public NumericColumn Column_BP { get; }

        public NumericColumn Column_TR { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    (double high, double low) = bt[i - 1] is Bar b_1 ? (Math.Max(b_1.Close, b.High), Math.Min(b_1.Close, b.Low)) : (b.High, b.Low);
                    b[Column_BP] = b.Close - low;
                    b[Column_TR] = high - low;
                }
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double bp_sum_fast = 0, bp_sum_middle = 0, bp_sum_slow = 0;
                    double tr_sum_fast = 0, tr_sum_middle = 0, tr_sum_slow = 0;

                    for (int j = 0; j < Interval_Slow; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        if (bt[k] is Bar b_sum)
                        {
                            double bp = b_sum[Column_BP];
                            double tr = b_sum[Column_TR];
                            bp_sum_slow += bp;
                            tr_sum_slow += tr;

                            if (j < Interval_Middle)
                            {
                                bp_sum_middle += bp;
                                tr_sum_middle += tr;
                            }

                            if (j < Interval_Fast)
                            {
                                bp_sum_fast += bp;
                                tr_sum_fast += tr;
                            }
                        }
                    }

                    double avg_fast = bp_sum_fast / tr_sum_fast;
                    double avg_middle = bp_sum_middle / tr_sum_middle;
                    double avg_slow = bp_sum_slow / tr_sum_slow;

                    b[Column_Result] = 100 * ((4 * avg_fast) + (2 * avg_middle) + avg_slow) / (4 + 2 + 1);
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; protected set; }

        public Color UpperColor { get; set; } = Color.ForestGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = Reference,
                        UpperLimit = UpperLimit,
                        LowerLimit = LowerLimit,
                        UpperColor = UpperColor,
                        LowerColor = LowerColor,
                        FixedTickStep_Right = 10,
                    });

                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}