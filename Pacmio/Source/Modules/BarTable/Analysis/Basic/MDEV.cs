/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Mean Deviation
/// 
/// ***************************************************************************

using System;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class MDEV : SMA
    {
        public MDEV(NumericColumn column, int interval)
            : this(column, new SMA(column, interval)) { }

        public MDEV(NumericColumn column, SMA sma)
        {
            Interval = sma.Interval;
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == BarTable.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + label;
            GroupName = (Column == BarTable.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            Result_Column = new NumericColumn(Name) { Label = label };

            SMA = sma;
            SMA.AddChild(this);

            LineSeries = new LineSeries(Result_Column) { DrawLimitShade = false };

            Description = "Mean Deviation " + (column is null ? "(error)" : label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ SMA.GetHashCode() ^ Interval;

        #region Calculation

        public SMA SMA { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double sum = 0;
                for (int j = 0; j < Interval; j++)
                {
                    int head = i - j;
                    if (i < Interval) head = 0;
                    sum += Math.Abs(bt[head][Column] - bt[i][SMA.Result_Column]);
                }
                bt[i][Result_Column] = sum / Interval;
            }
        }

        #endregion Calculation
    }
}
