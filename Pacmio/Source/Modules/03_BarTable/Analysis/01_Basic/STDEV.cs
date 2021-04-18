/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Standard Deviation
/// 
/// ***************************************************************************

using System;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class STDEV : CustomIntervalAnalysis
    {
        public STDEV(NumericColumn column, int interval)
            : this(column, new SMA(column, interval)) { }

        public STDEV(NumericColumn column, SMA sma)
        {
            Interval = sma.Interval;
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == Bar.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            Column_Result = new NumericColumn(Name, label);
            Column_Percent = new NumericColumn(Name + "_Percent", label);

            SMA = sma;
            SMA.AddChild(this);

            LineSeries = new LineSeries(Column_Result) { DrawLimitShade = false };

            Description = "Standard Deviation " + (column is null ? "(error)" : label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ SMA.GetHashCode() ^ Interval;

        #region Calculation

        public SMA SMA { get; }

        public NumericColumn Column_Percent { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double sum = 0;
                for (int j = 0; j < Interval; j++)
                {
                    int head = i - j;
                    if (i < Interval) head = 0;
                    sum += Math.Pow(bt[head][Column] - b[SMA.Column_Result], 2);
                }
                
                double stdev = b[Column_Result] = Math.Sqrt(sum / Interval);
                b[Column_Percent] = stdev * 100 / b[Column];
            }
        }

        #endregion Calculation
    }
}