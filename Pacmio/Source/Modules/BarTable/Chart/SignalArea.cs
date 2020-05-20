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
    public sealed class SignalArea : OscillatorArea
    {
        public SignalArea(BarChart chart, string name = "Signal", int height = 10) : base(chart, name, height)
        {
            BarChart = chart;
            Importance = Importance.Major;
            UpperLimit = 0;
            LowerLimit = 0;
            UpperColor = Color.GreenYellow;
            LowerColor = Color.Pink;
            Reference = 0;
            FixedTickStep_Right = 2;

            AddSeries(AccumulationSeries = new LineSeries(TableList.Column_ProfitChange, LineType.Step)
            {
                Color = Color.DimGray,
                Order = int.MinValue,
                Side = AlignType.Left,
                Name = "P Change",
                LegendName = "PCHG",
                LegendLabelFormat = "0.##"
            });

            AddSeries(SignalSeries = new SignalSeries(chart)
            {
                Order = int.MinValue + 1,
                Side = AlignType.Right,
                Name = "Signal",
                LegendName = "SIGNAL",
                LegendLabelFormat = "0.##"
            });
        }

        public BarChart BarChart { get; }

        public BarTable BarTable => BarChart.BarTable;

        public readonly SignalSeries SignalSeries;

        public readonly LineSeries AccumulationSeries;

        public static Dictionary<TradeActionType, Brush> ColorPalette { get; } = new Dictionary<TradeActionType, Brush>()
            {
                { TradeActionType.Long, new SolidBrush(Color.LimeGreen.Opaque(50)) },
                { TradeActionType.Sell, new SolidBrush(Color.Pink.Opaque(50)) },
                { TradeActionType.Short, new SolidBrush(Color.SkyBlue.Opaque(60)) },
                { TradeActionType.Cover, new SolidBrush(Color.Yellow.Opaque(50)) },
                { TradeActionType.LongHold, new SolidBrush(Color.LimeGreen.Opaque(25)) },
                { TradeActionType.ShortHold, new SolidBrush(Color.SkyBlue.Opaque(40)) },
            };

        public override void DrawCustomBackground(Graphics g)
        {
            if (BarTable.Count > 0)
            {
                List<(Brush br, Rectangle rect)> rectangles = new List<(Brush, Rectangle)>();

                int ref_pix = AxisY(AlignType.Right).ValueToPixel(0);

                int pt = 0;
                TradeActionType last_Type = TradeActionType.None;
                int rect_x = 0, rect_width;

                for (int i = StartPt; i < StopPt; i++)
                {
                    if (i >= BarTable.Count || i < 0) break;
                    TradeActionType this_Type = BarTable[i].ActionType;

                    if (i >= 0)
                    {
                        if (this_Type == last_Type && this_Type != TradeActionType.None)
                        {
                            if (i == StopPt - 1)
                            {
                                rect_width = IndexToPixel(pt) - rect_x + (AxisX.TickWidth / 2);
                                if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, Top, rect_width, ref_pix - Top)));
                                else
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, ref_pix, rect_width, Bottom - ref_pix)));
                            }
                        }
                        else if (this_Type != last_Type)
                        {
                            if (last_Type != TradeActionType.None)
                            {
                                rect_width = IndexToPixel(pt) - rect_x - (AxisX.TickWidth / 2);

                                if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, Top, rect_width, ref_pix - Top)));
                                else
                                    rectangles.Add((ColorPalette[last_Type], new Rectangle(rect_x, ref_pix, rect_width, Bottom - ref_pix)));
                            }

                            if (this_Type != TradeActionType.None)
                            {
                                rect_x = IndexToPixel(pt) - (AxisX.TickWidth / 2);
                                rect_width = AxisX.TickWidth;
                                if (i == StopPt - 1)
                                {
                                    if (last_Type == TradeActionType.Long || last_Type == TradeActionType.Sell || last_Type == TradeActionType.LongHold)
                                        rectangles.Add((ColorPalette[this_Type], new Rectangle(rect_x, Top, rect_width, ref_pix - Top)));
                                    else
                                        rectangles.Add((ColorPalette[this_Type], new Rectangle(rect_x, ref_pix, rect_width, Bottom - ref_pix)));
                                }
                            }
                        }
                    }

                    last_Type = this_Type;
                    pt++;
                }

                g.SetClip(Bounds);
                foreach (var (br, rect) in rectangles)
                    g.FillRectangle(br, rect);
                g.ResetClip();
            }
        }
    }

}
