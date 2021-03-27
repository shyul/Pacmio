/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class FlagAnalysis : PatternAnalysis, IChartBackground
    {
        public FlagAnalysis()
        {
            Name = GetType() + " Test Only";
            AreaName = MainBarChartArea.DefaultName;
            Column_Result = new PatternColumn(this, typeof(FlagDatum), MaximumInterval);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public int MinRunInterval { get; } = 4;

        public int MaxRunInterval { get; } = 8;

        public int MinPullPackInterval { get; } = 2;

        public int MaxPullPackInterval { get; } = 8;

        public double MinPullBackRatio { get; } = 0.0;

        public double MaxPullBackRatio { get; } = 0.4;

        public override int MaximumInterval => MaxRunInterval + MaxPullPackInterval;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                var bars = bt[i, MaximumInterval];

                if (bars.Count == MaximumInterval)
                {
                    //Console.WriteLine("Bars i = " + i + " | LastIndex = " + bars.Select(n => n.Index).ToString(",") + " | Count = " + bars.Count + " | Bar Index = " + bt[i].Index);

                    List<Bar> pull_back_bars = new();
                    List<Bar> run_bars = new();

                    FlagType type = FlagType.None;
                    bool testPullPack = true;

                    for (int j = 0; j < bars.Count; j++)
                    {
                        Bar b = bars[MaximumInterval - j - 1];

                        if (j == 0)
                        {
                            if (b.Open > b.Close)
                                type = FlagType.Bull;
                            else if (b.Open < b.Close)
                                type = FlagType.Bear;
                            else
                                break;
                        }

                        if (testPullPack)
                        {
                            if (b.Open > b.Close && type == FlagType.Bull)
                                pull_back_bars.Add(b);
                            else if (b.Open < b.Close && type == FlagType.Bear)
                                pull_back_bars.Add(b);
                            else if (j >= MinPullPackInterval)
                                testPullPack = false;
                            else
                                break;
                        }

                        if (!testPullPack)
                        {
                            if (b.Open < b.Close && type == FlagType.Bull)
                                run_bars.Add(b);
                            else if (b.Open > b.Close && type == FlagType.Bear)
                                run_bars.Add(b);
                            else
                                break;
                        }
                    }

                    if (type != FlagType.None && pull_back_bars.Count >= MinPullPackInterval && run_bars.Count >= MinRunInterval && run_bars.Count >= pull_back_bars.Count)
                    {
                        FlagDatum fd = new(type);

                        var run_up_highs = run_bars.Select(n => n.High);
                        var run_up_lows = run_bars.Select(n => n.Low);

                        fd.RunUpRange = new(run_up_lows.Min(), run_up_highs.Max());

                        var pull_back_highs = pull_back_bars.Select(n => n.High);
                        var pull_back_lows = pull_back_bars.Select(n => n.Low);

                        fd.PullBackRange = new(pull_back_lows.Min(), pull_back_highs.Max());
                        fd.TotalLevelRange = new(Math.Min(fd.RunUpRange.Min, fd.PullBackRange.Min), Math.Max(fd.RunUpRange.Max, fd.PullBackRange.Max));

                        fd.PullBackRatio = (fd.TotalLevelRange.Max - fd.PullBackRange.Min) / (fd.TotalLevelRange.Max - fd.TotalLevelRange.Min);

                        if (bt[i] is Bar b && fd.PullBackRatio >= MinPullBackRatio && fd.PullBackRatio <= MaxPullBackRatio)
                        {
                            // Yield Critical Levels
                            // Get the trend line of the pull back
                            fd.BreakOutLevel = fd.TotalLevelRange.Max;

                            fd.Levels.Add(new Level(fd.BreakOutLevel));

                            Console.WriteLine(bars.Last().Time + " | " + type + " is detected!!");
                            //Console.WriteLine(bars.Last().Time + " | " + bars.Last().BarType + " | " + type + " | pull_back_bars = " + pull_back_bars.Count + " | pull_back_bars = " + run_bars.Count);
                            // Yield Pattern
                            Bar b_p1 = run_bars.Last();
                            fd.P1 = new PivotPt(b_p1.Index, b_p1.Time, b_p1.Low);

                            Bar b_p2 = run_bars.First();
                            fd.P2 = new PivotPt(b_p2.Index, b_p2.Time, b_p2.High);
                            fd.P2_B = new PivotPt(b_p2.Index, b_p2.Time, b_p2.Low);

                            Bar b_p3 = pull_back_bars.First();
                            fd.P3 = new PivotPt(b_p3.Index, b_p3.Time, b_p3.High);
                            fd.P3_B = new PivotPt(b_p3.Index, b_p3.Time, b_p3.Low);

                            b[Column_Result] = fd;
                        }
                    }
                }
            }
        }

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
                        int x1 = a.IndexToPixel(fd.P1.Index - StartPt);
                        int x2 = a.IndexToPixel(fd.P2.Index - StartPt);
                        int x3 = a.IndexToPixel(fd.P3.Index - StartPt);

                        int y1 = a.AxisY(AlignType.Right).ValueToPixel(fd.P1.Level);
                        int y2 = a.AxisY(AlignType.Right).ValueToPixel(fd.P2.Level);
                        int y2_b = a.AxisY(AlignType.Right).ValueToPixel(fd.P2_B.Level);
                        int y3 = a.AxisY(AlignType.Right).ValueToPixel(fd.P3.Level);
                        int y3_b = a.AxisY(AlignType.Right).ValueToPixel(fd.P3_B.Level);

                        Point pt1 = new(x1, y1);
                        Point pt2 = new(x2, y2);
                        Point pt2_b = new(x2, y2_b);
                        Point pt3 = new(x3, y3);
                        Point pt3_b = new(x3, y3_b);

                        //Console.WriteLine(pt1.ToString());

                        GraphicsPath gp = new(); // new Point[] { pt1, pt2_b, pt2, pt3, pt3_b, pt2_b }, new byte[] { 0, 1, 1, 1, 1, 1 });

                        gp.AddLine(pt1, pt2);
                        gp.AddLine(pt2, pt3);
                        gp.AddLine(pt3, pt3_b);
                        gp.AddLine(pt3_b, pt2_b);
                        //gp.AddLine(pt2, pt2);

                        g.DrawPath(new Pen(Color.Blue, 2), gp);
                    }
                }

                g.SmoothingMode = SmoothingMode.Default;
                g.ResetClip();
            }
        }
    }
}
