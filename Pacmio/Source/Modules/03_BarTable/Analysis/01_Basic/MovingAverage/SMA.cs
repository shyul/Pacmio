/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=trading_strategies:moving_momentum
/// 
/// Moving averages are trend-following indicators that lag price. This means
/// the actual trend changes before the moving averages generate a signal.
/// Many traders are turned off by this lag, but this does not make them totally ineffective.
/// Moving averages smooth prices and provide chartists with a cleaner price plot,
/// which makes it easier to identify the general trend.
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
    public sealed class SMA : MovingAverage
    {
        public SMA(int interval) : this(Bar.Column_Close, interval) { }

        public SMA(NumericColumn column, int interval) : base(Bar.Column_Close, interval)
        {
            Description = "Simple Moving Average " + Label;
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            Calculate(bap.Table, Column, Column_Result, bap.StartPt, bap.StopPt, Interval);

            /*
            BarTable bt = bap.Table;

            double last_sum = 0;

            for (int i = 0; i < Interval; i++)
            {
                int j = bap.StartPt - i;
                if (j < 0) j = 0;
                last_sum += bt[j][Column];
            }

            bt[bap.StartPt][Column_Result] = last_sum / Interval;

            for (int i = bap.StartPt + 1; i < bap.StopPt; i++)
            {
                int head = i - Interval;
                if (head < 0) head = 0;
                last_sum = last_sum - bt[head][Column] + bt[i][Column];
                bt[i][Column_Result] = last_sum / Interval;
            }*/
        }

        public static void Calculate(BarTable bt, NumericColumn column, NumericColumn column_result, int startPt, int stopPt, int interval)
        {
            double last_sum = 0;

            for (int i = 0; i < interval; i++)
            {
                int j = startPt - i;
                if (j < 0) j = 0;
                last_sum += bt[j][column];
            }

            bt[startPt][column_result] = last_sum / interval;

            for (int i = startPt + 1; i < stopPt; i++)
            {
                int head = i - interval;
                if (head < 0) head = 0;
                last_sum = last_sum - bt[head][column] + bt[i][column];
                bt[i][column_result] = last_sum / interval;
            }
        }
    }
}