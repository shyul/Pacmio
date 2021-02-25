/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        internal static bool IsReady_AccountSummary => Connected && !ActiveRequestContains(RequestType.RequestAccountSummary);

        private const string ACCOUNT_SUMMARY_TAGS = "AccountType,NetLiquidation,TotalCashValue,SettledCash,AccruedCash,BuyingPower,EquityWithLoanValue,PreviousEquityWithLoanValue,"
                             + "GrossPositionValue,ReqTEquity,ReqTMargin,SMA,InitMarginReq,MaintMarginReq,AvailableFunds,ExcessLiquidity,Cushion,FullInitMarginReq,FullMaintMarginReq,FullAvailableFunds,"
                             + "FullExcessLiquidity,LookAheadNextChange,LookAheadInitMarginReq,LookAheadMaintMarginReq,LookAheadAvailableFunds,LookAheadExcessLiquidity,HighestSeverity,DayTradesRemaining,Leverage";

        internal static bool SendRequest_AccountSummary(string group = "All", string tags = ACCOUNT_SUMMARY_TAGS)
        {
            if (IsReady_AccountSummary)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestAccountSummary);

                SendRequest(new string[] {
                    requestType, // 62
                    "1",
                    requestId.ToString(),
                    group,
                    tags,
                });

                return true;
            }
            return false;
        }

        private static void Parse_AccountSummary(string[] fields)
        {
            Console.WriteLine(fields[3] + " >> Summary Item: " + fields.ToStringWithIndex());
            if (fields[1] == "1")
            {
                string accountId = fields[3];
                AccountInfo ac = AccountPositionManager.GetOrCreateAccountById(accountId);
                ac.IsLive = true;
                ac.UpdateFields(fields[4], fields[5]);
                //AccountPositionManager.AccountDataProvider.DataIsUpdated();
            }
        }

        private static void Parse_AccountSummaryEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            if (fields.Length == 3 && fields[1] == "1")
            {
                int reqId = int.Parse(fields[2]);
                RemoveRequest(reqId);
                AccountPositionManager.AccountDataProvider.Updated();
            }
        }

        /// <summary>
        /// Send RequestFamilyCodes: (0)"80"-
        /// Received FamilyCodes: (0)"78"-(1)"3"-(2)"Uxxxxxx"-(3)""-(4)"Uxxxxxx"-(5)""-(6)"Uxxxxxx"-
        /// </summary>
        /// <param name="fields">respond sequence</param>
        private static void Parse_ManagedAccounts(string[] fields)
        {
            if (fields[1] == "1")
            {
                string[] accountCodes = fields[2].Split(',');

                foreach (string accountId in accountCodes)
                {
                    if (accountId.Length > 0)
                    {
                        AccountInfo ac = AccountPositionManager.GetOrCreateAccountById(accountId);
                        ac.IsLive = true;
                    }
                }
                AccountPositionManager.AccountDataProvider.Updated();
            }
        }

        private static void Parse_AccountValue(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            if (fields[1] == "1")
            {
                /*
                int msgVersion = ReadInt();
                string key = ReadString();
                string value = ReadString();
                string currency = ReadString();
                string accountName = null;
                if (msgVersion >= 2)
                    accountName = ReadString();
                eWrapper.updateAccountValue(key, value, currency, accountName);
                */
            }
        }

        private static void Parse_AccountUpdateTime(string[] fields)
        {
            if (fields[1] == "1")
            {
                AccountPositionManager.AccountDataProvider.Updated();
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            }
        }

        private static void Parse_AccountDownloadEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            if (fields[1] == "1")
            {
                /*
                int msgVersion = ReadInt();
                string account = ReadString();
                eWrapper.accountDownloadEnd(account);
                */
            }
        }
    }
}
