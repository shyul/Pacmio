/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        internal static bool IsReady_Position => Connected && !IsBusy_Position;

        private static bool IsBusy_Position { get; set; } = false;

        /// <summary>
        /// Subscribes to position updates for all accessible accounts.
        /// All positions sent initially, and then only updates as positions change.
        /// Send RequestPositions: (0)"61"-(1)"1"-
        /// </summary>
        internal static void SendRequest_Position()
        {
            if (IsReady_Position) // !IsActiveAccountSummary &&
            {
                IsBusy_Position = true;
                AccountPositionManager.ResetAllPositionRefreshStatus();
                SendRequest(new string[] { RequestType.RequestPositions.Param(), "1" });
            }
            else
                throw new Exception("We are still busy with last position request.");
        }

        /// <summary>
        /// Received Position: (0)"61"-(1)"3"-(2)"U1564932"-(3)"107113386"-(4)"FB"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-
        /// (10)"NASDAQ"-(11)"USD"-(12)"FB"-(13)"NMS"-(14)"162"-(15)"184.27472345"-
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_Position(string[] fields)
        {
            int msgVersion = fields[1].ToInt32(-1);
            if (msgVersion == 3)
            {
                (bool contractValid, Contract c) = Util.GetContractByIbCode(fields[4], fields[10], fields[5], fields[3]);

                if (contractValid)
                {
                    string accountId = fields[2];
                    AccountInfo ac = AccountPositionManager.GetOrCreateAccountById(accountId);
                    c.Status = ContractStatus.Alive;
                    double totalQuantity = fields[14].ToDouble();
                    if (totalQuantity != 0)
                    {
                        double averagePrice = fields[15].ToDouble();
                        PositionInfo ps = ac.GetOrCreatePositionByContract(c);
                        ps.Set(totalQuantity, averagePrice);
                    }
                }

                Console.WriteLine("Parse Poistion | " + c?.ToString() + " | " + fields.ToStringWithIndex());
            }
        }

        /// <summary>
        /// Received PositionEnd: (0)"62"-(1)"1"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_PositionEnd(string[] fields)
        {
            /*
            if (fields[1] == "1")
            {

            }*/

            AccountPositionManager.ResetNonRefreshedPositions();

            IsBusy_Position = false;

            Console.WriteLine("Parse Poistion Ended | " + fields.ToStringWithIndex());
        }

        //
        //Received Position: (0)"61"-(1)"3"-(2)"U1564932"-(3)"107113386"-(4)"FB"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-(10)"NASDAQ"-(11)"USD"-(12)"FB"-(13)"NMS"-(14)"162"-(15)"184.27472345"-
        //Received PositionEnd: (0)"62"-(1)"1"-

        /*
        Send RequestPositions: (0)"61"-(1)"1"-
        Received Position: (0)"61"-(1)"3"-(2)"U1564932"-(3)"107113386"-(4)"FB"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-(10)"NASDAQ"-(11)"USD"-(12)"FB"-(13)"NMS"-(14)"162"-(15)"184.27472345"-
        Received PositionEnd: (0)"62"-(1)"1"-
        Send RequestFamilyCodes: (0)"80"-
        Received FamilyCodes: (0)"78"-(1)"3"-(2)"U3372058"-(3)""-(4)"U1564932"-(5)""-(6)"U2234582"-
        Received Position: (0)"61"-(1)"3"-(2)"U1564932"-(3)"107113386"-(4)"FB"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-(10)"NASDAQ"-(11)"USD"-(12)"FB"-(13)"NMS"-(14)"162"-(15)"184.27472345"-
        Received AccountSummary: (0)"63"-(1)"1"-(2)"50000001"-(3)"U1564932"-(4)"NetLiquidation"-(5)"112816.44"-(6)"USD"-
        */
    }
}
