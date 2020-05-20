/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class DualData : BarAnalysis, ISignalAnalysis, IChartSeries
    {
        public DualData(NumericColumn fast_column, NumericColumn slow_column) 
        {
            Fast_Column = fast_column;
            Slow_Column = slow_column;

            string label = "[" + Fast_Column.Name + "," + Slow_Column.Name + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = GetType().Name + ": ";

            Signal_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };
            CloudSeries = new CloudSeries(Fast_Column, Slow_Column)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                Importance = Importance.Minor,
                IsAntialiasing = true
            };

            ChartEnabled = true;
            //BullishColor = Color.Teal;
            //BearishColor = Color.Peru;
        }

        public DualData(ISingleData fast_analysis, ISingleData slow_analysis)
        {
            Fast_Column = fast_analysis.Result_Column;
            Slow_Column = slow_analysis.Result_Column;

            string label = "[" + Fast_Column.Name + "," + Slow_Column.Name + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = Description + ": ";

            slow_analysis.AddChild(this);
            fast_analysis.AddChild(this);

            Signal_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };
            CloudSeries = new CloudSeries(Fast_Column, Slow_Column)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                Importance = Importance.Minor,
                IsAntialiasing = true
            };

            if (fast_analysis is IChartSeries fast_ics)
            {
                fast_ics.ChartEnabled = false;
                Fast_Column.Label = "Fast";
                BullishColor = fast_ics.Color;
                AreaName = fast_ics.AreaName;
            }

            if (slow_analysis is IChartSeries slow_ics)
            {
                slow_ics.ChartEnabled = false;
                Slow_Column.Label = "Slow";// slow_analysis.Name;
                BearishColor = slow_ics.Color;
            }
        }

        public DualData(IDualData analysis)
        {
            Fast_Column = analysis.High_Column;
            Slow_Column = analysis.Low_Column;

            string label = "(" + analysis.Name + ")";

            Name = GetType().Name + label;
            GroupName = "DualData " + label + ": ";// "DualData: ";
            Description = "DualData " + label;

            analysis.AddChild(this);

            Signal_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };

            if (analysis is IChartSeries ics)
            {
                ics.ChartEnabled = true;
                ChartEnabled = false;
            }
            else
            {
                Fast_Column.Label = "Fast"; 
                Slow_Column.Label = "Slow";
                CloudSeries = new CloudSeries(Fast_Column, Slow_Column)
                {
                    Name = Name,
                    LegendName = GroupName,
                    Label = label,
                    Importance = Importance.Minor,
                    IsAntialiasing = true
                };
                BullishColor = Color.Teal;
                BearishColor = Color.Peru;
            }
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_Column.GetHashCode() ^ Slow_Column.GetHashCode();

        public NumericColumn Fast_Column { get; private set; }

        public NumericColumn Slow_Column { get; private set; }

        #endregion Parameters

        #region Calculation

        public ObjectColumn Signal_Column { get; private set; }

        public readonly Dictionary<DualDataType, double> TypeToScore = new Dictionary<DualDataType, double> 
        {
            { DualDataType.Above, 0.25 },
            { DualDataType.Below, 0.25 },
            { DualDataType.Expansion, 0.4 },
            { DualDataType.Contraction, 0.2 },
            { DualDataType.CrossUp, 3 },
            { DualDataType.CrossDown, 3 },
            { DualDataType.TrendUp, 1 },
            { DualDataType.TrendDown, 1 },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double last_delta = 0;

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }
            else
            {
                Bar b_1 = bt[bap.StartPt - 1];
                DualDataSignalDatum dsd_1 = (DualDataSignalDatum)b_1[Signal_Column];
                last_delta = dsd_1.Delta;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                DualDataSignalDatum dsd = new DualDataSignalDatum();
                b[Signal_Column] = dsd;

                double value_fast = bt[i, Fast_Column];
                double value_slow = bt[i, Slow_Column];
                double last_value_fast = bt[i - 1, Fast_Column];
                double last_value_slow = bt[i - 1, Slow_Column];

                if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow) && !double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                {
                    double delta = value_fast - value_slow;
                    dsd.DeltaChange = delta - last_delta;

                    if (delta > 0)
                    {
                        dsd.Types.Add(DualDataType.Above);
                        dsd.BullishScore += TypeToScore[DualDataType.Above];
                    }
                    else if (delta < 0)
                    {
                        dsd.Types.Add(DualDataType.Below);
                        dsd.BearishScore += TypeToScore[DualDataType.Below];
                    }

                    if (value_fast > last_value_fast && value_slow > last_value_slow)
                    {
                        dsd.Types.Add(DualDataType.TrendUp);
                        if (delta > 0) dsd.BullishScore += TypeToScore[DualDataType.TrendUp];
                    }
                    else if (value_fast < last_value_fast && value_slow < last_value_slow)
                    {
                        dsd.Types.Add(DualDataType.TrendDown);
                        if (delta < 0) dsd.BearishScore += TypeToScore[DualDataType.TrendDown];
                    }

                    if (delta >= 0 && last_delta < 0)
                    {
                        dsd.Types.Add(DualDataType.CrossUp);
                        dsd.BullishScore += TypeToScore[DualDataType.CrossUp];
                    }
                    else if (delta <= 0 && last_delta > 0)
                    {
                        dsd.Types.Add(DualDataType.CrossDown);
                        dsd.BearishScore += TypeToScore[DualDataType.CrossDown];
                    }

                    double delta_abs = Math.Abs(delta);
                    double last_delta_abs = Math.Abs(last_delta);
                    if (delta_abs > last_delta_abs)
                    {
                        dsd.Types.Add(DualDataType.Expansion);
                        if (delta > 0) dsd.BullishScore += TypeToScore[DualDataType.Expansion];
                        else if (delta < 0) dsd.BearishScore += TypeToScore[DualDataType.Expansion];
                    }
                    else
                    {
                        dsd.Types.Add(DualDataType.Contraction);
                        if (delta > 0) dsd.BearishScore += TypeToScore[DualDataType.Contraction];
                        else if (delta < 0) dsd.BullishScore += TypeToScore[DualDataType.Contraction];
                    }

                    //if (dsd.Types.Contains(DualDataShapeType.CrossUp)) Console.WriteLine(b.Time.ToString("yyyy-MM-dd HH:mm:ss") + " Dual Data: CrossUp");
                    //if (dsd.Types.Contains(DualDataShapeType.CrossDown)) Console.WriteLine(b.Time.ToString("yyyy-MM-dd HH:mm:ss") + " Dual Data: CrossDown");

                    dsd.Delta = last_delta = delta;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => BullishColor; set => BullishColor = value; }

        public Color BullishColor
        {
            get => BullishTheme.ForeColor;
            set
            {
                BullishTheme.ForeColor = BullishTheme.EdgeColor = CloudSeries.Color = value;
                BullishTheme.FillColor = CloudSeries.FillColor = value.Opaque(64);
            }
        }

        public Color BearishColor
        {
            get => BearishTheme.ForeColor;
            set
            {
                BearishTheme.ForeColor = BearishTheme.EdgeColor = CloudSeries.LowColor = value;
                BearishTheme.FillColor = CloudSeries.LowFillColor = value.Opaque(64);
            }
        }

        public ColorTheme BullishTheme { get; } = new ColorTheme(Color.Teal, Color.Teal.Opaque(64));

        public ColorTheme BearishTheme { get; } = new ColorTheme(Color.Orange, Color.Peru.Opaque(64));

        public CloudSeries CloudSeries { get; private set; }

        public bool ChartEnabled { get => Enabled && CloudSeries.Enabled; set => CloudSeries.Enabled = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; set; } = MainArea.DefaultName;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, 10)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(CloudSeries);
            }
        }

        #endregion Series
    }
}
