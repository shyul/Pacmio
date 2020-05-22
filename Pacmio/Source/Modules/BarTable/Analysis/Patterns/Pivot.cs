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
    public sealed class Pivot : BarAnalysis, IPatternAnalysis
    {
        public Pivot(BarFreq barFreq = BarFreq.Daily)
        {
            SourceBarFreq = barFreq;
            SourceFrequency = SourceBarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;

            string label = "[" + SourceFrequency.ToString() + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = "Pattern: ";

            Pattern_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };
        }

        public BarFreq SourceBarFreq { get; }

        public Frequency SourceFrequency { get; }

        public double Weight { get; }

        public double Tolerance { get; } = 0.005;

        public ObjectColumn Pattern_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bt.Frequency.Span < SourceFrequency.Span)
            {
                BarTable bt_high = bt.Contract.GetTable(SourceBarFreq, bt.Type);

                /*
                if (bt_high.IsEmpty || bt_high.FirstTime > bt.FirstTime.AddDays(-1))
                {
                    Period pd = new Period(bt.FirstTime.AddDays(-5), DateTime.Now); // AddDays(-?) should be replaced with Frequency aware alignment
                    //bt_high.LoadJsonFileToFileData();
                    //bt_high.TransferActualValuesFromFileDataToBars(pd);
                    //bt_high.AdjustThenCalculate();
                    bt_high.Reset(pd, null, null);
                    while (!bt_high.ReadyForTickCalculation) { }
                }
                */

                DateTime base_time = DateTime.MinValue;

                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = SourceFrequency.Align(b.Time, -1); // Get yesterday's time.

                    if (time != base_time)
                    {
                        Period length = new Period(SourceFrequency.Align(b.Time), SourceFrequency.Align(b.Time, 1));

                    Find_BP:

                        if (bt_high.IndexOf(ref time) < 2) break;
                        Bar b_p = bt_high[time];

                        if(b_p is null) 
                        {
                            time = SourceFrequency.Align(b.Time, -1);
                            goto Find_BP;
                        }

                        double p = b_p.Typical;
                        double s1 = (2 * p) - b_p.High;
                        double s2 = p - b_p.High + b_p.Low;
                        double r1 = (2 * p) - b_p.Low;
                        double r2 = p + b_p.High - b_p.Low;

                        Console.WriteLine("time: " + length.Start + "; p = " + p + "; s1 = " + s1 + "; s2 = " + s2 + "; r1 = " + r1 + "; r2 = " + r2);

                        PivotPatternDatum datum = new PivotPatternDatum()
                        {
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            R2 = r2,
                            R1 = r1,
                            P = p,
                            S1 = s1,
                            S2 = s2
                        };

                        TrendLineInfo tl_p = new TrendLineInfo()
                        {
                            Analysis = this,
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            Point1 = (length.Start, p),
                            Point2 = (length.Stop, p),
                            Label = "P",
                        };

                        TrendLineInfo tl_s1 = new TrendLineInfo()
                        {
                            Analysis = this,
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            Point1 = (length.Start, s1),
                            Point2 = (length.Stop, s1),
                            Label = "S1",
                        };

                        TrendLineInfo tl_s2 = new TrendLineInfo()
                        {
                            Analysis = this,
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            Point1 = (length.Start, s2),
                            Point2 = (length.Stop, s2),
                            Label = "S2",
                        };

                        TrendLineInfo tl_r1 = new TrendLineInfo()
                        {
                            Analysis = this,
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            Point1 = (length.Start, r1),
                            Point2 = (length.Stop, r1),
                            Label = "R1",
                        };

                        TrendLineInfo tl_r2 = new TrendLineInfo()
                        {
                            Analysis = this,
                            Length = length,
                            Weight = Weight,
                            Tolerance = Tolerance,
                            Point1 = (length.Start, r2),
                            Point2 = (length.Stop, r2),
                            Label = "R2",
                        };

                        datum.TrendLines.AddRange(new TrendLineInfo[] { tl_p, tl_s1, tl_s2, tl_r1, tl_r2 });

                        b[Pattern_Column] = datum;

                        // Check add...
                        bt.CurrentTrendLines.AddRange(datum.TrendLines);

                        base_time = time;
                    }
                }
            }
        }
    }
}
