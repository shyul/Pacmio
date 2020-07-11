/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pacmio
{
    [AttributeUsage(AttributeTargets.Class), Serializable, DataContract]
    public sealed class ContractTypeInfo : Attribute
    {
        public ContractTypeInfo(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string FullName { get; private set; }
        /*
        private static readonly Dictionary<string, Type> ApiToSecType = new Dictionary<string, Type>()
        {
            { "STK",    typeof(Stock) },
            { "FUND",   typeof(MutualFund) },
            { "CASH",   typeof(Forex) },
            { "IND",    typeof(Index) },
            //{ "CMDTY",  ContractType.COMMODITY },
            //{ "BOND",   ContractType.BOND },
            { "FUT",    typeof(Future) },
            { "OPT",    typeof(Option) },
            //{ "FOP",    ContractType.FUTURE_OPTION },
            //{ "CFD",    ContractType.CFD },
            //{ "WAR",    ContractType.WAR },
            //{ "IOPT",   ContractType.IOPT },
        };*/

        public static HashSet<string> UnknownSecurityTypeCode { get; private set; } = new HashSet<string>();
        /*
        public static (bool, ContractType) GetEnum(string value)
        {
            if (!ApiToSecType.ContainsKey(value))
            {
                lock (UnknownSecurityTypeCode)
                    if (!UnknownSecurityTypeCode.Contains(value))
                        UnknownSecurityTypeCode.Add(value);

                return (false, ContractType.UNKNOWN);
            }
            else
            {
                return (true, ApiToSecType[value]);
            }
        }*/
    }
}
