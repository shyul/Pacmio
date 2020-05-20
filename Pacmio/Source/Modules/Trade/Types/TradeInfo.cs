/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class TradeInfo : IEquatable<TradeInfo>, IEquatable<OrderInfo>, IEquatable<Contract>, IEquatable<(string name, Exchange exchange, string typeName)>
    {
        public TradeInfo(string execId) => ExecId = execId;

        [DataMember, Browsable(true)]
        public string ExecId { get; set; }

        /// <summary>
        /// Order id.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; } = -1;

        /// <summary>
        /// The Host order identifier.
        /// </summary>
        [DataMember]
        public int PermId { get; set; } = -1;

        [DataMember]
        public int ClientId { get; set; } = -1;

        [DataMember, Browsable(true)]
        public DateTime ExecuteTime { get; set; }

        [DataMember, Browsable(true)]
        public string Account { get; set; }

        [DataMember]
        public (string name, Exchange exchange, string typeName, int conId) ContractInfo { get; set; }

        [IgnoreDataMember]
        public Contract Contract
        {
            get
            {
                return m_Contract;
            }
            set
            {
                ContractInfo = (value.Name, value.Exchange, value.TypeName, value.ConId);
                m_Contract = value;
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        /// <summary>
        /// Positive means long position
        /// Negative means short position
        /// </summary>
        [DataMember, Browsable(true)]
        public double Quantity { get; set; }

        [DataMember, Browsable(true)]
        public double Price { get; set; }

        [DataMember, Browsable(true)]
        public double TotalQuantity { get; set; }

        [DataMember, Browsable(true)]
        public double AveragePrice { get; set; }

        [DataMember, Browsable(true)]
        public double RealizedPnL { get; set; }

        [DataMember, Browsable(true)]
        public double Commissions { get; set; }

        [DataMember, Browsable(true)]
        public string ModeCode { get; set; } = string.Empty;

        [DataMember, Browsable(true)]
        public int Liquidation { get; set; }

        [DataMember, Browsable(true)]
        public LiquidityType LastLiquidity { get; set; } = LiquidityType.None;

        [DataMember, Browsable(true)]
        public string FillExchangeCode { get; set; }

        [DataMember, Browsable(true)]
        public string Description { get; set; }

        [IgnoreDataMember]
        public TradeActionType Action
        {
            get
            {
                if (Quantity > 0 && LastLiquidity == LiquidityType.Added)
                    return TradeActionType.Long;
                else if (Quantity < 0 && LastLiquidity == LiquidityType.Added)
                    return TradeActionType.Short;
                else if (Quantity > 0 && LastLiquidity == LiquidityType.Removed)
                    return TradeActionType.Cover;
                else if (Quantity < 0 && LastLiquidity == LiquidityType.Removed)
                    return TradeActionType.Sell;

                return TradeActionType.None;
            }
        }

        public override bool Equals(object other)
        {
            if (other is TradeInfo tld)
                return Equals(tld);
            else if (other is Contract c)
                return Equals(c);
            else if (other is Tuple<string, Exchange, string> info)
                return Equals(info);
            //else if (other.GetType() == typeof((string, Exchange, ContractType)))
            //return Equals(((string, Exchange, ContractType))other);
            else
                return false;
        }

        public bool Equals(TradeInfo other) => ExecId == other.ExecId;

        public bool Equals(OrderInfo other) => PermId == other.PermId;

        public bool Equals(Contract other) => (ContractInfo.conId > 0 && ContractInfo.conId == other.ConId) || (ContractInfo.name, ContractInfo.exchange, ContractInfo.typeName) == other.Info;

        public bool Equals((string name, Exchange exchange, string typeName) other) => (ContractInfo.name, ContractInfo.exchange, ContractInfo.typeName) == other;

        public override int GetHashCode() => ExecId.GetHashCode();
        public static bool operator ==(TradeInfo left, TradeInfo right) => left.ExecId == right.ExecId;
        public static bool operator !=(TradeInfo left, TradeInfo right) => !(left == right);

        public static bool operator ==(TradeInfo left, OrderInfo right) => left.PermId == right.PermId;
        public static bool operator !=(TradeInfo left, OrderInfo right) => !(left == right);
    }
}
