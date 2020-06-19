/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Xu;
using Xu.GridView;


namespace Pacmio
{
    public class PositionGridView : GridWidget
    {
        public PositionGridView(string name, MarketDataTable mdt) : base(name)
        {
 

        }

        public override ITable Table => throw new NotImplementedException();

        public override IEnumerable<GridStripe> Stripes => throw new NotImplementedException();
    }
}
