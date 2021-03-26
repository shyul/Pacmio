/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
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

            if (BarTable is BarTable bt && bt.Count > 0)
            {
                List<(Brush br, Rectangle rect)> rectangles = new();

                if (Indicator is IndicatorExec idex)
                {





                }
                else if (Indicator is IndicatorFilter filter)
                {
                    FilterType lastType = FilterType.None;

                    for (int i = StartPt; i < StopPt; i++)
                    {
                        Bar b = bt[i];
                        FilterType thisType = b[filter];
                        int tickWidth = AxisX.TickWidth;
                        int half_tickWidth = (tickWidth / 2.0f).ToInt32();
                        if (thisType == FilterType.Bullish)
                        {
                            //g.FillRectangle(fillBrush, IndexToPixel(i) - half_tickWidth, upper_pix, tickWidth, height);

                        }
                        else if (thisType == FilterType.Bearish)
                        {


                        }

                        lastType = thisType;
                    }
                }

                g.SetClip(Bounds);
                foreach (var (br, rect) in rectangles)
                    g.FillRectangle(br, rect);
                g.ResetClip();
            }
        }
    }
}
