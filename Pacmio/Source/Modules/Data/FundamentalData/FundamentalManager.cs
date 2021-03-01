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
    public static class FundamentalManager
    {
        private static Dictionary<(string name, Exchange exchange, string typeName), FundamentalData> ContractFundamentalLUT { get; }
            = new Dictionary<(string name, Exchange exchange, string typeName), FundamentalData>();

        public static FundamentalData GetOrCreateFundamentalData(this Contract c)
        {
            lock (ContractFundamentalLUT)
            {
                if (!ContractFundamentalLUT.ContainsKey(c.Key))
                {
                    ContractFundamentalLUT[c.Key] = FundamentalData.LoadFile(c);
                }

                return ContractFundamentalLUT[c.Key];
            }
        }

        public static FundamentalData GetOrCreateFundamentalData(this (string name, Exchange exchange, string typeName) key)
        {
            lock (ContractFundamentalLUT)
            {
                if (!ContractFundamentalLUT.ContainsKey(key))
                {
                    ContractFundamentalLUT[key] = FundamentalData.LoadFile(key);
                }

                return ContractFundamentalLUT[key];
            }
        }


        // Save All when exiting...

        #region IB Client

        public static Contract IB_RequestContract { get; private set; }

        public static FundamentalRequestType IB_RequestType { get; private set; }

        public static int IB_RequestId { get; private set; }

        #endregion IB Client

        #region Reuters XML

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
            FundamentalRequestType type = GetRequestType<T>();
            FileInfo file = GetXmlFile(contractKey, type);

            if (file.Exists && (DateTime.Now - file.LastWriteTime).TotalDays < 30)
            {
                return Serialization.DeserializeXML<T>(File.ReadAllText(file.FullName));
            }
            else
                return default(T);
        }

        private static FundamentalRequestType GetRequestType<T>()
        {
            return nameof(T) switch
            {
                nameof(Reuters.CompanyOverview.ReportSnapshot) => FundamentalRequestType.CompanyOverview,
                nameof(Reuters.FinancialSummary.FinancialSummary) => FundamentalRequestType.FinancialSummary,
                nameof(Reuters.FinancialStatements.ReportFinancialStatements) => FundamentalRequestType.FinancialStatements,
                nameof(Reuters.AnalystEstimates.REarnEstCons) => FundamentalRequestType.AnalystEstimates,
                nameof(Reuters.Calendar.WSHData) => FundamentalRequestType.Calendar,
                nameof(Reuters.Ownership.OwnershipDetails) => FundamentalRequestType.Ownership,
                _ => throw new Exception("Unsupported Type!")
            };
        }

        private static string GetXmlFileName((string name, Exchange exchange, string typeName) contractKey, FundamentalRequestType type)
            => Root.HistoricalDataPath(contractKey) + "\\_FundamentalData\\$" + contractKey.name + "_" + type.ToString() + ".xml";

        private static FileInfo GetXmlFile((string name, Exchange exchange, string typeName) contractKey, FundamentalRequestType type)
            => new FileInfo(GetXmlFileName(contractKey, type));

        #endregion Reuters XML
    }
}
