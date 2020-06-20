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

        public override IEnumerable<GridStripe> Stripes => StripesSet;

        private readonly List<GridStripe> StripesSet = new List<GridStripe>() {

            new StringGridStripe() { Name = "Status", Column = MarketData.Column_Status, Importance = Importance.Major, Order = 0, Width = 70, AutoWidth = false },
            new ContractGridStripe() { Name = "Contract", Column = MarketData.Column_Contract, Importance = Importance.Huge, Order = 1, Width = 150, AutoWidth = true },
            new StringGridStripe() { Name = "Trade Time", Column = MarketData.Column_TradeTime, Importance = Importance.Major, Order = 2, Width = 120, AutoWidth = true },

            new StringGridStripe() { Name = "Bid Exch", Column = MarketData.Column_BidExchange, Importance = Importance.Minor, Order = 3, Width = 80, AutoWidth = true },
            new NumberGridStripe() { Name = "Bid Size", Column = MarketData.Column_BidSize, Importance = Importance.Major, Order = 4, Width = 70, AutoWidth = false },
            new NumberGridStripe() { Name = "Bid", Column = MarketData.Column_Bid, Importance = Importance.Major, Order = 5, Width = 60, AutoWidth = false },

            new NumberGridStripe() { Name = "Ask", Column = MarketData.Column_Ask, Importance = Importance.Major, Order = 6, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "Ask Size", Column = MarketData.Column_AskSize, Importance = Importance.Major, Order = 7, Width = 70, AutoWidth = false },
            new StringGridStripe() { Name = "Ask Exch", Column = MarketData.Column_AskExchange, Importance = Importance.Minor, Order = 8, Width = 80, AutoWidth = true },

            new NumberGridStripe() { Name = "Last", Column = MarketData.Column_Last, Importance = Importance.Huge, Order = 9, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "Last Size", Column = MarketData.Column_LastSize, Importance = Importance.Major, Order = 10, Width = 70, AutoWidth = false },
            new StringGridStripe() { Name = "Last Exch", Column = MarketData.Column_LastExchange, Importance = Importance.Minor, Order = 11, Width = 50, AutoWidth = true },

            new NumberGridStripe() { Name = "Open", Column = MarketData.Column_Open, Importance = Importance.Tiny, Order = 12, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "High", Column = MarketData.Column_High, Importance = Importance.Tiny, Order = 13, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "Low", Column = MarketData.Column_Low, Importance = Importance.Tiny, Order = 14, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "Close", Column = MarketData.Column_Close, Importance = Importance.Tiny, Order = 15, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "Volume", Column = MarketData.Column_Volume, Importance = Importance.Tiny, Order = 16, Width = 70, AutoWidth = false },

            new NumberGridStripe() { Name = "Short", Column = MarketData.Column_Short, Importance = Importance.Minor, Order = 17, Width = 60, AutoWidth = false },
            new NumberGridStripe() { Name = "S.Shares", Column = MarketData.Column_ShortShares, Importance = Importance.Minor, Order = 18, Width = 80, AutoWidth = false },

        };
    }
}
