/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:bollinger_bands
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class Chanderlier : BarAnalysis, IDualData, IChartSeries
    {
        public Chanderlier(int interval = 22, double spread = 3.0)
        {
            Spread = spread;

            string label = "(" + interval.ToString() + "," + Spread.ToString() + ")";
            Name = GetType().Name + label;
            Description = "Chanderlier Exit " + label;
            GroupName = Description + " ";

            ATR = new ATR(interval) { ChartEnabled = false };
            ATR.AddChild(this);

            Channel = new PriceChannel(interval) { ChartEnabled = false };
            Channel.AddChild(this);

            High_Column = new NumericColumn(Name + "_Long") { Label = "Long" };
            Low_Column = new NumericColumn(Name + "_Short") { Label = "Short" };

            LineSeries_H = new LineSeries(High_Column)
            {
                Name = Name + "_Long",
                LegendName = GroupName,
                Label = "Long",
                IsAntialiasing = true,
                DrawLimitShade = false,
                LineType = LineType.Step
            };

            LineSeries_L = new LineSeries(Low_Column)
            {
                Name = Name + "_Short",
                LegendName = GroupName,
                Label = "Short",
                IsAntialiasing = true,
                DrawLimitShade = false,
                LineType = LineType.Step
            };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval ^ Spread.GetHashCode();

        #region Parameters

        public int Interval => ATR.Interval;

        public double Spread { get; }

        #endregion Parameters

        #region Calculation

        public ATR ATR { get; }

        public PriceChannel Channel { get; }

        public NumericColumn High_Column { get; }

        public NumericColumn Low_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double atr = Spread * b[ATR.Result_Column];// b.TrueRange; // 

                //Console.WriteLine("atr = " + atr);

                b[High_Column] = b[Channel.High_Column] - atr;
                b[Low_Column] = b[Channel.Low_Column] + atr;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries_H.Color; set => LineSeries_H.Color = LineSeries_H.ShadeColor = value; }

        public Color Low_Color { get => LineSeries_L.Color; set => LineSeries_L.Color = LineSeries_L.ShadeColor = value; }

        public float LineWidth { get => LineSeries_H.Width; set => LineSeries_H.Width = LineSeries_L.Width = value; }

        public LineType LineType { get => LineSeries_H.LineType; set => LineSeries_H.LineType = LineSeries_L.LineType = value; }

        public LineSeries LineSeries_H { get; }

        public LineSeries LineSeries_L { get; }

        public bool ChartEnabled { get => Enabled && LineSeries_H.Enabled && LineSeries_L.Enabled; set => LineSeries_H.Enabled = LineSeries_L.Enabled = value; }

        public int SeriesOrder { get => LineSeries_H.Order; set => LineSeries_H.Order = LineSeries_L.Order = value; }

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
                a.AddSeries(LineSeries_H);
                a.AddSeries(LineSeries_L);
            }
        }

        #endregion Series
    }
}