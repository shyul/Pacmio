/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Xu;

namespace Pacmio
{
    public static class FinancialDataTools
    {
        #region IB Client

        public static Contract IB_RequestContract { get; set; } = null;

        public static FinancialDataRequestType IB_RequestType { get; set; }

        #endregion IB Client

        public static void ApplyData(this Contract c, FinancialDataRequestType type, string xml, bool writeFile = false) 
        {
            if(writeFile) File.WriteAllText(GetXmlFileName(c.Key, IB_RequestType), xml);
            switch (type) 
            {
                case FinancialDataRequestType.CompanyOverview:
                    c.ApplyDataCompanyOverview(xml);
                    break;
                case FinancialDataRequestType.FinancialSummary:
                    c.ApplyDataFinancialSummary(xml);
                    break;
                case FinancialDataRequestType.FinancialStatements:
                    c.ApplyDataFinancialStatements(xml);
                    break;
                case FinancialDataRequestType.Calendar:
                    c.ApplyDataCalendar(xml);
                    break;
                case FinancialDataRequestType.AnalystEstimates:
                    c.ApplyDataAnalystEstimates(xml);
                    break;
                case FinancialDataRequestType.Ownership:
                    c.ApplyDataOwnership(xml);
                    break;
            }
        }

        private static void ApplyDataCompanyOverview(this Contract c, string xml)
        {
            Reuters.CompanyOverview.ReportSnapshot data = Serialization.DeserializeXML<Reuters.CompanyOverview.ReportSnapshot>(xml);
        }

        private static void ApplyDataFinancialSummary(this Contract c, string xml)
        {
            Reuters.FinancialSummary.FinancialSummary data = Serialization.DeserializeXML<Reuters.FinancialSummary.FinancialSummary>(xml);
        }

        private static void ApplyDataFinancialStatements(this Contract c, string xml)
        {
            Reuters.FinancialStatements.ReportFinancialStatements data = Serialization.DeserializeXML<Reuters.FinancialStatements.ReportFinancialStatements>(xml);
        }

        private static void ApplyDataAnalystEstimates(this Contract c, string xml)
        {
            Reuters.AnalystEstimates.REarnEstCons data = Serialization.DeserializeXML<Reuters.AnalystEstimates.REarnEstCons>(xml);
        }

        private static void ApplyDataCalendar(this Contract c, string xml)
        {
            Reuters.Calendar.WSHData data = Serialization.DeserializeXML<Reuters.Calendar.WSHData>(xml);
        }

        private static void ApplyDataOwnership(this Contract c, string xml)
        {
            Reuters.Ownership.OwnershipDetails data = Serialization.DeserializeXML<Reuters.Ownership.OwnershipDetails>(xml);
        }

        private static T GetFileData<T>((string name, Exchange exchange, string typeName) contractKey)
        {
            FinancialDataRequestType type = GetRequestType<T>();
            FileInfo file = GetXmlFile(contractKey, type);

            if (file.Exists && (DateTime.Now - file.LastWriteTime).TotalDays < 30)
            {
                return Serialization.DeserializeXML<T>(File.ReadAllText(file.FullName));
            }
            else
                return default(T);
        }

        private static FinancialDataRequestType GetRequestType<T>()
        {
            return nameof(T) switch
            {
                nameof(Reuters.CompanyOverview.ReportSnapshot) => FinancialDataRequestType.CompanyOverview,
                nameof(Reuters.FinancialSummary.FinancialSummary) => FinancialDataRequestType.FinancialSummary,
                nameof(Reuters.FinancialStatements.ReportFinancialStatements) => FinancialDataRequestType.FinancialStatements,
                nameof(Reuters.AnalystEstimates.REarnEstCons) => FinancialDataRequestType.AnalystEstimates,
                nameof(Reuters.Calendar.WSHData) => FinancialDataRequestType.Calendar,
                nameof(Reuters.Ownership.OwnershipDetails) => FinancialDataRequestType.Ownership,
                _ => throw new Exception("Unsupported Type!")
            };
        }

        private static string GetXmlFileName((string name, Exchange exchange, string typeName) contractKey, FinancialDataRequestType type)
            => Root.HistoricalDataPath(contractKey) + "\\_FinancialData\\$" + contractKey.name + "_" + type.ToString() + ".xml";

        private static FileInfo GetXmlFile((string name, Exchange exchange, string typeName) contractKey, FinancialDataRequestType type)
            => new(GetXmlFileName(contractKey, type));
    }
}
