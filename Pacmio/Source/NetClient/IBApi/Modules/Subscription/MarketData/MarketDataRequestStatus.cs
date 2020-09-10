/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Collections.Concurrent;
using Xu;
using System.Linq;

namespace Pacmio.IB
{
    public class MarketDataRequestStatus
    {
        public MarketDataRequestStatus(MarketData md) => MarketData = md;

        public MarketData MarketData { get; }

        public bool IsActive { get; set; } = false;

        public int TickerId { get; set; } = int.MinValue;

        public string BBOExchangeId { get; set; } = string.Empty;

        public bool IsSnapshot { get; set; } = false;

        public int SnapshotPermissions { get; set; } = int.MinValue;

        public string GenericTickList { get; set; } = string.Empty;
    }
}
