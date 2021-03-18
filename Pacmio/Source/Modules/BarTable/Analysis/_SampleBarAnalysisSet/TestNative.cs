﻿/// ***************************************************************************
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
                DebugSeries csd_range = new DebugColumnSeries(Bar.Column_Range);
                DebugSeries csd_nr = new DebugColumnSeries(Bar.Column_NarrowRange);
                DebugSeries csd_gp = new DebugColumnSeriesOsc(Bar.Column_GapPercent);
                DebugSeries csd_typ = new DebugLineSeries(Bar.Column_Typical);
                DebugSeries csd_pivot = new DebugColumnSeriesOsc(Bar.Column_Pivot);
                DebugSeries csd_trend = new DebugColumnSeriesOsc(Bar.Column_TrendStrength);

                PositionOfTimeframe potf = new PositionOfTimeframe(BarFreq.Annually);
                DebugSeries csd_potf= new DebugLineSeries(potf);

                List<BarAnalysis> sample_list = new List<BarAnalysis>
                {
                    new NativePivotAnalysis(),
                    //new CandleStickDojiMarubozuAnalysis(),
                    //new CandleStickShadowStarAnalysis(),
                    csd_potf,
                    //csd_range,
                    //csd_nr,
                    //csd_gp,
                    csd_typ,
                    csd_pivot,
                    csd_trend,
                };

                BarAnalysisSet bas = new BarAnalysisSet(sample_list);

                return bas;
            }
        }
    }
}
