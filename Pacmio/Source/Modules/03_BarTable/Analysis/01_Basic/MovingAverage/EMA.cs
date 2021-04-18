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
    public sealed class EMA : MovingAverageAnalysis
    {
        public EMA(int interval) : this(Bar.Column_Close, interval) { }

        public EMA(NumericColumn column, int interval) : base(column, interval)
        {
            Multiplier = 2D / (Interval + 1D);
            Description = "Exponential Moving Average " + Label;
        }

        #region Calculation

        public double Multiplier { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            Calculate(bap.Table, Column, Column_Result, bap.StartPt, bap.StopPt, Multiplier);

            /*
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
            }*/
        }

        public static void Calculate(BarTable bt, NumericColumn column, NumericColumn column_result, int startPt, int stopPt, double multiplier)
        {
            for (int i = startPt; i < stopPt; i++)
            {
                if (i > 0)
                {
                    double y0 = bt[i - 1][column_result];
                    bt[i][column_result] = (bt[i][column] - y0) * multiplier + y0;
                }
                else
                    bt[i][column_result] = bt[i][column];
            }
        }

        #endregion Calculation
    }
}
