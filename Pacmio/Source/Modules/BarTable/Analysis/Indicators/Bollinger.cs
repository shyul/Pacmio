﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:bollinger_bands
/// Ref 2: https://www.udemy.com/course/advance-trading-strategies/learn/lecture/9783978#overview
/// Ref 3: https://www.udemy.com/course/technical_analysis/learn/lecture/7570094#overview
/// 
/// Bollinger Band 
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class Bollinger : BarAnalysis, IDualData, IChartSeries
    {
        public Bollinger(int interval, double spread) : this(TableList.Column_Close, interval, spread) { }

        public Bollinger(NumericColumn column, int interval, double spread = 2.0)
        {
            Spread = spread;

            string label = column == TableList.Column_Close ?
                            "(" + interval.ToString() + "," + Spread.ToString() + ")" :
                            "(" + column.Name + "," + interval.ToString() + "," + Spread.ToString() + ")";

            Name = GroupName = "BOLNGR" + (column is null ? "(error)" : label);
            Description = "Bollinger Band " + (column is null ? "(error)" : label);

            SMA = new SMA(column, interval)
            {
                GroupName = GroupName
            };

            SMA.LineSeries.Name = Name + "_MA";
            SMA.LineSeries.Label = "MA";
            SMA.LineSeries.LegendName = GroupName;
            SMA.AddChild(this);

            STDEV = new STDEV(column, SMA)
            {
                GroupName = GroupName
            };
            STDEV.ChartEnabled = false;
            STDEV.AddChild(this);

            High_Column = new NumericColumn(Name + "_BBH") { Label = "H" };
            Low_Column = new NumericColumn(Name + "_BBL") { Label = "L" };

            BandSeries = new BandSeries(High_Column, Low_Column, SMA.LineSeries.Color)
            {
                Name = Name,
                LegendName = GroupName,
                IsAntialiasing = true,
                Importance = Importance.Minor,
            };

            Color = Color.FromArgb(240, 224, 192);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ SMA.Column.GetHashCode() ^ SMA.Interval ^ Spread.GetHashCode();

        #region Parameters

        public int Interval => SMA.Interval;

        public double Spread { get; }

        #endregion Parameters

        #region Calculation

        public SMA SMA { get; }

        public STDEV STDEV { get; }

        public NumericColumn High_Column { get; }

        public NumericColumn Low_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double ma = bt[i][SMA.Result_Column];
                double stdev = bt[i][STDEV.Result_Column] * Spread;
                bt[i][High_Column] = ma + stdev;
                bt[i][Low_Column] = ma - stdev;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color
        {
            get
            {
                return SMA.LineSeries.Color;
            }
            set
            {
                BandSeries.Color =
                BandSeries.ShadeColor =
                BandSeries.LowColor =
                BandSeries.LowShadeColor =
                SMA.LineSeries.Color =
                SMA.LineSeries.ShadeColor = value;
                BandSeries.FillColor = value.Opaque(64);
            }
        }

        public float LineWidth { get => SMA.LineSeries.Width; set => SMA.LineSeries.Width = value; }

        public LineType LineType { get => SMA.LineSeries.LineType; set => BandSeries.LineType = SMA.LineSeries.LineType = value; }

        public BandSeries BandSeries { get; }

        public bool ChartEnabled { get => Enabled && SMA.ChartEnabled && BandSeries.Enabled; set => SMA.ChartEnabled = BandSeries.Enabled = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; private set; } = MainArea.DefaultName;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, 10)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(BandSeries);
                a.AddSeries(SMA.LineSeries);
            }
        }

        #endregion Series
    }
}
