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
    public sealed class SignalSeries : Series
    {
        public SignalSeries(BarChart chart)
        {
            BarChart = chart;
            LegendName = "Signal";
            Width = 40;
        }

        public BarChart BarChart { get; }

        public BarTable Table => BarChart.BarTable;

        public IEnumerable<SignalColumn> SignalColumns => BarChart.TradeRule.Analyses[BarChart.BarFreq].SignalColumns;

        public override void RefreshAxis(IArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);

            if (BarChart.HasSignalColumn)
            {
                for (int i = area.StartPt; i < area.StopPt; i++)
                {
                    var (bullish, bearish) = Table.TotalSignalScore(i, SignalColumns);
                    axisY.Range.Insert(bullish);
                    axisY.Range.Insert(bearish);
                }
            }
            else
            {
                axisY.Range.Insert(1);
                axisY.Range.Insert(-1);
            }

            axisY.Range.Insert(0);
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new List<(string text, Font font, Brush brush)>();



            return labels;
        }

        public override void Draw(Graphics g, IArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);
            int pt = 0;
            int ref_pix = axisY.ValueToPixel(0);
            int tickWidth = area.AxisX.TickWidth;
            if (tickWidth > 6) tickWidth = (tickWidth * 0.8f).ToInt32();
            if (tickWidth > Width) tickWidth = Width.ToInt32();

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count) break;
                if (i >= 0)
                {
                    int x = area.IndexToPixel(pt) - (tickWidth / 2);
                    int pos_base_pix = ref_pix, neg_base_pix = ref_pix;

                    foreach (SignalColumn sc in SignalColumns)
                    {
                        var (desc, score) = Table[i, sc];

                        Rectangle rect;
                        int height;

                        if (score > 0)
                        {
                            height = ref_pix - axisY.ValueToPixel(score);
                            pos_base_pix -= height;
                            rect = new Rectangle(x, pos_base_pix, tickWidth, height);
                            g.FillRectangleE(sc.BullishTheme.FillBrush, rect);
                            g.DrawRectangle(sc.BullishTheme.EdgePen, rect);
                        }
                        else if (score < 0)
                        {
                            height = axisY.ValueToPixel(-score) - ref_pix;
                            rect = new Rectangle(x, neg_base_pix, tickWidth, height);
                            neg_base_pix += height;
                            g.FillRectangleE(sc.BearishTheme.FillBrush, rect);
                            g.DrawRectangle(sc.BearishTheme.EdgePen, rect);
                        }
                    }
                }
                pt++;
            }
        }
    }
}
