/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    [KnownType(typeof(SimulationAccount))]
    [KnownType(typeof(InteractiveBrokersAccount))]
    public abstract class Account : IEquatable<Account>, IEquatable<string>
    {
        public virtual void Reset()
        {
            Positions = new ConcurrentDictionary<Contract, PositionStatus>();
        }

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Account Code")]
        [Description("The account ID number.")]
        public virtual string AccountCode { get; protected set; }

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Account Type")]
        [Description("Identifies the IB account structure.")]
        public virtual string AccountType { get; protected set; }

        [DataMember, Browsable(true), ReadOnly(true)]
        [Description("Number of Open/Close trades one could do before Pattern Day Trading is detected")]
        public virtual List<int> DayTradesRemaining { get; protected set; } = new List<int> { 0, 0, 0, 0, 0 };

        public override string ToString() => AccountCode;

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Buying Power")]
        [Description("Standard Margin Account: Available Funds x4")]
        public virtual double BuyingPower { get; protected set; } = 100000;

        [IgnoreDataMember]
        public virtual double TotalValue => BuyingPower + PositionValue;

        #region Positions

        [IgnoreDataMember]
        public virtual double PositionValue => (Positions is null && Positions.Count < 1) ? 0 : Positions.Values.Select(n => n.Value).Sum();

        [IgnoreDataMember]
        public ConcurrentDictionary<Contract, PositionStatus> Positions { get; set; }

        public PositionStatus GetPosition(Contract c)
        {
            if (!Positions.ContainsKey(c))
                Positions.TryAdd(c, new PositionStatus());

            return Positions[c];
        }

        public virtual void CloseAllPositions()
        {
            foreach (var item in Positions)
            {
                Exit(item.Key);
            }
        }

        #endregion Positions

        #region Order

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="time">Decision Bar's Time / Last Bar's time</param>
        /// <param name="quantity"></param>
        /// <param name="stop"></param>
        /// <param name="limit"></param>
        public abstract void Entry(Contract c, double quantity);//, double limit = double.NaN, double stop = double.NaN);

        //public abstract void Entry(Contract c, double quantity, DateTime expirationTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="time">Decision Bar's Time / Last Bar's time</param>
        /// <param name="stopLoss"></param>
        /// <param name="limitProfit"></param>
        public virtual void Modify(Contract c, double stop, double limit = double.NaN)
        {
            PositionStatus ps = GetPosition(c);
            ps.Stop = stop;
            ps.Limit = limit;
        }

        /// <summary>
        /// // This is how it closes the position
        /// Exit(c, b.Time, -io.GetPosition(c).Quantity);
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="time"></param>
        public abstract void Exit(Contract c);

        #endregion Order

        #region Equality

        public bool Equals(Account other) => AccountCode == other.AccountCode;
        public bool Equals(string other) => AccountCode == other;

        public static bool operator ==(Account s1, Account s2) => s1.Equals(s2);
        public static bool operator !=(Account s1, Account s2) => !s1.Equals(s2);
        public static bool operator ==(Account s1, string s2) => s1.Equals(s2);
        public static bool operator !=(Account s1, string s2) => !s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is Account ac)
                return Equals(ac);
            else if (other is string s)
                return Equals(s);
            else
                return false;
        }

        public override int GetHashCode() => AccountCode.GetHashCode();

        #endregion Equality

        #region Grid View

        public static readonly StringColumn Column_Account = new StringColumn("ACCOUNT");

        #endregion Grid View
    }
}
