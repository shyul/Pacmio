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
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using Xu;
using Pacmio.IB;

namespace Pacmio
{
    /*
     
    [Serializable, DataContract]
    public enum ContractTypeObsolete : int
    {
        [EnumMember]
        UNKNOWN = 0,

        [EnumMember, ApiCode("CMDTY")]
        [ContractTypeInfo("MEX", "Commodity")]
        COMMODITY = 400,

        [EnumMember, ApiCode("BOND")]
        [ContractTypeInfo("BOND", "Bond")]
        BOND = 500,

        [EnumMember, ApiCode("FOP")]
        [ContractTypeInfo("FOP", "Future Option")]
        FUTURE_OPTION = 800,

        [EnumMember, ApiCode("CFD")]
        [ContractTypeInfo("CFD", "Contract For Difference")]
        CFD = 900,

        [EnumMember, ApiCode("WAR")]
        [ContractTypeInfo("WAR", "Warrant")]
        WAR = 1000,

        [EnumMember, ApiCode("IOPT")]
        [ContractTypeInfo("IOPT", "Structured / Warrants")]
        IOPT = 1100
    }
         
         */

    public class BagComboContract : Contract, ITradable, IOption
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "BAG";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Bag Combo";

        public override string TypeApiCode => "BAG";

        public override bool RequestQuote(string genericTickList) => false;

        public override void CancelQuote() { }

        /// <summary>
        /// ############################## Can be simplified
        /// Identifier of the security type
        /// </summary>
        public string ISIN { get; set; }

        /// <summary>
        /// Local Symbol of Option
        /// </summary>
        public string LocalSymbol { get; set; } = string.Empty;

        public (bool valid, BusinessInfo bi) GetBusinessInfo() => BusinessInfoList.GetOrAdd(this);

        /// <summary>
        /// Security's identifier when querying contract's details or placing orders
        /// ISIN - Example: Apple: US0378331005
        /// CUSIP - Example: Apple: 037833100
        /// </summary>
        public string SecIdType => string.IsNullOrEmpty(ISIN) ? string.Empty : "ISIN";

        /// <summary>
        /// Combo contract
        /// </summary>
        public bool IsBAG { get; set; } = false;

        /// <summary>
        /// Use SMART exchange
        /// </summary>
        public bool AutoExchangeRoute { get; set; } = true;

        /// <summary>
        /// The contract's last trading day or contract month (for Options and Futures). 
        /// Strings with format YYYYMM will be interpreted as the Contract Month 
        /// whereas YYYYMMDD will be interpreted as Last Trading Day.
        /// </summary>
        public string LastTradeDateOrContractMonth { get; set; } = string.Empty;

        /// <summary>
        /// The option's strike price
        /// </summary>
        public double Strike { get; set; } = 0;

        /// <summary>
        /// Either Put or Call (i.e. Options). Valid values are P, PUT, C, CALL.
        /// </summary>
        public string Right { get; set; } = string.Empty;

        /// <summary>
        /// The instrument's multiplier (i.e. options, futures).
        /// </summary>
        public string Multiplier { get; set; } = string.Empty;

        /// <summary>
        /// The trading class name for this contract
        /// Available in TWS contract description window as well. For example, GBL Dec '13 future's trading class is "FGBL"
        /// </summary>
        public string TradingClass { get; set; } = string.Empty;

        /// <summary>
        /// If set to true, contract details requests and historical data queries can be performed pertaining to expired futures contracts.
        /// Expired options or other instrument types are not available.
        /// </summary>
        public bool IncludeExpired { get; set; } = false;

        /// <summary>
        /// Description of the combo legs.
        /// </summary>
        public string ComboLegsDescription { get; set; } = string.Empty;

        /// <summary>
        /// The legs of a combined contract definition
        /// </summary>
        public ICollection<ComboLeg> ComboLegs { get; set; }

        /// <summary>
        /// Delta and underlying price for Delta-Neutral combo orders.
        /// Underlying (STK or FUT), delta and underlying price goes into this attribute.
        /// </summary>
        public DeltaNeutralContract DeltaNeutralContract { get; set; }

        /// <summary>
        /// This contract is valid
        /// </summary>
        public bool IsValid => (ConId > 0) && (!string.IsNullOrEmpty(Name));

        /*
        public string[] GetParameters12()
        {
            if (!IsValid)
            {
                Console.WriteLine("Invalid Symbol: " + Name + " / ConId: " + ConId);
                return null;
            }

            (bool validExchange, string exchangeCode) = ApiCode.GetIbCode(Exchange);

            if (!validExchange)
            {
                Console.WriteLine("Invalid Exchange:" + Exchange);
                return null;
            }

            (bool validType, string secTypeCode) = ApiCode.GetIbCode(Type);

            if (!validType)
            {
                Console.WriteLine("Invalid SecurityType: " + Type);
                return null;
            }

            if (IsBAG) secTypeCode = "BAG";

            return new string[]
            {
                ConId.Param(),
                Name,
                secTypeCode,
                LastTradeDateOrContractMonth,
                (Strike == 0) ? "0" : Strike.ToString("0.0###"),
                Right,
                Multiplier,
                AutoExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                exchangeCode, // primaryExch,
                CurrencyCode, // USD / currency,
                LocalSymbol,
                TradingClass
            };
        }

        public string[] GetParameters13()
        {
            if (!IsValid)
            {
                Console.WriteLine("Invalid Symbol: " + Name + " / ConId: " + ConId);
                return null;
            }

            (bool validExchange, string exchangeCode) = ApiCode.GetIbCode(Exchange);

            if (!validExchange)
            {
                Console.WriteLine("Invalid Exchange:" + Exchange);
                return null;
            }

            (bool validType, string secTypeCode) = ApiCode.GetIbCode(Type);

            if (!validType)
            {
                Console.WriteLine("Invalid SecurityType: " + Type);
                return null;
            }

            if (IsBAG) secTypeCode = "BAG";

            return new string[]
            {
                ConId.Param(),
                Name,
                secTypeCode,
                LastTradeDateOrContractMonth,
                (Strike == 0) ? "0" : Strike.ToString("0.0###"),
                Right,
                Multiplier,
                AutoExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                exchangeCode, // primaryExch,
                CurrencyCode, // USD / currency,
                LocalSymbol,
                TradingClass,
                IncludeExpired.Param()
            };
        }
        */

        public bool Equals(BusinessInfo other) => ISIN == other.ISIN;
    }

}
