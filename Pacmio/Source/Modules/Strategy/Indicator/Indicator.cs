/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class Indicator
    {
        public string Name { get; }

        public int Order { get; set; }

        public List<BarAnalysis> BarAnalysesTimeFrame1 { get; }

        // List the scope of usable MAs, and testable range of intervals...

        // Different Time Frame

        public List<BarAnalysis> BarAnalysesTimeFrame2 { get; }


        // Choices of SMA / EMA / Different Oscillators and so on...


        // Will also be the one getting the Elevation Factors

        // Here is how many days for generating new TradeParameter
        public int BackTestingLength { get; } = 5;

        // Will re-generate the IndicatorParameter after 2 days.
        public int ValidTradingLength { get; } = 2;

        // So we use the past five days of trading history to generate the next 2 days trading parameter
    }
}
