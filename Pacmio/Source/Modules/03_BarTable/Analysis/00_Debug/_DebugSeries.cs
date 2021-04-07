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
    public abstract class DebugSeries : BarAnalysis, IChartSeries
    {
        public virtual Color Color { get => MainSeries.Color; set => MainSeries.Color = MainSeries.EdgeColor = value; }

        public bool ChartEnabled
        {
            get => Enabled && MainSeries.Enabled;
            set => MainSeries.Enabled = value;
        }

        public int DrawOrder
        {
            get => MainSeries.Order;
            set => MainSeries.Order = value;
        }
        
        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; }

        public virtual Series MainSeries { get; }

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea area =
                    bc[AreaName] is BarChartArea a ?
                    a :
                    bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                    { HasXAxisBar = false });

                area.AddSeries(MainSeries);
            }
        }

        protected override void Calculate(BarAnalysisPointer bap) { }
    }
}
