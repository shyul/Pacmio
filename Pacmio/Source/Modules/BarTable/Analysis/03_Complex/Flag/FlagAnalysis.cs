/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    class FlagAnalysis : BarAnalysis, ILevelAnalysis, IChartBackground
    {
        public FlagAnalysis()
        {
            Name = GetType() + "_TestOnly";
            AreaName = MainBarChartArea.DefaultName;
            Column_Result = new DatumColumn(Name + "_Column", typeof(FlagDatum));
        }

        public int MaximumInterval { get; } = 12;

        public double MinimumRangeLocationRatio { get; } = 0.3;

        public double RunningBarTrueRangeMinimumRatio { get; } = 1.5;

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                var bars = bt[i, MaximumInterval].OrderByDescending(n => n.Index);
                int barsCount = bars.Count();

                FlagDatum d = new();

                List<FlagTestElement> testElements = new();

                foreach (Bar b0 in bars)
                {
                    d.TotalRange.Insert(b0.High);
                    d.TotalRange.Insert(b0.Low);
                    testElements.Add(new FlagTestElement(b0, d.TotalRange.Maximum - d.TotalRange.Minimum));
                }

                int j = 0;
                foreach (Bar b0 in bars)
                {
                    double rangeLocation = (b0.Typical - d.TotalRange.Minimum) / (d.TotalRange.Maximum - d.TotalRange.Minimum);
                    testElements[j].RangeLocation = rangeLocation;
                    j++;
                }

                d.LastFlagBarRangeLocation = testElements.First().RangeLocation;
                bool isRunning = false;

                if (d.LastFlagBarRangeLocation > (1 - MinimumRangeLocationRatio)) // Detect Bull Flag when the last bar range location is at the top.
                {
                    for (j = 0; j < barsCount; j++)
                    {
                        if (testElements[j].RangeLocation < (1 - MinimumRangeLocationRatio) && testElements[j].RangeLocation > (MinimumRangeLocationRatio / 2))
                            isRunning = testElements[j].IsRunning = true;
                        else if (isRunning)
                            break;
                    }

                    var running_bars = testElements.Where(n => n.IsRunning).OrderByDescending(n => n.Bar.Index);

                    if (running_bars.Count() > 0)
                    {
                        var white_bars = running_bars.Where(n => n.Bar.Gain > 0);

                        if ((white_bars.Count() / running_bars.Count()) > 0.7)
                        {
                            Bar lastRunningBar = running_bars.First().Bar;
                            var flag_bars = testElements.Where(n => n.Bar.Index > lastRunningBar.Index && n.Bar.Type != BarType.White).OrderByDescending(n => n.Bar.Index);

                            if (flag_bars.Count() > 1)
                            {
                                Bar firstFlagBar = flag_bars.Last().Bar;
                                Bar firstRunningBar = running_bars.Last().Bar;

                                ApexPt runUp_pt1 = new ApexPt(firstRunningBar.Index, firstRunningBar.Time, firstRunningBar.Low);
                                ApexPt runUp_pt2 = new ApexPt(firstFlagBar.Index, firstFlagBar.Time, firstFlagBar.High);

                                d.RunUp = new TrendLine(runUp_pt1, runUp_pt2);

                                var upper_trends = flag_bars.Select(n => new ApexPt(n.Bar.Index, n.Bar.Time, n.Bar.High)).SelectPair().Select(n => new TrendLine(n));
                                var lower_trends = flag_bars.Select(n => new ApexPt(n.Bar.Index, n.Bar.Time, n.Bar.Low)).SelectPair().Select(n => new TrendLine(n));

                                double average_upper_trendrate = upper_trends.Select(n => n.TrendRate).Sum() / upper_trends.Count();

                                if (true) //average_upper_trendrate <= 0)
                                {
                                    double average_lower_trendrate = lower_trends.Select(n => n.TrendRate).Sum() / lower_trends.Count();

                                    // Console.WriteLine("\nIndex = " + i);
                                    // Console.WriteLine("average_upper_trendrate = " + average_upper_trendrate);
                                    // Console.WriteLine("average_lower_trendrate = " + average_lower_trendrate);

                                    Bar lastFlagBar = flag_bars.First().Bar;

                                    int trend_index1 = firstFlagBar.Index;
                                    int trend_index2 = lastFlagBar.Index;

                                    double upper_trend_level1 = lastFlagBar.High - (average_upper_trendrate * (trend_index2 - trend_index1));
                                    ApexPt upper_trend_pt2 = new ApexPt(trend_index2, lastFlagBar.Time, lastFlagBar.High);
                                    ApexPt upper_trend_pt1 = new ApexPt(trend_index1, firstFlagBar.Time, upper_trend_level1);

                                    d.UpperFlag = new TrendLine(upper_trend_pt1, upper_trend_pt2);

                                    double lower_trend_level1 = lastFlagBar.Low - (average_lower_trendrate * (trend_index2 - trend_index1));
                                    ApexPt lower_trend_pt2 = new ApexPt(trend_index2, lastFlagBar.Time, lastFlagBar.Low);
                                    ApexPt lower_trend_pt1 = new ApexPt(trend_index1, firstFlagBar.Time, lower_trend_level1);

                                    d.LowerFlag = new TrendLine(lower_trend_pt1, lower_trend_pt2);


                                    d.Type = FlagType.Bull;
                                }
                            }
                        }
                    }
                }
                else if (d.LastFlagBarRangeLocation < MinimumRangeLocationRatio) // At the lower buttom
                {
                    for (j = 0; j < barsCount; j++)
                    {
                        if (testElements[j].RangeLocation > MinimumRangeLocationRatio && testElements[j].RangeLocation < (1 - (MinimumRangeLocationRatio / 2)))
                            isRunning = testElements[j].IsRunning = true;
                        else if (isRunning)
                            break;
                    }

                    var running_bars = testElements.Where(n => n.IsRunning).OrderByDescending(n => n.Bar.Index);
                    if (running_bars.Count() > 0)
                    {
                        var red_bars = running_bars.Where(n => n.Bar.Type == BarType.Red);

                        if ((red_bars.Count() / running_bars.Count()) > 0.7)
                        {
                            //d.Type = FlagType.Bear;





                        }
                    }
                }

                b[Column_Result] = d;
            }
        }

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; }

        public int DrawOrder { get; set; } = 0;

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (AreaName is string areaName && bc[areaName] is Area a)
            {
                int StartPt = a.StartPt;
                int StopPt = a.StopPt;

                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (StopPt > bc.DataCount) StopPt = bc.DataCount;

                for (int i = StartPt; i < StopPt; i++)
                {
                    if (StartPt > -1 && bc.BarTable[i] is Bar b && b[Column_Result] is FlagDatum fd && fd.Type != FlagType.None)
                    {
                        DrawTrendLine(g, fd.RunUp, a, Color.Blue);
                        DrawTrendLine(g, fd.LowerFlag, a, Color.Red);
                        DrawTrendLine(g, fd.UpperFlag, a, Color.Green);
                    }
                }

                g.SmoothingMode = SmoothingMode.Default;
                g.ResetClip();
            }
        }

        public static void DrawTrendLine(Graphics g, TrendLine line, Area a, Color c)
        {
            if (line is TrendLine)// && line.X1 != line.X2) 
            {
                int StartPt = a.StartPt;
                //int StopPt = a.StopPt;

                int x1 = line.X1 - StartPt;

                if (x1 >= 0)
                {
                    int x2 = line.X2 - StartPt;

                    //int x3 = StopPt - StartPt - 1;
                    double y1 = line.Y1;
                    double y2 = line.Y2; //y1 + (line.TrendRate * (x2 - x1));

                    Point pt1 = new(a.IndexToPixel(x1), a.AxisY(AlignType.Right).ValueToPixel(y1));
                    Point pt3 = new(a.IndexToPixel(x2), a.AxisY(AlignType.Right).ValueToPixel(y2));

                    g.DrawLine(new Pen(c), pt1, pt3);
                    /*
                    if (y2 > y1)
                        g.DrawLine(new Pen(Color.YellowGreen), pt1, pt3);
                    else if (y2 < y1)
                        g.DrawLine(new Pen(Color.Pink), pt1, pt3);
                    else
                        g.DrawLine(new Pen(Color.SkyBlue), pt1, pt3);*/
                }

            }
        }
    }
}
