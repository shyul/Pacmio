/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Typical Price = (High + Low + Close)/3
/// 
/// Raw Money Flow = Typical Price x Volume
/// Money Flow Ratio = (14-period Positive Money Flow)/(14-period Negative Money Flow)
/// 
/// Money Flow Index = 100 - 100/(1 + Money Flow Ratio)
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:money_flow_index_mfi
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class MFI : ATR, IOscillator
    {
        public MFI(int interval)
        {
            Interval = interval;
            Column_Typical = BarTable.TrueRangeAnalysis.Column_Typical;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Money Flow Index " + label;

            PMF_Column = new NumericColumn(Name + "_PMF") { Label = "PMF" };
            NMF_Column = new NumericColumn(Name + "_NMF") { Label = "NMF" };

            Column_Result = new NumericColumn(Name) { Label = label };
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

        #region Calculation

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 80;

        public double LowerLimit { get; set; } = 20;

        public NumericColumn Column_Typical { get; }

        public NumericColumn PMF_Column { get; }

        public NumericColumn NMF_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            int startPt = bap.StartPt;

            double last_typical = (bap.StartPt > 0) ? bt[startPt - 1][Column_Typical] : bt[startPt][Column_Typical];

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double current_typical = b[Column_Typical];
                double val = b.Volume * current_typical;

                if (current_typical < last_typical)
                {
                    b[PMF_Column] = 0;
                    b[NMF_Column] = val;
                }
                else
                {
                    b[PMF_Column] = val;
                    b[NMF_Column] = 0;
                }

                last_typical = current_typical;

                double sum_p = 0, sum_n = 0;
                for (int j = 0; j < Interval; j++)
                {
                    int k = i - j;
                    if (k < 0) k = 0;
                    sum_p += bt[k][PMF_Column];
                    sum_n += bt[k][NMF_Column];
                }

                b[Column_Result] = (sum_n != 0) ? 100 - (100 / (1 + (sum_p / sum_n))) : 100;
            }
        }

        #endregion Calculation

        #region Series

        public Color UpperColor { get; set; } = Color.Green;

        public Color LowerColor { get; set; } = Color.OrangeRed;

        public override void ConfigChart(BarChart bc)
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
