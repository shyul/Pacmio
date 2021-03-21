/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class FlagAnalysis : PatternAnalysis
    {
        public FlagAnalysis() 
        {
            Name = GetType() + " Test Only";

            Column_Result = new PatternColumn(this, typeof(FlagDatum), MaximumInterval);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public int MinRunUpInterval { get; } = 3;

        public int MaxRunUpInterval { get; } = 8;

        public int MinPullPackInterval { get; } = 2;

        public int MaxPullPackInterval { get; } = 5;

        public double MinPullBackRatio { get; } = 0.0;

        public double MaxPullBackRatio { get; } = 0.25;

        public override int MaximumInterval => MaxRunUpInterval + MaxPullPackInterval;

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
                    List<Bar> run_up_bars = new();
                    FlagType type = FlagType.None;
                    bool testFlag = true;

                    for (int j = 0; j < bars.Count; j++)
                    {
                        Bar b = bt[i - j];

                        if (j == 0)
                        {
                            testFlag = true;

                            if (b.BarType == BarType.Red)
                                type = FlagType.Bull;
                            else if (b.BarType == BarType.White)
                                type = FlagType.Bear;
                            else
                                break;

                            pull_back_bars.Add(b);
                        }
                        else
                        {
                            if (j > MaxPullPackInterval)
                            {
                                testFlag = false;
                            }

                            if (testFlag)
                            {
                                if (b.BarType == BarType.Red && type == FlagType.Bull)
                                {
                                    pull_back_bars.Add(b);
                                }
                                else if (b.BarType == BarType.White && type == FlagType.Bear)
                                {
                                    pull_back_bars.Add(b);
                                }
                                else if (j >= MinPullPackInterval && (b.BarType == BarType.White || b.BarType == BarType.Red))
                                {
                                    testFlag = false;
                                    run_up_bars.Add(b);
                                }
                                else
                                    break;
                            }
                            else if (b.BarType == BarType.White || b.BarType == BarType.Red)
                            {
                                run_up_bars.Add(b);
                            }
                            else
                                break;
                        }
                    }

                    if (type != FlagType.None && pull_back_bars.Count > MinPullPackInterval && run_up_bars.Count > MinRunUpInterval)
                    {
                        Bar b = bt[i];

                        FlagDatum fd = new FlagDatum(type);
                        b[Column_Result] = fd;

                        var run_up_highs = run_up_bars.Select(n => n.High);
                        var run_up_lows = run_up_bars.Select(n => n.Low);

                        fd.RunUpRange = new(run_up_lows.Min(), run_up_highs.Max());

                        var pull_back_highs = pull_back_bars.Select(n => n.High);
                        var pull_back_lows = pull_back_bars.Select(n => n.Low);

                        fd.PullBackRange = new(pull_back_lows.Min(), pull_back_highs.Max());
                        fd.TotalRange = new(Math.Min(fd.RunUpRange.Min, fd.PullBackRange.Min), Math.Max(fd.RunUpRange.Max, fd.PullBackRange.Max));

                        fd.PullBackRatio = (fd.TotalRange.Max - fd.PullBackRange.Min) / (fd.TotalRange.Max - fd.TotalRange.Min);

                        if (fd.PullBackRatio > MinPullBackRatio && fd.PullBackRatio < MaxPullBackRatio)
                        {
                            // Get the trend line of the pull back

                            fd.BreakOutLevel = fd.TotalRange.Max;


                        }

                        Console.WriteLine(type + " is detected!!");

                        // Yield Pattern
                        // Yield Critical Levels
                    }
                }



            }
        }
    }
}
