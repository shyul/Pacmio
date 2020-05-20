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
    public class DivergenceSignalDatum : ISignalDatum
    {
        /*
        public int Index { get; set; }

        public double Peak_N_Trough { get; set; } = 0;

        public bool HasExceededInidcatorLimit { get; set; } = false;

        public double Value { get; set; } = double.NaN;

        public double Indicator_Value { get; set; } = double.NaN;
        */

        public int Distance => Point2.index - Point1.index;

        public (int index, double peak, double value, double indicator_value) Point1 { get; set; } = (0, 0, 0, 0);

        public (int index, double peak, double value, double indicator_value) Point2 { get; set; } = (0, 0, 0, 0);

        public DivergenceType Type { get; set; } = DivergenceType.None;

        public double BullishScore { get; set; } = 0;

        public double BearishScore { get; set; } = 0;
    }
}
