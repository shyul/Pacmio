/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class SignalSeries : Series
    {
        public SignalSeries(BarChart chart)
        {
            BarChart = chart;

            if (BarChart.Strategy is Strategy s && s.PositionAnalysis is PositionAnalysis pas)
                PositionAnalysis = pas;

            Name = "Signal";
            LegendName = "SIGNAL: ";
            LegendLabelFormat = "0.##";
            Importance = Importance.Major;
            Order = int.MinValue + 1;
            Side = AlignType.Right;

            Width = 40;
        }

        public BarChart BarChart { get; }

        public BarTable Table => BarChart.BarTable;

        public BarAnalysisSet BarAnalysisSet => BarChart.BarAnalysisSet;

        public PositionAnalysis PositionAnalysis { get; }

        //public IEnumerable<SignalColumn> SignalColumns => AnalysisSetting.Analyses[BarChart.BarFreq].SignalColumns;

        public override void RefreshAxis(IArea area, ITable table)
        {
            ContinuousAxis axisY = area.AxisY(Side);

            // TODO: If PositionAnalysis is available, please use PositionAnalysis SignalColumn CASH -> LONG -> SHORT
            if (BarChart.HasSignalColumn)
            {
                for (int i = area.StartPt; i < area.StopPt; i++)
                {
                    if (i >= table.Count)
                        break;
                    else if (i > 0)
                    {
                        var (bullish, bearish) = Table[i].SignalScore(BarAnalysisSet);
                        axisY.Range.Insert(bullish);
                        axisY.Range.Insert(bearish);
                    }
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

            var (bullish, bearish) = Table[pt].SignalScore(BarAnalysisSet);
            double score = bullish + bearish;

            if (score > 0)
            {
                labels.Add((score.ToString(), Main.Theme.FontBold, BarTable.Upper_TextTheme.ForeBrush));
            }
            else if (score < 0)
            {
                labels.Add((score.ToString(), Main.Theme.FontBold, BarTable.Lower_TextTheme.ForeBrush));
            }
            else
            {
                labels.Add((score.ToString(), Main.Theme.FontBold, BarChart.Theme.ForeBrush));
            }

            foreach (SignalColumn sc in BarChart.BarAnalysisSet.SignalColumns)
            {
                SignalDatum sd = Table[pt][sc];

                if (sd.Score > 0)
                    labels.Add((sc.Name + ": " + sd.Score + " / " + sd.Description, Main.Theme.Font, sc.BullishTheme.ForeBrush));
                else if (sd.Score < 0)
                    labels.Add((sc.Name + ": " + sd.Score + " / " + sd.Description, Main.Theme.Font, sc.BearishTheme.ForeBrush));
            }

            return labels;
        }

        public override void Draw(Graphics g, IArea area, ITable table)
        {
            if (BarChart.HasSignalColumn)
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

                        foreach (SignalColumn sc in BarChart.BarAnalysisSet.SignalColumns)
                        {
                            SignalDatum sd = Table[i][sc];

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
                    pt++;
                }
            }
        }
    }
}
