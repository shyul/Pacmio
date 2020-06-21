/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public partial class Client
    {
        public bool IsReady_AccountSummary => Connected && !ActiveRequestContains(RequestType.RequestAccountSummary);

        private const string ACCOUNT_SUMMARY_TAGS = "AccountType,NetLiquidation,TotalCashValue,SettledCash,AccruedCash,BuyingPower,EquityWithLoanValue,PreviousEquityWithLoanValue,"
                             + "GrossPositionValue,ReqTEquity,ReqTMargin,SMA,InitMarginReq,MaintMarginReq,AvailableFunds,ExcessLiquidity,Cushion,FullInitMarginReq,FullMaintMarginReq,FullAvailableFunds,"
                             + "FullExcessLiquidity,LookAheadNextChange,LookAheadInitMarginReq,LookAheadMaintMarginReq,LookAheadAvailableFunds,LookAheadExcessLiquidity,HighestSeverity,DayTradesRemaining,Leverage";

        public bool SendRequest_AccountSummary(string group = "All", string tags = ACCOUNT_SUMMARY_TAGS)
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

        private void Parse_AccountSummary(string[] fields)
        {
            Console.WriteLine(fields[3] + " >> Summary Item: " + fields.ToStringWithIndex());
            if (fields[1] == "1")
            {
                string accountCode = fields[3];
                Account ac = AccountManager.GetOrAdd(accountCode);
                ac.UpdateFields(fields[4], fields[5]);
                AccountManager.UpdatedTime = DateTime.Now;
            }
        }

        private void Parse_AccountSummaryEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            if (fields.Length == 3 && fields[1] == "1")
            {
                int reqId = int.Parse(fields[2]);
                RemoveRequest(reqId);
                AccountManager.Update(1, "Account updated by IBClient");
            }
        }
    }
}
