/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class FundamentalDatum
    {
        public (string name, Exchange exchange, string typeName) ContractInfo { get; set; }

        //public Contract Contract => ContractManager
    }
}
