/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://www.forex.in.rs/smoothed-moving-average/
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
    public sealed class SMMA : SMA
    {
        public SMMA(int interval) : this(Bar.Column_Close, interval) { }

        public SMMA(NumericColumn column, int interval) : base(column, interval)
        {
            Description = "Smoothed Moving Average " + LineSeries.Label;
        }

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double sum = (bap.StartPt == 0) ? bt[0][Column] * Interval : bt[bap.StartPt - 1][Column_Result] * Interval;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (i != 0)
                    sum = sum - bt[i - 1][Column_Result] + bt[i][Column];

                bt[i][Column_Result] = sum / Interval;
            }
        }

        #endregion Calculation
    }
}
