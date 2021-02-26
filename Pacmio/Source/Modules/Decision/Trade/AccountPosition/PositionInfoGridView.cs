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
    public class PositionInfoGridView : GridWidget<PositionInfo>
    {
        public PositionInfoGridView() : base("Positions")
        {
            AccountPositionManager.PositionDataProvider.AddDataConsumer(this);
        }

        ~PositionInfoGridView()
        {
            AccountPositionManager.PositionDataProvider.RemoveDataConsumer(this);
            Dispose();
        }

        public override void DataIsUpdated(IDataProvider provider)
        {
            SourceRows = AccountPositionManager.Positions;
            base.DataIsUpdated(provider);
        }
    }
}
