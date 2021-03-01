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
    public class DividendDatum
    {
        public DividendDatum(DateTime asOfDate)
        {
            AsOfDate = asOfDate;
        }

        [DataMember]
        public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

        [DataMember]
        public DateTime DeclareDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Once the company sets the record date, the ex-dividend date is set based on stock exchange rules.
        /// The ex-dividend date for stocks is usually set one business day before the record date.
        /// If you purchase a stock on its ex-dividend date or after, you will not receive the next dividend payment.
        /// Instead, the seller gets the dividend. If you purchase before the ex-dividend date, you get the dividend.
        /// </summary>
        [DataMember]
        public DateTime ExDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// When a company declares a dividend, it sets a record date when you must be on the company's books 
        /// as a shareholder to receive the dividend. Companies also use this date to determine who is sent 
        /// proxy statements, financial reports, and other information.
        /// </summary>
        [DataMember]
        public DateTime RecordDate { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime PayDate { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime AsOfDate { get => PayDate; private set => PayDate = value; }

        [DataMember]
        public double Close_Price { get; set; } = double.NaN;

        [DataMember]
        public double Dividend { get; set; } = double.NaN;

        [IgnoreDataMember]
        public double Ratio => Close_Price > 0 ? Dividend / Close_Price : 0;
    }
}
