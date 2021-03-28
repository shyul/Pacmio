/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:bollinger_bands
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

            Column_High = new NumericColumn(Name + "_Long") { Label = "Long" };
            Column_Low = new NumericColumn(Name + "_Short") { Label = "Short" };

            LineSeries_H = new LineSeries(Column_High)
            {
                Name = Name + "_Long",
                LegendName = GroupName,
                Label = "Long",
                IsAntialiasing = true,
                DrawLimitShade = false,
                LineType = LineType.Step
            };

            LineSeries_L = new LineSeries(Column_Low)
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

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double atr = Spread * b[ATR.Column_Result];// b.TrueRange; // 

                //Console.WriteLine("atr = " + atr);

                b[Column_High] = b[Channel.Column_High] - atr;
                b[Column_Low] = b[Channel.Column_Low] + atr;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries_H.Color; set => LineSeries_H.Color = LineSeries_H.EdgeColor = value; }

        public Color UpperColor { get => Color; set => Color = value; }

        public Color LowerColor { get => LineSeries_L.Color; set => LineSeries_L.Color = LineSeries_L.EdgeColor = value; }

        public float LineWidth { get => LineSeries_H.Width; set => LineSeries_H.Width = LineSeries_L.Width = value; }

        public LineType LineType { get => LineSeries_H.LineType; set => LineSeries_H.LineType = LineSeries_L.LineType = value; }

        public Series MainSeries => LineSeries_H;

        public LineSeries LineSeries_H { get; }

        public LineSeries LineSeries_L { get; }

        public bool ChartEnabled { get => Enabled && LineSeries_H.Enabled && LineSeries_L.Enabled; set => LineSeries_H.Enabled = LineSeries_L.Enabled = value; }

        public int DrawOrder { get => LineSeries_H.Order; set => LineSeries_H.Order = LineSeries_L.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; private set; } = MainBarChartArea.DefaultName;

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea a = bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                {
                    Order = AreaOrder,
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries_H);
                a.AddSeries(LineSeries_L);
            }
        }

        #endregion Series
    }
}