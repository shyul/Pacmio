/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public static class TestNative
    {
        public static BarAnalysisSet BarAnalysisSet
        {
            get
            {
                Relative relative_body = new Relative(Bar.Column_Body, 20);
                Relative relative_volume = new Relative(Bar.Column_Volume, 20);

                DebugSeries csd_relative_body = new DebugColumnSeries(relative_body);
                DebugSeries csd_relative_volume = new DebugColumnSeries(relative_volume);
                DebugSeries csd_consecutive_type = new DebugColumnSeries(Bar.Column_ConsecutiveType);

                DebugSeries csd_range = new DebugColumnSeries(Bar.Column_Range);
                DebugSeries csd_nr = new DebugColumnSeries(Bar.Column_NarrowRange);
                DebugSeries csd_gp = new DebugColumnSeriesOsc(Bar.Column_GapPercent);
                DebugSeries csd_typ = new DebugLineSeries(Bar.Column_Typical);
                DebugSeries csd_pivot = new DebugColumnSeriesOsc(Bar.Column_Pivot);
                DebugSeries csd_pivotstr = new DebugColumnSeriesOsc(Bar.Column_PivotStrength);

                DebugSeries csd_trend = new DebugColumnSeriesOsc(Bar.Column_TrendStrength);

                var stdev = new STDEV(Bar.Column_Typical, 20);
                DebugSeries csd_stdev = new DebugColumnSeries(stdev.Column_Percent);
                stdev.AddChild(csd_stdev);

                TimeFramePricePosition potf = new(BarFreq.Annually);
                DebugSeries csd_potf = new DebugLineSeries(potf);

                List<BarAnalysis> sample_list = new()
                {
                    new NativeApexAnalysis(),

                    new CandleStickDojiMarubozuSignal(),
                    new CandleStickShadowStarSignal(),
                    //csd_potf,
                    //csd_range,
                    //csd_nr,
                    //csd_gp,
                    //csd_typ,
                    csd_relative_body,
                    csd_relative_volume,
                    csd_consecutive_type,
                    csd_trend,
                    //csd_pivot,
                    //csd_pivotstr,
                    //csd_trend,

                    //csd_stdev, 
                    //new CHOP(20),

                    //new CrossIndicator(),
                    //new CHOP(),
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }

        public static BarAnalysisSet BarAnalysisSetTimeFrame
        {
            get
            {
                TimeFramePricePosition potf = new(BarFreq.Daily);
                DebugSeries csd_potf = new DebugColumnSeries(potf);

                TimeFrameCumulativeVolume tfcv = new TimeFrameCumulativeVolume(BarFreq.Daily);
                DebugSeries csd_tfcv = new DebugColumnSeries(tfcv);

                TimeFrameRelativeVolume tfrv = new TimeFrameRelativeVolume(20, BarFreq.Daily);
                DebugSeries csd_tfrv = new DebugColumnSeries(tfrv);
                DebugSeries csd_tfrv_ema = new DebugColumnSeries(tfrv.Column_EMA);

                List<BarAnalysis> sample_list = new()
                {
                    //csd_potf,
                    csd_tfcv,
                    csd_tfrv_ema,
                    csd_tfrv,
                   
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }
    }
}
