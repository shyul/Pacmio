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

            AddSeries(AccumulationSeries = new PositionSeries(BarChart)
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

        public PositionSeries AccumulationSeries { get; }

        public override void DrawCustomBackground(Graphics g)
        {
            //if (BarChart.Strategy is Strategy s)
            //DrawPosition(g, this, BarTable, s);
        }

        public static Dictionary<TradeExecutionType, Brush> ColorPalette { get; } = new Dictionary<TradeExecutionType, Brush>()
            {
                { TradeExecutionType.Long, new SolidBrush(Color.LimeGreen.Opaque(50)) },
                { TradeExecutionType.Sell, new SolidBrush(Color.Pink.Opaque(50)) },
                { TradeExecutionType.Short, new SolidBrush(Color.SkyBlue.Opaque(60)) },
                { TradeExecutionType.Cover, new SolidBrush(Color.Yellow.Opaque(50)) },
                { TradeExecutionType.LongHold, new SolidBrush(Color.LimeGreen.Opaque(25)) },
                { TradeExecutionType.ShortHold, new SolidBrush(Color.SkyBlue.Opaque(40)) },
            };

        public static void DrawPosition(Graphics g, OscillatorArea area, BarTable bt, IndicatorSet s)
        {/*
            if (s is Strategy && bt.Count > 0)
            {
                List<(Brush br, Rectangle rect)> rectangles = new();

                int ref_pix = area.AxisY(AlignType.Right).ValueToPixel(0);

                int pt = 0;
                TradeActionType last_Type = TradeActionType.None;
                int rect_x = 0, rect_width;

                for (int i = area.StartPt; i < area.StopPt; i++)
                {
                    if (i >= bt.Count) break;
                    else if (i < 0) continue;

                    TradeActionType this_Type = TradeActionType.None;


                    
                    if (bt[i][s] is BarPositionData bp)
                    {
                        this_Type = bp.ActionType;
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
                    }

                    

                    last_Type = this_Type;
                    pt++;
                }

                g.SetClip(area.Bounds);
                foreach (var (br, rect) in rectangles)
                    g.FillRectangle(br, rect);
                g.ResetClip();
            }*/
        }
    }
}
