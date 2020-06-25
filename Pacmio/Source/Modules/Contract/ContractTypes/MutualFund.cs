/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#cash
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Pacmio.IB;
using Xu;

namespace Pacmio
{
    /*
     
     Contract contract = new Contract();
            contract.Symbol = "VINIX";
            contract.SecType = "FUND";
            contract.Exchange = "FUNDSERV";
            contract.Currency = "USD";
     */
    [Serializable, DataContract(Name = "MutualFund")]
    public class MutualFund : Contract, ITradable
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "MFUND";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Mutual Fund";

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public override string TypeApiCode => "FUND";

        public string ISIN { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool AutoExchangeRoute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public bool Equals(BusinessInfo other)
        {
            throw new NotImplementedException();
        }

        public (bool valid, BusinessInfo bi) GetBusinessInfo()
        {
            throw new NotImplementedException();
        }
    }
}
