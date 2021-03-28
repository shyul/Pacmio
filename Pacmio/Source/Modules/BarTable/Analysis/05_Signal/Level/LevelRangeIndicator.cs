/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// How To Avoid FALSE Breakouts
/// https://www.youtube.com/watch?v=WMRDuAdk7q4
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
    public class LevelRangeIndicator : BarAnalysis, ISingleComplex, IChartBackground
    {
        public LevelRangeIndicator(ILevelAnalysis pa, double tolerance = 0.01)
        {
            LevelAnalysis = pa;
            TolerancePercent = tolerance;
            Name = GetType().Name + "(" + pa.Name + ")";
            Column_Result = new(Name, typeof(LevelRangeDatum));
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ TolerancePercent.GetHashCode();

        public double TolerancePercent { get; }

        public ILevelAnalysis LevelAnalysis { get; set; }

        public DatumColumn Column_Result { get; }

        public NumericColumn Column_Strength { get; }

        public NumericColumn Column_Direction { get; }

        public string AreaName => LevelAnalysis.AreaName;

        public bool ChartEnabled { get; set; } = true;

        public int DrawOrder { get; set; } = 0;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b && b[Column_Result] is null && b[LevelAnalysis] is ILevelDatum ild)
                {
                    LevelRangeDatum lrd = new LevelRangeDatum(ild, TolerancePercent);
                    b[Column_Result] = lrd;
                }
            }
        }

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (bc.LastBar_1 is Bar b && AreaName is string areaName && bc[areaName] is Area a && b[Column_Result] is LevelRangeDatum lrd)
            {
                /*
                if (a.BarChart.LastBar_1 is Bar b && b[a] is RangeBoundDatum prd)
                {
                    var rangeList = prd.BoxList.OrderByDescending(n => n.Weight);

                    if (rangeList.Count() > 0)
                    {
                        double max_weight = rangeList.Select(n => n.Weight).Max();// .Last().Weight;

                        foreach (var pr in rangeList)
                        {
                            int y1 = a.AxisY(AlignType.Right).ValueToPixel(pr.Box.Max);
                            int y2 = a.AxisY(AlignType.Right).ValueToPixel(pr.Box.Min);
                            int height = y2 - y1;

                            double weight = pr.Weight;

                            int width = (weight * full_width / max_weight).ToInt32();

                            Rectangle rect = new Rectangle(x2 - width, y1, width, height);

                            g.FillRectangle(a.BarChart.Theme.FillBrush, rect);
                            g.DrawRectangle(new Pen(Color.Magenta), rect);

                        }
                    }
                }
                */

            }
        }
    }

    /*
    public class RangeBoundAnalysis : BarAnalysis
    {
        public RangeBoundAnalysis() 
            => Name = GetType().Name;

        public override int GetHashCode() => GetType().GetHashCode();

        public override void Update(BarAnalysisPointer bap)
        {
            if (!bap.IsUpToDate && bap.Count > 0)
            {
                bap.StopPt = bap.Count - 1;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;

                Calculate(bap);
                bap.StartPt = bap.StopPt;
                bap.StopPt++;
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b) b.CalculateRangeBoundDatums();
            }
        }
    }*/

    /*
    private Dictionary<string, RangeBoundDatum> RangeBoundDatums { get; } = new Dictionary<string, RangeBoundDatum>();

    public void CalculateRangeBoundDatums()
    {
        RangeBoundDatums.Clear();
        foreach (var p in Pivots)
        {
            string areaName = p.Source.AreaName;
            if (!RangeBoundDatums.ContainsKey(areaName))
                RangeBoundDatums[areaName] = new RangeBoundDatum();

            RangeBoundDatum prd = RangeBoundDatums[areaName];// = new PivotRangeDatum();
            prd.Insert(p);
        }
    }

    public RangeBoundDatum GetRangeBoundDatum() => GetRangeBoundDatum(MainBarChartArea.DefaultName);

    public RangeBoundDatum GetRangeBoundDatum(string areaName)
    {
        if (RangeBoundDatums.ContainsKey(areaName))
            return RangeBoundDatums[areaName];
        else
            return null;
    }

    public RangeBoundDatum this[IBarChartArea column]
    {
        get
        {
            if (RangeBoundDatums.ContainsKey(column.Name))
                return RangeBoundDatums[column.Name];
            else
                return null;
        }
    }

    public RangeBoundDatum this[IChartOverlay column]
    {
        get
        {
            if (RangeBoundDatums.ContainsKey(column.AreaName))
                return RangeBoundDatums[column.AreaName];
            else
                return null;
        }
    }
    */
}
