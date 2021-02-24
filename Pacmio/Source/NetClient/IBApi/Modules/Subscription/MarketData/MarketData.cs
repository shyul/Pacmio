/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Collections.Concurrent;
using Xu;
using System;
using System.Reflection;
using System.Threading;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static void DataRequest_MarketData(MarketData md)
        {
            lock (RequestLockObject)
            {
                SendRequest_MarketData(md, true);

                while (ActiveSubscriptionCount > 80) //(IsActive(md)) 
                {
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// enericTickList:
        ///     100 Option Volume(currently for stocks)
        ///     101 Option Open Interest(currently for stocks)
        ///     104 Historical Volatility(currently for stocks)
        ///     105 Average Option Volume(currently for stocks)
        ///     106 Option Implied Volatility(currently for stocks)
        ///     162 Index Future Premium
        ///     165 Miscellaneous Stats
        ///     221 Mark Price(used in TWS P&L computations)
        ///     225 Auction values(volume, price and imbalance)
        ///     233 RTVolume - contains the last trade price, last trade size, last trade time, total volume, VWAP, and single trade flag.
        ///     236 Shortable
        ///     256 Inventory
        ///     258 Fundamental Ratios
        ///     411 Realtime Historical Volatility
        ///     456 IBDividends
        ///     375 RT Volume filtered for BarTable
        ///     
        /// https://interactivebrokers.github.io/tws-api/tick_types.html
        /// 
        /// Regulatory Snapshots:
        /// For stocks, there are individual exchange-specific market data subscriptions necessary to receive streaming quotes.
        /// For instance, for NYSE stocks this subscription is known as "Network A", for ARCA/AMEX stocks it is called "Network B" and for NASDAQ stocks it is "Network C".
        /// Each subscription is added a la carte and has a separate market data fee.
        /// 
        /// Alternatively, there is also a "US Securities Snapshot Bundle" subscription which does not provide streaming data but which allows for real time calculated snapshots of US market NBBO prices.
        /// By setting the 5th parameter in the function IBApi::EClient::reqMktData to True, a regulatory snapshot request can be made from the API.
        /// The returned value is a calculation of the current market state based on data from all available exchanges.
        /// 
        /// Important: Each regulatory snapshot made will incur a fee of 0.01 USD to the account. This applies to both live and paper accounts..
        /// If the monthly fee for regulatory snapshots reaches the price of a particular 'Network' subscription,
        /// the user will automatically be subscribed to that Network subscription for continuous streaming quotes and charged the associated fee for that month.
        /// At the end of the month the subscription will be terminated.
        /// Each listing exchange will be capped independently and will not be combined across listing exchanges.
        /// 
        /// https://interactivebrokers.github.io/tws-api/md_request.html#regulatory_snapshot
        /// 
        /// client.reqMktData(1004, ContractSamples.USStockAtSmart(), "233,236,258", false, false, null);
        /// (0)"1"-(1)"11"-(2)"10000002"-(3)"0"-(4)"AAPL"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"0"-(16)""-(17)"0"-(18)"0"
        /// (0)"1"-(1)"11"-(2)"10000001"-(3)"0"-(4)"FB"  -(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"0"-(16)""-(17)"0"-(18)"0"
        /// </summary>
        /// <param name="c"></param>
        /// <param name="genericTickList">"233,236,258"</param>
        /// <param name="isSnapshot">snapshot means a single request</param>
        /// <param name="regulatorySnaphsot">Regulatory Snapshots</param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static bool SendRequest_MarketData(MarketData md, bool isSnapshot = false,
            bool regulatorySnaphsot = false, ICollection<(string, string)> options = null)
        {
            Contract c = md.Contract;
            string genericTickList = "221,"; // "258,";// string.Empty;

            if (!isSnapshot)
            {
                if (md is StockData sd)
                {
                    //genericTickList += sd.FilteredTicks ? "375," : "233,";
                    genericTickList += sd.FilteredTicks ? "233,375," : "233,375,";
                    if (sd.EnableShortableShares) genericTickList += "236,";
                    if (sd.EnableNews) genericTickList += "292,";
                }

                genericTickList = genericTickList.TrimEnd(',');
            }

         

            if (isSnapshot || genericTickList != GetMarketDataRequestStatus(md).GenericTickList)
            {
                if (IsActive(md))
                    UnregisterMarketDataRequest(md, true);
            }

            var (tickerId, requestType, exchangeCode, mds) = RegisterMarketDataRequest(md);

            Console.WriteLine("Add MarketData: " + c + " | " + isSnapshot + " | " + genericTickList + " | " + tickerId);

            if (tickerId > 0)
            {
                mds.IsSnapshot = isSnapshot;
                mds.GenericTickList = genericTickList;

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

                List<string> paramsList = new List<string>() {
                    requestType,
                    "11",
                    tickerId.Param(),
                    c.ConId.Param(), // Contract id
                    c.Name, // Contract Symbol
                    c.TypeCode(), // Contract SecType
                    lastTradeDateOrContractMonth, // Contract Date
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // Contract Right
                    multiplier, // Contract Multiplier
                    c.SmartExchangeRoute ? "SMART" : exchangeCode,  // "ISLAND", Contract Exchange, Specify the Primary Exchange attribute to avoid contract ambiguity
                    exchangeCode, // PrimaryExch,
                    c.CurrencyCode, //  "USD", // Contract Currency
                    string.Empty, // Contract Local Symnbol
                    string.Empty, // Contract Trading Class
                };

                if (c is ICombo ic)
                {
                    if (ic.ComboLegs is null)
                    {
                        paramsList.Add("0");
                    }
                    else
                    {
                        paramsList.Add(ic.ComboLegs.Count.ParamPos());
                        foreach (ComboLeg leg in ic.ComboLegs)
                        {
                            paramsList.AddRange(new string[] {
                                leg.ConId.ParamPos(),
                                leg.Ratio.Param(),
                                leg.Action,
                                leg.Exchange,
                            });
                        }
                    }
                }

                if (c is IDeltaNeutral dnc && !(dnc.DeltaNeutralContract is null))
                {
                    DeltaNeutralContract deltaNeutralContract = dnc.DeltaNeutralContract;
                    paramsList.AddRange(new string[] {
                        "1",
                        deltaNeutralContract.ConId.ParamPos(),
                        deltaNeutralContract.Delta.Param(),
                        deltaNeutralContract.Price.Param(),
                    });
                }
                else
                {
                    paramsList.Add("0");
                }

                paramsList.AddRange(new string[] {
                    genericTickList,
                    isSnapshot.Param(),
                    regulatorySnaphsot.Param(),
                    options.Param(),
                });

                SendRequest(paramsList);
                return true;
            }

            return false;
        }

        public static void SendCancel_MarketData(MarketData md) => UnregisterMarketDataRequest(md, true);

        /// <summary>
        /// Unknown Message: 57: (0)"57"-(1)"1"-(2)"97" 
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_TickSnapshotEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            if (fields[1] == "1")
            {
                int tickerId = fields[2].ToInt32(-1);
                if (UnregisterMarketDataRequest(tickerId, false) is MarketDataRequestStatus mds) 
                    mds.MarketData.Update();
            }
        }

        private static void ParseError_MarketData(string[] fields)
        {
            int tickerId = fields[2].ToInt32(-1);

            if (UnregisterMarketDataRequest(tickerId) is MarketDataRequestStatus mds && fields[3] == "200")
            {
                mds.MarketData.Contract.Status = ContractStatus.Error;
                mds.MarketData.Contract.UpdateTime = DateTime.Now;
            }

            /*
            RemoveRequest(tickerId, false);
            ActiveMarketDataTicks.TryRemove(tickerId, out MarketData md);

            if (fields[3] == "200")
            {
                md.Contract.Status = ContractStatus.Error;
                md.Contract.UpdateTime = DateTime.Now;
            }*/

            Console.WriteLine("RequestHistoricalTick errors: " + fields.ToStringWithIndex());
        }
    }


}
