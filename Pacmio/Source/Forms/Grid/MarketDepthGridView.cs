/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class MarketDepthGridView : GridWidget<MarketDepthDatum>
    {
        public MarketDepthGridView(Contract c) : base(c.ToString() + " | Market Depth")
        {
            Contract = c;

        }


        public Contract Contract { get; }

        public void UpdateGrid()
        {
            if (Contract is Stock s)
            {
                DataIsUpdated();// Update(s.StockData.MarketDepth());
            }
        }

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);
    }
}
