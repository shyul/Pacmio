﻿// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// https://jsonformatter.org/xml-viewer
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Reuters.FinancialSummary
{
    #region Financial Summary

    [XmlRoot(ElementName = "DividendPerShare")]
    public class DividendPerShare
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public DateTime AsofDate { get; set; }

        [XmlAttribute(AttributeName = "reportType")]
        public string ReportType { get; set; }

        [XmlAttribute(AttributeName = "period")]
        public string Period { get; set; }

        [XmlText]
        public double Value { get; set; }
    }

    [XmlRoot(ElementName = "DividendPerShares")]
    public class DividendPerShares
    {
        [XmlElement(ElementName = "DividendPerShare")]
        public List<DividendPerShare> DividendPerShare { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "TotalRevenue")]
    public class TotalRevenue
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlAttribute(AttributeName = "reportType")]
        public string ReportType { get; set; }

        [XmlAttribute(AttributeName = "period")]
        public string Period { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "TotalRevenues")]
    public class TotalRevenues
    {
        [XmlElement(ElementName = "TotalRevenue")]
        public List<TotalRevenue> TotalRevenue { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "Dividend")]
    public class Dividend
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "exDate")]
        public string ExDate { get; set; }

        [XmlAttribute(AttributeName = "recordDate")]
        public string RecordDate { get; set; }

        [XmlAttribute(AttributeName = "payDate")]
        public string PayDate { get; set; }

        [XmlAttribute(AttributeName = "declarationDate")]
        public string DeclarationDate { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Dividends")]
    public class Dividends
    {
        [XmlElement(ElementName = "Dividend")]
        public List<Dividend> Dividend { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "EPS")]
    public class EPS
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlAttribute(AttributeName = "reportType")]
        public string ReportType { get; set; }

        [XmlAttribute(AttributeName = "period")]
        public string Period { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "EPSs")]
    public class EPSs
    {
        [XmlElement(ElementName = "EPS")]
        public List<EPS> EPS { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "FinancialSummary")]
    public class FinancialSummary
    {
        [XmlElement(ElementName = "DividendPerShares")]
        public DividendPerShares DividendPerShares { get; set; }

        [XmlElement(ElementName = "TotalRevenues")]
        public TotalRevenues TotalRevenues { get; set; }

        [XmlElement(ElementName = "Dividends")]
        public Dividends Dividends { get; set; }

        [XmlElement(ElementName = "EPSs")]
        public EPSs EPSs { get; set; }
    }

    #endregion Financial Summary
}
