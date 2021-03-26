/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class SignalArea : OscillatorArea, IBarChartArea
    {
        public SignalArea(BarChart chart, SignalSeries sgs) : base(chart, "Signal", 10)
        {
            BarChart = chart;
            Importance = Importance.Major;
            //Order = int.MinValue + 1;

            UpperLimit = 0;
            LowerLimit = 0;
            UpperColor = Color.GreenYellow;
            LowerColor = Color.Pink;
            Reference = 0;
            //FixedTickStep_Right = 2;

            SignalSeries = sgs;
            //AddSeries(SignalSeries = new SignalSeries(chart));
        }

        public BarChart BarChart { get; }

        public BarTable BarTable => BarChart.BarTable;

        public SignalSeries SignalSeries { get; }

        public Indicator Indicator => SignalSeries.Indicator;

        public override void DrawCustomBackground(Graphics g)
        {
            //if (BarChart.Strategy is Strategy s)
            //PositionArea.DrawPosition(g, this, BarTable, s);
            if (Indicator is IndicatorExec idex)
            {





            }
            else if(Indicator is IndicatorFilter filter) 
            {
            
            
            
            }

        }
    }
}
