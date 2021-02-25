/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public sealed class AccountInfo : IDataProvider, IEquatable<AccountInfo>, IEquatable<string>
    {
        public AccountInfo(string accountId)
        {
            AccountId = accountId;
        }

        public override string ToString() => AccountId;

        [IgnoreDataMember]
        public bool IsLive { get; set; } = false;

        [IgnoreDataMember]
        public string Name => "Account: " + AccountId;

        [DataMember]
        public DateTime UpdateTime { get; private set; } = DateTime.MinValue;

        [IgnoreDataMember]
        public List<IDataConsumer> DataConsumers { get; private set; }

        public bool AddDataConsumer(IDataConsumer idk)
        {
            if (DataConsumers is null) DataConsumers = new List<IDataConsumer>();
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            return (DataConsumers is not null) ? DataConsumers.CheckRemove(idk) : false;
        }

        public void Updated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers?.ForEach(n => n.DataIsUpdated(this));
        }

        [DataMember, Browsable(true), ReadOnly(true)]
        [Description("Number of Open/Close trades one could do before Pattern Day Trading is detected")]
        public List<int> DayTradesRemaining { get; private set; } = new List<int> { 0, 0, 0, 0, 0 };

        #region Positions

        [IgnoreDataMember]
        public double TotalSize => BuyingPower + PositionValue;

        [IgnoreDataMember]
        public double TotalSizeIB => BuyingPower + PositionValueIB;

        [IgnoreDataMember]
        public double PositionValue
        {
            get
            {
                if (PositionPerEachContractLUT is null) PositionPerEachContractLUT = new Dictionary<Contract, PositionInfo>();

                lock (PositionPerEachContractLUT)
                {
                    return (PositionPerEachContractLUT is Dictionary<Contract, PositionInfo> lut && lut.Count > 0) ? lut.Values.Select(n => n.Value).Sum() : 0;
                }
            }
        }

        [IgnoreDataMember]
        public PositionInfo this[Contract c]
        {
            get
            {
                if (PositionPerEachContractLUT is null) PositionPerEachContractLUT = new Dictionary<Contract, PositionInfo>();

                lock (PositionPerEachContractLUT)
                {
                    return (PositionPerEachContractLUT is Dictionary<Contract, PositionInfo> lut && lut.ContainsKey(c)) ? lut[c] : null;
                }
            }
        }

        public PositionInfo GetOrCreatePositionByContract(Contract c)
        {
            if (PositionPerEachContractLUT is null) PositionPerEachContractLUT = new Dictionary<Contract, PositionInfo>();

            lock (PositionPerEachContractLUT)
            {
                if (!PositionPerEachContractLUT.ContainsKey(c))
                    PositionPerEachContractLUT.Add(c, new PositionInfo(this, c));
            }
            return PositionPerEachContractLUT[c];
        }

        public void ResetAllPositionRefreshStatus()
        {
            if (PositionPerEachContractLUT is null) PositionPerEachContractLUT = new Dictionary<Contract, PositionInfo>();

            lock (PositionPerEachContractLUT)
            {
                PositionPerEachContractLUT.Select(n => n.Value).ToList().ForEach(n => n.Refreshed = false);
            }
        }

        public void ResetNonRefreshedPositions()
        {
            if (PositionPerEachContractLUT is null) PositionPerEachContractLUT = new Dictionary<Contract, PositionInfo>();

            lock (PositionPerEachContractLUT)
            {
                PositionPerEachContractLUT
                    .Select(n => n.Value)
                    .Where(n => !n.Refreshed && n.Quantity != 0)
                    .ToList()
                    .ForEach(n => n.Reset());
            }
        }

        [IgnoreDataMember]
        private Dictionary<Contract, PositionInfo> PositionPerEachContractLUT { get; set; }

        #endregion Positions

        #region Order

        public void EmergencyCloseAllPositions()
        {
            foreach (var ps in PositionPerEachContractLUT.Values.Where(n => n.Quantity != 0))
            {
                ps.EmergencyClose();
            }
        }

        #endregion Order

        #region 1: Account Basic

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Account Code")]
        [Description("The account ID number.")]
        public string AccountId { get; private set; }

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Account Type")]
        [Description("Identifies the IB account structure.")]
        public string AccountType { get; private set; }


        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic"), DisplayName("System Account Code")]
        public string AccountCodeSys { get; private set; }

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic")]
        [Description("For internal use only")]
        public bool AccountReady { get; private set; } = false;

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic"), DisplayName("Account Or Group")]
        [Description("AccountOrGroup: (All) to return account summary data for all accounts, or set to a specific Advisor Account Group name that has already been created in TWS Global Configuration.")]
        public string AccountOrGroup { get; private set; } // "U1165638"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Currency")]
        [Description("Open positions are grouped by currency.")]
        public string Currency { get; private set; } // "BASE"

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic")]
        [Description("Open positions are grouped by currency.")]
        public string RealCurrency { get; private set; } // "BASE"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Currency Exchange Rate")]
        [Description("The exchange rate of the currency to your base currency.")]
        public double ExchangeRate { get; private set; } // 1.00

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Trading Type")]
        [Description("Trading Type name")]
        public string TradingType { get; private set; } // "STKMRGN" "IRAMRGN" "STKNOPT"

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic")]
        public string TradingType_S { get; private set; }  // "STKMRGN" "IRAMRGN" "STKNOPT"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Segment Title")]
        [Description("Account segment name")]
        public string SegmentTitle { get; private set; }

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic")]
        public string SegmentTitle_S { get; private set; }

        [DataMember, Browsable(false), ReadOnly(true), Category("1: Basic")]
        public string SegmentTitle_C { get; private set; }

        [DataMember, Browsable(false), ReadOnly(true)]
        [Description("WhatIfPMEnabled: To check projected margin requirements under Portfolio Margin model.")]
        public bool WhatIfPMEnabled { get; private set; } // "true"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Unrealized Profit & Loss")]
        [Description("")]
        public double UnrealizedPnL { get; private set; } = double.NaN; // "295"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Realized Profit & Loss")]
        [Description("")]
        public double RealizedPnL { get; private set; } = double.NaN; // "603"

        [DataMember]
        [Description("Excess liquidity as a percentage of net liquidation value")]
        public double Cushion { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Leverage")]
        [Description("Leverage = Gross Position value / Net Liquidation value")]
        public double Leverage { get; private set; } = double.NaN;

        [DataMember]
        [Description("")]
        public double Leverage_S { get; private set; } = double.NaN; // "0.04"

        #endregion

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Buying Power")]
        [Description("Standard Margin Account: Available Funds x4")]
        public double BuyingPower { get; private set; } = double.NaN; // "198153.11"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Cash Balance")]
        [Description("Cash recognized at the time of trade + futures PNL")]
        public double CashBalance { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Total Cash Balance")]
        [Description("")]
        public double TotalCashBalance { get; private set; } = double.NaN; // 48101

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Total Cash Value")]
        [Description("Total Cash Value: Settled cash + sales at the time of trade")]
        public double TotalCashValue { get; private set; } = double.NaN;


        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Total Cash Value S")]
        [Description("")]
        public double TotalCashValue_S { get; private set; } = double.NaN; // "48101.36"


        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Total Cash Value C")]
        [Description("")]
        public double TotalCashValue_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Available Funds")]
        [Description("Equity with Loan Value - Initial margin")]
        public double AvailableFunds { get; private set; } = double.NaN; // 49538.28

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Available Funds S")]
        [Description("")]
        public double AvailableFunds_S { get; private set; } = double.NaN; // 49538.28

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Available Funds C")]
        [Description("")]
        public double AvailableFunds_C { get; private set; } = double.NaN; // 0

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Net Liquidation")]
        [Description("Total cash value + stock value + securities options value + bond value")]
        public double NetLiquidation { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Net Liquidation S")]
        [Description("")]
        public double NetLiquidation_S { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Net Liquidation C")]
        [Description("")]
        public double NetLiquidation_C { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Net Liquidation Uncertainty")]
        [Description("")]
        public double NetLiquidationUncertainty { get; private set; } = double.NaN; // "0.00"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Net Liquidation By Currency")]
        [Description("")]
        public double NetLiquidationByCurrency { get; private set; } = double.NaN; // 50078

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Cash")]
        [Description("")]
        public double AccruedCash { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Cash S")]
        [Description("")]
        public double AccruedCash_S { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Cash C")]
        [Description("")]
        public double AccruedCash_C { get; private set; } = double.NaN;

        // Special Memorandum Account https://ibkr.info/node/66
        // Line of credit created when the market value of securities in a Regulation T account increase in value
        // Max ((EWL - US initial margin requirements)*, (Prior Day SMA +/- change in day's cash +/- US initial margin requirements** for trades made during the day.))
        // * calculated end of day under US Stock rules, regardless of country of trading.
        // ** at the time of the trade
        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("SMA")]
        [Description("")]
        public double SMA { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("SMA S")]
        [Description("")]
        public double SMA_S { get; private set; } = double.NaN; // for security segment


        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Next Change")]
        [Description("")]
        public double LookAheadNextChange { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Stock Market Value")]
        [Description("")]
        public double StockMarketValue { get; set; } = double.NaN; // 1945

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Option Market Value")]
        [Description("")]
        public double OptionMarketValue { get; set; } = double.NaN; // "0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Future Market Value")]
        [Description("")]
        public double FutureOptionValue { get; set; } = double.NaN; // "0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Futures Profit & Loss")]
        [Description("Real-time change in futures value since last settlement.")]
        public double FuturesPNL { get; private set; } = double.NaN; // "0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Fund Value")]
        [Description("")]
        public double FundValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double MutualFundValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double MoneyMarketFundValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double CorporateBondValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double TBondValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double TBillValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double WarrantValue { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double FxCashBalance { get; private set; } = double.NaN; // "0"

        [DataMember]
        [Description("")]
        public double IssuerOptionValue { get; private set; } = double.NaN; // "0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Net Dividend")]
        [Description("")]
        public double NetDividend { get; private set; } = double.NaN; // "0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Dividend")]
        [Description("")]
        public double AccruedDividend { get; private set; } = double.NaN; // "30.99"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Dividend S")]
        [Description("")]
        public double AccruedDividend_S { get; private set; } = double.NaN; // "30.99"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Accrued Dividend C")]
        [Description("")]
        public double AccruedDividend_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Billable")]
        [Description("")]
        public double Billable { get; private set; } = double.NaN; // "0.00"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Billable S")]
        [Description("")]
        public double Billable_S { get; private set; } = double.NaN; // "0.00"

        [DataMember, Browsable(true), ReadOnly(true), Category("1: Basic"), DisplayName("Billable C")]
        [Description("")]
        public double Billable_C { get; private set; } = double.NaN; // "0.00"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Equity With Loan Value")]
        [Description("")]
        public double EquityWithLoanValue { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Equity With Loan Value S")]
        [Description("")]
        public double EquityWithLoanValue_S { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Equity With Loan Value C")]
        [Description("")]
        public double EquityWithLoanValue_C { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Excess Liquidity")]
        [Description("")]
        public double ExcessLiquidity { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Excess Liquidity S")]
        [Description("")]
        public double ExcessLiquidity_S { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Excess Liquidity C")]
        [Description("")]
        public double ExcessLiquidity_C { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Available Funds")]
        [Description("")]
        public double FullAvailableFunds { get; private set; } = double.NaN; // "49538.28"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Available Funds S")]
        [Description("")]
        public double FullAvailableFunds_S { get; private set; } = double.NaN; // "49538.28"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Available Funds C")]
        [Description("")]
        public double FullAvailableFunds_C { get; private set; } = double.NaN; // "0.0"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Excess Liquidity")]
        [Description("")]
        public double FullExcessLiquidity { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Excess Liquidity S")]
        [Description("")]
        public double FullExcessLiquidity_S { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Full Excess Liquidity C")]
        [Description("")]
        public double FullExcessLiquidity_C { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Initial Margin Requirement")]
        [Description("")]
        public double FullInitMarginReq { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Initial Margin Requirement S")]
        public double FullInitMarginReq_S { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Initial Margin Requirement C")]
        public double FullInitMarginReq_C { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Maint Margin Requirement")]
        [Description("")]
        public double FullMaintMarginReq { get; private set; } = double.NaN; // "526.95"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Maint Margin Requirement S")]
        [Description("")]
        public double FullMaintMarginReq_S { get; private set; } = double.NaN; // "526.95"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Full Maint Margin Requirement C")]
        [Description("")]
        public double FullMaintMarginReq_C { get; private set; } = double.NaN; // "526.95"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Gross Position Value")]
        [Description("")]
        public double PositionValueIB { get; set; } = double.NaN; // "1945.42"

        [DataMember, Browsable(true), ReadOnly(true), Category("Market Value"), DisplayName("Gross Position Value S")]
        [Description("")]
        public double PositionValueIB_S { get; private set; } = double.NaN; // "1945.42"

        [DataMember]
        [Description("")]
        public double Guarantee { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double Guarantee_S { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double Guarantee_C { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("Margin rule for IB-IN accounts")]
        public double IndianStockHaircut { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double IndianStockHaircut_S { get; private set; } = double.NaN;

        [DataMember]
        [Description("")]
        public double IndianStockHaircut_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Initial Margin Requirement")]
        [Description("")]
        public double InitMarginReq { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Initial Margin Requirement S")]
        [Description("")]
        public double InitMarginReq_S { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Initial Margin Requirement C")]
        [Description("")]
        public double InitMarginReq_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Available Funds")]
        [Description("")]
        public double LookAheadAvailableFunds { get; private set; } = double.NaN; // "49538.28"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Available Funds S")]
        [Description("")]
        public double LookAheadAvailableFunds_S { get; private set; } = double.NaN; // "49538.28"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Available Funds C")]
        [Description("")]
        public double LookAheadAvailableFunds_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Excess Liquidity")]
        [Description("")]
        public double LookAheadExcessLiquidity { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Excess Liquidity S")]
        [Description("")]
        public double LookAheadExcessLiquidity_S { get; private set; } = double.NaN; // "49550.82"

        [DataMember, Browsable(true), ReadOnly(true), Category("Funds"), DisplayName("Look Ahead Excess Liquidity C")]
        [Description("")]
        public double LookAheadExcessLiquidity_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Initial Margin Requirement")]
        [Description("")]
        public double LookAheadInitMarginReq { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Initial Margin Requirement S")]
        [Description("")]
        public double LookAheadInitMarginReq_S { get; private set; } = double.NaN; // "539.50"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Initial Margin Requirement C")]
        [Description("")]
        public double LookAheadInitMarginReq_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Maint Margin Requirement")]
        [Description("")]
        public double LookAheadMaintMarginReq { get; private set; } = double.NaN; // "526.95"


        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Maint Margin Requirement S")]
        [Description("")]
        public double LookAheadMaintMarginReq_S { get; private set; } = double.NaN; // "526.95"


        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Look Ahead Maint Margin Requirement C")]
        [Description("")]
        public double LookAheadMaintMarginReq_C { get; private set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Maint Margin Required")]
        [Description("")]
        public double MaintMarginReq { get; private set; } = double.NaN; // "526.95"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Maint Margin Required S")]
        [Description("")]
        public double MaintMarginReq_S { get; private set; } = double.NaN; // "526.95"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Maint Margin Required C")]
        [Description("")]
        public double MaintMarginReq_C { get; private set; } = double.NaN;

        [DataMember]
        [Description("")]
        public double PASharesValue { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PASharesValue_S { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PASharesValue_C { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationExcess { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationExcess_S { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationExcess_C { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationMargin { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationMargin_S { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PostExpirationMargin_C { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double PreviousDayEquityWithLoanValue { get; private set; } = double.NaN; // "50082.13"

        [DataMember]
        [Description("")]
        public double PreviousDayEquityWithLoanValue_S { get; private set; } = double.NaN; // "50082.13"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Reg-T Equity")]
        [Description("")]
        public double RegTEquity { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Balance"), DisplayName("Reg-T Equity S")]
        [Description("")]
        public double RegTEquity_S { get; private set; } = double.NaN; // "50077.77"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Reg-T Margin")]
        [Description("")]
        public double RegTMargin { get; private set; } = double.NaN; // "972.71"

        [DataMember, Browsable(true), ReadOnly(true), Category("Margins"), DisplayName("Reg-T Margin S")]
        [Description("")]
        public double RegTMargin_S { get; private set; } = double.NaN; // "972.71"

        [DataMember]
        [Description("")]
        public double TotalDebitCardPendingCharges { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double TotalDebitCardPendingCharges_S { get; private set; } = double.NaN; // "0.00"

        [DataMember]
        [Description("")]
        public double TotalDebitCardPendingCharges_C { get; private set; } = double.NaN;

        public void UpdateFields(string tagName, string value)
        {
            switch (tagName)
            {
                case ("AccountId"):
                    AccountCodeSys = value;
                    break;
                case ("AccountReady"):
                    AccountReady = (value == "true");
                    break;
                case ("AccountType"):
                    AccountType = value;
                    break;
                case ("TradingType"):
                    TradingType = value;
                    break;
                case ("TradingType-S"):
                    TradingType_S = value;
                    break;
                case ("SegmentTitle"):
                    SegmentTitle = value;
                    break;
                case ("SegmentTitle-S"):
                    SegmentTitle_S = value;
                    break;
                case ("SegmentTitle-C"):
                    SegmentTitle_C = value;
                    break;
                case ("WhatIfPMEnabled"):
                    WhatIfPMEnabled = (value == "true");
                    break;
                case ("AccountOrGroup"):
                    AccountOrGroup = value;
                    break;
                case ("Currency"):
                    Currency = value;
                    break;
                case ("RealCurrency"):
                    RealCurrency = value;
                    break;
                case ("DayTradesRemaining"):
                    DayTradesRemaining[0] = value.ToInt32(0);
                    break;
                case ("DayTradesRemainingT+1"):
                    DayTradesRemaining[1] = value.ToInt32(0);
                    break;
                case ("DayTradesRemainingT+2"):
                    DayTradesRemaining[2] = value.ToInt32(0);
                    break;
                case ("DayTradesRemainingT+3"):
                    DayTradesRemaining[3] = value.ToInt32(0);
                    break;
                case ("DayTradesRemainingT+4"):
                    DayTradesRemaining[4] = value.ToInt32(0);
                    break;
                case ("CashBalance"):
                    CashBalance = value.ToDouble();
                    break;
                case ("TotalCashBalance"):
                    TotalCashBalance = value.ToDouble();
                    break;
                case ("StockMarketValue"):
                    StockMarketValue = value.ToDouble();
                    break;
                case ("OptionMarketValue"):
                    OptionMarketValue = value.ToDouble();
                    break;
                case ("FutureOptionValue"):
                    FutureOptionValue = value.ToDouble();
                    break;
                case ("FuturesPNL"):
                    FuturesPNL = value.ToDouble();
                    break;
                case ("NetLiquidationByCurrency"):
                    NetLiquidationByCurrency = value.ToDouble();
                    break;
                case ("UnrealizedPnL"):
                    UnrealizedPnL = value.ToDouble();
                    break;
                case ("RealizedPnL"):
                    RealizedPnL = value.ToDouble();
                    break;
                case ("ExchangeRate"):
                    ExchangeRate = value.ToDouble();
                    break;
                case ("FundValue"):
                    FundValue = value.ToDouble();
                    break;
                case ("NetDividend"):
                    NetDividend = value.ToDouble();
                    break;
                case ("MutualFundValue"):
                    MutualFundValue = value.ToDouble();
                    break;
                case ("MoneyMarketFundValue"):
                    MoneyMarketFundValue = value.ToDouble();
                    break;
                case ("Cushion"):
                    Cushion = value.ToDouble();
                    break;
                case ("LookAheadNextChange"):
                    LookAheadNextChange = value.ToDouble();
                    break;
                case ("AccruedCash"):
                    AccruedCash = value.ToDouble();
                    break;
                case ("AccruedCash-S"):
                    AccruedCash_S = value.ToDouble();
                    break;
                case ("AccruedCash-C"):
                    AccruedCash_C = value.ToDouble();
                    break;
                case ("SMA"):
                    SMA = value.ToDouble();
                    break;
                case ("SMA-S"):
                    SMA_S = value.ToDouble();
                    break;
                case ("TotalCashValue"):
                    TotalCashValue = value.ToDouble();
                    break;
                case ("TotalCashValue-S"):
                    TotalCashValue_S = value.ToDouble();
                    break;
                case ("TotalCashValue-C"):
                    TotalCashValue_C = value.ToDouble();
                    break;
                case ("Leverage"):
                    Leverage = value.ToDouble();
                    break;
                case ("Leverage-S"):
                    Leverage_S = value.ToDouble();
                    break;
                case ("AccruedDividend"):
                    AccruedDividend = value.ToDouble();
                    break;
                case ("AccruedDividend-S"):
                    AccruedDividend_S = value.ToDouble();
                    break;
                case ("AccruedDividend-C"):
                    AccruedDividend_C = value.ToDouble();
                    break;
                case ("AvailableFunds"):
                    AvailableFunds = value.ToDouble();
                    break;
                case ("AvailableFunds-S"):
                    AvailableFunds_S = value.ToDouble();
                    break;
                case ("AvailableFunds-C"):
                    AvailableFunds_C = value.ToDouble();
                    break;
                case ("Billable"):
                    Billable = value.ToDouble();
                    break;
                case ("Billable-S"):
                    Billable_S = value.ToDouble();
                    break;
                case ("Billable-C"):
                    Billable_C = value.ToDouble();
                    break;
                case ("BuyingPower"):
                    BuyingPower = value.ToDouble();
                    break;
                case ("EquityWithLoanValue"):
                    EquityWithLoanValue = value.ToDouble();
                    break;
                case ("EquityWithLoanValue-S"):
                    EquityWithLoanValue_S = value.ToDouble();
                    break;
                case ("EquityWithLoanValue-C"):
                    EquityWithLoanValue_C = value.ToDouble();
                    break;
                case ("ExcessLiquidity"):
                    ExcessLiquidity = value.ToDouble();
                    break;
                case ("ExcessLiquidity-S"):
                    ExcessLiquidity_S = value.ToDouble();
                    break;
                case ("ExcessLiquidity-C"):
                    ExcessLiquidity_C = value.ToDouble();
                    break;
                case ("FullAvailableFunds"):
                    FullAvailableFunds = value.ToDouble();
                    break;
                case ("FullAvailableFunds-S"):
                    FullAvailableFunds_S = value.ToDouble();
                    break;
                case ("FullAvailableFunds-C"):
                    FullAvailableFunds_C = value.ToDouble();
                    break;
                case ("FullExcessLiquidity"):
                    FullExcessLiquidity = value.ToDouble();
                    break;
                case ("FullExcessLiquidity-S"):
                    FullExcessLiquidity_S = value.ToDouble();
                    break;
                case ("FullExcessLiquidity-C"):
                    FullExcessLiquidity_C = value.ToDouble();
                    break;
                case ("FullInitMarginReq"):
                    FullInitMarginReq = value.ToDouble();
                    break;
                case ("FullInitMarginReq-S"):
                    FullInitMarginReq_S = value.ToDouble();
                    break;
                case ("FullInitMarginReq-C"):
                    FullInitMarginReq_C = value.ToDouble();
                    break;
                case ("FullMaintMarginReq"):
                    FullMaintMarginReq = value.ToDouble();
                    break;
                case ("FullMaintMarginReq-S"):
                    FullMaintMarginReq_S = value.ToDouble();
                    break;
                case ("FullMaintMarginReq-C"):
                    FullMaintMarginReq_C = value.ToDouble();
                    break;
                case ("GrossPositionValue"):
                    PositionValueIB = value.ToDouble();
                    break;
                case ("GrossPositionValue-S"):
                    PositionValueIB_S = value.ToDouble();
                    break;
                case ("Guarantee"):
                    Guarantee = value.ToDouble();
                    break;
                case ("Guarantee-S"):
                    Guarantee_S = value.ToDouble();
                    break;
                case ("Guarantee-C"):
                    Guarantee_C = value.ToDouble();
                    break;
                case ("IndianStockHaircut"):
                    IndianStockHaircut = value.ToDouble();
                    break;
                case ("IndianStockHaircut-S"):
                    IndianStockHaircut_S = value.ToDouble();
                    break;
                case ("IndianStockHaircut-C"):
                    IndianStockHaircut_C = value.ToDouble();
                    break;
                case ("InitMarginReq"):
                    InitMarginReq = value.ToDouble();
                    break;
                case ("InitMarginReq-S"):
                    InitMarginReq_S = value.ToDouble();
                    break;
                case ("InitMarginReq-C"):
                    InitMarginReq_C = value.ToDouble();
                    break;
                case ("LookAheadAvailableFunds"):
                    LookAheadAvailableFunds = value.ToDouble();
                    break;
                case ("LookAheadAvailableFunds-S"):
                    LookAheadAvailableFunds_S = value.ToDouble();
                    break;
                case ("LookAheadAvailableFunds-C"):
                    LookAheadAvailableFunds_C = value.ToDouble();
                    break;
                case ("LookAheadExcessLiquidity"):
                    LookAheadExcessLiquidity = value.ToDouble();
                    break;
                case ("LookAheadExcessLiquidity-S"):
                    LookAheadExcessLiquidity_S = value.ToDouble();
                    break;
                case ("LookAheadExcessLiquidity-C"):
                    LookAheadExcessLiquidity_C = value.ToDouble();
                    break;
                case ("LookAheadInitMarginReq"):
                    LookAheadInitMarginReq = value.ToDouble();
                    break;
                case ("LookAheadInitMarginReq-S"):
                    LookAheadInitMarginReq_S = value.ToDouble();
                    break;
                case ("LookAheadInitMarginReq-C"):
                    LookAheadInitMarginReq_C = value.ToDouble();
                    break;
                case ("LookAheadMaintMarginReq"):
                    LookAheadMaintMarginReq = value.ToDouble();
                    break;
                case ("LookAheadMaintMarginReq-S"):
                    LookAheadMaintMarginReq_S = value.ToDouble();
                    break;
                case ("LookAheadMaintMarginReq-C"):
                    LookAheadMaintMarginReq_C = value.ToDouble();
                    break;
                case ("MaintMarginReq"):
                    MaintMarginReq = value.ToDouble();
                    break;
                case ("MaintMarginReq-S"):
                    MaintMarginReq_S = value.ToDouble();
                    break;
                case ("MaintMarginReq-C"):
                    MaintMarginReq_C = value.ToDouble();
                    break;
                case ("NetLiquidation"):
                    NetLiquidation = value.ToDouble();
                    break;
                case ("NetLiquidation-S"):
                    NetLiquidation_S = value.ToDouble();
                    break;
                case ("NetLiquidation-C"):
                    NetLiquidation_C = value.ToDouble();
                    break;
                case ("NetLiquidationUncertainty"):
                    NetLiquidationUncertainty = value.ToDouble();
                    break;
                case ("PASharesValue"):
                    PASharesValue = value.ToDouble();
                    break;
                case ("PASharesValue-S"):
                    PASharesValue_S = value.ToDouble();
                    break;
                case ("PASharesValue-C"):
                    PASharesValue_C = value.ToDouble();
                    break;
                case ("PostExpirationExcess"):
                    PostExpirationExcess = value.ToDouble();
                    break;
                case ("PostExpirationExcess-S"):
                    PostExpirationExcess_S = value.ToDouble();
                    break;
                case ("PostExpirationExcess-C"):
                    PostExpirationExcess_C = value.ToDouble();
                    break;
                case ("PostExpirationMargin"):
                    PostExpirationMargin = value.ToDouble();
                    break;
                case ("PostExpirationMargin-S"):
                    PostExpirationMargin_S = value.ToDouble();
                    break;
                case ("PostExpirationMargin-C"):
                    PostExpirationMargin_C = value.ToDouble();
                    break;
                case ("PreviousDayEquityWithLoanValue"):
                    PreviousDayEquityWithLoanValue = value.ToDouble();
                    break;
                case ("PreviousDayEquityWithLoanValue-S"):
                    PreviousDayEquityWithLoanValue_S = value.ToDouble();
                    break;
                case ("RegTEquity"):
                    RegTEquity = value.ToDouble();
                    break;
                case ("RegTEquity-S"):
                    RegTEquity_S = value.ToDouble();
                    break;
                case ("RegTMargin"):
                    RegTMargin = value.ToDouble();
                    break;
                case ("RegTMargin-S"):
                    RegTMargin_S = value.ToDouble();
                    break;
                case ("TotalDebitCardPendingCharges"):
                    TotalDebitCardPendingCharges = value.ToDouble();
                    break;
                case ("TotalDebitCardPendingCharges-S"):
                    TotalDebitCardPendingCharges_S = value.ToDouble();
                    break;
                case ("TotalDebitCardPendingCharges-C"):
                    TotalDebitCardPendingCharges_C = value.ToDouble();
                    break;
                case ("CorporateBondValue"):
                    CorporateBondValue = value.ToDouble();
                    break;
                case ("TBondValue"):
                    TBondValue = value.ToDouble();
                    break;
                case ("TBillValue"):
                    TBillValue = value.ToDouble();
                    break;
                case ("WarrantValue"):
                    WarrantValue = value.ToDouble();
                    break;
                case ("FxCashBalance"):
                    FxCashBalance = value.ToDouble();
                    break;
                case ("IssuerOptionValue"):
                    IssuerOptionValue = value.ToDouble();
                    break;
                default:
                    Log.Print("### Unknown Tag: " + tagName);
                    break;
            }

            Updated();
        }

        #region Equality

        public bool Equals(AccountInfo other) => AccountId == other.AccountId;
        public bool Equals(string other) => AccountId == other;

        public static bool operator ==(AccountInfo s1, AccountInfo s2) => s1.Equals(s2);
        public static bool operator !=(AccountInfo s1, AccountInfo s2) => !s1.Equals(s2);
        public static bool operator ==(AccountInfo s1, string s2) => s1.Equals(s2);
        public static bool operator !=(AccountInfo s1, string s2) => !s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is AccountInfo ac)
                return Equals(ac);
            else if (other is string s)
                return Equals(s);
            else
                return false;
        }

        public override int GetHashCode() => AccountId.GetHashCode();

        #endregion Equality

        #region Grid View

        public static readonly StringColumn Column_Account = new StringColumn("ACCOUNT");

        #endregion Grid View
    }
}
