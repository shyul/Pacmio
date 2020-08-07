/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartGraphics
    {



        string AreaName { get; }

        void DrawOverlay(Graphics g, BarChart bc);

        void DrawBackground(Graphics g, BarChart bc);
    }


}
