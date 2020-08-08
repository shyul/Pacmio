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

namespace Pacmio // Can be derived from SMA
{
    public sealed class WMA : SMA
    {
        public WMA(int interval) : this(Bar.Column_Close, interval) { }

        public WMA(NumericColumn column, int interval) : base(column, interval)
        {
            Description = "Weighted Moving Average " + LineSeries.Label;
        }

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double sum = 0;
            for (int j = 1; j <= Interval; j++)
            {
                sum += j;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double val = 0;

                for (int pt = 0; pt < Interval; pt++)
                {
                    int ik = i - pt;
                    if (ik < 0) ik = 0;
                    val += bt[ik][Column] * (Interval - pt) / sum;
                }

                bt[i][Column_Result] = val;
            }
        }

        #endregion Calculation
    }
}
