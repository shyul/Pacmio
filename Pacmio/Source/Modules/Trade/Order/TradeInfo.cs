/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

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
        public string AccountCode { get; set; }

        [IgnoreDataMember]
        public Account Account => AccountManager.Get(AccountCode);

        [DataMember]
        public int ConId
        {
            get
            {
                if (Contract is Contract c)
                    return c.ConId;
                else
                    return m_ConId;
            }

            set
            {
                if (value != m_ConId || Contract is null || Contract.ConId != value)
                {
                    Contract = ContractList.GetOrFetch(m_ConId);
                    Console.WriteLine("Added Contract to TradeInfo: " + Contract);
                    m_ConId = value;
                }
            }
        }

        private int m_ConId = -1;

        [IgnoreDataMember]
        public Contract Contract { get; set; }

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

        #region Equality

        public override int GetHashCode() => ExecId.GetHashCode();

        public override bool Equals(object other)
        {
            if (other is TradeInfo tld)
                return Equals(tld);
            else if (other is OrderInfo od)
                return Equals(od);
            else if (other is Contract c)
                return Equals(c);
            else if (other is Tuple<string, Exchange, string> info)
                return Equals(info);
            else
                return false;
        }

        public bool Equals(TradeInfo other) => ExecId == other.ExecId;

        public bool Equals(OrderInfo other) => PermId == other.PermId;

        public bool Equals(Contract other) => (ConId > 0 && ConId == other.ConId) || Contract == other;

        public bool Equals((string name, Exchange exchange, string typeName) other) => Contract.Info == other;

        public static bool operator ==(TradeInfo left, TradeInfo right) => left.ExecId == right.ExecId;
        public static bool operator !=(TradeInfo left, TradeInfo right) => !(left == right);

        public static bool operator ==(TradeInfo left, OrderInfo right) => left.PermId == right.PermId;
        public static bool operator !=(TradeInfo left, OrderInfo right) => !(left == right);

        #endregion Equality
    }
}
