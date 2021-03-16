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
        public PositionInfoGridView() : base("Positions | All")
        {
            AccountPositionManager.PositionDataProvider.AddDataConsumer(this);
            DataIsUpdated(null);
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
            //Console.WriteLine("PositionInfoGridView | Rows.Count() = " + Rows.Count());

    


            double unrealizedPnL = SourceRows.Select(n => n.UnrealizedPnL).Sum();

            if (Math.Abs(unrealizedPnL - UnrealizedPnL) > 0.005)
            {
                Console.WriteLine("Total PnL = " + unrealizedPnL.ToString("0.###"));
            }

            UnrealizedPnL = unrealizedPnL;
        }

        public double UnrealizedPnL { get; set; }

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);
    }
}
