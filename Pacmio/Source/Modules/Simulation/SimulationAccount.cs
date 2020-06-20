/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;
using System.Windows.Forms;
using ReutersXml;

namespace Pacmio
{
    [Serializable, DataContract]
    public class SimulationAccount : Account
    {
        public SimulationAccount(double initialValue)
        {
            BuyingPower = initialValue;
            Reset();
        }

        [DataMember]
        public override string AccountCode => "SIMULATION";

        //[DataMember]
        // double Commissions { get; set; } = 0.68;

        public static double EstimatedCommission(double quantity, double price) 
        {
            quantity = Math.Abs(quantity);
            double value = quantity * price;
            double comms = quantity * 0.0035;
            if (comms < 0.35) comms = 0.35;
            else if (comms > 0.01 * value) comms = 0.01 * value;

            double exchange_fee = 0.00045 * quantity;
            double transaction_fee = 0.0000221 * value;
            double finra_fee = 0.00056 * comms;
            double nyse_pass_fee = 0.000175 * comms;

            return comms + exchange_fee + transaction_fee + finra_fee + nyse_pass_fee;
        }

        [DataMember]
        public List<TradeInfo> TradeLog { get; protected set; } = new List<TradeInfo>();

        [IgnoreDataMember]
        public (BarTable bt, Bar b) QuoteCondition { get; private set; }

        public void SetSimulationQuoteCondition(BarTable bt, Bar b)
        {
            QuoteCondition = (bt, b);
            /*
            PositionStatus ps = GetPosition(bt.Contract);

            if ((ps.Quantity > 0 && b.Low < ps.Stop) || (ps.Quantity < 0 && b.High > ps.Stop))
                Close(bt.Contract);*/
        }

        [IgnoreDataMember]
        public int Count { get; set; } = 0;

        public override void Entry(Contract c, double quantity)//, double stopLoss = double.NaN, double profitLimit = double.NaN, double entryLimit = double.NaN)
        {
            lock (this)
            {
                PositionStatus ps = GetPosition(c);
                int qty = quantity.ToInt32(0);

                if (qty != 0)
                {
                    double price = QuoteCondition.b.Open;
                    var (proceeds, pnl, liquidityType) = ps.Add(qty, price);
                    double commissions = EstimatedCommission(qty, price);

                    pnl -= commissions;
                    proceeds += commissions; // Commissions;
                    BuyingPower -= proceeds;

                    Console.WriteLine(Count + " | ActionType: " + ps.ActionType + " | TotalValue: " + TotalValue + "| Proceeds: " + proceeds + " | PnL: " + pnl);

                    if (liquidityType != LiquidityType.None)
                    {
                        TradeLog.Add(new TradeInfo(c.ToString() + "|" + QuoteCondition.b.Time)
                        {
                            Contract = c,
                            OrderId = Count,
                            PermId = Count,
                            ClientId = Root.IBClient.ClientId,
                            Account = AccountCode,
                            FillExchangeCode = "PacmioSim",
                            TotalQuantity = qty,
                            AveragePrice = price,
                            Quantity = qty,
                            Price = price,
                            Commissions = commissions,
                            LastLiquidity = liquidityType,
                            ExecuteTime = QuoteCondition.b.Time,
                            Liquidation = 0,
                            RealizedPnL = pnl,
                        });

                        //if (pnl != 0)
                        //Console.WriteLine(Count + "| Commission: " + Commissions + " | PnL: " + pnl);
                        Count++;
                    }

                    ps.PnL = pnl;
                }

                //ps.Stop = stopLoss;
                //ps.Limit = profitLimit;
                if (ps.Value > 0) ps.MaxTradeValue = Math.Max(ps.MaxTradeValue, ps.Value);
            }
        }

        public override void Exit(Contract c)
        {
            lock (this)
            {
                PositionStatus ps = GetPosition(c);

                if (!ps.PendingExit)
                {
                    ps.PendingExit = true;

                    double price = ps.Stop;

                    if (ps.Stop > QuoteCondition.b.High || ps.Stop < QuoteCondition.b.Low)
                    {
                        price = QuoteCondition.b.Open;
                    }

                    /*
                    if (ps.Quantity > 0) 
                    {


                    }
                    else if(ps.Quantity < 0) 
                    {
                        if (ps.Stop < QuoteCondition.b.Low || ps.Stop < QuoteCondition.b.Low)
                        {
                            price = QuoteCondition.b.Open;
                        }
                    }*/

                    double qty = ps.Quantity;
                    var (proceeds, pnl, liquidityType) = ps.Close(price);
                    double commissions = EstimatedCommission(qty, price);

                    pnl -= commissions;
                    proceeds += commissions;
                    BuyingPower -= proceeds;

                    Console.WriteLine(Count + " | ActionType: " + ps.ActionType + " | TotalValue: " + TotalValue + "| Proceeds: " + proceeds + " | PnL: " + pnl);

                    if (liquidityType != LiquidityType.None)
                    {
                        TradeLog.Add(new TradeInfo(c.ToString() + "|" + QuoteCondition.b.Time)
                        {
                            Contract = c,
                            OrderId = Count,
                            PermId = Count,
                            ClientId = Root.IBClient.ClientId,
                            Account = AccountCode,
                            FillExchangeCode = "PacmioSim",
                            TotalQuantity = -qty,
                            AveragePrice = price,
                            Quantity = -qty,
                            Price = price,
                            Commissions = commissions,
                            LastLiquidity = liquidityType,
                            ExecuteTime = QuoteCondition.b.Time,
                            Liquidation = 0,
                            RealizedPnL = pnl,
                        });

                        //Console.WriteLine(Count + "| Commission: " + Commissions + " | PnL: " + pnl);
                        Count++;
                    }

                    ps.PnL = pnl;
                    ps.Stop = double.NaN;
                    ps.Limit = double.NaN;
                }
            }
        }
    }
}
