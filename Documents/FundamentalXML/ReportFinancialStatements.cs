   /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
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
		public Issue Issue { get; set; }
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
		[XmlElement(ElementName="ReportingCurrency")]
		public ReportingCurrency ReportingCurrency { get; set; }
		[XmlElement(ElementName="MostRecentExchange")]
		public MostRecentExchange MostRecentExchange { get; set; }
	}

	[XmlRoot(ElementName="COAType")]
	public class COAType {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="BalanceSheetDisplay")]
	public class BalanceSheetDisplay {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="CashFlowMethod")]
	public class CashFlowMethod {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="StatementInfo")]
	public class StatementInfo {
		[XmlElement(ElementName="COAType")]
		public COAType COAType { get; set; }
		[XmlElement(ElementName="BalanceSheetDisplay")]
		public BalanceSheetDisplay BalanceSheetDisplay { get; set; }
		[XmlElement(ElementName="CashFlowMethod")]
		public CashFlowMethod CashFlowMethod { get; set; }
	}

	[XmlRoot(ElementName="CFAAvailability")]
	public class CFAAvailability {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName="IAvailability")]
	public class IAvailability {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName="ISIAvailability")]
	public class ISIAvailability {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName="BSIAvailability")]
	public class BSIAvailability {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName="CFIAvailability")]
	public class CFIAvailability {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName="Notes")]
	public class Notes {
		[XmlElement(ElementName="CFAAvailability")]
		public CFAAvailability CFAAvailability { get; set; }
		[XmlElement(ElementName="IAvailability")]
		public IAvailability IAvailability { get; set; }
		[XmlElement(ElementName="ISIAvailability")]
		public ISIAvailability ISIAvailability { get; set; }
		[XmlElement(ElementName="BSIAvailability")]
		public BSIAvailability BSIAvailability { get; set; }
		[XmlElement(ElementName="CFIAvailability")]
		public CFIAvailability CFIAvailability { get; set; }
	}

	[XmlRoot(ElementName="mapItem")]
	public class MapItem {
		[XmlAttribute(AttributeName="coaItem")]
		public string CoaItem { get; set; }
		[XmlAttribute(AttributeName="statementType")]
		public string StatementType { get; set; }
		[XmlAttribute(AttributeName="lineID")]
		public string LineID { get; set; }
		[XmlAttribute(AttributeName="precision")]
		public string Precision { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="COAMap")]
	public class COAMap {
		[XmlElement(ElementName="mapItem")]
		public List<MapItem> MapItem { get; set; }
	}

	[XmlRoot(ElementName="periodType")]
	public class PeriodType {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="UpdateType")]
	public class UpdateType {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="AuditorName")]
	public class AuditorName {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="AuditorOpinion")]
	public class AuditorOpinion {
		[XmlAttribute(AttributeName="Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Source")]
	public class Source {
		[XmlAttribute(AttributeName="Date")]
		public string Date { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="FPHeader")]
	public class FPHeader {
		[XmlElement(ElementName="PeriodLength")]
		public string PeriodLength { get; set; }
		[XmlElement(ElementName="periodType")]
		public PeriodType PeriodType { get; set; }
		[XmlElement(ElementName="UpdateType")]
		public UpdateType UpdateType { get; set; }
		[XmlElement(ElementName="AccountingStd")]
		public string AccountingStd { get; set; }
		[XmlElement(ElementName="StatementDate")]
		public string StatementDate { get; set; }
		[XmlElement(ElementName="AuditorName")]
		public AuditorName AuditorName { get; set; }
		[XmlElement(ElementName="AuditorOpinion")]
		public AuditorOpinion AuditorOpinion { get; set; }
		[XmlElement(ElementName="Source")]
		public Source Source { get; set; }
	}

	[XmlRoot(ElementName="lineItem")]
	public class LineItem {
		[XmlAttribute(AttributeName="coaCode")]
		public string CoaCode { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Statement")]
	public class Statement {
		[XmlElement(ElementName="FPHeader")]
		public FPHeader FPHeader { get; set; }
		[XmlElement(ElementName="lineItem")]
		public List<LineItem> LineItem { get; set; }
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName="FiscalPeriod")]
	public class FiscalPeriod {
		[XmlElement(ElementName="Statement")]
		public List<Statement> Statement { get; set; }
		[XmlAttribute(AttributeName="Type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="EndDate")]
		public string EndDate { get; set; }
		[XmlAttribute(AttributeName="FiscalYear")]
		public string FiscalYear { get; set; }
		[XmlAttribute(AttributeName="FiscalPeriodNumber")]
		public string FiscalPeriodNumber { get; set; }
	}

	[XmlRoot(ElementName="AnnualPeriods")]
	public class AnnualPeriods {
		[XmlElement(ElementName="FiscalPeriod")]
		public List<FiscalPeriod> FiscalPeriod { get; set; }
	}

	[XmlRoot(ElementName="InterimPeriods")]
	public class InterimPeriods {
		[XmlElement(ElementName="FiscalPeriod")]
		public List<FiscalPeriod> FiscalPeriod { get; set; }
	}

	[XmlRoot(ElementName="FinancialStatements")]
	public class FinancialStatements {
		[XmlElement(ElementName="COAMap")]
		public COAMap COAMap { get; set; }
		[XmlElement(ElementName="AnnualPeriods")]
		public AnnualPeriods AnnualPeriods { get; set; }
		[XmlElement(ElementName="InterimPeriods")]
		public InterimPeriods InterimPeriods { get; set; }
	}

	[XmlRoot(ElementName="ReportFinancialStatements")]
	public class ReportFinancialStatements {
		[XmlElement(ElementName="CoIDs")]
		public CoIDs CoIDs { get; set; }
		[XmlElement(ElementName="Issues")]
		public Issues Issues { get; set; }
		[XmlElement(ElementName="CoGeneralInfo")]
		public CoGeneralInfo CoGeneralInfo { get; set; }
		[XmlElement(ElementName="StatementInfo")]
		public StatementInfo StatementInfo { get; set; }
		[XmlElement(ElementName="Notes")]
		public Notes Notes { get; set; }
		[XmlElement(ElementName="FinancialStatements")]
		public FinancialStatements FinancialStatements { get; set; }
		[XmlAttribute(AttributeName="Major")]
		public string Major { get; set; }
		[XmlAttribute(AttributeName="Minor")]
		public string Minor { get; set; }
		[XmlAttribute(AttributeName="Revision")]
		public string Revision { get; set; }
	}

}
