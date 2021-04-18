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
    public sealed class HMA : MovingAverageAnalysis
    {
        public HMA(int interval) : this(Bar.Column_Close, interval) { }

        public HMA(NumericColumn column, int interval)
        {
            Interval = interval;
            Interval_Fast = (Interval / 2D).ToInt32();
            Interval_Sqrt = Math.Sqrt(Interval).ToInt32();
            Column = column;

            Label = (Column is null) ? "(error)" : ((Column == Bar.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + Label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Hull Moving Average " + Label;

            // Set Names for Each Column here
            IM_Column = new NumericColumn(Name + "_IM");

            /// Now Column knows you are using its data
            /// When deleting Column, you have to delete all
            /// Column's downstream analysis data first

            WMA_Fast = new WMA(Column, Interval_Fast) { ChartEnabled = false };
            WMA_Slow = new WMA(Column, Interval) { ChartEnabled = false };
        
            Order = WMA_Slow.Order + 1;
            HMA_Result = new WMA(IM_Column, Interval_Sqrt) { ChartEnabled = false, Order = Order + 1 };

            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                LegendName = GroupName,
                Label = Label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };

            WMA_Fast.AddChild(this);
            WMA_Slow.AddChild(this);
            this.AddChild(HMA_Result);
        }

        #region Calculation

        public override string Label { get; }

        public int Interval_Fast { get; }

        public int Interval_Sqrt { get; }

        public WMA WMA_Fast { get; }

        public WMA WMA_Slow { get; }

        public NumericColumn IM_Column { get; }

        public WMA HMA_Result { get; }

        public override NumericColumn Column_Result => HMA_Result.Column_Result;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                bt[i][IM_Column] = (2 * bt[i][WMA_Fast.Column_Result]) - bt[i][WMA_Slow.Column_Result];
            }
        }
        /*
        public static void Calculate(BarTable bt, NumericColumn column, NumericColumn column_result, int startPt, int stopPt, int interval, int interval_fast, int interval_sqrt)
        {
            WMA.Calculate(bt, column, Column_Result, startPt, stopPt, interval_fast);
            WMA.Calculate(bt, column, Column_Result, startPt, stopPt, interval);

            for (int i = startPt; i < stopPt; i++)
            {
                bt[i][IM_Column] = (2 * bt[i][WMA_Fast.Column_Result]) - bt[i][WMA_Slow.Column_Result];
            }

            WMA.Calculate(bt, column, Column_Result, startPt, stopPt, interval_sqrt);
        }
        */


        #endregion Calculation
    }
}
