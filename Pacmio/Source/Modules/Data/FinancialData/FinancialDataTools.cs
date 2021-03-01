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

        public static Contract IB_RequestContract { get; private set; }

        public static FinancialDataRequestType IB_RequestType { get; private set; }

        public static int IB_RequestId { get; private set; }

        #endregion IB Client

        private static void ApplyDataCompanyOverview(this Contract c, Reuters.CompanyOverview.ReportSnapshot data)
        {

        }

        private static void ApplyDataFinancialSummary(this Contract c, Reuters.FinancialSummary.FinancialSummary data)
        {

        }

        private static void ApplyDataFinancialStatements(this Contract c, Reuters.FinancialStatements.ReportFinancialStatements data)
        {

        }

        private static void ApplyDataAnalystEstimates(this Contract c, Reuters.AnalystEstimates.REarnEstCons data)
        {

        }

        private static void ApplyDataCalendar(this Contract c, Reuters.Calendar.WSHData data)
        {

        }

        private static void ApplyDataOwnership(this Contract c, Reuters.Ownership.OwnershipDetails data)
        {

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
            => new FileInfo(GetXmlFileName(contractKey, type));
    }
}
