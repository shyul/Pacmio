// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// https://jsonformatter.org/xml-viewer
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Xml.Serialization;

namespace IbXmlAE
{
    #region AnalystEstimates

    [XmlRoot(ElementName = "Name")]
    public class Name
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CoName")]
    public class CoName
    {
        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }
    }

    [XmlRoot(ElementName = "CoId")]
    public class CoId
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CoIds")]
    public class CoIds
    {
        [XmlElement(ElementName = "CoId")]
        public List<CoId> CoId { get; set; }
    }

    [XmlRoot(ElementName = "Exchange")]
    public class Exchange
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Country")]
    public class Country
    {
        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SecId")]
    public class SecId
    {
        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SecIds")]
    public class SecIds
    {
        [XmlElement(ElementName = "SecId")]
        public List<SecId> SecId { get; set; }
    }

    [XmlRoot(ElementName = "MarketDataItem")]
    public class MarketDataItem
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "currCode")]
        public string CurrCode { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlAttribute(AttributeName = "updated")]
        public string Updated { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MarketData")]
    public class MarketData
    {
        [XmlElement(ElementName = "MarketDataItem")]
        public List<MarketDataItem> MarketDataItem { get; set; }
    }

    [XmlRoot(ElementName = "CDSIds")]
    public class CDSIds
    {
        [XmlElement(ElementName = "SecId")]
        public List<SecId> SecId { get; set; }
    }

    [XmlRoot(ElementName = "Security")]
    public class Security
    {
        [XmlElement(ElementName = "Exchange")]
        public Exchange Exchange { get; set; }

        [XmlElement(ElementName = "Country")]
        public Country Country { get; set; }

        [XmlElement(ElementName = "SecIds")]
        public SecIds SecIds { get; set; }

        [XmlElement(ElementName = "MarketData")]
        public MarketData MarketData { get; set; }

        [XmlElement(ElementName = "CDSIds")]
        public CDSIds CDSIds { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "SecurityInfo")]
    public class SecurityInfo
    {
        [XmlElement(ElementName = "Security")]
        public Security Security { get; set; }
    }

    [XmlRoot(ElementName = "Sector")]
    public class Sector
    {
        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Industry")]
    public class Industry
    {
        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Primary")]
    public class Primary
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Currency")]
    public class Currency
    {
        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CurFiscalPeriod")]
    public class CurFiscalPeriod
    {
        [XmlAttribute(AttributeName = "fYear")]
        public string FYear { get; set; }

        [XmlAttribute(AttributeName = "fyem")]
        public string Fyem { get; set; }

        [XmlAttribute(AttributeName = "periodType")]
        public string PeriodType { get; set; }

        [XmlAttribute(AttributeName = "periodNum")]
        public string PeriodNum { get; set; }
    }

    [XmlRoot(ElementName = "Interim")]
    public class Interim
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "periodNum")]
        public string PeriodNum { get; set; }

        [XmlAttribute(AttributeName = "endMonth")]
        public string EndMonth { get; set; }

        [XmlAttribute(AttributeName = "endCalYear")]
        public string EndCalYear { get; set; }

        [XmlAttribute(AttributeName = "periodUnit")]
        public string PeriodUnit { get; set; }

        [XmlAttribute(AttributeName = "periodLength")]
        public string PeriodLength { get; set; }

        [XmlAttribute(AttributeName = "expectDate")]
        public string ExpectDate { get; set; }

        [XmlAttribute(AttributeName = "dateStatus")]
        public string DateStatus { get; set; }
    }

    [XmlRoot(ElementName = "Annual")]
    public class Annual
    {
        [XmlElement(ElementName = "Interim")]
        public List<Interim> Interim { get; set; }

        [XmlAttribute(AttributeName = "fyNum")]
        public string FyNum { get; set; }

        [XmlAttribute(AttributeName = "fYear")]
        public string FYear { get; set; }

        [XmlAttribute(AttributeName = "endMonth")]
        public string EndMonth { get; set; }

        [XmlAttribute(AttributeName = "periodUnit")]
        public string PeriodUnit { get; set; }

        [XmlAttribute(AttributeName = "periodLength")]
        public string PeriodLength { get; set; }
    }

    [XmlRoot(ElementName = "CompanyPeriods")]
    public class CompanyPeriods
    {
        [XmlElement(ElementName = "Annual")]
        public List<Annual> Annual { get; set; }
    }

    [XmlRoot(ElementName = "CompanyInfo")]
    public class CompanyInfo
    {
        [XmlElement(ElementName = "Sector")]
        public List<Sector> Sector { get; set; }

        [XmlElement(ElementName = "Industry")]
        public List<Industry> Industry { get; set; }

        [XmlElement(ElementName = "Primary")]
        public List<Primary> Primary { get; set; }

        [XmlElement(ElementName = "Currency")]
        public Currency Currency { get; set; }

        [XmlElement(ElementName = "CurFiscalPeriod")]
        public CurFiscalPeriod CurFiscalPeriod { get; set; }

        [XmlElement(ElementName = "CompanyPeriods")]
        public CompanyPeriods CompanyPeriods { get; set; }
    }

    [XmlRoot(ElementName = "Company")]
    public class Company
    {
        [XmlElement(ElementName = "CoName")]
        public CoName CoName { get; set; }

        [XmlElement(ElementName = "CoIds")]
        public CoIds CoIds { get; set; }

        [XmlElement(ElementName = "SecurityInfo")]
        public SecurityInfo SecurityInfo { get; set; }

        [XmlElement(ElementName = "CompanyInfo")]
        public CompanyInfo CompanyInfo { get; set; }
    }

    [XmlRoot(ElementName = "ActValue")]
    public class ActValue
    {
        [XmlAttribute(AttributeName = "updated")]
        public string Updated { get; set; }

        [XmlAttribute(AttributeName = "announceDate")]
        public string AnnounceDate { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "FYPeriod")]
    public class FYPeriod
    {
        [XmlElement(ElementName = "ActValue")]
        public ActValue ActValue { get; set; }

        [XmlAttribute(AttributeName = "endCalYear")]
        public string EndCalYear { get; set; }

        [XmlAttribute(AttributeName = "endMonth")]
        public string EndMonth { get; set; }

        [XmlAttribute(AttributeName = "fYear")]
        public string FYear { get; set; }

        [XmlAttribute(AttributeName = "periodNum")]
        public string PeriodNum { get; set; }

        [XmlAttribute(AttributeName = "periodType")]
        public string PeriodType { get; set; }

        [XmlElement(ElementName = "ConsEstimate")]
        public List<ConsEstimate> ConsEstimate { get; set; }
    }

    [XmlRoot(ElementName = "FYActual")]
    public class FYActual
    {
        [XmlElement(ElementName = "FYPeriod")]
        public List<FYPeriod> FYPeriod { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "FYActuals")]
    public class FYActuals
    {
        [XmlElement(ElementName = "FYActual")]
        public List<FYActual> FYActual { get; set; }
    }

    [XmlRoot(ElementName = "Actuals")]
    public class Actuals
    {
        [XmlElement(ElementName = "FYActuals")]
        public FYActuals FYActuals { get; set; }
    }

    [XmlRoot(ElementName = "ConsValue")]
    public class ConsValue
    {
        [XmlAttribute(AttributeName = "dateType")]
        public string DateType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ConsEstimate")]
    public class ConsEstimate
    {
        [XmlElement(ElementName = "ConsValue")]
        public List<ConsValue> ConsValue { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "FYEstimate")]
    public class FYEstimate
    {
        [XmlElement(ElementName = "FYPeriod")]
        public List<FYPeriod> FYPeriod { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "FYEstimates")]
    public class FYEstimates
    {
        [XmlElement(ElementName = "FYEstimate")]
        public List<FYEstimate> FYEstimate { get; set; }
    }

    [XmlRoot(ElementName = "NPEstimate")]
    public class NPEstimate
    {
        [XmlElement(ElementName = "ConsEstimate")]
        public List<ConsEstimate> ConsEstimate { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "NPEstimates")]
    public class NPEstimates
    {
        [XmlElement(ElementName = "NPEstimate")]
        public List<NPEstimate> NPEstimate { get; set; }
    }

    [XmlRoot(ElementName = "ConsOpValue")]
    public class ConsOpValue
    {
        [XmlElement(ElementName = "ConsValue")]
        public List<ConsValue> ConsValue { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "ConsOpinion")]
    public class ConsOpinion
    {
        [XmlElement(ElementName = "ConsOpValue")]
        public ConsOpValue ConsOpValue { get; set; }

        [XmlAttribute(AttributeName = "set")]
        public string Set { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }

    [XmlRoot(ElementName = "STOpinion")]
    public class STOpinion
    {
        [XmlElement(ElementName = "ConsOpinion")]
        public List<ConsOpinion> ConsOpinion { get; set; }
    }

    [XmlRoot(ElementName = "Recommendations")]
    public class Recommendations
    {
        [XmlElement(ElementName = "STOpinion")]
        public STOpinion STOpinion { get; set; }
    }

    [XmlRoot(ElementName = "ConsEstimates")]
    public class ConsEstimates
    {
        [XmlElement(ElementName = "FYEstimates")]
        public FYEstimates FYEstimates { get; set; }

        [XmlElement(ElementName = "NPEstimates")]
        public NPEstimates NPEstimates { get; set; }

        [XmlElement(ElementName = "Recommendations")]
        public Recommendations Recommendations { get; set; }
    }

    [XmlRoot(ElementName = "REarnEstCons")]
    public class REarnEstCons
    {
        [XmlElement(ElementName = "Company")]
        public Company Company { get; set; }

        [XmlElement(ElementName = "Actuals")]
        public Actuals Actuals { get; set; }

        [XmlElement(ElementName = "ConsEstimates")]
        public ConsEstimates ConsEstimates { get; set; }
    }

    #endregion
}
