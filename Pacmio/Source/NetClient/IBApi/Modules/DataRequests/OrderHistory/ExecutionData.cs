/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_ExecutionData => Connected && requestId_ExecutionData == -1;
        private static int requestId_ExecutionData = -1;

        internal static void SendRequest_ExecutionData()
        {
            if (IsReady_ExecutionData)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestExecutions);
                requestId_ExecutionData = requestId;

                SendRequest(new string[] {
                    requestType,
                    "3",
                    requestId.ToString(),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                });
            }
        }

        /// <summary>
        /// 
        /// Parse Execution Data: (0)"11"-(1)"-1"-(2)"1"-(3)"756733"-(4)"SPY"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-
        /// (10)"DRCTEDGE"-(11)"USD"-(12)"SPY"-(13)"SPY"-(14)"0000e22a.5eac83de.01.01"-
        /// (15)"20200427  10:50:17"-(16)"DU332281"-(17)"DRCTEDGE"-(18)"BOT"-(19)"5"-(20)"286.26"-(21)"130443181"-
        /// (22)"182"-(23)"0"-(24)"5"-(25)"286.26"-(26)""-(27)""-(28)""-(29)""-(30)"2"
        /// 
        /// Received ExecutionData: (0)"11"-(1)"1"-(2)"17"-(3)"3691937"-(4)"AMZN"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-
        /// (10)"ARCA"-(11)"USD"-(12)"AMZN"-(13)"NMS"-(14)"0000e0d5.5ea9265d.01.01"-
        /// (15)"20200428  08:52:58"-(16)"DU332281"-(17)"ARCA"-(18)"SLD"-(19)"9"-(20)"2319.17"-(21)"1422412007"-
        /// (22)"100"-(23)"0"-(24)"9"-(25)"2319.17"-(26)""-(27)""-(28)""-(29)""-(30)"2"
        /// 
        /// Received ExecutionData: (0)"11"-(1)"1"-(2)"0"-(3)"269753"-(4)"GILD"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-
        /// (10)"ARCA"-(11)"USD"-(12)"GILD"-(13)"NMS"-(14)"0000e0d5.5ea93d29.01.01"-
        /// (15)"20200428  09:28:04"-(16)"DU332281"-(17)"ARCA"-(18)"SLD"-(19)"100"-(20)"78.39"-(21)"975677963"-
        /// (22)"0"-(23)"0"-(24)"100"-(25)"78.39"-(26)"Xu's BookTrader"-(27)""-(28)""-(29)""-(30)"2"
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_ExecutionData(string[] fields)
        {
            // int requestId = fields[1].ToInt32();
            string execId = fields[14];
            TradeInfo ti = TradeManager.GetOrCreateTradeByExecId(execId);

            ti.OrderId = fields[2].ToInt32();
            ti.ConId = fields[3].ToInt32();

            ti.PermId = fields[21].ToInt32();
            ti.ClientId = fields[22].ToInt32();
            ti.Description = fields[26];
            ti.AccountId = fields[16];
            ti.FillExchangeCode = fields[17];

            double quantity = fields[19].ToDouble();
            double totalQuantity = fields[24].ToDouble();

            if (fields[18] == "SLD")
            {
                quantity = -quantity;
                totalQuantity = -totalQuantity;
            }

            ti.Quantity = quantity;
            ti.Price = fields[20].ToDouble();

            ti.TotalQuantity = totalQuantity;
            ti.AveragePrice = fields[25].ToDouble();

            ti.ModeCode = fields[29];
            ti.Liquidation = fields[23].ToInt32();

            // TODO: Check ExecutionData LiquidityType

            if (ti.Description == OrderManager.EntryOrderDescription)
                ti.LastLiquidity = LiquidityType.Added;
            else if (ti.Description == OrderManager.StopLossOrderDescription ||
                    ti.Description == OrderManager.ProfitTakerOrderDescription)
                ti.LastLiquidity = LiquidityType.Removed;
            else
                ti.LastLiquidity = (LiquidityType)fields[30].ToInt32();

            ti.ExecuteTime = ti.Contract is Contract c ? Util.ParseTime(fields[15], c.TimeZone) : Util.ParseTime(fields[15], TimeZoneInfo.Local);

            Console.WriteLine("fields[15] = " + fields[15] + " | ti.ExecuteTime = " + ti.ExecuteTime + " | " + (DateTime.Now - ti.ExecuteTime).TotalSeconds + " Second ago... ");

            string evRule = fields[27];
            double evMultiplier = fields[28].ToDouble();

            // TODO: Emit trade log is updated so the BarTable / BarChart with bonding strategy / and Contract Position can be updated too
            //TradeLogManager.Update(fields[0].ToInt32(-1), execId);

            TradeManager.DataProvider.DataIsUpdated();

            Console.WriteLine("\nParse Execution Data | " + evRule + " | " + evMultiplier + " : " + fields.ToStringWithIndex());
        }

        private static void Parse_ExecutionDataEnd(string[] fields)
        {
            string msgVersion = fields[1];
            int requestId = fields[2].ToInt32();
            //if (requestId == requestId_ExecutionData) 
            requestId_ExecutionData = -1;

            //TradeLogManager.Update(fields[0].ToInt32(-1), requestId.ToString());

            Console.WriteLine("\nParse Execution Data End | " + msgVersion + ": " + fields.ToStringWithIndex());
        }

        /// <summary>
        /// 59: (0)"59"-(1)"1"-(2)"0000e22a.5eac83de.01.01"-(3)"0.364507"-(4)"USD"-(5)"1.7976931348623157E308"-(6)"1.7976931348623157E308"
        /// Received CommissionsReport: (0)"59"-(1)"1"-(2)"0000e0d5.5ea9265d.01.01"-(3)"0.841411"-(4)"USD"-(5)"958.590689"-(6)"1.7976931348623157E308"
        /// Commissions Report | USD | 1.79769313486232E+308 | 0 : (0)"59"-(1)"1"-(2)"0000e0d5.5efbe8f4.01.01"-(3)"0.353064"-(4)"USD"-(5)"1.7976931348623157E308"-(6)"1.7976931348623157E308"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_CommissionsReport(string[] fields)
        {
            string execId = fields[2];
            TradeInfo ti = TradeManager.GetOrCreateTradeByExecId(execId);

            ti.Commissions = fields[3].ToDouble();

            if (ti.Position is PositionInfo ps)
            {
                double previousAverageComm = ps.AverageCommissionPerUnit;
                ps.AverageCommissionPerUnit = double.IsNaN(previousAverageComm) ? ti.AverageCommissionPerUnit : (previousAverageComm + ti.AverageCommissionPerUnit) / 2;
            }

            string pnlstring = fields[5];
            ti.RealizedPnL = pnlstring.Contains("1.7976931348623157E308") ? 0 : pnlstring.ToDouble();

            //TradeLogManager.Update(fields[0].ToInt32(-1), execId);

            string currency = fields[4];
            double yield = fields[6].ToDouble();
            int yieldRedemptionDate = 0;
            if (fields.Length == 8)
            {
                yieldRedemptionDate = fields[7].ToInt32();
            }
            Console.WriteLine("\nCommissions Report | " + currency + " | " + yield + " | " + yieldRedemptionDate + " : " + fields.ToStringWithIndex());
            TradeManager.DataProvider.DataIsUpdated();
        }
    }
}
