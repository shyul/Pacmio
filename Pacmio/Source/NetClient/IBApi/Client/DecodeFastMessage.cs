/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static bool IsFastMessageBufferEmpty => FastMessageBuffer.IsEmpty;

        private static void FlushFastMessageBuffer() { lock (FastMessageBuffer) while (!IsFastMessageBufferEmpty) FastMessageBuffer.TryDequeue(out _); } //(out string[] fields); }

        private static ConcurrentQueue<string[]> FastMessageBuffer { get; } = new ConcurrentQueue<string[]>();

        private static Task DecodeFastMessageTask { get; set; }

        private static void DecodeFastMessageWorker()
        {
            while (!IsCancelled)
            {
                if (!IsFastMessageBufferEmpty)
                {
                    FastMessageBuffer.TryDequeue(out string[] fields);
                    int messageType = fields[0].ToInt32(-1);

                    Task.Run(() =>
                    {
                        switch (messageType)
                        {
                            #region Account

                            case IncomingMessage.ManagedAccounts:
                                Parse_ManagedAccounts(fields); // Adding available accounts
                                break;

                            case IncomingMessage.AccountValue:
                                Parse_AccountValue(fields);
                                break;

                            case IncomingMessage.AccountUpdateTime:
                                Parse_AccountUpdateTime(fields);
                                break;

                            case IncomingMessage.AccountDownloadEnd:
                                Parse_AccountDownloadEnd(fields);
                                break;

                            case IncomingMessage.PnL:
                                Parse_PnL(fields);
                                break;

                            case IncomingMessage.PnLSingle:
                                Parse_PnLSingle(fields);
                                break;

                            #endregion Account

                            #region Contract

                            case IncomingMessage.SymbolSamples:
                                Parse_ContractSamples(fields);
                                break;

                            case IncomingMessage.DeltaNeutralValidation:
                                Parse_DeltaNeutralValidation(fields);
                                break;

                            #endregion Contract

                            #region Position

                            case IncomingMessage.PortfolioValue:
                                Parse_PortfolioValue(fields);
                                break;

                            #endregion Position

                            #region Order

                            case (IncomingMessage.OrderStatus):
                                Parse_OrderStatus(fields);
                                break;

                            case IncomingMessage.VerifyCompleted:
                                Parse_VerifyCompleted(fields);
                                break;

                            #endregion Order

                            #region Scanner

                            case IncomingMessage.ScannerData:
                                Parse_ScannerSubscription(fields);
                                break;

                            case IncomingMessage.ScannerParameters:
                                Parse_ScannerParameters(fields);
                                break;

                            #endregion Scanner

                            #region Market Data

                            case IncomingMessage.SmartComponents:
                                Parse_SmartComponents(fields);
                                break;

                            case IncomingMessage.MarketDataType:
                                Parse_MarketDataType(fields);
                                break;

                            case IncomingMessage.TickReqParams:
                                Parse_TickReqParams(fields);
                                break;

                            case IncomingMessage.TickPrice:
                                Parse_TickPrice(fields);
                                break;

                            case IncomingMessage.TickSize:
                                Parse_TickSize(fields);
                                break;

                            case IncomingMessage.TickString:
                                Parse_TickString(fields);
                                break;

                            case IncomingMessage.TickGeneric:
                                Parse_TickGeneric(fields);
                                break;

                            case IncomingMessage.TickEFP:
                                Parse_TickEFP(fields);
                                break;

                            case IncomingMessage.TickOptionComputation:
                                Parse_TickOptionComputation(fields);
                                break;

                            case IncomingMessage.MarketDepth:
                                Parse_MarketDepth(fields);
                                break;

                            case IncomingMessage.MarketDepthL2:
                                Parse_MarketDepthL2(fields);
                                break;

                            case IncomingMessage.MktDepthExchanges:
                                Parse_MktDepthExchanges(fields);
                                break;

                            case IncomingMessage.RealTimeBars:
                                Parse_RealTimeBars(fields);
                                break;

                            case IncomingMessage.TickSnapshotEnd:
                                Parse_TickSnapshotEnd(fields);
                                break;

                            #endregion Market Data

                            #region News

                            case IncomingMessage.TickNews:
                                Parse_TickNews(fields);
                                break;

                            case IncomingMessage.NewsProviders:
                                Parse_NewsProviders(fields);
                                break;

                            case IncomingMessage.NewsArticle:
                                Parse_NewsArticle(fields);
                                break;

                            #endregion News

                            #region Historical Data

                            case IncomingMessage.HistoricalData:
                                Parse_HistoricalData(fields);
                                break;

                            case IncomingMessage.HistoricalDataUpdate:
                                Parse_HistoricalDataUpdate(fields);
                                break;

                            case IncomingMessage.HeadTimestamp:
                                Parse_HistoricalHeadDataTimestamp(fields);
                                break;

                            case IncomingMessage.HistogramData:
                                Parse_HistogramData(fields);
                                break;

                            case IncomingMessage.HistoricalTick:
                                Parse_HistoricalTick(fields);
                                break;

                            case IncomingMessage.HistoricalTickBidAsk:
                                Parse_HistoricalTickBidAsk(fields);
                                break;

                            case IncomingMessage.HistoricalTickLast:
                                Parse_HistoricalTickLast(fields);
                                break;

                            #endregion Historical Data

                            #region Fundamental Data

                            case IncomingMessage.FundamentalData:
                                Parse_FundamentalData(fields);
                                break;

                            #endregion Fundamental Data

                            #region System

                            case IncomingMessage.CurrentTime:
                                Console.WriteLine("CurrentTime: " + messageType.ToString() + ": " + fields.ToStringWithIndex());
                                break;

                            case IncomingMessage.NotValid:
                                Console.WriteLine("NotValid: " + messageType.ToString() + ": " + fields.ToStringWithIndex());
                                break;

                            case IncomingMessage.Error:
                                Parse_Errors(fields);
                                break;

                            case IncomingMessage.NewsBulletins:
                            case IncomingMessage.ReceiveFA:
                            case IncomingMessage.BondContractData:
                            case IncomingMessage.VerifyMessageApi:
                            case IncomingMessage.DisplayGroupList:
                            case IncomingMessage.DisplayGroupUpdated:
                            case IncomingMessage.VerifyAndAuthMessageApi:
                            case IncomingMessage.VerifyAndAuthCompleted:
                            case IncomingMessage.SoftDollarTier:
                            case IncomingMessage.FamilyCodes:
                            case IncomingMessage.RerouteMktDataReq:
                            case IncomingMessage.RerouteMktDepthReq:
                            case IncomingMessage.MarketRule:
                            case IncomingMessage.TickByTick:
                            case IncomingMessage.OrderBound:
                            default:
                                Console.WriteLine("Unknown Message: " + messageType.ToString() + ": " + fields.ToStringWithIndex());
                                break;

                                #endregion System
                        }
                    });
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
