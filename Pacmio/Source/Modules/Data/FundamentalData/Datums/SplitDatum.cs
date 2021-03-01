/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public class SplitDatum
    {
        public SplitDatum(DateTime asOfDate) 
        {
            AsOfDate = asOfDate;
        }

        [DataMember]
        public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

        [DataMember]
        public DateTime AsOfDate { get; private set; }

        [DataMember]
        public double Close_Price { get; set; } = double.NaN;

        [DataMember]
        public double Split { get; set; } = 1;
    }
}
