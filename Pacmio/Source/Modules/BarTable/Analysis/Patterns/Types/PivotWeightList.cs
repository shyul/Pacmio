/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public class PivotWeightList
    {
        // Creat a new one, must have AreaName assigned!!!

        public string AreaName { get; }

        // Get HashCode Here!!

        // And Equalization to IPattern and IArea by Name / AreaName

        // Shall we just merge IPattern and IChartGraphics ????

        public Dictionary<Range<double>, double> Weight { get; } = new Dictionary<Range<double>, double>();
    }
}
