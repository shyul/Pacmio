/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// https://interactivebrokers.github.io/tws-api/tick_types.html
/// 
/// https://interactivebrokers.github.io/tws-api/md_receive.html
/// 
/// For bid, ask, and last data, there will always be a IBApi::EWrapper::tickSize callback following each IBApi::EWrapper::tickPrice. 
/// This is important because with combo contracts, an actively trading contract can have a price of zero. 
/// In this case it will have a positive IBApi::EWrapper::tickSize value. 
/// IBApi::EWrapper::tickSize is also invoked every time there is a change in the size of the last traded price. 
/// For that reason, there will be duplicate tickSize events when both the price and size of a trade changes.
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /*
            Send MarketDataRequest: (0)"1"-(1)"11"-(2)"10000001"-(3)"0"-(4)"FB"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"0"-(16)""-(17)"0"-(18)"0"
            Send MarketDataRequest: (0)"1"-(1)"11"-(2)"10000001"-(3)"0"-(4)"FB"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"0"-(16)""-(17)"0"-(18)"0"
            
            Received TickReqParams: (0)"81"-(1)"10000001"-(2)"0.01"-(3)"9c"-(4)"3"
            Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"1"-(4)"-1.00"-(5)"0"-(6)"1"
            Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"2"-(4)"-1.00"-(5)"0"-(6)"1"
            Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"9"-(4)"219.76"-(5)"0"-(6)"0"

            Send MarketDataRequest: (0)"1"-(1)"2"-(2)"88"
            Received ManagedAccounts: (0)"15"-(1)"1"-(2)"DU332281"
            Received NextValidId: (0)"9"-(1)"1"-(2)"1"

            Received MarketDataType: (0)"58"-(1)"1"-(2)"10000001"-(3)"1"
            Received TickReqParams: (0)"81"-(1)"10000001"-(2)"0.01"-(3)"9c0001"-(4)"3"
            Received Tickstring: (0)"46"-(1)"6"-(2)"10000001"-(3)"45"-(4)"1580146404"

            Received Error: (0)"4"-(1)"2"-(2)"10000001"-(3)"10197"-(4)"No market data during competing live session"
        */

        /// <summary>
        /// Received MarketDataType: (0)"58"-(1)"1"-(2)"10000001"-(3)"1"
        /// Live            1	Live market data is streaming data relayed back in real time. Market data subscriptions are required to receive live market data.
        /// Frozen          2	Frozen market data is the last data recorded at market close.In TWS, Frozen data is displayed in gray numbers.When you set the market data type to Frozen, you are asking TWS to send the last available quote when there is not one currently available. For instance, if a market is currently closed and real time data is requested, -1 values will commonly be returned for the bid and ask prices to indicate there is no current bid/ask data available.TWS will often show a 'frozen' bid/ask which represents the last value recorded by the system. To receive the last know bid/ask price before the market close, switch to market data type 2 from the API before requesting market data.API frozen data requires TWS/IBG v.962 or higher and the same market data subscriptions necessary for real time streaming data.
        /// Delayed         3	Free, delayed data is 15 - 20 minutes delayed. In TWS, delayed data is displayed in brown background. When you set market data type to delayed, you are telling TWS to automatically switch to delayed market data if the user does not have the necessary real time data subscription.If live data is available a request for delayed data would be ignored by TWS.Delayed market data is returned with delayed Tick Types (Tick ID 66~76).
        ///                 Note: TWS Build 962 or higher is required and API version 9.72.18 or higher is suggested.
        /// Delayed Frozen  4	Requests delayed "frozen" data for a user without market data subscriptions.
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_MarketDataType(string[] fields)
        {
            //Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToFlat());
            // Console.WriteLine("\nParse Market Data Type: " + fields.ToFlat());
            string msgVersion = fields[1];
            int requestId = fields[2].ToInt32(-1);
            lock (ActiveMarketTicks)
                if (msgVersion == "1" && ActiveMarketTicks.ContainsKey(requestId) && fields.Length == 4)
                {
                    Contract c = ActiveMarketTicks[requestId];
                    if (c is Stock stk) stk.Status = ContractStatus.Alive;

                    c.TickStatus = (MarketTickStatus)fields[3].ToInt32(0);
                    MarketDataManager.UpdateUI(c);
                }
        }

        /// <summary>
        /// TickReqParams:
        /// (0)"81"-(1)"24"-(2)"0.01"-(3)"9c"-(4)"3"-
        /// (0)"81"-(1)"6"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// 
        /// ParseTickReqParams: (0)"81"-(1)"17"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"18"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"19"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"20"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"21"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"22"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"23"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"24"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"25"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"26"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"27"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"28"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"29"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"31"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"32"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"33"-(2)"0.01"-(3)"a60001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"34"-(2)"0.01"-(3)"9c0001"-(4)"3"
        /// ParseTickReqParams: (0)"81"-(1)"30"-(2)"1.0E-4"-(3)""-(4)"3"
        /// bboExchange will be a symbol such as "a6" which can be used to decode the single letter exchange abbreviations
        /// returned to the bidExch, askExch, and lastExch fields by invoking the function IBApi::EClient::reqSmartComponents.
        /// More information about Component Exchanges.
        /// https://interactivebrokers.github.io/tws-api/smart_components.html
        /// client.reqSmartComponents(13002, testImpl.BboExchange);
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickReqParams(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            // Console.WriteLine("\nParse Tick Req Params: " + fields.ToFlat());

            int tickerId = fields[1].ToInt32(-1);
            lock (ActiveMarketTicks)
                if (ActiveMarketTicks.ContainsKey(tickerId) && fields.Length == 5)
                {
                    Contract c = ActiveMarketTicks[tickerId];

                    c.MinimumTick = fields[2].ToDouble(0);
                    c.BBOExchangeId = fields[3];

                    int snapshotPermissions = fields[4].ToInt32(-1);

                    MarketDataManager.UpdateUI(c);
                }
                else
                    SendCancel_MarketTicks(tickerId);
        }

        /// <summary>
        /// TickPrice
        /// (0)"1"-(1)"6"-(2)"24"-(3)"4"-(4)"186.40"-(5)"2"-(6)"0"-
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickPrice(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            lock (ActiveMarketTicks)
                if (msgVersion == "6" && ActiveMarketTicks.ContainsKey(tickerId) && fields.Length == 7)
                {
                    Contract c = ActiveMarketTicks[tickerId];
                    TickType tickType = fields[3].ToTickType();
                    double price = fields[4].ToDouble();
                    double size = fields[5].ToDouble();
                    //int attrMask = fields[6].ToInt32(-1);

                    if (price < 0) c.TickStatus = MarketTickStatus.Unknown;

                    switch (tickType)
                    {
                        case TickType.BidPrice when c is IBidAsk q:
                            q.Bid = price;
                            q.BidSize = size * 100;
                            break;

                        case TickType.AskPrice when c is IBidAsk q:
                            q.Ask = price;
                            q.AskSize = size * 100;
                            break;

                        case TickType.LastPrice when c is IBidAsk q:
                            q.Last = price;
                            q.LastSize = size * 100;
                            break;

                        case TickType.Open when c is IBidAsk q:
                            q.Open = price;
                            break;

                        case TickType.High when c is IBidAsk q:
                            q.High = price;
                            break;

                        case TickType.Low when c is IBidAsk q:
                            q.Low = price;
                            break;

                        case TickType.LastClose when c is IBidAsk q:
                            q.LastClose = price;
                            break;

                        default:
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | " + tickType + ": " + fields.ToStringWithIndex());
                            break;
                    }

                    MarketDataManager.UpdateUI(c);
                }
        }

        /// <summary>
        /// TickSize: (0)"2"-(1)"6"-(2)"10000001"-(3)"8"-(4)"69980"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickSize(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            lock (ActiveMarketTicks)
                if (msgVersion == "6" && ActiveMarketTicks.ContainsKey(tickerId) && fields.Length == 5)
                {
                    Contract c = ActiveMarketTicks[tickerId];
                    TickType tickType = fields[3].ToTickType();
                    double size = fields[4].ToDouble();

                    switch (tickType)
                    {
                        case TickType.BidSize when c is IBidAsk q:
                            q.BidSize = size * 100;
                            break;

                        case TickType.AskSize when c is IBidAsk q:
                            q.AskSize = size * 100;
                            break;

                        case TickType.LastSize when c is IBidAsk q:
                            q.LastSize = size * 100;
                            break;

                        case TickType.Volume when c is IBidAsk q:
                            q.Volume = size * 100;
                            break;

                        case TickType.ShortableShares when c is Stock q:
                            q.ShortableShares = size;
                            break;

                        default:
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
                            // Console.WriteLine("\nParse Tick Size: " + fields.ToFlat());
                            break;
                    }

                    MarketDataManager.UpdateUI(c);
                }
        }

        /// <summary>
        /// Tickstring
        /// (0)"46"-(1)"6"-(2)"24"-(3)"84"-(4)"Q"-
        /// Smart Components
        /// A: AMEX
        /// B: BEX
        /// C: NYSENAT
        /// D: FINRA
        /// J: EDGEA
        /// K: DRCTEDGE
        /// M: CHX
        /// N: NYSE
        /// P: ARCA
        /// Q: ISLAND
        /// T: ISLAND
        /// V: IEX
        /// X: PSX
        /// Y: BYX
        /// Z: BATS
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickString(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            lock (ActiveMarketTicks)
                if (msgVersion == "6" && ActiveMarketTicks.ContainsKey(tickerId) && fields.Length == 5)
                {
                    Contract c = ActiveMarketTicks[tickerId];
                    TickType tickType = fields[3].ToTickType();

                    switch (tickType)
                    {
                        case TickType.AskExchange when c is IBidAsk q:
                            q.AskExchange = fields[4];
                            break;

                        case TickType.BidExchange when c is IBidAsk q:
                            q.BidExchange = fields[4];
                            break;

                        case TickType.LastExchange when c is IBidAsk q:
                            q.LastExchange = fields[4];
                            break;

                        case TickType.LastTimestamp:
                            long epoch = fields[4].ToInt64(0); // (3)"45"-(4)"1580146441"
                            c.LastTradeTime = TimeTool.FromEpoch(epoch);
                            break;

                        /*
                            The RT Volume tick type corresponds to the TWS' Time & Sales window and contains the
                                1. last trade's price, 2. size and 
                                3. time along with 
                                4. current day's total traded volume, 
                                5. Volume Weighted Average Price (VWAP) and 
                                6. whether or not the trade was filled by a single market maker.
                            There is a new setting available starting in TWS v969 which displays tick-by-tick data in the TWS Time & Sales Window.
                            If this setting is checked, it will provide a higher granularity of data than RTVolume.

                            Example: 701.28;1;1348075471534;67854;701.46918464;true

                            As volume for US stocks is reported in lots, a volume of 0 reported in RTVolume will typically indicate an odd lot data point (less than 100 shares).
                            It is important to note that while the TWS Time & Sales Window also has information about trade conditions available with data points,
                            this data is not available through the API. So for instance, the 'unreportable' trade status displayed with points in the Time & Sales Window is not available through the API,
                            and that trade data will appear in the API just as any other data point.
                            
                            As always, an API application needs to exercise caution in responding to single data points.

                            https://interactivebrokers.github.io/tws-api/tick_types.html#rt_trd_volume
                        */
                        case TickType.RTVolumeTimeSales:
                            Console.WriteLine("RTVolumeTimeSales: " + fields.ToStringWithIndex());
                            break;

                        /*
                            The RT Trade Volume is similar to RT Volume, but designed to avoid relaying back "Unreportable Trades" shown in TWS Time&Sales via the API.
                            RT Trade Volume will not contain average price or derivative trades which are included in RTVolume.
                        */
                        case TickType.RTTradeVolume:
                            string[] volFields = fields[4].Split(';');

                            double last = volFields[0].ToDouble();
                            if (double.IsNaN(last) && c is IBidAsk iba) last = iba.Last;

                            double vol = volFields[1].ToDouble() * 100;
                            if (vol == 0) vol = 20;

                            long epochMs = volFields[2].ToInt64();
                            DateTime time = TimeZoneInfo.ConvertTimeFromUtc(TimeTool.Epoch.AddMilliseconds(epochMs), c.TimeZone);

                            //double totalVol = volFields[3].ToDouble() * 100;
                            TableList.Inbound(new MarketTick(c, time, last, vol));
                            /*
                            if (!TicksList.ContainsKey(c)) 
                                TicksList[c] = new StringBuilder();
                            else if(TicksList[c].Length > 16e3)
                            {
                                string path = Root.ResourcePath + "HistoricalBars\\" + c.Type.ToString() + "\\" + c.Exchange.ToString() + "\\Ticks\\";
                                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                                File.WriteAllText(path + "$" + c.Name + ".csv", TicksList[c].ToString());
                            }

                            TicksList[c].AppendLine(string.Join(",", fields));
                            */
                            break;

                        default:
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex()); // Parse Tick String
                            break;
                    }

                    MarketDataManager.UpdateUI(c);
                }
        }


        /// <summary>
        /// TickGeneric
        /// (0)"45"-(1)"6"-(2)"6"-(3)"46"-(4)"3.0"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickGeneric(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            lock (ActiveMarketTicks)
                if (msgVersion == "6" && ActiveMarketTicks.ContainsKey(tickerId) && fields.Length == 5)
                {
                    Contract c = ActiveMarketTicks[tickerId];
                    TickType tickType = fields[3].ToTickType();

                    switch (tickType)
                    {
                        case TickType.Shortable when c is Stock q:
                            q.ShortStatus = fields[4].ToDouble(-1);
                            break;

                        default:
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
                            // Console.WriteLine("\nParse Tick Generic: " + fields.ToFlat());
                            break;
                    }

                    MarketDataManager.UpdateUI(c);
                }
        }

        /// <summary>
        /// Parse Tick EFP
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickEFP(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            // Console.WriteLine("\nParse Tick EFP: " + fields.ToFlat());

            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);
            TickType tickType = fields[3].ToTickType();
            double basisPoints = fields[4].ToDouble(-1);
            string formattedBasisPoints = fields[5];
            double impliedFuturesPrice = fields[6].ToDouble(-1);
            int holdDays = fields[7].ToInt32(-1);
            string futureLastTradeDate = fields[8];
            double dividendImpact = fields[9].ToDouble(-1);
            double dividendsToLastTradeDate = fields[10].ToDouble(-1);


        }

        /// <summary>
        /// Parse Tick Option Computation
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickOptionComputation(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            // Console.WriteLine("\nParse Tick Option Computation: " + fields.ToFlat());

            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);
            TickType tickType = fields[3].ToTickType();
            double impliedVolatility = fields[4].ToDouble(-1);


        }

        /// <summary>
        /// (0)"84"-(1)"2"-(2)"1578657239000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2d715e"-(5)"Bernstein initiated Facebook (FB) coverage with Outperform"-(6)"K:1.00"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickNews(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            int tickerId = fields[1].ToInt32(-1);
            long epoch = fields[2].ToInt64(0); // (3)"45"-(4)"1580146441"



        }
    }
}
