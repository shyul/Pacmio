/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class DebugColumnSeriesOsc : DebugColumnSeries
    {
        public DebugColumnSeriesOsc(NumericColumn column) : base(column) { }

        public DebugColumnSeriesOsc(ISingleData isd) : base(isd) { }

        public override void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea area =
                    bc[AreaName] is BarChartOscillatorArea oa ?
                    oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area.AddSeries(ColumnSeries);
            }
        }
    }
}
