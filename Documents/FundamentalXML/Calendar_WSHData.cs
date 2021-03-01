   /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName="Earnings")]
	public class Earnings {
		[XmlElement(ElementName="Q1")]
		public string Q1 { get; set; }
		[XmlElement(ElementName="Q2")]
		public string Q2 { get; set; }
		[XmlElement(ElementName="Q3")]
		public string Q3 { get; set; }
		[XmlElement(ElementName="Q4")]
		public string Q4 { get; set; }
		[XmlElement(ElementName="Period")]
		public string Period { get; set; }
		[XmlElement(ElementName="Time")]
		public string Time { get; set; }
		[XmlElement(ElementName="Etype")]
		public string Etype { get; set; }
		[XmlElement(ElementName="Date")]
		public string Date { get; set; }
		[XmlElement(ElementName="TimeStamp")]
		public string TimeStamp { get; set; }
	}

	[XmlRoot(ElementName="EarningsList")]
	public class EarningsList {
		[XmlElement(ElementName="Earnings")]
		public Earnings Earnings { get; set; }
	}

	[XmlRoot(ElementName="EarningsCallTranscript")]
	public class EarningsCallTranscript {
		[XmlElement(ElementName="Period")]
		public string Period { get; set; }
		[XmlElement(ElementName="URL")]
		public string URL { get; set; }
		[XmlElement(ElementName="TimeStamp")]
		public string TimeStamp { get; set; }
	}

	[XmlRoot(ElementName="EarningsCallTranscriptList")]
	public class EarningsCallTranscriptList {
		[XmlElement(ElementName="EarningsCallTranscript")]
		public EarningsCallTranscript EarningsCallTranscript { get; set; }
	}

	[XmlRoot(ElementName="ShareHolderMeeting")]
	public class ShareHolderMeeting {
		[XmlElement(ElementName="Type")]
		public string Type { get; set; }
		[XmlElement(ElementName="Date")]
		public string Date { get; set; }
		[XmlElement(ElementName="TimeStamp")]
		public string TimeStamp { get; set; }
	}

	[XmlRoot(ElementName="ShareHolderMeetingList")]
	public class ShareHolderMeetingList {
		[XmlElement(ElementName="ShareHolderMeeting")]
		public ShareHolderMeeting ShareHolderMeeting { get; set; }
	}

	[XmlRoot(ElementName="Presentation")]
	public class Presentation {
		[XmlElement(ElementName="TimeZone")]
		public string TimeZone { get; set; }
		[XmlElement(ElementName="Title")]
		public string Title { get; set; }
		[XmlElement(ElementName="Time")]
		public string Time { get; set; }
		[XmlElement(ElementName="EventName")]
		public string EventName { get; set; }
		[XmlElement(ElementName="TimeStamp")]
		public string TimeStamp { get; set; }
		[XmlElement(ElementName="Date")]
		public string Date { get; set; }
		[XmlElement(ElementName="Name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName="PresentationList")]
	public class PresentationList {
		[XmlElement(ElementName="Presentation")]
		public Presentation Presentation { get; set; }
	}

	[XmlRoot(ElementName="Company")]
	public class Company {
		[XmlElement(ElementName="Name")]
		public string Name { get; set; }
		[XmlElement(ElementName="Ticker")]
		public string Ticker { get; set; }
		[XmlElement(ElementName="ISIN")]
		public string ISIN { get; set; }
		[XmlElement(ElementName="Exchange")]
		public string Exchange { get; set; }
		[XmlElement(ElementName="Country")]
		public string Country { get; set; }
		[XmlElement(ElementName="conid")]
		public List<string> Conid { get; set; }
		[XmlElement(ElementName="EarningsList")]
		public EarningsList EarningsList { get; set; }
		[XmlElement(ElementName="EarningsCallTranscriptList")]
		public EarningsCallTranscriptList EarningsCallTranscriptList { get; set; }
		[XmlElement(ElementName="ShareHolderMeetingList")]
		public ShareHolderMeetingList ShareHolderMeetingList { get; set; }
		[XmlElement(ElementName="PresentationList")]
		public PresentationList PresentationList { get; set; }
	}

	[XmlRoot(ElementName="WSHData")]
	public class WSHData {
		[XmlElement(ElementName="Company")]
		public Company Company { get; set; }
	}

}
