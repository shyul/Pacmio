/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartOverlay : IChartAnalysis
    {
        void DrawOverlay(Graphics g, BarChart bc);
    }
}
