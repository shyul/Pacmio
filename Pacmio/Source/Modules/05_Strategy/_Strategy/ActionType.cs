/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public enum ActionType : int
    {
        [EnumMember]
        None = 0,

        // Add Liquidation, Enter

        [EnumMember]
        Long = 3,

        [EnumMember]
        Short = -3,

        // Hold Liquidation

        [EnumMember]
        LongHold = 1,

        [EnumMember]
        ShortHold = -1,

        // Remove Liquidation, Exit

        [EnumMember]
        Sell = -2,

        [EnumMember]
        Cover = 2,
    }


    public interface IExecution 
    { 
        ExecutionType Type { get; }

        //double ExecutionPrice { get; }
    }

    public interface IPriceExecution : IExecution
    {
        double ProfitTakePrice { get; }

        double StopLossPrice { get; }
    }



    public enum ExecutionType : int
    {
        None = 0,

        BuyLimit = 1,

        BuyStop = 2,

        SellStop = -1,

        SellLimit = -2,
    }



    public class EntryExecution : IPriceExecution
    {
        public ExecutionType Type { get; set; }

        public double ExecutionPrice { get; set; }

        public double ProfitTakePrice { get; set; }

        public double StopLossPrice { get; set; }

        public double Risk => ExecutionPrice - StopLossPrice;
    }

    public class ScaleExecution : EntryExecution
    {
        /// <summary>
        /// Positive means adding scale amount of position
        /// Negative means removing scale amount of position
        /// This is a ratio data: 1 means initial entry of the maximum riskable position, 2 means add double
        /// 0.5 (Remove Liq) means remove half, 1 (Remove) means empty the position.
        /// The actual "quantity" will be calculated with R/R and WinRate of the backtesting result.
        /// </summary>
        public double Scale { get; set; }
    }

    public class ExitExecution : IExecution
    {
        public ExecutionType Type { get; set; } = ExecutionType.None;
    }






}
