/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// Licensed under the Apache License, Version 2.0
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// ***************************************************************************

using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace IbXmlScanParamSchema
{
	[XmlRoot(ElementName = "Instrument")]
	public class Instrument
	{
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "filters")]
		public string Filters { get; set; }
		[XmlElement(ElementName = "group")]
		public string Group { get; set; }
		[XmlElement(ElementName = "shortName")]
		public string ShortName { get; set; }
		[XmlElement(ElementName = "cloudScanNotSupported")]
		public string CloudScanNotSupported { get; set; }
		[XmlElement(ElementName = "secType")]
		public string SecType { get; set; }
		[XmlElement(ElementName = "nscanSecType")]
		public string NscanSecType { get; set; }
		[XmlElement(ElementName = "featureCodes")]
		public string FeatureCodes { get; set; }
		[XmlElement(ElementName = "context")]
		public string Context { get; set; }
	}

	[XmlRoot(ElementName = "InstrumentList")]
	public class InstrumentList
	{
		[XmlElement(ElementName = "Instrument")]
		public List<Instrument> Instrument { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "Location")]
	public class Location
	{
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "locationCode")]
		public string LocationCode { get; set; }
		[XmlElement(ElementName = "instruments")]
		public string Instruments { get; set; }
		[XmlElement(ElementName = "routeExchange")]
		public string RouteExchange { get; set; }
		[XmlElement(ElementName = "LocationTree")]
		public LocationTree LocationTree { get; set; }
		[XmlElement(ElementName = "rawPriceOnly")]
		public string RawPriceOnly { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
	}

	[XmlRoot(ElementName = "LocationTree")]
	public class LocationTree
	{
		[XmlElement(ElementName = "Location")]
		public List<Location> Location { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ColumnSetRef")]
	public class ColumnSetRef
	{
		[XmlElement(ElementName = "colId")]
		public string ColId { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "display")]
		public string Display { get; set; }
		[XmlElement(ElementName = "displayType")]
		public string DisplayType { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
	}

	[XmlRoot(ElementName = "Column")]
	public class Column
	{
		[XmlElement(ElementName = "colId")]
		public string ColId { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "display")]
		public string Display { get; set; }
		[XmlElement(ElementName = "displayType")]
		public string DisplayType { get; set; }
		[XmlElement(ElementName = "section")]
		public string Section { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "ScannerSortList")]
		public ScannerSortList ScannerSortList { get; set; }
		[XmlElement(ElementName = "width")]
		public string Width { get; set; }
		[XmlElement(ElementName = "alignment")]
		public string Alignment { get; set; }
		[XmlElement(ElementName = "uniqueId")]
		public string UniqueId { get; set; }
		[XmlElement(ElementName = "maxSymbolWidth")]
		public string MaxSymbolWidth { get; set; }
		[XmlElement(ElementName = "overrideBg")]
		public string OverrideBg { get; set; }
		[XmlElement(ElementName = "overrideFg")]
		public string OverrideFg { get; set; }
		[XmlElement(ElementName = "fieldKey")]
		public string FieldKey { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "showPercent")]
		public string ShowPercent { get; set; }
	}

	[XmlRoot(ElementName = "Columns")]
	public class Columns
	{
		[XmlElement(ElementName = "ColumnSetRef")]
		public ColumnSetRef ColumnSetRef { get; set; }
		[XmlElement(ElementName = "Column")]
		public List<Column> Column { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ScanType")]
	public class ScanType
	{
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "scanCode")]
		public string ScanCode { get; set; }
		[XmlElement(ElementName = "instruments")]
		public string Instruments { get; set; }
		[XmlElement(ElementName = "absoluteColumns")]
		public string AbsoluteColumns { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "supportsSorting")]
		public string SupportsSorting { get; set; }
		[XmlElement(ElementName = "respSizeLimit")]
		public string RespSizeLimit { get; set; }
		[XmlElement(ElementName = "snapshotSizeLimit")]
		public string SnapshotSizeLimit { get; set; }
		[XmlElement(ElementName = "searchDefault")]
		public string SearchDefault { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "searchName")]
		public string SearchName { get; set; }
		[XmlElement(ElementName = "delayedAvail")]
		public string DelayedAvail { get; set; }
		[XmlElement(ElementName = "feature")]
		public string Feature { get; set; }
		[XmlElement(ElementName = "settings")]
		public string Settings { get; set; }
		[XmlElement(ElementName = "locationFilter")]
		public string LocationFilter { get; set; }
		[XmlElement(ElementName = "MobileColumns")]
		public MobileColumns MobileColumns { get; set; }
		[XmlElement(ElementName = "reuters")]
		public string Reuters { get; set; }
	}

	[XmlRoot(ElementName = "MobileColumn")]
	public class MobileColumn
	{
		[XmlElement(ElementName = "colId")]
		public string ColId { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "MobileColumns")]
	public class MobileColumns
	{
		[XmlElement(ElementName = "MobileColumn")]
		public List<MobileColumn> MobileColumn { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ScanTypeList")]
	public class ScanTypeList
	{
		[XmlElement(ElementName = "ScanType")]
		public List<ScanType> ScanType { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "Value")]
	public class Value
	{
		[XmlElement(ElementName = "valueCode")]
		public string ValueCode { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
	}

	[XmlRoot(ElementName = "ValueList")]
	public class ValueList
	{
		[XmlElement(ElementName = "Value")]
		public List<Value> Value { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ComboSetting")]
	public class ComboSetting
	{
		[XmlElement(ElementName = "settingCode")]
		public string SettingCode { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "ValueList")]
		public ValueList ValueList { get; set; }
	}

	[XmlRoot(ElementName = "SettingList")]
	public class SettingList
	{
		[XmlElement(ElementName = "ComboSetting")]
		public ComboSetting ComboSetting { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "AbstractField")]
	public class AbstractField
	{
		[XmlElement(ElementName = "code")]
		public string Code { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "varName")]
		public string VarName { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string _VarName { get; set; }
		[XmlElement(ElementName = "dontAllowClearAll")]
		public string DontAllowClearAll { get; set; }
		[XmlElement(ElementName = "acceptNegatives")]
		public string AcceptNegatives { get; set; }
		[XmlElement(ElementName = "dontAllowNegative")]
		public string DontAllowNegative { get; set; }
		[XmlElement(ElementName = "skipNotEditableField")]
		public string SkipNotEditableField { get; set; }
		[XmlElement(ElementName = "fraction")]
		public string Fraction { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string _Type { get; set; }
		[XmlElement(ElementName = "type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "abbrev")]
		public string Abbrev { get; set; }
		[XmlElement(ElementName = "maxValue")]
		public string MaxValue { get; set; }
		[XmlElement(ElementName = "minValue")]
		public string MinValue { get; set; }
		[XmlElement(ElementName = "suffix")]
		public string Suffix { get; set; }
		[XmlElement(ElementName = "ComboValues")]
		public List<ComboValues> ComboValues { get; set; }
		[XmlElement(ElementName = "master")]
		public string Master { get; set; }
		[XmlElement(ElementName = "narrowField")]
		public string NarrowField { get; set; }
		[XmlElement(ElementName = "radioButtons")]
		public string RadioButtons { get; set; }
		[XmlElement(ElementName = "typeAhead")]
		public string TypeAhead { get; set; }
		[XmlElement(ElementName = "CheckBoxTranslator")]
		public List<CheckBoxTranslator> CheckBoxTranslator { get; set; }
		[XmlElement(ElementName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlElement(ElementName = "codeNot")]
		public string CodeNot { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractFieldItem { get; set; }
		[XmlElement(ElementName = "separator")]
		public string Separator { get; set; }
		[XmlElement(ElementName = "ignoreCase")]
		public string IgnoreCase { get; set; }
		[XmlElement(ElementName = "inverseCheckbox")]
		public string InverseCheckbox { get; set; }
		[XmlElement(ElementName = "defaultValue")]
		public string DefaultValue { get; set; }
		[XmlElement(ElementName = "prefix")]
		public string Prefix { get; set; }
		[XmlElement(ElementName = "precision")]
		public string Precision { get; set; }
		[XmlElement(ElementName = "style")]
		public string Style { get; set; }
		[XmlElement(ElementName = "DoubleField")]
		public DoubleField DoubleField { get; set; }
		[XmlElement(ElementName = "default")]
		public string Default { get; set; }
		[XmlElement(ElementName = "displayType")]
		public string DisplayType { get; set; }
		[XmlElement(ElementName = "displayHint")]
		public string DisplayHint { get; set; }
		[XmlElement(ElementName = "linkGroup")]
		public string LinkGroup { get; set; }
		[XmlElement(ElementName = "filters")]
		public string Filters { get; set; }
		[XmlElement(ElementName = "fixedFilters")]
		public string FixedFilters { get; set; }
		[XmlElement(ElementName = "defaultFilters")]
		public string DefaultFilters { get; set; }
	}

	[XmlRoot(ElementName = "RangeFilter")]
	public class RangeFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "histogram")]
		public string Histogram { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public List<AbstractField> AbstractField { get; set; }
		[XmlElement(ElementName = "skipValidation")]
		public string SkipValidation { get; set; }
		[XmlElement(ElementName = "abbrev")]
		public string Abbrev { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "ref")]
		public string Ref { get; set; }
		[XmlElement(ElementName = "volatilityUnits")]
		public string VolatilityUnits { get; set; }
		[XmlElement(ElementName = "reuters")]
		public string Reuters { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
	}

	[XmlRoot(ElementName = "SimpleFilter")]
	public class SimpleFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "histogram")]
		public string Histogram { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractField { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
	}

	[XmlRoot(ElementName = "ComboValue")]
	public class ComboValue
	{
		[XmlElement(ElementName = "default")]
		public string Default { get; set; }
		[XmlElement(ElementName = "syntheticAll")]
		public string SyntheticAll { get; set; }
		[XmlElement(ElementName = "code")]
		public string Code { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ComboValues")]
	public class ComboValues
	{
		[XmlElement(ElementName = "ComboValue")]
		public List<ComboValue> ComboValue { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "EmbeddedComboValue")]
		public List<EmbeddedComboValue> EmbeddedComboValue { get; set; }
	}

	[XmlRoot(ElementName = "CheckBoxTranslator")]
	public class CheckBoxTranslator
	{
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "TripleComboField")]
	public class TripleComboField
	{
		[XmlElement(ElementName = "code")]
		public string Code { get; set; }
		[XmlElement(ElementName = "codeNot")]
		public string CodeNot { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "dontAllowClearAll")]
		public string DontAllowClearAll { get; set; }
		[XmlElement(ElementName = "separator")]
		public string Separator { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "displayHint")]
		public string DisplayHint { get; set; }
	}

	[XmlRoot(ElementName = "TripleComboFilter")]
	public class TripleComboFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "TripleComboField")]
		public TripleComboField TripleComboField { get; set; }
	}

	[XmlRoot(ElementName = "DoubleField")]
	public class DoubleField
	{
		[XmlElement(ElementName = "code")]
		public string Code { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlElement(ElementName = "varName")]
		public string VarName { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string _VarName { get; set; }
		[XmlElement(ElementName = "dontAllowClearAll")]
		public string DontAllowClearAll { get; set; }
		[XmlElement(ElementName = "abbrev")]
		public string Abbrev { get; set; }
		[XmlElement(ElementName = "acceptNegatives")]
		public string AcceptNegatives { get; set; }
		[XmlElement(ElementName = "dontAllowNegative")]
		public string DontAllowNegative { get; set; }
		[XmlElement(ElementName = "default")]
		public string Default { get; set; }
		[XmlElement(ElementName = "skipNotEditableField")]
		public string SkipNotEditableField { get; set; }
		[XmlElement(ElementName = "fraction")]
		public string Fraction { get; set; }
	}

	[XmlRoot(ElementName = "SwitchFilter")]
	public class SwitchFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractField { get; set; }
	}

	[XmlRoot(ElementName = "WeightSliderConfiguration")]
	public class WeightSliderConfiguration
	{
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "position")]
		public string Position { get; set; }
		[XmlElement(ElementName = "displayType")]
		public string DisplayType { get; set; }
		[XmlElement(ElementName = "helpHint")]
		public string HelpHint { get; set; }
		[XmlElement(ElementName = "defaultValue")]
		public string DefaultValue { get; set; }
		[XmlElement(ElementName = "rebalanceValue")]
		public string RebalanceValue { get; set; }
		[XmlElement(ElementName = "minRestrictedValue")]
		public string MinRestrictedValue { get; set; }
		[XmlElement(ElementName = "notifyWhenDraggingStopped")]
		public string NotifyWhenDraggingStopped { get; set; }
	}

	[XmlRoot(ElementName = "WeightSliderConfigurations")]
	public class WeightSliderConfigurations
	{
		[XmlElement(ElementName = "WeightSliderConfiguration")]
		public List<WeightSliderConfiguration> WeightSliderConfiguration { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "TreeFilter")]
	public class TreeFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "WeightSliderConfigurations")]
		public WeightSliderConfigurations WeightSliderConfigurations { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "TripleComboField")]
		public TripleComboField TripleComboField { get; set; }
	}

	[XmlRoot(ElementName = "ConfigItem")]
	public class ConfigItem
	{
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public List<ConfigItem> ConfigItems { get; set; }
	}

	[XmlRoot(ElementName = "ListFilter")]
	public class ListFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "displayHint")]
		public string DisplayHint { get; set; }
		[XmlElement(ElementName = "WeightSliderConfigurations")]
		public WeightSliderConfigurations WeightSliderConfigurations { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractField { get; set; }
	}

	[XmlRoot(ElementName = "InvestmentRuleSortingFilter")]
	public class InvestmentRuleSortingFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractField { get; set; }
	}

	[XmlRoot(ElementName = "FilterList")]
	public class FilterList
	{
		[XmlElement(ElementName = "RangeFilter")]
		public List<RangeFilter> RangeFilter { get; set; }
		[XmlElement(ElementName = "SimpleFilter")]
		public List<SimpleFilter> SimpleFilter { get; set; }
		[XmlElement(ElementName = "TripleComboFilter")]
		public TripleComboFilter TripleComboFilter { get; set; }
		[XmlElement(ElementName = "SwitchFilter")]
		public SwitchFilter SwitchFilter { get; set; }
		[XmlElement(ElementName = "TreeFilter")]
		public TreeFilter TreeFilter { get; set; }
		[XmlElement(ElementName = "ListFilter")]
		public ListFilter ListFilter { get; set; }
		[XmlElement(ElementName = "InvestmentRuleSortingFilter")]
		public InvestmentRuleSortingFilter InvestmentRuleSortingFilter { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "VirtualFilter")]
		public List<VirtualFilter> VirtualFilter { get; set; }
		[XmlElement(ElementName = "EmbeddedFilter")]
		public List<EmbeddedFilter> EmbeddedFilter { get; set; }
	}

	[XmlRoot(ElementName = "ScannerParameter")]
	public class ScannerParameter
	{
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "LayoutComponent")]
	public class LayoutComponent
	{
		[XmlElement(ElementName = "ScannerParameter")]
		public ScannerParameter ScannerParameter { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "filters")]
		public string Filters { get; set; }
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
	}

	[XmlRoot(ElementName = "FilterComponent")]
	public class FilterComponent
	{
		[XmlElement(ElementName = "code")]
		public string Code { get; set; }
		[XmlElement(ElementName = "ComboValue")]
		public ComboValue ComboValue { get; set; }
	}

	[XmlRoot(ElementName = "FilterComponents")]
	public class FilterComponents
	{
		[XmlElement(ElementName = "FilterComponent")]
		public List<FilterComponent> FilterComponent { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "VirtualFilter")]
	public class VirtualFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "displayName")]
		public List<string> DisplayName { get; set; }
		[XmlElement(ElementName = "linkGroup")]
		public string LinkGroup { get; set; }
		[XmlElement(ElementName = "FilterComponents")]
		public FilterComponents FilterComponents { get; set; }
	}

	[XmlRoot(ElementName = "ScannerLayout")]
	public class ScannerLayout
	{
		[XmlElement(ElementName = "instrument")]
		public string Instrument { get; set; }
		[XmlElement(ElementName = "LayoutComponent")]
		public List<LayoutComponent> LayoutComponent { get; set; }
		[XmlElement(ElementName = "FilterList")]
		public FilterList FilterList { get; set; }
	}

	[XmlRoot(ElementName = "ScannerLayoutList")]
	public class ScannerLayoutList
	{
		[XmlElement(ElementName = "ScannerLayout")]
		public List<ScannerLayout> ScannerLayout { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "InstrumentGroup")]
	public class InstrumentGroup
	{
		[XmlElement(ElementName = "group")]
		public string Group { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "secType")]
		public string SecType { get; set; }
		[XmlElement(ElementName = "routeExchange")]
		public string RouteExchange { get; set; }
		[XmlElement(ElementName = "nscanSecType")]
		public string NscanSecType { get; set; }
	}

	[XmlRoot(ElementName = "InstrumentGroupList")]
	public class InstrumentGroupList
	{
		[XmlElement(ElementName = "InstrumentGroup")]
		public List<InstrumentGroup> InstrumentGroup { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "AdvancedFilter")]
	public class AdvancedFilter
	{
		[XmlElement(ElementName = "industryLike")]
		public string IndustryLike { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "MKTCAP")]
		public string MKTCAP { get; set; }
		[XmlElement(ElementName = "usdMarketCapAbove")]
		public string UsdMarketCapAbove { get; set; }
		[XmlElement(ElementName = "avgVolumeAbove")]
		public string AvgVolumeAbove { get; set; }
		[XmlElement(ElementName = "optVolumeAbove")]
		public string OptVolumeAbove { get; set; }
		[XmlElement(ElementName = "marketCapAbove1e6")]
		public string MarketCapAbove1e6 { get; set; }
		[XmlElement(ElementName = "avgUsdVolumeAbove")]
		public string AvgUsdVolumeAbove { get; set; }
		[XmlElement(ElementName = "etfAssetsAbove")]
		public string EtfAssetsAbove { get; set; }
		[XmlElement(ElementName = "mfDividendYieldWAvgAbove")]
		public string MfDividendYieldWAvgAbove { get; set; }
		[XmlElement(ElementName = "mfEPSGrowth5yrAbove")]
		public string MfEPSGrowth5yrAbove { get; set; }
		[XmlElement(ElementName = "mfDividendPayoutRatio5yrBelow")]
		public string MfDividendPayoutRatio5yrBelow { get; set; }
		[XmlElement(ElementName = "mfCalculatedAverageQualityAbove")]
		public string MfCalculatedAverageQualityAbove { get; set; }
		[XmlElement(ElementName = "mfTotalReturnScoreOverallAbove")]
		public string MfTotalReturnScoreOverallAbove { get; set; }
		[XmlElement(ElementName = "mfExpenseScoreOverallAbove")]
		public string MfExpenseScoreOverallAbove { get; set; }
		[XmlElement(ElementName = "btNotional")]
		public string BtNotional { get; set; }
		[XmlElement(ElementName = "btNotionalAmount_no_encode")]
		public string BtNotionalAmount_no_encode { get; set; }
		[XmlElement(ElementName = "btLeverageLong")]
		public string BtLeverageLong { get; set; }
		[XmlElement(ElementName = "btLeverageShort")]
		public string BtLeverageShort { get; set; }
		[XmlElement(ElementName = "btIndex")]
		public string BtIndex { get; set; }
		[XmlElement(ElementName = "btLongPosTopBottom")]
		public string BtLongPosTopBottom { get; set; }
		[XmlElement(ElementName = "btLongPosRank")]
		public string BtLongPosRank { get; set; }
		[XmlElement(ElementName = "btShortPosTopBottom")]
		public string BtShortPosTopBottom { get; set; }
		[XmlElement(ElementName = "btShortPosRank")]
		public string BtShortPosRank { get; set; }
		[XmlElement(ElementName = "btWeight")]
		public string BtWeight { get; set; }
		[XmlElement(ElementName = "btRebalanceFrequency")]
		public string BtRebalanceFrequency { get; set; }
		[XmlElement(ElementName = "Rebalance")]
		public string Rebalance { get; set; }
		[XmlElement(ElementName = "btTestPeriod")]
		public string BtTestPeriod { get; set; }
		[XmlElement(ElementName = "btBenchmark")]
		public string BtBenchmark { get; set; }
		[XmlElement(ElementName = "BT_SORTING_CRITERIA_no_encode")]
		public string BT_SORTING_CRITERIA_no_encode { get; set; }
		[XmlElement(ElementName = "researchProviderIs")]
		public string ResearchProviderIs { get; set; }
		[XmlElement(ElementName = "btOptObj")]
		public string BtOptObj { get; set; }
		[XmlElement(ElementName = "histDividendFrdYieldAbove")]
		public string HistDividendFrdYieldAbove { get; set; }
		[XmlElement(ElementName = "BT_FILTERING_CRITERIA_no_encode")]
		public string BT_FILTERING_CRITERIA_no_encode { get; set; }
		[XmlElement(ElementName = "maxPeRatio")]
		public string MaxPeRatio { get; set; }
		[XmlElement(ElementName = "priceAbove")]
		public string PriceAbove { get; set; }
		[XmlElement(ElementName = "reutEPSChgPct_TTMAbove")]
		public string ReutEPSChgPct_TTMAbove { get; set; }
		[XmlElement(ElementName = "minPeRatio")]
		public string MinPeRatio { get; set; }
		[XmlElement(ElementName = "priceYTDPctAbove")]
		public string PriceYTDPctAbove { get; set; }
		[XmlElement(ElementName = "sort_weights")]
		public string Sort_weights { get; set; }
		[XmlElement(ElementName = "ihInsiderOfFloatPercAbove")]
		public string IhInsiderOfFloatPercAbove { get; set; }
		[XmlElement(ElementName = "reutLongTermDebt2Equity_MRQBelow")]
		public string ReutLongTermDebt2Equity_MRQBelow { get; set; }
		[XmlElement(ElementName = "moodyRatingAbove")]
		public string MoodyRatingAbove { get; set; }
	}

	[XmlRoot(ElementName = "CompressedScan")]
	public class CompressedScan
	{
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "scanCode")]
		public string ScanCode { get; set; }
		[XmlElement(ElementName = "AdvancedFilter")]
		public AdvancedFilter AdvancedFilter { get; set; }
		[XmlElement(ElementName = "reuters")]
		public string Reuters { get; set; }
	}

	[XmlRoot(ElementName = "CompressedScans")]
	public class CompressedScans
	{
		[XmlElement(ElementName = "CompressedScan")]
		public List<CompressedScan> CompressedScan { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "SimilarProductsDefault")]
	public class SimilarProductsDefault
	{
		[XmlElement(ElementName = "instrumentType")]
		public string InstrumentType { get; set; }
		[XmlElement(ElementName = "CompressedScans")]
		public CompressedScans CompressedScans { get; set; }
	}

	[XmlRoot(ElementName = "SimilarProductsDefaults")]
	public class SimilarProductsDefaults
	{
		[XmlElement(ElementName = "SimilarProductsDefault")]
		public SimilarProductsDefault SimilarProductsDefault { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "Filter")]
	public class Filter
	{
		[XmlElement(ElementName = "priceBelow")]
		public string PriceBelow { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "DefaultTickersScan")]
	public class DefaultTickersScan
	{
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "scanCode")]
		public string ScanCode { get; set; }
		[XmlElement(ElementName = "Filter")]
		public Filter Filter { get; set; }
		[XmlElement(ElementName = "instrumentType")]
		public string InstrumentType { get; set; }
		[XmlElement(ElementName = "delayed")]
		public string Delayed { get; set; }
		[XmlElement(ElementName = "secType")]
		public string SecType { get; set; }
		[XmlElement(ElementName = "maxItems")]
		public string MaxItems { get; set; }
	}

	[XmlRoot(ElementName = "DefaultTickersScans")]
	public class DefaultTickersScans
	{
		[XmlElement(ElementName = "DefaultTickersScan")]
		public List<DefaultTickersScan> DefaultTickersScan { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "MainScreenDefaultTickers")]
	public class MainScreenDefaultTickers
	{
		[XmlElement(ElementName = "DefaultTickersScans")]
		public DefaultTickersScans DefaultTickersScans { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ColumnSet")]
	public class ColumnSet
	{
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
	}

	[XmlRoot(ElementName = "ColumnSets")]
	public class ColumnSets
	{
		[XmlElement(ElementName = "ColumnSet")]
		public List<ColumnSet> ColumnSet { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "DefaultSort")]
	public class DefaultSort
	{
		[XmlElement(ElementName = "instruments")]
		public string Instruments { get; set; }
		[XmlElement(ElementName = "scanCodes")]
		public string ScanCodes { get; set; }
	}

	[XmlRoot(ElementName = "DefaultSorts")]
	public class DefaultSorts
	{
		[XmlElement(ElementName = "DefaultSort")]
		public DefaultSort DefaultSort { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ArString")]
	public class ArString
	{
		[XmlElement(ElementName = "String")]
		public List<string> String { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ScannerSort")]
	public class ScannerSort
	{
		[XmlElement(ElementName = "colId")]
		public string ColId { get; set; }
		[XmlElement(ElementName = "direction")]
		public string Direction { get; set; }
		[XmlElement(ElementName = "scanCode")]
		public string ScanCode { get; set; }
	}

	[XmlRoot(ElementName = "ScannerSortList")]
	public class ScannerSortList
	{
		[XmlElement(ElementName = "ScannerSort")]
		public List<ScannerSort> ScannerSort { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "SidecarScannerTemplate")]
	public class SidecarScannerTemplate
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "AdvancedFilter")]
		public AdvancedFilter AdvancedFilter { get; set; }
		[XmlElement(ElementName = "ArString")]
		public List<ArString> ArString { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "predefined")]
		public string Predefined { get; set; }
		[XmlElement(ElementName = "sortReversed")]
		public string SortReversed { get; set; }
		[XmlElement(ElementName = "leftChartDividerLocation")]
		public string LeftChartDividerLocation { get; set; }
		[XmlElement(ElementName = "rightChartDividerLocation")]
		public string RightChartDividerLocation { get; set; }
		[XmlElement(ElementName = "product")]
		public string Product { get; set; }
	}

	[XmlRoot(ElementName = "SidecarScannerTemplateList")]
	public class SidecarScannerTemplateList
	{
		[XmlElement(ElementName = "SidecarScannerTemplate")]
		public List<SidecarScannerTemplate> SidecarScannerTemplate { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "ScannerProductType")]
	public class ScannerProductType
	{
		[XmlElement(ElementName = "secType")]
		public string SecType { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlElement(ElementName = "defaultRegion")]
		public string DefaultRegion { get; set; }
	}

	[XmlRoot(ElementName = "ScannerProductTypeList")]
	public class ScannerProductTypeList
	{
		[XmlElement(ElementName = "ScannerProductType")]
		public List<ScannerProductType> ScannerProductType { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "FieldConfiguration")]
	public class FieldConfiguration
	{
		[XmlElement(ElementName = "colId")]
		public string ColId { get; set; }
		[XmlElement(ElementName = "deleteForbidden")]
		public string DeleteForbidden { get; set; }
		[XmlElement(ElementName = "displaySupported")]
		public string DisplaySupported { get; set; }
	}

	[XmlRoot(ElementName = "FieldsConfigurationList")]
	public class FieldsConfigurationList
	{
		[XmlElement(ElementName = "FieldConfiguration")]
		public List<FieldConfiguration> FieldConfiguration { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "BacktestingToolTemplate")]
	public class BacktestingToolTemplate
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "AdvancedFilter")]
		public AdvancedFilter AdvancedFilter { get; set; }
		[XmlElement(ElementName = "predefined")]
		public string Predefined { get; set; }
		[XmlElement(ElementName = "customNameDefined")]
		public string CustomNameDefined { get; set; }
	}

	[XmlRoot(ElementName = "BacktestingToolTemplateList")]
	public class BacktestingToolTemplateList
	{
		[XmlElement(ElementName = "BacktestingToolTemplate")]
		public List<BacktestingToolTemplate> BacktestingToolTemplate { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "SidecarScannerDefaults")]
	public class SidecarScannerDefaults
	{
		[XmlElement(ElementName = "DefaultSorts")]
		public DefaultSorts DefaultSorts { get; set; }
		[XmlElement(ElementName = "SidecarScannerTemplateList")]
		public SidecarScannerTemplateList SidecarScannerTemplateList { get; set; }
		[XmlElement(ElementName = "ScannerProductTypeList")]
		public ScannerProductTypeList ScannerProductTypeList { get; set; }
		[XmlElement(ElementName = "FieldsConfigurationList")]
		public FieldsConfigurationList FieldsConfigurationList { get; set; }
		[XmlElement(ElementName = "BacktestingToolTemplateList")]
		public BacktestingToolTemplateList BacktestingToolTemplateList { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "InstrumentConfiguration")]
	public class InstrumentConfiguration
	{
		[XmlElement(ElementName = "instrumentType")]
		public string InstrumentType { get; set; }
		[XmlElement(ElementName = "scanCode")]
		public string ScanCode { get; set; }
		[XmlElement(ElementName = "AdvancedFilter")]
		public AdvancedFilter AdvancedFilter { get; set; }
	}

	[XmlRoot(ElementName = "InstrumentConfigurations")]
	public class InstrumentConfigurations
	{
		[XmlElement(ElementName = "InstrumentConfiguration")]
		public InstrumentConfiguration InstrumentConfiguration { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "AdvancedScannerDefaults")]
	public class AdvancedScannerDefaults
	{
		[XmlElement(ElementName = "InstrumentConfigurations")]
		public InstrumentConfigurations InstrumentConfigurations { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "TwsUiConfig")]
	public class TwsUiConfig
	{
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
	}

	[XmlRoot(ElementName = "AbstractFilter")]
	public class AbstractFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "ref")]
		public string Ref { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "histogram")]
		public string Histogram { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "Columns")]
		public Columns Columns { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public List<AbstractField> AbstractField { get; set; }
		[XmlElement(ElementName = "skipValidation")]
		public string SkipValidation { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "varName")]
		public string VarName { get; set; }
		[XmlElement(ElementName = "reuters")]
		public string Reuters { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
	}

	[XmlRoot(ElementName = "EmbeddedComboValue")]
	public class EmbeddedComboValue
	{
		[XmlElement(ElementName = "default")]
		public string Default { get; set; }
		[XmlElement(ElementName = "syntheticAll")]
		public string SyntheticAll { get; set; }
		[XmlElement(ElementName = "AbstractFilter")]
		public AbstractFilter AbstractFilter { get; set; }
		[XmlElement(ElementName = "Column")]
		public Column Column { get; set; }
	}

	[XmlRoot(ElementName = "EmbeddedFilter")]
	public class EmbeddedFilter
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "category")]
		public string Category { get; set; }
		[XmlElement(ElementName = "access")]
		public string Access { get; set; }
		[XmlElement(ElementName = "minTwsBuild")]
		public string MinTwsBuild { get; set; }
		[XmlElement(ElementName = "AbstractField")]
		public AbstractField AbstractField { get; set; }
		[XmlElement(ElementName = "WeightSliderConfigurations")]
		public WeightSliderConfigurations WeightSliderConfigurations { get; set; }
		[XmlElement(ElementName = "ConfigItem")]
		public ConfigItem ConfigItem { get; set; }
		[XmlElement(ElementName = "displayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "displayHint")]
		public string DisplayHint { get; set; }
	}

	[XmlRoot(ElementName = "ScanParameterResponse")]
	public class ScanParameterResponse
	{
		[XmlElement(ElementName = "InstrumentList")]
		public List<InstrumentList> InstrumentList { get; set; }
		[XmlElement(ElementName = "LocationTree")]
		public LocationTree LocationTree { get; set; }
		[XmlElement(ElementName = "ScanTypeList")]
		public ScanTypeList ScanTypeList { get; set; }
		[XmlElement(ElementName = "SettingList")]
		public SettingList SettingList { get; set; }
		[XmlElement(ElementName = "FilterList")]
		public List<FilterList> FilterList { get; set; }
		[XmlElement(ElementName = "ScannerLayoutList")]
		public ScannerLayoutList ScannerLayoutList { get; set; }
		[XmlElement(ElementName = "InstrumentGroupList")]
		public InstrumentGroupList InstrumentGroupList { get; set; }
		[XmlElement(ElementName = "SimilarProductsDefaults")]
		public SimilarProductsDefaults SimilarProductsDefaults { get; set; }
		[XmlElement(ElementName = "MainScreenDefaultTickers")]
		public MainScreenDefaultTickers MainScreenDefaultTickers { get; set; }
		[XmlElement(ElementName = "ColumnSets")]
		public ColumnSets ColumnSets { get; set; }
		[XmlElement(ElementName = "SidecarScannerDefaults")]
		public SidecarScannerDefaults SidecarScannerDefaults { get; set; }
		[XmlElement(ElementName = "AdvancedScannerDefaults")]
		public AdvancedScannerDefaults AdvancedScannerDefaults { get; set; }
		[XmlElement(ElementName = "TwsUiConfig")]
		public TwsUiConfig TwsUiConfig { get; set; }
	}

}
