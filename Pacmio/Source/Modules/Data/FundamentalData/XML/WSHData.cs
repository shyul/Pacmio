// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// https://jsonformatter.org/xml-viewer
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Reuters.Calendar
{
    [XmlRoot(ElementName = "Earnings")]
    public class Earnings
    {
        [XmlElement(ElementName = "Q1")]
        public string Q1 { get; set; }

        [XmlElement(ElementName = "Q2")]
        public string Q2 { get; set; }

        [XmlElement(ElementName = "Q3")]
        public string Q3 { get; set; }

        [XmlElement(ElementName = "TimeZone")]
        public string TimeZone { get; set; }

        [XmlElement(ElementName = "Q4")]
        public string Q4 { get; set; }

        [XmlElement(ElementName = "Period")]
        public string Period { get; set; }

        [XmlElement(ElementName = "Time")]
        public string Time { get; set; }

        [XmlElement(ElementName = "Etype")]
        public string Etype { get; set; }

        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "TimeStamp")]
        public string TimeStamp { get; set; }
    }

    [XmlRoot(ElementName = "EarningsList")]
    public class EarningsList
    {
        [XmlElement(ElementName = "Earnings")]
        public Earnings Earnings { get; set; }
    }

    [XmlRoot(ElementName = "EarningsCall")]
    public class EarningsCall
    {
        [XmlElement(ElementName = "TimeZone")]
        public string TimeZone { get; set; }

        [XmlElement(ElementName = "GotoBroadcast")]
        public string GotoBroadcast { get; set; }

        [XmlElement(ElementName = "Time")]
        public string Time { get; set; }

        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "TimeStamp")]
        public string TimeStamp { get; set; }
    }

    [XmlRoot(ElementName = "EarningsCallList")]
    public class EarningsCallList
    {
        [XmlElement(ElementName = "EarningsCall")]
        public List<EarningsCall> EarningsCall { get; set; }
    }

    [XmlRoot(ElementName = "Company")]
    public class Company
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Ticker")]
        public string Ticker { get; set; }

        [XmlElement(ElementName = "ISIN")]
        public string ISIN { get; set; }

        [XmlElement(ElementName = "Exchange")]
        public string Exchange { get; set; }

        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "conid")]
        public List<string> Conid { get; set; }

        [XmlElement(ElementName = "EarningsList")]
        public EarningsList EarningsList { get; set; }

        [XmlElement(ElementName = "EarningsCallList")]
        public EarningsCallList EarningsCallList { get; set; }
    }

    [XmlRoot(ElementName = "WSHData")]
    public class WSHData
    {
        [XmlElement(ElementName = "Company")]
        public Company Company { get; set; }
    }
}
