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

        public override void RefreshAxis(IArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);
            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= table.Count)
                    break;
                else if (i >= 0)
                {
                    axisY.Range.Insert(Table[i].BullishScore);
                    axisY.Range.Insert(-Table[i].BearishScore);
                }

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
                    
                    foreach (ISignalAnalysis ca in Table.SignalAnalyses)
                    {
                        if(Table[i][ca.Signal_Column] is ISignalDatum data) 
                        {
                            Rectangle rect;
                            int height;

                            if (data.BullishScore > 0)
                            {
                                height = ref_pix - axisY.ValueToPixel(data.BullishScore);
                                pos_base_pix -= height;
                                rect = new Rectangle(x, pos_base_pix, tickWidth, height);
                                g.FillRectangleE(ca.BullishTheme.FillBrush, rect);
                                g.DrawRectangle(ca.BullishTheme.EdgePen, rect);
                            }

                            if (data.BearishScore > 0)
                            {
                                height = axisY.ValueToPixel(-data.BearishScore) - ref_pix;
                                rect = new Rectangle(x, neg_base_pix, tickWidth, height);
                                neg_base_pix += height;
                                g.FillRectangleE(ca.BearishTheme.FillBrush, rect);
                                g.DrawRectangle(ca.BearishTheme.EdgePen, rect);
                            }
                        }
                    }
                }
                pt++;
            }
        }
    }
}
