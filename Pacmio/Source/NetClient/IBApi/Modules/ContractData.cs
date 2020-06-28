/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using Xu;


namespace Pacmio.IB
{
    public static partial class Client
    {
        private static readonly List<Contract> active_ContractData_ResultList = new List<Contract>();

        private static Contract active_ContractData = null;

        private static void SendRequest_ContractData(Contract c, bool includeExpired = false)
        {
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            if (DataRequestReady && valid_exchange)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestContractData);
                DataRequestID = requestId;
                active_ContractData_ResultList.Clear();
                active_ContractData = c;

                string lastTradeDateOrContractMonth = "";
                double strike = 0;
                string right = "";
                string multiplier = "";

                if (c is IOption opt)
                {
                    lastTradeDateOrContractMonth = opt.LastTradeDateOrContractMonth;
                    strike = opt.Strike;
                    right = opt.Right;
                    multiplier = opt.Multiplier;
                }

                SendRequest(new string[] {
                    requestType, // 9
                    "8",
                    requestId.ToString(),
                    c.ConId.Param(), // "0"
                    c.Name,
                    c.TypeCode(),
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // Right
                    multiplier, // Multiplier
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    includeExpired.Param(),
                    string.Empty, // SecIdType
                    string.Empty, // SecId
                });
            }
        }

        /// <summary>
        /// Send RequestContractData: (0)"'9"-(1)"8"-(2)"60000001"-(3)"4762"-(4)"ba"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)""-(11)""-(12)""-(13)""-(14)""-(15)"0"
        /// Send RequestContractData: (0)",9"-(1)"8"-(2)"60000001"-(3)"4762"-(4)"ba"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)""-(12)""-(13)""-(14)""-(15)"0"
        /// Send RequestContractData: (0)""9"-(1)"8"-(2)"60000001"-(3)"4762"-(4)""-(5)""-(6)""-(7)"0"-(8)""-(9)""-(10)""-(11)""-(12)""-(13)""-(14)""-(15)"0"
        /// </summary>
        /// <param name="name"></param>
        /// <param name="secTypeCode"></param>
        /// <param name="conId"></param>
        /// <param name="includeExpired"></param>
        private static void SendRequest_ContractData(int conId, string name = "", string exchangeCode = "", string typeCode = "", bool includeExpired = false)
        {
            if (DataRequestReady)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestContractData);
                DataRequestID = requestId;
                active_ContractData_ResultList.Clear();
                active_ContractData = null;

                SendRequest(new string[] {
                    requestType, // 9
                    "8",
                    requestId.ToString(),
                    conId.Param(), // "0"
                    name, // c.Name,
                    typeCode, // c.Param(),
                    string.Empty, // lastTradeDateOrContractMonth,
                    "0", // (strike == 0) ? "0" : strike.ToString("0.0###"),
                    string.Empty, // Right
                    string.Empty, // Multiplier
                    string.Empty, // exchange,
                    exchangeCode, // primaryExch,
                    string.Empty, // currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    includeExpired.Param(),
                    string.Empty, // SecIdType
                    "0", // SecId
                });
            }
        }

        private static void SendCancel_ContractData()
        {
            RemoveRequest(DataRequestID, true);
            DataRequestID = -1;
        }

        /// <summary>
        /// Received ContractData: (0)"10"-(1)"8"-(2)"60000001"-(3)"FB"-(4)"STK"-(5)""-(6)"0"-(7)""-(8)"SMART"-(9)"USD"-(10)"FB"-(11)"NMS"-(12)"NMS"-(13)"107113386"-(14)"0.01"-(15)"100"-(16)""-
        /// (17)"ACTIVETIM,AD,ADJUST,ALERT,ALGO,ALLOC,AVGCOST,BASKET,BENCHPX,CASHQTY,COND,CONDORDER,DARKONLY,DARKPOLL,DAY,DEACT,DEACTDIS,DEACTEOD,DIS,GAT,GTC,GTD,GTT,HID,IBKRATS,ICE,IMB,IOC,LIT,LMT,LOC,MIDPX,MIT,MKT,MOC,MTL,NGCOMB,NODARK,NONALGO,OCA,OPG,OPGREROUT,PEGBENCH,POSTONLY,PREOPGRTH,PRICECHK,REL,RPI,RTH,SCALE,SCALEODD,SCALERST,SIZECHK,SMARTSTG,SNAPMID,SNAPMKT,SNAPREL,STP,STPLMT,SWEEP,TRAIL,TRAILLIT,TRAILLMT,TRAILMIT,WHATIF"-
        /// (18)"SMART,AMEX,NYSE,CBOE,PHLX,ISE,CHX,ARCA,ISLAND,DRCTEDGE,BEX,BATS,EDGEA,CSFBALGO,JEFFALGO,BYX,IEX,EDGX,FOXRIVER,TPLUS1,NYSENAT,PSX"-(19)"1"-(20)"0"-(21)"FACEBOOK INC-CLASS A"-(22)"NASDAQ"-(23)""-
        /// (24)"Communications"-(25)"Internet"-(26)"Internet Content-Entmnt"-
        /// (27)"EST (Eastern Standard Time)"-
        /// (28)"20200623:0400-20200623:2000;20200624:0400-20200624:2000;20200625:0400-20200625:2000;20200626:0400-20200626:2000;20200627:CLOSED;20200628:CLOSED;20200629:0400-20200629:2000;20200630:0400-20200630:2000;20200701:0400-20200701:2000;20200702:0400-20200702:2000;20200703:CLOSED;20200704:CLOSED;20200705:CLOSED;20200706:0400-20200706:2000;20200707:0400-20200707:2000;20200708:0400-20200708:2000;20200709:0400-20200709:2000;20200710:0400-20200710:2000;20200711:CLOSED;20200712:CLOSED;20200713:0400-20200713:2000;20200714:0400-20200714:2000;20200715:0400-20200715:2000;20200716:0400-20200716:2000;20200717:0400-20200717:2000;20200718:CLOSED;20200719:CLOSED;20200720:0400-20200720:2000;20200721:0400-20200721:2000;20200722:0400-20200722:2000;20200723:0400-20200723:2000;20200724:0400-20200724:2000;20200725:CLOSED;20200726:CLOSED;20200727:0400-20200727:2000"-
        /// (29)"20200623:0930-20200623:1600;20200624:0930-20200624:1600;20200625:0930-20200625:1600;20200626:0930-20200626:1600;20200627:CLOSED;20200628:CLOSED;20200629:0930-20200629:1600;20200630:0930-20200630:1600;20200701:0930-20200701:1600;20200702:0930-20200702:1600;20200703:CLOSED;20200704:CLOSED;20200705:CLOSED;20200706:0930-20200706:1600;20200707:0930-20200707:1600;20200708:0930-20200708:1600;20200709:0930-20200709:1600;20200710:0930-20200710:1600;20200711:CLOSED;20200712:CLOSED;20200713:0930-20200713:1600;20200714:0930-20200714:1600;20200715:0930-20200715:1600;20200716:0930-20200716:1600;20200717:0930-20200717:1600;20200718:CLOSED;20200719:CLOSED;20200720:0930-20200720:1600;20200721:0930-20200721:1600;20200722:0930-20200722:1600;20200723:0930-20200723:1600;20200724:0930-20200724:1600;20200725:CLOSED;20200726:CLOSED;20200727:0930-20200727:1600"-
        /// (30)""-(31)""-(32)"1"-(33)"ISIN"-(34)"US30303M1027"-(35)"1"-(36)""-(37)""-(38)"26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26"
        /// Received ContractDataEnd: (0)"52"-(1)"1"-(2)"60000001"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_ContractData(string[] fields) // 10
        {
            //int requestId = fields[2].ToInt32(-1);

            if (fields[1] == "8")// && requestId == DataRequestID)
            {
                string symbolStr = fields[3];
                string conIdStr = fields[13];
                string secTypeCode = fields[4];
                string exchangeStr = fields[22]; // + "." + fields[11];

                (bool contractValid, Contract c) = Util.GetContractByIbCode(symbolStr, exchangeStr, secTypeCode, conIdStr);

                if (contractValid)
                {
                    c.MarketData.OrderTypes.FromString(fields[17], ',');
                    c.MarketData.ValidExchanges.FromString(fields[18], ',');

                    string industry = fields[24];
                    string category = fields[25];
                    string subcategory = fields[26];

                    if (!UnknownItemList.Industry.ContainsKey(industry))
                        UnknownItemList.Industry.Add(industry, new Dictionary<string, HashSet<string>>());

                    if (!UnknownItemList.Industry[industry].ContainsKey(category))
                        UnknownItemList.Industry[industry].Add(category, new HashSet<string>());

                    UnknownItemList.Industry[industry][category].CheckAdd(subcategory);

                    c.MarketData.TradingPeriods.Clear();
                    ApplyTradingPeriods(c.MarketData, fields[28]);
                    ApplyTradingPeriods(c.MarketData, fields[29]);

                    // Get ISIN here.
                    int tagNum = fields[32].ToInt32(0);

                    int pt = 33;

                    Dictionary<string, string> tags = new Dictionary<string, string>();

                    for (int i = 0; i < tagNum; i++)
                    {
                        tags.Add(fields[pt], fields[pt + 1]);
                        pt += 2;
                    }

                    if (c is IBusiness it)
                    {
                        if (tags.ContainsKey("ISIN"))
                        {
                            it.ISIN = tags["ISIN"];

                            //string isin_header = si.ISIN.Substring(0, 2);
                            //if (isin_header == "US")
                            //   si.CUSIP = si.ISIN.Substring(2, 9);
                        }
                    }



                    c.MarketData.MarketRules.FromString(fields[pt + 3], ','); // 38

                    //if (c.FullName.Length < 5)
                        c.FullName = fields[21].Replace("\"", "");

                    c.UpdateTime = DateTime.Now;

                    active_ContractData_ResultList.Add(c);
                }
            }
        }

        private static void ApplyTradingPeriods(MarketData q, string field)
        {
            string[] pd_string_list = field.Split(';');
            foreach (string pd_string in pd_string_list)
            {
                if (!pd_string.ToUpper().Contains("CLOSED")) 
                {
                    string[] pd_string_pair = pd_string.Split('-');
                    Period pd = new Period(DateTime.ParseExact(pd_string_pair[0], "yyyyMMdd:HHmm", CultureInfo.InvariantCulture), DateTime.ParseExact(pd_string_pair[1], "yyyyMMdd:HHmm", CultureInfo.InvariantCulture));
                    q.TradingPeriods.Add(pd);
                }
            }
        }

        private static void Parse_ContractDataEnd(string[] fields) // 52
        {
            int requestId = fields[2].ToInt32(-1);
            //if (requestId != DataRequestID) throw new Exception("DataRequestID miss aligned!");

            if (requestId > -1) RemoveRequest(requestId, false);
            DataRequestID = -1;

            active_ContractData = null;
        }

        private static void ParseError_ContractData(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            string message = fields[4];

            Console.WriteLine(">>>>>>>>> Symbol Data Error: " + fields[2] + ": " + message);

            if (requestId > -1) RemoveRequest(requestId, false);
            DataRequestID = -1;

            if (active_ContractData is Contract c)
            {
                c.Status = ContractStatus.Unknown;
                c.UpdateTime = DateTime.Now;
            }
            active_ContractData = null;
        }
    }
}
