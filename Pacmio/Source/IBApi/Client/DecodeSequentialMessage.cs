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
        private static bool IsSequentialMessageBufferEmpty => SequentialMessageBuffer.IsEmpty;

        private static void FlushSequentialMessageBuffer() { lock (SequentialMessageBuffer) while (!IsSequentialMessageBufferEmpty) SequentialMessageBuffer.TryDequeue(out _); } //(out string[] fields); }

        private static ConcurrentQueue<string[]> SequentialMessageBuffer { get; } = new ConcurrentQueue<string[]>();

        private static Task DecodeSequentialMessageTask { get; set; }

        private static void DecodeSequentialMessageWorker()
        {
            while (!IsCancelled)
            {
                if (!IsSequentialMessageBufferEmpty)
                {
                    SequentialMessageBuffer.TryDequeue(out string[] fields);
                    int messageType = fields[0].ToInt32(-1);

                    switch (messageType)
                    {
                        case IncomingMessage.AccountSummary:
                            Parse_AccountSummary(fields);
                            break;

                        case IncomingMessage.AccountSummaryEnd:
                            Parse_AccountSummaryEnd(fields);
                            break;

                        case IncomingMessage.AccountUpdateMulti:
                            Parse_AccountUpdateMulti(fields);
                            break;

                        case IncomingMessage.AccountUpdateMultiEnd:
                            Parse_AccountUpdateMultiEnd(fields);
                            break;

                        case IncomingMessage.ContractData:
                            Parse_ContractData(fields);
                            break;

                        case IncomingMessage.ContractDataEnd:
                            Parse_ContractDataEnd(fields);
                            break;

                        case IncomingMessage.OpenOrder:
                            Parse_OpenOrder(fields);
                            break;

                        case IncomingMessage.OpenOrderEnd:
                            Parse_OpenOrderEnd(fields);
                            break;

                        case IncomingMessage.CommissionsReport:
                            Parse_CommissionsReport(fields);
                            break;

                        case IncomingMessage.ExecutionData:
                            Parse_ExecutionData(fields);
                            break;

                        case IncomingMessage.ExecutionDataEnd:
                            Parse_ExecutionDataEnd(fields);
                            break;

                        case IncomingMessage.CompletedOrder:
                            Parse_CompletedOrder(fields);
                            break;

                        case IncomingMessage.CompletedOrdersEnd:
                            Parse_CompletedOrdersEnd(fields);
                            break;

                        case IncomingMessage.Position:
                            Parse_Position(fields);
                            break;

                        case IncomingMessage.PositionEnd:
                            Parse_PositionEnd(fields);
                            break;

                        case IncomingMessage.PositionMulti:
                            Parse_PositionMulti(fields);
                            break;

                        case IncomingMessage.PositionMultiEnd:
                            Parse_PositionMultiEnd(fields);
                            break;

                        case IncomingMessage.HistoricalNews:
                            Parse_HistoricalNews(fields);
                            break;

                        case IncomingMessage.HistoricalNewsEnd:
                            Parse_HistoricalNewsEnd(fields);
                            break;

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

                        case IncomingMessage.SecurityDefinitionOptionParameter:
                        case IncomingMessage.SecurityDefinitionOptionParameterEnd:
                        default:
                            Console.WriteLine("Unknown Message: " + messageType.ToString() + ": " + fields.ToStringWithIndex());
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
