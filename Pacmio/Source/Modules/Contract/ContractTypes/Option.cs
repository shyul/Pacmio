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
using Pacmio.IB;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "Option")]
    public class Option : Contract, ITradable, IOption
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "OPTION";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Option";

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public override string TypeApiCode => "OPT";

        public string ISIN { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool AutoExchangeRoute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Right { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double Strike { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Multiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LastTradeDateOrContractMonth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Equals(BusinessInfo other)
        {
            throw new NotImplementedException();
        }

        public (bool valid, BusinessInfo bi) GetBusinessInfo()
        {
            throw new NotImplementedException();
        }

        public override bool RequestQuote(string param)
        {
            throw new NotImplementedException();
        }

        public override void CancelQuote()
        {
            throw new NotImplementedException();
        }
    }
}
