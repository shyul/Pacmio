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


                        Bar b = bt[i];

                        FlagDatum fd = new FlagDatum(type);
                        b[Column_Result] = fd;

                        var run_up_highs = run_bars.Select(n => n.High);
                        var run_up_lows = run_bars.Select(n => n.Low);

                        fd.RunUpRange = new(run_up_lows.Min(), run_up_highs.Max());

                        var pull_back_highs = pull_back_bars.Select(n => n.High);
                        var pull_back_lows = pull_back_bars.Select(n => n.Low);

                        fd.PullBackRange = new(pull_back_lows.Min(), pull_back_highs.Max());
                        fd.TotalRange = new(Math.Min(fd.RunUpRange.Min, fd.PullBackRange.Min), Math.Max(fd.RunUpRange.Max, fd.PullBackRange.Max));

                        fd.PullBackRatio = (fd.TotalRange.Max - fd.PullBackRange.Min) / (fd.TotalRange.Max - fd.TotalRange.Min);

                        if (fd.PullBackRatio >= MinPullBackRatio && fd.PullBackRatio <= MaxPullBackRatio)
                        {
                            // Get the trend line of the pull back
                            fd.BreakOutLevel = fd.TotalRange.Max;
                            Console.WriteLine(bars.Last().Time + " | " + type + " is detected!!");
                            //Console.WriteLine(bars.Last().Time + " | " + bars.Last().BarType + " | " + type + " | pull_back_bars = " + pull_back_bars.Count + " | pull_back_bars = " + run_bars.Count);

                        }

                        // Yield Pattern
                        // Yield Critical Levels
                    }
                }



            }
        }
    }
}
