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
    [Serializable, DataContract(Name = "Forex")]
    public class Forex : Contract
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "FX";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Forex";

        public override void LoadMarketData()
        {
            throw new NotImplementedException();
        }

        public override void SaveMarketData()
        {
            throw new NotImplementedException();
        }


        public void Cancel_MarketDepth()
        {
            throw new NotImplementedException();
        }


        public bool Request_MarketDepth()
        {
            throw new NotImplementedException();
        }


    }
}
