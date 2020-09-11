/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using Pacmio.Analysis;
using System;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio.IB
{
    [AttributeUsage(AttributeTargets.Field), Serializable, DataContract]
    public sealed class ApiCode : Attribute
    {
        public ApiCode(string code) => Code = code;

        public string Code { get; private set; }
    }
}
