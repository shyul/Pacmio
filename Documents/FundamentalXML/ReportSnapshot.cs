   /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0

    https://xmltocsharp.azurewebsites.net/
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName="CoID")]
	public class CoID {
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="CoIDs")]
	public class CoIDs {
		[XmlElement(ElementName="CoID")]
		public List<CoID> CoID { get; set; }
	}

	[XmlRoot(ElementName="IssueID")]
	public class IssueID {
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Exchange")]
	public class Exchange {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlAttribute(AttributeName="Country")]
		public string Country { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="MostRecentSplit")]
	public class MostRecentSplit {
		[XmlAttribute(AttributeName="Date")]
		public string Date { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Issue")]
	public class Issue {
		[XmlElement(ElementName="IssueID")]
		public List<IssueID> IssueID { get; set; }
		[XmlElement(ElementName="Exchange")]
		public Exchange Exchange { get; set; }
		[XmlElement(ElementName="GlobalListingType")]
		public string GlobalListingType { get; set; }
		[XmlElement(ElementName="MostRecentSplit")]
		public MostRecentSplit MostRecentSplit { get; set; }
		[XmlAttribute(AttributeName="ID")]
		public string ID { get; set; }
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="Desc")]
		public string Desc { get; set; }
		[XmlAttribute(AttributeName="Order")]
		public string Order { get; set; }
	}

	[XmlRoot(ElementName="Issues")]
	public class Issues {
		[XmlElement(ElementName="Issue")]
		public List<Issue> Issue { get; set; }
	}

	[XmlRoot(ElementName="CoStatus")]
	public class CoStatus {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="CoType")]
	public class CoType {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Employees")]
	public class Employees {
		[XmlAttribute(AttributeName="LastUpdated")]
		public string LastUpdated { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="SharesOut")]
	public class SharesOut {
		[XmlAttribute(AttributeName="Date")]
		public string Date { get; set; }
		[XmlAttribute(AttributeName="TotalFloat")]
		public string TotalFloat { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="ReportingCurrency")]
	public class ReportingCurrency {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="MostRecentExchange")]
	public class MostRecentExchange {
		[XmlAttribute(AttributeName="Date")]
		public string Date { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="CoGeneralInfo")]
	public class CoGeneralInfo {
		[XmlElement(ElementName="CoStatus")]
		public CoStatus CoStatus { get; set; }
		[XmlElement(ElementName="CoType")]
		public CoType CoType { get; set; }
		[XmlElement(ElementName="LastModified")]
		public string LastModified { get; set; }
		[XmlElement(ElementName="LatestAvailableAnnual")]
		public string LatestAvailableAnnual { get; set; }
		[XmlElement(ElementName="LatestAvailableInterim")]
		public string LatestAvailableInterim { get; set; }
		[XmlElement(ElementName="Employees")]
		public Employees Employees { get; set; }
		[XmlElement(ElementName="SharesOut")]
		public SharesOut SharesOut { get; set; }
		[XmlElement(ElementName="ReportingCurrency")]
		public ReportingCurrency ReportingCurrency { get; set; }
		[XmlElement(ElementName="MostRecentExchange")]
		public MostRecentExchange MostRecentExchange { get; set; }
	}

	[XmlRoot(ElementName="Text")]
	public class Text {
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="lastModified")]
		public string LastModified { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="TextInfo")]
	public class TextInfo {
		[XmlElement(ElementName="Text")]
		public List<Text> Text { get; set; }
	}

	[XmlRoot(ElementName="streetAddress")]
	public class StreetAddress {
		[XmlAttribute(AttributeName="line")]
		public string Line { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="country")]
	public class Country {
		[XmlAttribute(AttributeName="code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="phone")]
	public class Phone {
		[XmlElement(ElementName="countryPhoneCode")]
		public string CountryPhoneCode { get; set; }
		[XmlElement(ElementName="city-areacode")]
		public string Cityareacode { get; set; }
		[XmlElement(ElementName="number")]
		public string Number { get; set; }
		[XmlAttribute(AttributeName="type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName="contactInfo")]
	public class ContactInfo {
		[XmlElement(ElementName="streetAddress")]
		public List<StreetAddress> StreetAddress { get; set; }
		[XmlElement(ElementName="city")]
		public string City { get; set; }
		[XmlElement(ElementName="state-region")]
		public string Stateregion { get; set; }
		[XmlElement(ElementName="postalCode")]
		public string PostalCode { get; set; }
		[XmlElement(ElementName="country")]
		public Country Country { get; set; }
		[XmlElement(ElementName="contactName")]
		public string ContactName { get; set; }
		[XmlElement(ElementName="contactTitle")]
		public string ContactTitle { get; set; }
		[XmlElement(ElementName="phone")]
		public Phone Phone { get; set; }
		[XmlAttribute(AttributeName="lastUpdated")]
		public string LastUpdated { get; set; }
	}

	[XmlRoot(ElementName="webSite")]
	public class WebSite {
		[XmlAttribute(AttributeName="mainCategory")]
		public string MainCategory { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="eMail")]
	public class EMail {
		[XmlAttribute(AttributeName="mainCategory")]
		public string MainCategory { get; set; }
	}

	[XmlRoot(ElementName="webLinks")]
	public class WebLinks {
		[XmlElement(ElementName="webSite")]
		public WebSite WebSite { get; set; }
		[XmlElement(ElementName="eMail")]
		public EMail EMail { get; set; }
		[XmlAttribute(AttributeName="lastUpdated")]
		public string LastUpdated { get; set; }
	}

	[XmlRoot(ElementName="Industry")]
	public class Industry {
		[XmlAttribute(AttributeName="type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="order")]
		public string Order { get; set; }
		[XmlAttribute(AttributeName="reported")]
		public string Reported { get; set; }
		[XmlAttribute(AttributeName="code")]
		public string Code { get; set; }
		[XmlAttribute(AttributeName="mnem")]
		public string Mnem { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="IndustryInfo")]
	public class IndustryInfo {
		[XmlElement(ElementName="Industry")]
		public List<Industry> Industry { get; set; }
	}

	[XmlRoot(ElementName="peerInfo")]
	public class PeerInfo {
		[XmlElement(ElementName="IndustryInfo")]
		public IndustryInfo IndustryInfo { get; set; }
		[XmlElement(ElementName="Indexconstituet")]
		public List<string> Indexconstituet { get; set; }
		[XmlAttribute(AttributeName="lastUpdated")]
		public string LastUpdated { get; set; }
	}

	[XmlRoot(ElementName="title")]
	public class Title {
		[XmlAttribute(AttributeName="startYear")]
		public string StartYear { get; set; }
		[XmlAttribute(AttributeName="startMonth")]
		public string StartMonth { get; set; }
		[XmlAttribute(AttributeName="startDay")]
		public string StartDay { get; set; }
		[XmlAttribute(AttributeName="iD1")]
		public string ID1 { get; set; }
		[XmlAttribute(AttributeName="abbr1")]
		public string Abbr1 { get; set; }
		[XmlAttribute(AttributeName="iD2")]
		public string ID2 { get; set; }
		[XmlAttribute(AttributeName="abbr2")]
		public string Abbr2 { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="officer")]
	public class Officer {
		[XmlElement(ElementName="firstName")]
		public string FirstName { get; set; }
		[XmlElement(ElementName="mI")]
		public string MI { get; set; }
		[XmlElement(ElementName="lastName")]
		public string LastName { get; set; }
		[XmlElement(ElementName="age")]
		public string Age { get; set; }
		[XmlElement(ElementName="title")]
		public Title Title { get; set; }
		[XmlAttribute(AttributeName="rank")]
		public string Rank { get; set; }
		[XmlAttribute(AttributeName="since")]
		public string Since { get; set; }
	}

	[XmlRoot(ElementName="officers")]
	public class Officers {
		[XmlElement(ElementName="officer")]
		public List<Officer> Officer { get; set; }
	}

	[XmlRoot(ElementName="Ratio")]
	public class Ratio {
		[XmlAttribute(AttributeName="FieldName")]
		public string FieldName { get; set; }
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlText]
		public string Text { get; set; }
		[XmlElement(ElementName="Value")]
		public Value Value { get; set; }
	}

	[XmlRoot(ElementName="Group")]
	public class Group {
		[XmlElement(ElementName="Ratio")]
		public List<Ratio> Ratio { get; set; }
		[XmlAttribute(AttributeName="ID")]
		public string ID { get; set; }
	}

	[XmlRoot(ElementName="Ratios")]
	public class Ratios {
		[XmlElement(ElementName="Group")]
		public List<Group> Group { get; set; }
		[XmlAttribute(AttributeName="PriceCurrency")]
		public string PriceCurrency { get; set; }
		[XmlAttribute(AttributeName="ReportingCurrency")]
		public string ReportingCurrency { get; set; }
		[XmlAttribute(AttributeName="ExchangeRate")]
		public string ExchangeRate { get; set; }
		[XmlAttribute(AttributeName="LatestAvailableDate")]
		public string LatestAvailableDate { get; set; }
	}

	[XmlRoot(ElementName="Value")]
	public class Value {
		[XmlAttribute(AttributeName="PeriodType")]
		public string PeriodType { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="ForecastData")]
	public class ForecastData {
		[XmlElement(ElementName="Ratio")]
		public List<Ratio> Ratio { get; set; }
		[XmlAttribute(AttributeName="ConsensusType")]
		public string ConsensusType { get; set; }
		[XmlAttribute(AttributeName="CurFiscalYear")]
		public string CurFiscalYear { get; set; }
		[XmlAttribute(AttributeName="CurFiscalYearEndMonth")]
		public string CurFiscalYearEndMonth { get; set; }
		[XmlAttribute(AttributeName="CurInterimEndCalYear")]
		public string CurInterimEndCalYear { get; set; }
		[XmlAttribute(AttributeName="CurInterimEndMonth")]
		public string CurInterimEndMonth { get; set; }
		[XmlAttribute(AttributeName="EarningsBasis")]
		public string EarningsBasis { get; set; }
	}

	[XmlRoot(ElementName="ReportSnapshot")]
	public class ReportSnapshot {
		[XmlElement(ElementName="CoIDs")]
		public CoIDs CoIDs { get; set; }
		[XmlElement(ElementName="Issues")]
		public Issues Issues { get; set; }
		[XmlElement(ElementName="CoGeneralInfo")]
		public CoGeneralInfo CoGeneralInfo { get; set; }
		[XmlElement(ElementName="TextInfo")]
		public TextInfo TextInfo { get; set; }
		[XmlElement(ElementName="contactInfo")]
		public ContactInfo ContactInfo { get; set; }
		[XmlElement(ElementName="webLinks")]
		public WebLinks WebLinks { get; set; }
		[XmlElement(ElementName="peerInfo")]
		public PeerInfo PeerInfo { get; set; }
		[XmlElement(ElementName="officers")]
		public Officers Officers { get; set; }
		[XmlElement(ElementName="Ratios")]
		public Ratios Ratios { get; set; }
		[XmlElement(ElementName="ForecastData")]
		public ForecastData ForecastData { get; set; }
		[XmlAttribute(AttributeName="Major")]
		public string Major { get; set; }
		[XmlAttribute(AttributeName="Minor")]
		public string Minor { get; set; }
		[XmlAttribute(AttributeName="Revision")]
		public string Revision { get; set; }
	}

}
