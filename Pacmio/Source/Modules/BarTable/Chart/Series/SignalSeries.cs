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
    public sealed class SignalSeries : Series
    {
        public SignalSeries(Indicator ind)
        {
            Indicator = ind;

            Name = "Signal";
            LegendName = "SIGNAL: ";
            LegendLabelFormat = "0.##";
            Importance = Importance.Major;
            Order = int.MinValue + 1;
            Side = AlignType.Right;

            Width = 40;
        }

        public Indicator Indicator { get; }

        public override void RefreshAxis(IIndexArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);

            if (table is BarTable bt)
            {
                for (int i = area.StartPt; i < area.StopPt; i++)
                {
                    if (i >= table.Count)
                        break;
                    else if (i > 0)
                    {
                        var (bullish, bearish) = bt[i].GetSignalScore(Indicator);
                        axisY.Range.Insert(bullish);
                        axisY.Range.Insert(bearish);
                    }
                }
            }

            axisY.Range.Insert(0);
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new();

            if (table is BarTable bt)
            {
                var (bullish, bearish) = bt[pt].GetSignalScore(Indicator);
                double score = bullish + bearish;

                if (score > 0)
                {
                    labels.Add((score.ToString(), Main.Theme.FontBold, Root.Upper_TextTheme.ForeBrush));
                }
                else if (score < 0)
                {
                    labels.Add((score.ToString(), Main.Theme.FontBold, Root.Lower_TextTheme.ForeBrush));
                }
                else
                {
                    labels.Add((score.ToString(), Main.Theme.FontBold, Main.Theme.DimTextBrush));
                }

                foreach (SignalColumn sc in Indicator.SignalColumns)
                {
                    if (bt[pt][sc] is SignalDatum sd)
                    {
                        if (sd.Score > 0)
                            labels.Add((sc.Name + ": " + sd.Score + " / " + sd.Description, Main.Theme.Font, sc.BullishTheme.ForeBrush));
                        else if (sd.Score < 0)
                            labels.Add((sc.Name + ": " + sd.Score + " / " + sd.Description, Main.Theme.Font, sc.BearishTheme.ForeBrush));
                    }
                }
            }
            return labels;
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            if (table is BarTable bt)
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

                        foreach (SignalColumn sc in Indicator.SignalColumns)
                        {
                            if (bt[i][sc] is SignalDatum sd)
                            {
                                string desc = sd.Description;
                                double score = sd.Score;

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
                                    height = axisY.ValueToPixel(score) - ref_pix;
                                    rect = new Rectangle(x, neg_base_pix, tickWidth, height);
                                    neg_base_pix += height;
                                    g.FillRectangleE(sc.BearishTheme.FillBrush, rect);
                                    g.DrawRectangle(sc.BearishTheme.EdgePen, rect);
                                }
                            }
                        }
                    }
                    pt++;
                }
            }
        }
    }
}
