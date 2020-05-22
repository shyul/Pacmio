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
    public class ConstantSignalDatum : ISignalDatum
    {
        public ConstantDataType Type { get; set; } = ConstantDataType.None;

        public double BullishScore { get; set; } = 0;

        public double BearishScore { get; set; } = 0;
    }
}
