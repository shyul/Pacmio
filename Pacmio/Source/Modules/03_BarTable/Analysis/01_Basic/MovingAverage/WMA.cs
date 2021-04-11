/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public sealed class WMA : MovingAverage
    {
        public WMA(int interval) : this(Bar.Column_Close, interval) { }

        public WMA(NumericColumn column, int interval) : base(column, interval)
        {
            Description = "Weighted Moving Average " + Label;
        }

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            Calculate(bap.Table, Column, Column_Result, bap.StartPt, bap.StopPt, Interval);
            /*
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
            }*/
        }

        public static void Calculate(BarTable bt, NumericColumn column, NumericColumn column_result, int startPt, int stopPt, int interval)
        {
            double sum = 0;
            for (int j = 1; j <= interval; j++)
            {
                sum += j;
            }

            for (int i = startPt; i < stopPt; i++)
            {
                double val = 0;

                for (int pt = 0; pt < interval; pt++)
                {
                    int ik = i - pt;
                    if (ik < 0) ik = 0;
                    val += bt[ik][column] * (interval - pt) / sum;
                }

                bt[i][column_result] = val;
            }
        }

        #endregion Calculation
    }
}
