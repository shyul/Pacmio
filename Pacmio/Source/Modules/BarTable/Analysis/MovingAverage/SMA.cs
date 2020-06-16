/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class SMA : BarAnalysis, ISingleData, IChartSeries
    {
        public SMA(int interval) : this(BarTable.Column_Close, interval) { }

        public SMA(NumericColumn column, int interval)
        {
            Interval = interval;
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == BarTable.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + label;
            GroupName = (Column == BarTable.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Simple Moving Average " + label;

            Result_Column = new NumericColumn(Name) { Label = label };
            LineSeries = new LineSeries(Result_Column)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };
        }

        protected SMA() { }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Interval;

        public virtual int Interval { get; protected set; }

        public NumericColumn Column { get; protected set; }

        #endregion Parameters

        #region Calculation

        public virtual NumericColumn Result_Column { get; protected set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double last_sum = 0;

            for (int i = 0; i < Interval; i++)
            {
                int j = bap.StartPt - i;
                if (j < 0) j = 0;
                last_sum += bt[j][Column];
            }

            bt[bap.StartPt][Result_Column] = last_sum / Interval;

            for (int i = bap.StartPt + 1; i < bap.StopPt; i++)
            {
                int head = i - Interval;
                if (head < 0) head = 0;
                last_sum = last_sum - bt[head][Column] + bt[i][Column];
                bt[i][Result_Column] = last_sum / Interval;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = LineSeries.ShadeColor = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public LineSeries LineSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; } = MainArea.DefaultName;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, 10)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}