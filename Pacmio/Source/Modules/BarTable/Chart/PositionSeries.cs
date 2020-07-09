/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class PositionSeries : Series
    {
        public PositionSeries(BarChart chart)
        {
            BarChart = chart;
            LegendName = "Position";
            Width = 40;
        }

        public BarChart BarChart { get; }

        public BarTable Table => BarChart.BarTable;

        public TradeRule TradeRule => BarChart.TradeRule;

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new List<(string text, Font font, Brush brush)>();



            return labels;
        }

        public override void Draw(Graphics g, IArea area, ITable table)
        {



        }
    }
}
