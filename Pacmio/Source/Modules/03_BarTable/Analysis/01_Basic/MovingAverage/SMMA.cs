/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public sealed class SMMA : MovingAverage
    {
        public SMMA(int interval) : this(Bar.Column_Close, interval) { }

        public SMMA(NumericColumn column, int interval) : base(column, interval)
        {
            Description = "Smoothed Moving Average " + Label;
        }

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            Calculate(bap.Table, Column, Column_Result, bap.StartPt, bap.StopPt, Interval);
            /*
            BarTable bt = bap.Table;

            double sum = (bap.StartPt == 0) ? bt[0][Column] * Interval : bt[bap.StartPt - 1][Column_Result] * Interval;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (i != 0)
                    sum = sum - bt[i - 1][Column_Result] + bt[i][Column];

                bt[i][Column_Result] = sum / Interval;
            }*/
        }

        public static void Calculate(BarTable bt, NumericColumn column, NumericColumn column_result, int startPt, int stopPt, int interval)
        {
            double sum = (startPt == 0) ? bt[0][column] * interval : bt[startPt - 1][column_result] * interval;

            for (int i = startPt; i < stopPt; i++)
            {
                if (i != 0)
                    sum = sum - bt[i - 1][column_result] + bt[i][column];

                bt[i][column_result] = sum / interval;
            }
        }

        #endregion Calculation
    }
}
