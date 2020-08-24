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
    public sealed class HMA : SMA
    {
        public HMA(int interval) : this(Bar.Column_Close, interval) { }

        public HMA(NumericColumn column, int interval)
        {
            Interval = interval;
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == Bar.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Hull Moving Average " + label;

            // Set Names for Each Column here
            IM_Column = new NumericColumn(Name + "_IM");

            /// Now Column knows you are using its data
            /// When deleting Column, you have to delete all
            /// Column's downstream analysis data first

            WMA_Fast = new WMA(Column, (Interval / 2D).ToInt32());
            WMA_Fast.AddChild(this);

            WMA_Slow = new WMA(Column, Interval);
            WMA_Slow.AddChild(this);

            Order = WMA_Slow.Order + 1;
            HMA_Result = new WMA(IM_Column, Math.Sqrt(Interval).ToInt32()) { Order = Order + 1 };
            this.AddChild(HMA_Result);

            //Result_Column.Name = Name;
            //Result_Column.Label = label;
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };
        }

        #region Calculation

        public WMA WMA_Fast { get; }

        public WMA WMA_Slow { get; }

        public NumericColumn IM_Column { get; }

        public WMA HMA_Result { get; }

        public override NumericColumn Column_Result => HMA_Result.Column_Result;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            //WMA_Fast.Update(bap);
            //WMA_Slow.Update(bap);

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                bt[i][IM_Column] = (2 * bt[i][WMA_Fast.Column_Result]) - bt[i][WMA_Slow.Column_Result];
            }

            //HMA_Result.Update(bap);
        }

        #endregion Calculation
    }
}
