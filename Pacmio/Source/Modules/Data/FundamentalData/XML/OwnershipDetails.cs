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

namespace Reuters.Ownership
{
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
    public class OwnershipDetails
    {
        [XmlElement(ElementName = "ISIN")]
        public string ISIN { get; set; }

        [XmlElement(ElementName = "floatShares")]
        public FloatShares FloatShares { get; set; }

        [XmlElement(ElementName = "Owner")]
        public List<Owner> Owner { get; set; }
    }
}
