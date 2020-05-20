/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public class PatternInfo
    {
        public IPatternAnalysis Analysis { get; set; }

        public string Name => Analysis.Name;

        public List<(DateTime time, double value, string label)> Points { get; } = new List<(DateTime time, double value, string label)>();

        public ColorTheme Theme { get; set; }
    }
}
