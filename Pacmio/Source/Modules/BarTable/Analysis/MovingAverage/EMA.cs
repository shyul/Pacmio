/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public sealed class EMA : SMA
    {
        public EMA(int interval) : this(Bar.Column_Close, interval) { }

        public EMA(NumericColumn column, int interval) : base(column, interval)
        {
            Multiplier = 2D / (Interval + 1D);
            Description = "Exponential Moving Average " + LineSeries.Label;
        }

        #region Calculation

        public double Multiplier { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (i > 0)
                {
                    double y0 = bt[i - 1][Column_Result];
                    bt[i][Column_Result] = (bt[i][Column] - y0) * Multiplier + y0;
                }
                else
                    bt[i][Column_Result] = bt[i][Column];
            }
        }

        #endregion Calculation
    }
}
