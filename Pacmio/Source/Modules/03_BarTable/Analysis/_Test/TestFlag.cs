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
    public static class TestFlag
    {
        public static BarAnalysisList BarAnalysisSet
        {
            get
            {
                //DebugSeries csd_tr = new DebugColumnSeries(Bar.Column_TrueRange);

                var FlagAnalysis = new FlagAnalysis();

                List<BarAnalysis> sample_list = new()
                {

                    //csd_tr,

                    FlagAnalysis
                };

                BarAnalysisList bat = new(sample_list);

                return bat;
            }
        }
    }
}
