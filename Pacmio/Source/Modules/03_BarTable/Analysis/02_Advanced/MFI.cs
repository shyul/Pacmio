/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public sealed class MFI : OscillatorAnalysis
    {
        public MFI(int interval)
        {
            Interval = interval;

            Label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Money Flow Index " + Label;

            PMF_Column = new NumericColumn(Name + "_PMF") { Label = "PMF" };
            NMF_Column = new NumericColumn(Name + "_NMF") { Label = "NMF" };

            Column_Result = new NumericColumn(Name, Label);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                Label = Label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            Reference = 50;
            UpperLimit = 80;
            LowerLimit = 20;

            Color = Color.FromArgb(255, 96, 96, 96);
            UpperColor = Color.Green;
            LowerColor = Color.OrangeRed;
        }

        public override string Label { get; }

        #region Calculation

        public int Interval { get; }

        public NumericColumn PMF_Column { get; }

        public NumericColumn NMF_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            int startPt = bap.StartPt;

            double last_typical = (bap.StartPt > 0) ? bt[startPt - 1].Typical : bt[startPt].Typical;

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double current_typical = b.Typical;
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
    }
}
