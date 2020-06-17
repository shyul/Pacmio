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
    public sealed class PositionArea : OscillatorArea
    {
        public PositionArea(BarChart chart) : base(chart, "Position Accumulation", 10)
        {

            BarChart = chart;
            Importance = Importance.Major;
            Order = int.MinValue;

            UpperLimit = 0;
            LowerLimit = 0;
            UpperColor = Color.GreenYellow;
            LowerColor = Color.Pink;
            Reference = 0;
            //Visible = true;
            //FixedTickStep_Right = 2;

            AddSeries(AccumulationSeries = new LineSeries(Bar.Column_ProfitChange, LineType.Step)
            {
                Color = Color.DimGray,
                Order = int.MinValue,
                Side = AlignType.Right,
                Name = "P Change",
                LegendName = "PCHG",
                LegendLabelFormat = "0.##"
            });
        }

        public BarChart BarChart { get; }

        public BarTable BarTable => BarChart.BarTable;

        public readonly LineSeries AccumulationSeries;

        public override void DrawCustomBackground(Graphics g)
        {
            DrawPosition(g, this, BarTable);
        }

        public static Dictionary<TradeActionType, Brush> ColorPalette { get; } = new Dictionary<TradeActionType, Brush>()
            {
                { TradeActionType.Long, new SolidBrush(Color.LimeGreen.Opaque(50)) },
                { TradeActionType.Sell, new SolidBrush(Color.Pink.Opaque(50)) },
                { TradeActionType.Short, new SolidBrush(Color.SkyBlue.Opaque(60)) },
                { TradeActionType.Cover, new SolidBrush(Color.Yellow.Opaque(50)) },
                { TradeActionType.LongHold, new SolidBrush(Color.LimeGreen.Opaque(25)) },
                { TradeActionType.ShortHold, new SolidBrush(Color.SkyBlue.Opaque(40)) },
            };

        public static void DrawPosition(Graphics g, OscillatorArea area, BarTable bt)
        {
            if (bt.Count > 0)
            {
                List<(Brush br, Rectangle rect)> rectangles = new List<(Brush, Rectangle)>();

                int ref_pix = area.AxisY(AlignType.Right).ValueToPixel(0);

                int pt = 0;
                TradeActionType last_Type = TradeActionType.None;
                int rect_x = 0, rect_width;

                for (int i = area.StartPt; i < area.StopPt; i++)
                {
                    if (i >= bt.Count) break;
                    else if (i < 0) continue;

                    TradeActionType this_Type = bt[i].ActionType;

                    if (i >= 0)
                    {
                        if (this_Type == last_Type && this_Type != TradeActionType.None)
                        {
                            if (i == area.StopPt - 1)
                            {
                                rect_width = area.IndexToPixel(pt) - rect_x + (area.AxisX.TickWidth / 2);
                                if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, area.Top, rect_width, ref_pix - area.Top)));
                                else
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, ref_pix, rect_width, area.Bottom - ref_pix)));
                            }
                        }
                        else if (this_Type != last_Type)
                        {
                            if (last_Type != TradeActionType.None)
                            {
                                rect_width = area.IndexToPixel(pt) - rect_x - (area.AxisX.TickWidth / 2);

                                if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, area.Top, rect_width, ref_pix - area.Top)));
                                else
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, ref_pix, rect_width, area.Bottom - ref_pix)));
                            }

                            if (this_Type != TradeActionType.None)
                            {
                                rect_x = area.IndexToPixel(pt) - (area.AxisX.TickWidth / 2);
                                rect_width = area.AxisX.TickWidth;
                                if (i == area.StopPt - 1)
                                {
                                    if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                        rectangles.Add((ColorPalette[this_Type], new Rectangle(rect_x, area.Top, rect_width, ref_pix - area.Top)));
                                    else
                                        rectangles.Add((ColorPalette[this_Type], new Rectangle(rect_x, ref_pix, rect_width, area.Bottom - ref_pix)));
                                }
                            }
                        }
                    }

                    last_Type = this_Type;
                    pt++;
                }

                g.SetClip(area.Bounds);
                foreach (var (br, rect) in rectangles)
                    g.FillRectangle(br, rect);
                g.ResetClip();
            }
        }
    }
}
