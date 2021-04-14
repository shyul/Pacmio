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
        public string AccountId { get; set; } = null;

        [IgnoreDataMember]
        public AccountInfo Account
        {
            get
            {
                if (AccountId is string id)
                {
                    if (!(m_Account is AccountInfo ac && ac == id))
                    {
                        m_Account = AccountPositionManager.GetAccountById(AccountId);
                    }

                    return m_Account;
                }

                else
                    return null;
            }

            set
            {
                m_Account = value;

                if (m_Account is AccountInfo ac)
                    AccountId = ac.AccountId;
                else
                    AccountId = null;
            }
        }

        [IgnoreDataMember]
        private AccountInfo m_Account = null;

        [DataMember]
        public int ConId { get; set; } = -1;

        [IgnoreDataMember]
        public Contract Contract
        {
            get
            {
                if (ConId > 0)
                {
                    if (!(m_Contract is Contract c && c.ConId == ConId))
                    {
                        m_Contract = ContractManager.GetOrFetch(ConId);
                    }

                    return m_Contract;
                }

                else
                    return null;
            }

            set
            {
                m_Contract = value;

                if (m_Contract is Contract c)
                    ConId = c.ConId;
                else
                    ConId = -1;
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        [IgnoreDataMember]
        public PositionInfo Position => Contract is Contract c && Account is AccountInfo ac ? ac[c] : null;

        /// <summary>
        /// Positive means long position
        /// Negative means short position
        /// </summary>
        [DataMember, Browsable(true)]
        public double Quantity { get; set; } = double.NaN;

        [DataMember, Browsable(true)]
        public double Price { get; set; }

        [DataMember, Browsable(true)]
        public double TotalQuantity { get; set; }

        [DataMember, Browsable(true)]
        public double AveragePrice { get; set; }

        [DataMember, Browsable(true)]
        public double RealizedPnL { get; set; }

        [DataMember, Browsable(true)]
        public double Commissions { get; set; } = double.NaN;

        [IgnoreDataMember]
        public double AverageCommissionPerUnit => (!double.IsNaN(Commissions)) && (!double.IsNaN(Quantity)) && Quantity != 0 ? (Commissions / Quantity) : double.NaN;

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
        public ExecutionType Action
        {
            get
            {
                if (Quantity > 0 && LastLiquidity == LiquidityType.Added)
                    return ExecutionType.Long;
                else if (Quantity < 0 && LastLiquidity == LiquidityType.Added)
                    return ExecutionType.Short;
                else if (Quantity > 0 && LastLiquidity == LiquidityType.Removed)
                    return ExecutionType.Cover;
                else if (Quantity < 0 && LastLiquidity == LiquidityType.Removed)
                    return ExecutionType.Sell;

                return ExecutionType.None;
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

        public bool Equals((string name, Exchange exchange, string typeName) other) => Contract.Key == other;

        public static bool operator ==(TradeInfo left, TradeInfo right) => left.ExecId == right.ExecId;
        public static bool operator !=(TradeInfo left, TradeInfo right) => !(left == right);

        public static bool operator ==(TradeInfo left, OrderInfo right) => left.PermId == right.PermId;
        public static bool operator !=(TradeInfo left, OrderInfo right) => !(left == right);

        #endregion Equality
    }
}
