/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartPattern
    {
        string Name { get; }

        //CalculatePivotRange CalculatePivotRange { get; }

        string AreaName { get; }

        int TestInterval { get; }

        bool ChartEnabled { get; set; }

        void DrawOverlay(Graphics g, BarChart bc);

        void DrawBackground(Graphics g, BarChart bc);
    }
}
