/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public interface IBidAsk
    {
        double Ask { get; set; }

        double AskSize { get; set; }

        string AskExchange { get; set; }

        double Bid { get; set; }

        double BidSize { get; set; }

        string BidExchange { get; set; }

        double Open { get; set; }

        double High { get; set; }

        double Low { get; set; }

        double Last { get; set; }

        double LastSize { get; set; }

        double Volume { get; set; }

        string LastExchange { get; set; }

        double LastClose { get; set; }
    }
}
