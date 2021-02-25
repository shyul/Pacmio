/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class MarketDepthGridView : GridWidget<OrderInfo>
    {
        public MarketDepthGridView(MarketDepth mdt) : base("Market Depth | " + mdt.Contract.Name)
        {
            MarketDepth = mdt;
        }

        public MarketDepth MarketDepth { get; }

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);
    }
}
