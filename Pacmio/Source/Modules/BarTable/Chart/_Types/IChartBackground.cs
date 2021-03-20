/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartBackground : IChartAnalysis
    {
        void DrawBackground(Graphics g, BarChart bc);
    }
}
