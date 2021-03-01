   /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName="floatShares")]
	public class FloatShares {
		[XmlAttribute(AttributeName="asofDate")]
		public string AsofDate { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="name")]
	public class Name {
		[XmlAttribute(AttributeName="asofDate")]
		public string AsofDate { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="quantity")]
	public class Quantity {
		[XmlAttribute(AttributeName="asofDate")]
		public string AsofDate { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="currency")]
	public class Currency {
		[XmlAttribute(AttributeName="asofDate")]
		public string AsofDate { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Owner")]
	public class Owner {
		[XmlElement(ElementName="type")]
		public string Type { get; set; }
		[XmlElement(ElementName="name")]
		public Name Name { get; set; }
		[XmlElement(ElementName="quantity")]
		public Quantity Quantity { get; set; }
		[XmlElement(ElementName="currency")]
		public Currency Currency { get; set; }
		[XmlAttribute(AttributeName="ownerId")]
		public string OwnerId { get; set; }
	}

	[XmlRoot(ElementName="OwnershipDetails")]
	public class OwnershipDetails {
		[XmlElement(ElementName="ISIN")]
		public string ISIN { get; set; }
		[XmlElement(ElementName="floatShares")]
		public FloatShares FloatShares { get; set; }
		[XmlElement(ElementName="Owner")]
		public List<Owner> Owner { get; set; }
	}

}
