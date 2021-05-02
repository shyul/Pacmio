/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class SignalSeries : Series
    {
        public SignalSeries(ISignalSource isa)
        {
            Source = isa;

            Name = "Signal";
            LegendName = "SIGNAL: ";
            LegendLabelFormat = "0.##";
            Importance = Importance.Major;
            Order = int.MinValue + 1;
            Side = AlignType.Right;

            Width = 40;
        }


        public ISignalSource Source { get; }

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
                        var (bullish, bearish) = bt[i].GetSignalScore(Source.SignalList);
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

            if (table is BarTable bt && bt[pt] is Bar b)
            {
                var (bullish, bearish) = b.GetSignalScore(Source.SignalList);
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

                foreach (SignalAnalysis sa in Source.SignalList.Where(sa => sa.BarFreq >= BarFreq.Daily || sa.TimeInForce.Contains(b.Time)))
                {
                    if (b[sa] is SignalDatum sd)
                    {
                        if (sd.Points > 0)
                            labels.Add((sa.Name + ": " + sd.Points + " / " + sd.Description, Main.Theme.Font, sa.BullishTheme.ForeBrush));
                        else if (sd.Points < 0)
                            labels.Add((sa.Name + ": " + sd.Points + " / " + sd.Description, Main.Theme.Font, sa.BearishTheme.ForeBrush));
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
                    if (i >= table.Count)
                        break;
                    else if (i >= 0 && bt[i] is Bar b)
                    {
                        int x = area.IndexToPixel(pt) - (tickWidth / 2);
                        int pos_base_pix = ref_pix, neg_base_pix = ref_pix;

                        foreach (SignalAnalysis sa in Source.SignalList.Where(sa => sa.BarFreq >= BarFreq.Daily || sa.TimeInForce.Contains(b.Time)))
                        {
                            if (b[sa] is SignalDatum sd)
                            {
                                string desc = sd.Description;
                                double score = sd.Points;

                                Rectangle rect;
                                int height;

                                if (score > 0)
                                {
                                    height = ref_pix - axisY.ValueToPixel(score);
                                    pos_base_pix -= height;
                                    rect = new Rectangle(x, pos_base_pix, tickWidth, height);
                                    g.FillRectangleE(sa.BullishTheme.FillBrush, rect);
                                    g.DrawRectangle(sa.BullishTheme.EdgePen, rect);
                                }
                                else if (score < 0)
                                {
                                    height = axisY.ValueToPixel(score) - ref_pix;
                                    rect = new Rectangle(x, neg_base_pix, tickWidth, height);
                                    neg_base_pix += height;
                                    g.FillRectangleE(sa.BearishTheme.FillBrush, rect);
                                    g.DrawRectangle(sa.BearishTheme.EdgePen, rect);
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
