/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio.IB
{
    [Serializable, DataContract]
    public class ExchangeDescription
    {
        /*
        public ExchangeDescription(string exchangeCode, string secTypeCode, string listingExch, string serviceDataType, int aggGroup)
        {
            ExchangeCode = exchangeCode;
            TypeCode = secTypeCode;
            ListingExch = listingExch;
            ServiceDataType = serviceDataType;
            AggGroup = aggGroup;
        }*/

        //[DataMember]
        //public Exchange Exchange { get; set; }

        [DataMember]
        public string ExchangeCode { get; set; }

        [DataMember]
        public string TypeCode { get; set; }

        [DataMember]
        public string ListingExch { get; set; }

        [DataMember]
        public string ServiceDataType { get; set; }

        [DataMember]
        public int AggGroup { get; set; }
    }
}
