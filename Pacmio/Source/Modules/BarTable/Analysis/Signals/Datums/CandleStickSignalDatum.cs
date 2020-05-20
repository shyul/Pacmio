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
    public class CandleStickSignalDatum : ISignalDatum
    {
        public string Name { get; }

        /// <summary>
        /// The diff of Open and Close
        /// </summary>
        public double CandleLength { get; set; }

        /// <summary>
        /// The diff of High and Low
        /// </summary>
        public double CandleShadowLength { get; set; } = 0;

        public double BullishScore { get; set; } = 0;

        public double BearishScore { get; set; } = 0;
    }
}
