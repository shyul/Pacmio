/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
using System.Reflection;
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
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            if (msgVersion == "1" && GetMarketData(tickerId) is MarketData md && fields.Length == 4)
            {
                md.Contract.Status = ContractStatus.Alive;
                md.Status = (MarketTickStatus)(fields[3].ToInt32(0));

                md.Update();
                // WatchListManager.UpdateUI(md);
            }
            else
                UnregisterMarketDataRequest(tickerId, true);

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

            int tickerId = fields[1].ToInt32(-1);

            if (GetMarketDataRequestStatus(tickerId) is MarketDataRequestStatus mds && fields.Length == 5)
            {
                mds.MarketData.MinimumTick = fields[2].ToDouble(0);
                mds.BBOExchangeId = fields[3];
                mds.SnapshotPermissions = fields[4].ToInt32(-1);

                // WatchListManager.UpdateUI(mds.MarketData);
                mds.MarketData.Update();
            }
            else
                UnregisterMarketDataRequest(tickerId, true);
        }

        /// <summary>
        /// TickPrice
        /// (0)"1"-(1)"6"-(2)"24"-(3)"4"-(4)"186.40"-(5)"2"-(6)"0"-
        /// Parse_TickPrice | MarkPrice: (0)"1"-(1)"6"-(2)"1"-(3)"37"-(4)"118.51000214"-(5)"0"-(6)"0"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickPrice(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);


            if (msgVersion == "6" && GetMarketData(tickerId) is MarketData md && fields.Length == 7)
            {
                TickType tickType = fields[3].ToTickType();
                double price = fields[4].ToDouble();
                double size = fields[5].ToDouble();
                //int attrMask = fields[6].ToInt32(-1);

                if (!(price >= 0)) md.Status = MarketTickStatus.Unknown;

                switch (tickType)
                {
                    case TickType.BidPrice when md is BidAskData q:
                        q.Bid = price;
                        q.BidSize = size * 100;
                        break;

                    case TickType.AskPrice when md is BidAskData q:
                        q.Ask = price;
                        q.AskSize = size * 100;
                        break;

                    case TickType.LastPrice:
                        md.LastPrice = price;
                        md.LastSize = size * 100;
                        break;

                    case TickType.Open:
                        md.Open = price;
                        break;

                    case TickType.High:
                        md.High = price;
                        break;

                    case TickType.Low:
                        md.Low = price;
                        break;

                    case TickType.LastClose:
                        md.PreviousClose = price;
                        break;

                    case TickType.MarkPrice:
                        md.MarkPrice = price;
                        break;

                    default:
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | " + tickType + ": " + fields.ToStringWithIndex());
                        break;
                }

                md.Update();
                // WatchListManager.UpdateUI(md);
            }
            else
                UnregisterMarketDataRequest(tickerId, true);

        }

        /// <summary>
        /// TickSize: (0)"2"-(1)"6"-(2)"10000001"-(3)"8"-(4)"69980"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickSize(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);


            if (msgVersion == "6" && GetMarketData(tickerId) is MarketData md && fields.Length == 5)
            {
                TickType tickType = fields[3].ToTickType();
                double size = fields[4].ToDouble();

                switch (tickType)
                {
                    case TickType.BidSize when md is BidAskData q:
                        q.BidSize = size * 100;
                        break;

                    case TickType.AskSize when md is BidAskData q:
                        q.AskSize = size * 100;
                        break;

                    case TickType.LastSize when md is BidAskData q:
                        q.LastSize = size * 100;
                        break;

                    case TickType.Volume:
                        md.Volume = size * 100;
                        break;

                    case TickType.ShortableShares when md is StockData q:
                        q.ShortableShares = size;
                        break;

                    default:
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
                        // Console.WriteLine("\nParse Tick Size: " + fields.ToFlat());
                        break;
                }

                md.Update();
                // WatchListManager.UpdateUI(md);
            }
            else
                UnregisterMarketDataRequest(tickerId, true);
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

        // Parse_TickString: (0)"46"-(1)"6"-(2)"1"-(3)"47"-(4)"TTMNPMGN=21.33376;NLOW=52.9275;ACFSHR=3.646175;ALTCL=-99999.99;TTMPRCFPS=27.58134;TTMCFSHR=3.94469;ASFCF=-2922;AEPSNORM=2.97145;TTMRECTURN=17.10003;AATCA=162819;QCSHPS=5.42871;TTMFCF=57657;LATESTADATE=2020-06-27;APTMGNPCT=25.26655;IAD=3.2799999999999994;TTMNIAC=58424;EV_Cur=1949856;QATCA=140065;PR2TANBK=26.69417;TTMFCFSHR=3.25113;NPRICE=112.82;ASICF=56391;REVTRENDGR=7.31487;QSCEX=-1565;PRICE2BK=26.74602;ALSTD=-99999.99;AOTLO=69391;TTMPAYRAT=24.05176;QPR2REV=32.39099;;TTMREVCHG=5.72241;TTMROAPCT=18.2694;QTOTCE=72282;APENORM=37.968;QLTCL=95318;QSFCF=872;TTMROIPCT=25.70568;DIVGRPCT=11.2299;QOTLO=16271;TTMEPSCHG=-71.42684;YIELD=0.7268215;TTMREVPS=15.44208;TTMEBT=68317;ADIV5YAVG=0.614;Frac52Wk=0.7339798;NHIG=137.98;ASCEX=-10495;QTA=317344;TTMGROSMGN=38.18781;QTL=245062;AFPRD=-7819;QCURRATIO=1.46945;TTMREV=273857;TTMINVTURN=46.16855;QCASH=93025;QLSTD=11166;TTMOPMGN=24.51571;TTMPR2REV=1.764841;QSICF=-3600;TTMNIPEREM=426452.6;EPSCHNGYR=18.37973;TTMPRFCFPS=33.46529;TTMPTMGN=24.94623;AREVPS=13.99112;AEBTNORM=65737;ASOPI=63930;NetDebt_I=20348;PRYTDPCTR=49.01821;TTMEBITD=78671;AFEEPSNTM=3.645;EPSTRENDGR=12.99319;QTOTD2EQ=156.8482;QSOPI=13091;QBVPS=4.2182;YLD5YAVG=1.4865;PR13WKPCT=31.4264;PR52WKPCT=110.3173;AROAPCT=15.69236;QTOTLTD=94048;TTMEPSXCLX=3.28931;QPRCFPS=117.66112511613206;QTANBVPS=4.2182;AROIPCT=22.89958;QEBIT=13091;QEBITDA=15843;MKTCAP=1929508;TTMINTCOV=-99999.99;TTMROEPCT=69.24818;TTMREVPERE=1998956;AEPSXCLXOR=2.971447;QFPRD=-441;REVCHNGYR=10.92011;AFPSS=-66116;CURRENCY=USD;EV2EBITDA_Cur=123.0737;PEEXCLXOR=34.29899;QQUICKRATI=1.42772;ASINN=-99999.99;QFPSS=-15891;BETA=1.32555;ANIACNORM=55256;PR1WKPCT=-15.91891;QLTD2EQ=130.9842;QSINN=-99999.99;PR4WKPCT=3.149714;AEBIT=64352"

        private static void Parse_TickString(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            if (msgVersion == "6" && GetMarketData(tickerId) is MarketData md && fields.Length == 5)
            {
                TickType tickType = fields[3].ToTickType();

                switch (tickType)
                {
                    case TickType.AskExchange when md is BidAskData q:
                        q.AskExchange = fields[4];
                        break;

                    case TickType.BidExchange when md is BidAskData q:
                        q.BidExchange = fields[4];
                        break;

                    case TickType.LastExchange:
                        md.LastExchange = fields[4];
                        break;

                    case TickType.LastTimestamp:
                        long epoch = fields[4].ToInt64(0); // (3)"45"-(4)"1580146441"
                        md.LastTradeTime = TimeZoneInfo.ConvertTimeFromUtc(TimeTool.FromEpoch(epoch), md.Contract.TimeZone);
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
                    case TickType.RTVolumeTimeSales when md is StockData sd:
                        //Console.WriteLine("RTVolumeTimeSales: " + fields.ToStringWithIndex());
                        RTVolume(fields[4], sd);
                        break;

                    /*
                        The RT Trade Volume is similar to RT Volume, but designed to avoid relaying back "Unreportable Trades" shown in TWS Time&Sales via the API.
                        RT Trade Volume will not contain average price or derivative trades which are included in RTVolume.
                    */
                    case TickType.RTTradeVolume when md is StockData sd:
                        //Console.WriteLine("RTVolumeTime: " + fields.ToStringWithIndex());
                        RTVolume(fields[4], sd);
                        break;

                    default:
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex()); // Parse Tick String
                        break;
                }

                md.Update();
                // WatchListManager.UpdateUI(md);
            }
            else
                UnregisterMarketDataRequest(tickerId, true);
        }

        //  contains the last trade price, last trade size, last trade time, total volume, VWAP, and single trade flag.
        private static void RTVolume(string field4, StockData sd)
        {
            string[] fields = field4.Split(';');
            double last = fields[0].ToDouble();
            double vol = fields[1].ToDouble() * 100;
            if (vol == 0) vol = 33;

            long epochMs = fields[2].ToInt64();
            DateTime time = TimeZoneInfo.ConvertTimeFromUtc(TimeTool.Epoch.AddMilliseconds(epochMs), sd.Contract.TimeZone);
            sd.InboundLiveTick(time, last, vol);
        }


        /// <summary>
        /// TickGeneric
        /// (0)"45"-(1)"6"-(2)"6"-(3)"46"-(4)"3.0"
        /// Parse_TickGeneric: (0)"45"-(1)"6"-(2)"97"-(3)"49"-(4)"0.0"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickGeneric(string[] fields)
        {
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);

            if (msgVersion == "6" && GetMarketData(tickerId) is MarketData md && fields.Length == 5)
            {
                TickType tickType = fields[3].ToTickType();

                switch (tickType)
                {
                    case TickType.Shortable when md is StockData q:
                        q.ShortStatus = fields[4].ToDouble(-1);
                        break;

                    case TickType.Halted:
                        //q.ShortStatus = fields[4].ToDouble(-1);
                        md.Status = MarketTickStatus.Frozen;

                        // Halted!!!

                        break;

                    default:
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
                        // Console.WriteLine("\nParse Tick Generic: " + fields.ToFlat());
                        break;
                }

                md.Update();
                // WatchListManager.UpdateUI(md);
            }
            else
                UnregisterMarketDataRequest(tickerId, true);
        }

        /// <summary>
        /// Parse Tick EFP
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickEFP(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            /*
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
            */

        }

        /// <summary>
        /// Parse Tick Option Computation
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickOptionComputation(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            /*
            string msgVersion = fields[1];
            int tickerId = fields[2].ToInt32(-1);
            TickType tickType = fields[3].ToTickType();
            double impliedVolatility = fields[4].ToDouble(-1);
            */

        }

        /// <summary>
        /// (0)"84"-(1)"2"-(2)"1578657239000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2d715e"-(5)"Bernstein initiated Facebook (FB) coverage with Outperform"-(6)"K:1.00"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickNews(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            //int tickerId = fields[1].ToInt32(-1);
            //long epoch = fields[2].ToInt64(0); // (3)"45"-(4)"1580146441"



        }


    }
}
