/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static readonly List<(Account, Contract)> UpdatedPositions = new List<(Account, Contract)>();

        /// <summary>
        /// Subscribes to position updates for all accessible accounts.
        /// All positions sent initially, and then only updates as positions change.
        /// Send RequestPositions: (0)"61"-(1)"1"-
        /// </summary>
        internal static void SendRequest_Postion()
        {
            UpdatedPositions.Clear();
            if (Connected) // !IsActiveAccountSummary &&
            {
                SendRequest(new string[] { RequestType.RequestPositions.Param(), "1" });
            }
        }

        /// <summary>
        /// Received Position: (0)"61"-(1)"3"-(2)"U1564932"-(3)"107113386"-(4)"FB"-(5)"STK"-(6)""-(7)"0.0"-(8)""-(9)""-
        /// (10)"NASDAQ"-(11)"USD"-(12)"FB"-(13)"NMS"-(14)"162"-(15)"184.27472345"-
        /// </summary>
        /// <param name="fields"></param>
        private static void ParsePoistion(string[] fields) 
        {
            int msgVersion = fields[1].ToInt32(-1);
            if(msgVersion == 3) 
            {
                (bool symbolInfoValid, Contract c) = Util.GetSymbolByIbCode(fields[4], fields[10], fields[5], fields[3]);

                if (symbolInfoValid)
                {
                    string accountCode = fields[2];
                    Account ac = AccountManager.GetOrAdd(accountCode);
                    c.Status = ContractStatus.Alive;
                    double totalQuantity = fields[14].ToDouble();
                    if (totalQuantity != 0) 
                    {
                        double averagePrice = fields[15].ToDouble();
                        PositionStatus ps = ac.GetPosition(c);
                        ac.Positions[c].Quantity = totalQuantity;
                        ac.Positions[c].AveragePrice = averagePrice;
                        UpdatedPositions.Add((ac, c));
                    }
                }
            }

            Console.WriteLine("Parse Poistion | " + fields.ToStringWithIndex());
        }

        /// <summary>
        /// Received PositionEnd: (0)"62"-(1)"1"
        /// </summary>
        /// <param name="fields"></param>
        private static void ParsePoistionEnd(string[] fields)
        {
            if(fields[1] == "1") 
            {
                foreach (Account ac in AccountManager.List)
                {
                    foreach(Contract c in ac.Positions.Keys) 
                    {
                        var item = (ac, c);
                        if (!UpdatedPositions.Contains(item)) 
                        {
                            if (ac.Positions.ContainsKey(c))
                            {
                                ac.Positions[c].Quantity = 0;
                                ac.Positions[c].AveragePrice = double.NaN;
                            }
                        }
                    }                       
                }
                UpdatedPositions.Clear();
            }
            AccountManager.Update(2, "Position updated by IBClient");
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
