/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;
using Xu.Chart;

namespace Pacmio
{
    public interface IChartOverlay : IChartAnalysis
    {
        void DrawOverlay(Graphics g, BarChart bc);
    }
}
