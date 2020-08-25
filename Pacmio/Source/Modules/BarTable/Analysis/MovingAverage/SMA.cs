/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class SMA : BarAnalysis, ISingleData, IChartSeries
    {
        public SMA(int interval) : this(Bar.Column_Close, interval) { }

        public SMA(NumericColumn column, int interval)
        {
            Interval = interval;
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == Bar.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Simple Moving Average " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            LineSeries = new LineSeries(Column_Result)
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

        public virtual NumericColumn Column_Result { get; protected set; }

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

            bt[bap.StartPt][Column_Result] = last_sum / Interval;

            for (int i = bap.StartPt + 1; i < bap.StopPt; i++)
            {
                int head = i - Interval;
                if (head < 0) head = 0;
                last_sum = last_sum - bt[head][Column] + bt[i][Column];
                bt[i][Column_Result] = last_sum / Interval;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = LineSeries.EdgeColor = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; set; } = MainBarChartArea.DefaultName;

        public float AreaRatio { get; set; } = 12;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea a = bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}