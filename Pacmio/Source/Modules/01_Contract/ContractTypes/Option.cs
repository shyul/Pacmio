/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#opt
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract(Name = "Option")]
    public class Option : Contract, IBusiness, IOption
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "OPTION";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Option";

        public string ISIN { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public string Right { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double Strike { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Multiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LastTradeDateOrContractMonth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
