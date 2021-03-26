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
    public class PatternLevelAnalysis : BarAnalysis
    {
        public PatternLevelAnalysis(PatternAnalysis pa, double tolerance = 0) 
        {
            PatternAnalysis = pa;
            Tolerance = tolerance;

            Column_Result = new(Name, typeof(PatternLevelDatum));
        }

        public double Tolerance { get; }

        public PatternAnalysis PatternAnalysis { get; set; }

        public DatumColumn Column_Result { get; }

        public NumericColumn Column_Strength { get; }

        public NumericColumn Column_Direction { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

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
