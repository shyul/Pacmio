/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {



        //Send ReqPnL: (0)"92"-(1)"600065159"-(2)"U1564932"-
        //Received PnL: (0)"94"-(1)"600065159"-(2)"-81.00009719999798"-(3)"6053.1747039"-(4)"0.0"-
        //Received PnL: (0)"94"-(1)"600065159"-(2)"-97.20108764648467"-(3)"6036.9737134535135"-(4)"0.0"-
        private static void Parse_PnL(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_PnLSingle(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }
}

/*
namespace IBApi
{
    public abstract partial class EClient
    {
    
        * @brief Creates subscription for real time daily PnL and unrealized PnL updates
        * @param account account for which to receive PnL updates
        * @param modelCode specify to request PnL updates for a specific model
      

        public void reqPnL(int reqId, string account, string modelCode)
        {
            if (!CheckConnection())
                return;

            if (!CheckServerVersion(MinServerVer.PNL,
                    "  It does not support PnL requests."))
                return;

            var paramsList = new BinaryWriter(new MemoryStream());
            var lengthPos = prepareBuffer(paramsList);

            paramsList.AddParameter(OutgoingMessages.ReqPnL);
            paramsList.AddParameter(reqId);
            paramsList.AddParameter(account);
            paramsList.AddParameter(modelCode);

            CloseAndSend(paramsList, lengthPos, EClientErrors.FAIL_SEND_REQPNL);
        }

     
        * @brief cancels subscription for real time updated daily PnL
        * params reqId
       

        public void cancelPnL(int reqId)
        {
            if (!CheckConnection())
                return;

            if (!CheckServerVersion(MinServerVer.PNL,
                    "  It does not support PnL requests."))
                return;

            var paramsList = new BinaryWriter(new MemoryStream());
            var lengthPos = prepareBuffer(paramsList);

            paramsList.AddParameter(OutgoingMessages.CancelPnL);
            paramsList.AddParameter(reqId);

            CloseAndSend(paramsList, lengthPos, EClientErrors.FAIL_SEND_CANCELPNL);
        }

        
        * @brief Requests real time updates for daily PnL of individual positions
        * @param reqId
        * @param account account in which position exists
        * @param modelCode model in which position exists
        * @param conId contract ID (conId) of contract to receive daily PnL updates for.  
        * Note: does not return message if invalid conId is entered
        

public void reqPnLSingle(int reqId, string account, string modelCode, int conId)
        {
            if (!CheckConnection())
                return;

            if (!CheckServerVersion(MinServerVer.PNL,
                    "  It does not support PnL requests."))
                return;

            var paramsList = new BinaryWriter(new MemoryStream());
            var lengthPos = prepareBuffer(paramsList);

            paramsList.AddParameter(OutgoingMessages.ReqPnLSingle);
            paramsList.AddParameter(reqId);
            paramsList.AddParameter(account);
            paramsList.AddParameter(modelCode);
            paramsList.AddParameter(conId);

            CloseAndSend(paramsList, lengthPos, EClientErrors.FAIL_SEND_REQPNLSINGLE);
        }

        
        * @brief Cancels real time subscription for a positions daily PnL information
        * @param reqId
        

        public void cancelPnLSingle(int reqId)
        {
            if (!CheckConnection())
                return;

            if (!CheckServerVersion(MinServerVer.PNL,
                    "  It does not support PnL requests."))
                return;

            var paramsList = new BinaryWriter(new MemoryStream());
            var lengthPos = prepareBuffer(paramsList);

            paramsList.AddParameter(OutgoingMessages.CancelPnLSingle);
            paramsList.AddParameter(reqId);

            CloseAndSend(paramsList, lengthPos, EClientErrors.FAIL_SEND_REQPNLSINGLE);
        }
    }

    partial class EDecoder
    {
        private void PnLSingleEvent()
        {
            int reqId = ReadInt();
            int pos = ReadInt();
            double dailyPnL = ReadDouble();
            double unrealizedPnL = double.MaxValue;
            double realizedPnL = double.MaxValue;

            if (serverVersion >= MinServerVer.UNREALIZED_PNL)
            {
                unrealizedPnL = ReadDouble();
            }

            if (serverVersion >= MinServerVer.REALIZED_PNL)
            {
                realizedPnL = ReadDouble();
            }

            double value = ReadDouble();

            eWrapper.pnlSingle(reqId, pos, dailyPnL, unrealizedPnL, realizedPnL, value);
        }

        private void PnLEvent()
        {
            int reqId = ReadInt();
            double dailyPnL = ReadDouble();
            double unrealizedPnL = double.MaxValue;
            double realizedPnL = double.MaxValue;

            if (serverVersion >= MinServerVer.UNREALIZED_PNL)
            {
                unrealizedPnL = ReadDouble();
            }

            if (serverVersion >= MinServerVer.REALIZED_PNL)
            {
                realizedPnL = ReadDouble();
            }

            eWrapper.pnl(reqId, dailyPnL, unrealizedPnL, realizedPnL);
        }
    }
}
*/