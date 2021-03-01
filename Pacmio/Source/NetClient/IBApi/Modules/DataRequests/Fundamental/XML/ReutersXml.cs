// ***************************************************************************
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

namespace ReutersXml
{
    public interface IFundamentalResponse 
    {
    
    }

    #region ReportSnapshot

    [XmlRoot(ElementName = "ReportSnapshot")]
    public class ReportSnapshot : IFundamentalResponse
    {
        [XmlElement(ElementName = "CoIDs")]
        public CoIDs CoIDs { get; set; }

        [XmlElement(ElementName = "Issues")]
        public Issues Issues { get; set; }

        [XmlElement(ElementName = "CoGeneralInfo")]
        public CoGeneralInfo CoGeneralInfo { get; set; }

        [XmlElement(ElementName = "TextInfo")]
        public TextInfo TextInfo { get; set; }

        [XmlElement(ElementName = "contactInfo")]
        public ContactInfo ContactInfo { get; set; }

        [XmlElement(ElementName = "webLinks")]
        public WebLinks WebLinks { get; set; }

        [XmlElement(ElementName = "peerInfo")]
        public PeerInfo PeerInfo { get; set; }

        [XmlElement(ElementName = "officers")]
        public Officers Officers { get; set; }

        [XmlElement(ElementName = "Ratios")]
        public Ratios Ratios { get; set; }

        [XmlElement(ElementName = "ForecastData")]
        public ForecastData ForecastData { get; set; }

        [XmlAttribute(AttributeName = "Major")]
        public string Major { get; set; }

        [XmlAttribute(AttributeName = "Minor")]
        public string Minor { get; set; }

        [XmlAttribute(AttributeName = "Revision")]
        public string Revision { get; set; }
    }

    [XmlRoot(ElementName = "CoID")]
    public class CoID
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CoIDs")]
    public class CoIDs
    {
        [XmlElement(ElementName = "CoID")]
        public List<CoID> CoID { get; set; }
    }

    [XmlRoot(ElementName = "IssueID")]
    public class IssueID
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Exchange")]
    public class Exchange
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "Country")]
        public string Country { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MostRecentSplit")]
    public class MostRecentSplit
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Issue")]
    public class Issue
    {
        [XmlElement(ElementName = "IssueID")]
        public List<IssueID> IssueID { get; set; }

        [XmlElement(ElementName = "Exchange")]
        public Exchange Exchange { get; set; }

        [XmlElement(ElementName = "MostRecentSplit")]
        public MostRecentSplit MostRecentSplit { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "Desc")]
        public string Desc { get; set; }

        [XmlAttribute(AttributeName = "Order")]
        public string Order { get; set; }
    }

    [XmlRoot(ElementName = "Issues")]
    public class Issues
    {
        [XmlElement(ElementName = "Issue")]
        public List<Issue> Issue { get; set; }
    }

    [XmlRoot(ElementName = "CoStatus")]
    public class CoStatus
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CoType")]
    public class CoType
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Employees")]
    public class Employees
    {
        [XmlAttribute(AttributeName = "LastUpdated")]
        public string LastUpdated { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "SharesOut")]
    public class SharesOut
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlAttribute(AttributeName = "TotalFloat")]
        public string TotalFloat { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "CommonShareholders")]
    public class CommonShareholders
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ReportingCurrency")]
    public class ReportingCurrency
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MostRecentExchange")]
    public class MostRecentExchange
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlText]
        public double Value { get; set; }
    }

    [XmlRoot(ElementName = "CoGeneralInfo")]
    public class CoGeneralInfo
    {
        [XmlElement(ElementName = "CoStatus")]
        public CoStatus CoStatus { get; set; }

        [XmlElement(ElementName = "CoType")]
        public CoType CoType { get; set; }

        [XmlElement(ElementName = "LastModified")]
        public string LastModified { get; set; }

        [XmlElement(ElementName = "LatestAvailableAnnual")]
        public string LatestAvailableAnnual { get; set; }

        [XmlElement(ElementName = "LatestAvailableInterim")]
        public string LatestAvailableInterim { get; set; }

        [XmlElement(ElementName = "Employees")]
        public Employees Employees { get; set; }

        [XmlElement(ElementName = "SharesOut")]
        public SharesOut SharesOut { get; set; }

        [XmlElement(ElementName = "CommonShareholders")]
        public CommonShareholders CommonShareholders { get; set; }

        [XmlElement(ElementName = "ReportingCurrency")]
        public ReportingCurrency ReportingCurrency { get; set; }

        [XmlElement(ElementName = "MostRecentExchange")]
        public MostRecentExchange MostRecentExchange { get; set; }
    }

    [XmlRoot(ElementName = "Text")]
    public class Text
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "lastModified")]
        public string LastModified { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "TextInfo")]
    public class TextInfo
    {
        [XmlElement(ElementName = "Text")]
        public List<Text> Text { get; set; }
    }

    [XmlRoot(ElementName = "streetAddress")]
    public class StreetAddress
    {
        [XmlAttribute(AttributeName = "line")]
        public string Line { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "country")]
    public class Country
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "phone")]
    public class Phone
    {
        [XmlElement(ElementName = "countryPhoneCode")]
        public string CountryPhoneCode { get; set; }

        [XmlElement(ElementName = "city-areacode")]
        public string Cityareacode { get; set; }

        [XmlElement(ElementName = "number")]
        public string Number { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "contactInfo")]
    public class ContactInfo
    {
        [XmlElement(ElementName = "streetAddress")]
        public List<StreetAddress> StreetAddress { get; set; }

        [XmlElement(ElementName = "city")]
        public string City { get; set; }

        [XmlElement(ElementName = "state-region")]
        public string Stateregion { get; set; }

        [XmlElement(ElementName = "postalCode")]
        public string PostalCode { get; set; }

        [XmlElement(ElementName = "country")]
        public Country Country { get; set; }

        [XmlElement(ElementName = "contactName")]
        public string ContactName { get; set; }

        [XmlElement(ElementName = "contactTitle")]
        public string ContactTitle { get; set; }

        [XmlElement(ElementName = "phone")]
        public Phones Phone { get; set; }

        [XmlAttribute(AttributeName = "lastUpdated")]
        public string LastUpdated { get; set; }
    }

    [XmlRoot(ElementName = "phone")]
    public class Phones
    {
        [XmlElement(ElementName = "phone")]
        public List<Phone> Phone { get; set; }
    }

    [XmlRoot(ElementName = "webSite")]
    public class WebSite
    {
        [XmlAttribute(AttributeName = "mainCategory")]
        public string MainCategory { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "eMail")]
    public class EMail
    {
        [XmlAttribute(AttributeName = "mainCategory")]
        public string MainCategory { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "webLinks")]
    public class WebLinks
    {
        [XmlElement(ElementName = "webSite")]
        public List<WebSite> WebSite { get; set; }

        [XmlElement(ElementName = "eMail")]
        public List<EMail> EMail { get; set; }

        [XmlAttribute(AttributeName = "lastUpdated")]
        public string LastUpdated { get; set; }
    }

    [XmlRoot(ElementName = "Industry")]
    public class Industry
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public int Order { get; set; }

        [XmlAttribute(AttributeName = "reported")]
        public string Reported { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mnem")]
        public string Mnem { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "IndustryInfo")]
    public class IndustryInfo
    {
        [XmlElement(ElementName = "Industry")]
        public List<Industry> Industry { get; set; }
    }

    [XmlRoot(ElementName = "peerInfo")]
    public class PeerInfo
    {
        [XmlElement(ElementName = "IndustryInfo")]
        public IndustryInfo IndustryInfo { get; set; }

        [XmlElement(ElementName = "Indexconstituet")]
        public List<string> Indexconstituet { get; set; }

        [XmlAttribute(AttributeName = "lastUpdated")]
        public string LastUpdated { get; set; }
    }

    [XmlRoot(ElementName = "title")]
    public class Title
    {
        [XmlAttribute(AttributeName = "startYear")]
        public string StartYear { get; set; }

        [XmlAttribute(AttributeName = "startMonth")]
        public string StartMonth { get; set; }

        [XmlAttribute(AttributeName = "startDay")]
        public string StartDay { get; set; }

        [XmlAttribute(AttributeName = "iD1")]
        public string ID1 { get; set; }

        [XmlAttribute(AttributeName = "abbr1")]
        public string Abbr1 { get; set; }

        [XmlAttribute(AttributeName = "iD2")]
        public string ID2 { get; set; }

        [XmlAttribute(AttributeName = "abbr2")]
        public string Abbr2 { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "officer")]
    public class Officer
    {
        [XmlElement(ElementName = "firstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "mI")]
        public string MI { get; set; }

        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "age")]
        public string Age { get; set; }

        [XmlElement(ElementName = "title")]
        public Title Title { get; set; }

        [XmlAttribute(AttributeName = "rank")]
        public string Rank { get; set; }

        [XmlAttribute(AttributeName = "since")]
        public string Since { get; set; }
    }

    [XmlRoot(ElementName = "officers")]
    public class Officers
    {
        [XmlElement(ElementName = "officer")]
        public List<Officer> Officer { get; set; }
    }

    [XmlRoot(ElementName = "Ratio")]
    public class Ratio
    {
        [XmlAttribute(AttributeName = "FieldName")]
        public string FieldName { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "Value")]
        public Value Value { get; set; }
    }

    [XmlRoot(ElementName = "Group")]
    public class Group
    {
        [XmlElement(ElementName = "Ratio")]
        public List<Ratio> Ratio { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "Ratios")]
    public class Ratios
    {
        [XmlElement(ElementName = "Group")]
        public List<Group> Group { get; set; }

        [XmlAttribute(AttributeName = "PriceCurrency")]
        public string PriceCurrency { get; set; }

        [XmlAttribute(AttributeName = "ReportingCurrency")]
        public string ReportingCurrency { get; set; }

        [XmlAttribute(AttributeName = "ExchangeRate")]
        public string ExchangeRate { get; set; }

        [XmlAttribute(AttributeName = "LatestAvailableDate")]
        public string LatestAvailableDate { get; set; }
    }

    [XmlRoot(ElementName = "Value")]
    public class Value
    {
        [XmlAttribute(AttributeName = "PeriodType")]
        public string PeriodType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ForecastData")]
    public class ForecastData
    {
        [XmlElement(ElementName = "Ratio")]
        public List<Ratio> Ratio { get; set; }

        [XmlAttribute(AttributeName = "ConsensusType")]
        public string ConsensusType { get; set; }

        [XmlAttribute(AttributeName = "CurFiscalYear")]
        public string CurFiscalYear { get; set; }

        [XmlAttribute(AttributeName = "CurFiscalYearEndMonth")]
        public string CurFiscalYearEndMonth { get; set; }

        [XmlAttribute(AttributeName = "CurInterimEndCalYear")]
        public string CurInterimEndCalYear { get; set; }

        [XmlAttribute(AttributeName = "CurInterimEndMonth")]
        public string CurInterimEndMonth { get; set; }

        [XmlAttribute(AttributeName = "EarningsBasis")]
        public string EarningsBasis { get; set; }
    }

    #endregion ReportSnapshot

    #region ReportFinancialStatements

    [XmlRoot(ElementName = "COAType")]
    public class COAType
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "BalanceSheetDisplay")]
    public class BalanceSheetDisplay
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CashFlowMethod")]
    public class CashFlowMethod
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "StatementInfo")]
    public class StatementInfo
    {
        [XmlElement(ElementName = "COAType")]
        public COAType COAType { get; set; }

        [XmlElement(ElementName = "BalanceSheetDisplay")]
        public BalanceSheetDisplay BalanceSheetDisplay { get; set; }

        [XmlElement(ElementName = "CashFlowMethod")]
        public CashFlowMethod CashFlowMethod { get; set; }
    }

    [XmlRoot(ElementName = "CFAAvailability")]
    public class CFAAvailability
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "IAvailability")]
    public class IAvailability
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "ISIAvailability")]
    public class ISIAvailability
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "BSIAvailability")]
    public class BSIAvailability
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "CFIAvailability")]
    public class CFIAvailability
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "Notes")]
    public class Notes
    {
        [XmlElement(ElementName = "CFAAvailability")]
        public CFAAvailability CFAAvailability { get; set; }

        [XmlElement(ElementName = "IAvailability")]
        public IAvailability IAvailability { get; set; }

        [XmlElement(ElementName = "ISIAvailability")]
        public ISIAvailability ISIAvailability { get; set; }

        [XmlElement(ElementName = "BSIAvailability")]
        public BSIAvailability BSIAvailability { get; set; }

        [XmlElement(ElementName = "CFIAvailability")]
        public CFIAvailability CFIAvailability { get; set; }
    }

    [XmlRoot(ElementName = "mapItem")]
    public class MapItem
    {
        [XmlAttribute(AttributeName = "coaItem")]
        public string CoaItem { get; set; }

        [XmlAttribute(AttributeName = "statementType")]
        public string StatementType { get; set; }

        [XmlAttribute(AttributeName = "lineID")]
        public string LineID { get; set; }

        [XmlAttribute(AttributeName = "precision")]
        public string Precision { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "COAMap")]
    public class COAMap
    {
        [XmlElement(ElementName = "mapItem")]
        public List<MapItem> MapItem { get; set; }
    }

    [XmlRoot(ElementName = "periodType")]
    public class PeriodType
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "UpdateType")]
    public class UpdateType
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "AuditorName")]
    public class AuditorName
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "AuditorOpinion")]
    public class AuditorOpinion
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Source")]
    public class Source
    {
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "FPHeader")]
    public class FPHeader
    {
        [XmlElement(ElementName = "PeriodLength")]
        public string PeriodLength { get; set; }

        [XmlElement(ElementName = "periodType")]
        public PeriodType PeriodType { get; set; }

        [XmlElement(ElementName = "UpdateType")]
        public UpdateType UpdateType { get; set; }

        [XmlElement(ElementName = "AccountingStd")]
        public string AccountingStd { get; set; }

        [XmlElement(ElementName = "StatementDate")]
        public string StatementDate { get; set; }

        [XmlElement(ElementName = "AuditorName")]
        public AuditorName AuditorName { get; set; }

        [XmlElement(ElementName = "AuditorOpinion")]
        public AuditorOpinion AuditorOpinion { get; set; }

        [XmlElement(ElementName = "Source")]
        public Source Source { get; set; }
    }

    [XmlRoot(ElementName = "lineItem")]
    public class LineItem
    {
        [XmlAttribute(AttributeName = "coaCode")]
        public string CoaCode { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Statement")]
    public class Statement
    {
        [XmlElement(ElementName = "FPHeader")]
        public FPHeader FPHeader { get; set; }

        [XmlElement(ElementName = "lineItem")]
        public List<LineItem> LineItem { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "FiscalPeriod")]
    public class FiscalPeriod
    {
        [XmlElement(ElementName = "Statement")]
        public List<Statement> Statement { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "EndDate")]
        public string EndDate { get; set; }

        [XmlAttribute(AttributeName = "FiscalYear")]
        public string FiscalYear { get; set; }

        [XmlAttribute(AttributeName = "FiscalPeriodNumber")]
        public string FiscalPeriodNumber { get; set; }
    }

    [XmlRoot(ElementName = "AnnualPeriods")]
    public class AnnualPeriods
    {
        [XmlElement(ElementName = "FiscalPeriod")]
        public List<FiscalPeriod> FiscalPeriod { get; set; }
    }

    [XmlRoot(ElementName = "InterimPeriods")]
    public class InterimPeriods
    {
        [XmlElement(ElementName = "FiscalPeriod")]
        public List<FiscalPeriod> FiscalPeriod { get; set; }
    }

    [XmlRoot(ElementName = "FinancialStatements")]
    public class FinancialStatements
    {
        [XmlElement(ElementName = "COAMap")]
        public COAMap COAMap { get; set; }

        [XmlElement(ElementName = "AnnualPeriods")]
        public AnnualPeriods AnnualPeriods { get; set; }

        [XmlElement(ElementName = "InterimPeriods")]
        public InterimPeriods InterimPeriods { get; set; }
    }

    [XmlRoot(ElementName = "ReportFinancialStatements")]
    public class ReportFinancialStatements : IFundamentalResponse
    {
        [XmlElement(ElementName = "CoIDs")]
        public CoIDs CoIDs { get; set; }

        [XmlElement(ElementName = "Issues")]
        public Issues Issues { get; set; }

        [XmlElement(ElementName = "CoGeneralInfo")]
        public CoGeneralInfo CoGeneralInfo { get; set; }

        [XmlElement(ElementName = "StatementInfo")]
        public StatementInfo StatementInfo { get; set; }

        [XmlElement(ElementName = "Notes")]
        public Notes Notes { get; set; }

        [XmlElement(ElementName = "FinancialStatements")]
        public FinancialStatements FinancialStatements { get; set; }

        [XmlAttribute(AttributeName = "Major")]
        public string Major { get; set; }

        [XmlAttribute(AttributeName = "Minor")]
        public string Minor { get; set; }

        [XmlAttribute(AttributeName = "Revision")]
        public string Revision { get; set; }
    }

    #endregion ReportFinancialStatements

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
    public class FinancialSummary : IFundamentalResponse
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

    #region Owners

    [XmlRoot(ElementName = "floatShares")]
    public class FloatShares
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "name")]
    public class Name
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "quantity")]
    public class Quantity
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "currency")]
    public class Currency
    {
        [XmlAttribute(AttributeName = "asofDate")]
        public string AsofDate { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Owner")]
    public class Owner
    {
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "quantity")]
        public Quantity Quantity { get; set; }

        [XmlElement(ElementName = "currency")]
        public Currency Currency { get; set; }

        [XmlAttribute(AttributeName = "ownerId")]
        public string OwnerId { get; set; }
    }

    [XmlRoot(ElementName = "OwnershipDetails")]
    public class OwnershipDetails : IFundamentalResponse
    {
        [XmlElement(ElementName = "ISIN")]
        public string ISIN { get; set; }

        [XmlElement(ElementName = "floatShares")]
        public FloatShares FloatShares { get; set; }

        [XmlElement(ElementName = "Owner")]
        public List<Owner> Owner { get; set; }
    }

    #endregion Owners

    #region Calendar

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

    #endregion Calendar
}
