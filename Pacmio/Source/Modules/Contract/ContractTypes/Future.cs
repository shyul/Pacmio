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
    [Serializable, DataContract(Name = "Future")]
    public class Future : Contract, ITradable
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "FUTURE";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Future";

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public override string TypeApiCode => "FUT";

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

        public override bool RequestQuote(string genericTickList)
        {
            throw new NotImplementedException();
        }

        public override void StopQuote()
        {
            throw new NotImplementedException();
        }
    }
}

