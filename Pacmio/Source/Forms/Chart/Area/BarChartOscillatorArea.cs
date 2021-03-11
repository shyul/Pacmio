/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class BarChartOscillatorArea : OscillatorArea, IBarChartArea
    {
        public BarChartOscillatorArea(BarChart chart, string name, float height, float leftAxisRatio = 1) : base(chart, name, height)
        {
            AxisLeft.HeightRatio = leftAxisRatio;
            BarChart = chart;
        }

        public BarChart BarChart { get; private set; }

        public BarTable BarTable => BarChart.BarTable;

        public override void DrawCustomBackground(Graphics g) => BarChartArea.DrawCustomBackground(g, this);

        public override void DrawCustomOverlay(Graphics g) => BarChartArea.DrawCustomOverlay(g, this);
    }
}
