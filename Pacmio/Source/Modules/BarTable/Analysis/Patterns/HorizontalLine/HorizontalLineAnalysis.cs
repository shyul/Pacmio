/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class HorizontalLineAnalysis : PatternAnalysis, IChartBackground
    {
        public HorizontalLineAnalysis(int maximumInterval)
        {
            string label = "(" + maximumInterval + ")";
            Name = GetType().Name + label;

            TrailingPivotPointAnalysis = new TrailingPivotPtAnalysis(maximumInterval);
            TrailingPivotPointAnalysis.AddChild(this);

            Column_Result = new PatternColumn(this, typeof(HorizontalLineDatum), maximumInterval);
            AreaName = MainBarChartArea.DefaultName;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        #region Calculation

        public TrailingPivotPtAnalysis TrailingPivotPointAnalysis { get; }

        public override int MaximumInterval => TrailingPivotPointAnalysis.MaximumInterval;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b && b[Column_Result] is null)
                {
                    double range_delta = (b.PivotRange.Max - b.PivotRange.Min) / 2;
                    double center = b.Close;

                    HorizontalLineDatum hld = new HorizontalLineDatum();
                    hld.TotalRange = new(center - range_delta, center + range_delta);
                    hld.AddLine(b.PivotPts.Select(n => n.Value));
                    b[Column_Result] = hld;
                }
            }
        }

        #endregion Calculation

        #region Chart Graphics

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && bc.LastBar_1 is Bar b && AreaName is string areaName && bc[areaName] is Area a && b[Column_Result] is HorizontalLineDatum hld)
            {
                int StartPt = a.StartPt;
                int StopPt = a.StopPt;

                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                double maxStrength = hld.StrengthRange.Max;
                foreach (HorizontalLine line in hld)
                {
                    int x1 = line.X1 - StartPt;
                    if (x1 >= 0)
                    {
                        int x3 = StopPt - StartPt - 1;
                        double y1 = line.Y1;
                        int py1 = a.AxisY(AlignType.Right).ValueToPixel(y1);

                        Point pt1 = new Point(a.IndexToPixel(x1), py1);
                        Point pt3 = new Point(a.IndexToPixel(x3), py1);

                        int intensity = (255 * line.Strength / maxStrength).ToInt32();

                        if (intensity > 255) intensity = 255;
                        else if (intensity < 1) intensity = 1;

                        g.DrawLine(new Pen(Color.FromArgb(intensity, 80, 80, 180)), pt1, pt3);
                    }
                }

                g.SmoothingMode = SmoothingMode.Default;
                g.ResetClip();
            }
        }

        #endregion Chart Graphics
    }
}


//var all_points = b.PivotPts.Select(n => n.Value);

/*
tld.TrendLines.CheckAdd(new TrendLine())

for (int j = 0; j < all_points.Length; j++)
{
var pt1 = all_points[j];
var pt1_strength = pt1.Value.Strength;

if (pt1_strength > 8 && pd.LevelRange.Contains(pt1.Value.Level))
{

// consider the distant to date as a factor for fading
HorizontalLine level = new(this, pt1.Value, tolerance)
{
Strength = pt1_strength * 2
};
pd.Add(level);
}

for (int k = j + 1; k < all_points.Length; k++)
{
var pt2 = all_points[k];
var pt2_strength = pt2.Value.Strength;
TrendLine line = new(this, pt1.Value, pt2.Value, i + 1, tolerance);

if (pd.LevelRange.Contains(line.Level))
{
// consider the distant to date as a factor for fading
line.Strength = 1 * line.DeltaX * (pt1_strength + pt2_strength);
pd.Add(line);
}
}
}
b[Column_Result] = pd;
*/