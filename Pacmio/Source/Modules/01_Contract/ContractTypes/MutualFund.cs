﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#cash
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

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
    public class MutualFund : Contract, IBusiness
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "MFUND";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Mutual Fund";

        public string ISIN { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BusinessInfo BusinessInfo => throw new NotImplementedException();

        public string Industry { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Category { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Subcategory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Equals(BusinessInfo other)
        {
            throw new NotImplementedException();
        }

    }
}
