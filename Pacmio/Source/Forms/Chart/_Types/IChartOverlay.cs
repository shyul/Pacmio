/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartOverlay
    {
        string Name { get; }

        string AreaName { get; }

        bool ChartEnabled { get; set; }

        void DrawOverlay(Graphics g, BarChart bc);

        void DrawBackground(Graphics g, BarChart bc);
    }
}
