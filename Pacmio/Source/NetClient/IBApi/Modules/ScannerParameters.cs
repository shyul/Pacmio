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
using System.Text;
using Xu;
using IbXmlScannerParameter;

namespace Pacmio.IB
{
    public partial class Client
    {
        public bool IsActiveRequestScannerParameters = false;

        public ScanParameterResponse ScanParameters { get; set; }

        public void RequestScannerParameters()
        {
            if (Connected && !IsActiveRequestScannerParameters)
            {
                IsActiveRequestScannerParameters = true;
                SendRequest(new string[] { RequestType.RequestScannerParameters.Param(), "1" });
            }
        }

        private void Parse_ScannerParameters(string[] fields)
        {
            //Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToFlat());
            string msgVersion = fields[1];
            if(msgVersion == "1")
            {
                string xmlString = fields[2];
                ScanParameters = Serialization.DeserializeXML<ScanParameterResponse>(xmlString);

               

                //Console.WriteLine("\n\n" + xmlString);

                ScanParameters.SerializeXMLFile("D:\\test.xml");

                List<string> allFilterList = new List<string>();

                foreach(var ins in ScanParameters.InstrumentList[0].Instrument) 
                {
                    Console.WriteLine(ins.Type + ": " + ins.Name);

                    if (ins.Filters is null) continue;

                    string[] filterList = ins.Filters.Split(',');
                    allFilterList.CheckAdd(filterList);
                }

                Console.WriteLine("\n\n");

                foreach(string code in allFilterList) 
                {
                    Console.Write(code + ",");
                }

                Console.WriteLine("\n\n");

                List<string> codeList = new List<string>();

                foreach (var flt in ScanParameters.FilterList[0].RangeFilter) 
                {
                    foreach(var abf in flt.AbstractField) 
                    {
                        string code = abf.Code;
                        codeList.CheckAdd(code);
                    }
                }

                foreach (var flt in ScanParameters.FilterList[0].SimpleFilter)
                {
                    string code = flt.AbstractField.Code;
                    codeList.CheckAdd(code);
                    codeList.CheckAdd(code);
                }

                foreach (string code in codeList)
                {
                    Console.Write(code + ",");
                }

            }




            Console.WriteLine("\n\nmessage version = " + msgVersion);

            IsActiveRequestScannerParameters = false;
        }
    }
}


/*

IBClient [ 16:34:16 - Connected ]: Connect Success: 1/30/2020 4:34:16 PM
Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"
Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2106"-(4)"HMDS data farm connection is OK:ushmds"
Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2158"-(4)"Sec-def data farm connection is OK:secdefnj"
Send RequestScannerParameters: (0)"24"-(1)"1"


    STK: US Stocks
    ETF.EQ.US: US Equity ETFs
    ETF.FI.US: US Fixed Income ETFs
    FUT.US: US Futures
    IND.US: US Indexes
    SSF.US: US SSFs
    BOND: Corporate Bonds
    BOND.CD: US CDs
    BOND.AGNCY: US Agency Bonds
    BOND.GOVT: US Treasuries
    BOND.MUNI: US Municipal Bonds
    BOND.GOVT.NON-US: Non-US Sovereign Bonds
    SLB.US: US SBLs
    STOCK.NA: America Non-US Stocks
    FUT.NA: America Non-US Futures
    SSF.NA: America Non-US SSFs
    STOCK.EU: Europe Stocks
    FUT.EU: Europe Futures
    IND.EU: Europe Indexes
    WAR.EU: Europe Warrants
    SSF.EU: Europe SSFs
    STOCK.ME: MidEast Stocks
    STOCK.HK: Asia Stocks
    FUT.HK: Asia Futures
    IND.HK: Asia Indexes
    SSF.HK: Asia SSFs
    NATCOMB: Native Combos
    Global: Global Stocks
    Global: Global Futures
    Global: Global Indexes
    Global: Global SSFs



AFTERHRSCHANGEPERC,AVGOPTVOLUME,AVGPRICETARGET,AVGRATING,AVGTARGET2PRICERATIO,AVGVOLUME,AVGVOLUME_USD,CHANGEOPENPERC,CHANGEPERC,
EMA_20,EMA_50,EMA_100,EMA_200,PRICE_VS_EMA_20,PRICE_VS_EMA_50,PRICE_VS_EMA_100,PRICE_VS_EMA_200,DAYSTOCOVER,DIVIB,DIVYIELD,DIVYIELDIB,FEERATE,FIRSTTRADEDATE,GROWTHRATE,HALTED,HASOPTIONS,HISTDIVIB,HISTDIVYIELDIB,
IMBALANCE,IMBALANCEADVRATIOPERC,IMPVOLAT,IMPVOLATOVERHIST,INSIDEROFFLOATPERC,INSTITUTIONALOFFLOATPERC,MACD,MACD_SIGNAL,MACD_HISTOGRAM,MKTCAP,MKTCAP_USD,
NEXTDIVAMOUNT,NEXTDIVDATE,NUMPRICETARGETS,NUMRATINGS,
NUMSHARESINSIDER,NUMSHARESINSTITUTIONAL,NUMSHARESSHORT,OPENGAPPERC,OPTVOLUME,OPTVOLUMEPCRATIO,PERATIO,
PPO,PPO_SIGNAL,PPO_HISTOGRAM,PRICE,PRICE2BK,PRICE2TANBK,PRICERANGE,PRICE_USD,QUICKRATIO,REBATERATE,REGIMBALANCE,REGIMBALANCEADVRATIOPERC,RETEQUITY,SHORTABLESHARES,SHORTOFFLOATPERC,SHORTSALERESTRICTED,
SIC,ISSUER_COUNTRY_CODE,
SOCSACT,SOCSNET,STKTYPE,STVOLUME_3MIN,STVOLUME_5MIN,STVOLUME_10MIN,TRADECOUNT,TRADERATE,UNSHORTABLE,VOLUME,VOLUMERATE,VOLUME_USD,RCGLTCLASS,RCGLTENDDATE,RCGLTIVALUE,RCGLTTRADE,RCGITCLASS,RCGITENDDATE,RCGITIVALUE,RCGITTRADE,RCGSTCLASS,RCGSTENDDATE,RCGSTIVALUE,RCGSTTRADE,ESG_SCORE,ESG_COMBINED_SCORE,ESG_CONTROVERSIES_SCORE,ESG_RESOURCE_USE_SCORE,ESG_EMISSIONS_SCORE,ESG_ENV_INNOVATION_SCORE,ESG_WORKFORCE_SCORE,ESG_HUMAN_RIGHTS_SCORE,ESG_COMMUNITY_SCORE,ESG_PRODUCT_RESPONSIBILITY_SCORE,ESG_MANAGEMENT_SCORE,ESG_SHAREHOLDERS_SCORE,ESG_CSR_STRATEGY_SCORE,ESG_ENV_PILLAR_SCORE,ESG_SOCIAL_PILLAR_SCORE,ESG_CORP_GOV_PILLAR_SCORE,IV_RANK13,IV_RANK26,IV_RANK52,IV_PERCENTILE13,IV_PERCENTILE26,IV_PERCENTILE52,HV_RANK13,HV_RANK26,HV_RANK52,HV_PERCENTILE13,HV_PERCENTILE26,HV_PERCENTILE52,PRICE_2_SALES,EQUITY_PER_SHARE,UTILIZATION,AVASSETS,AVEXPENSE,AVALTAR,AVAVGALTAR,AVTRYTD,AVTR1YR,AVTR5YR,AVTR10YR,AVTRINCEP,AVTRACKINGDIFFPCT,AVBIDASKPCT,AVEMG,AVDEV,AV5YREPS,AVLEVERAGE,AVASSETTURNS,AV1MOCHNG,AV3MOCHNG,AVPAYOUT,AVFWD_PE,AVFWD_PCF,AVFWD_YLD,AVBETASPX,AVLTG,AVSHT_INT,AVRSI,AVCOMP_COUNT,AVDISTRIBFREQ,AVFYPRVBVPS,AVFYPRVDY,AVFYPRVDPG,AVFYPRVEPS,AVFYPRVYOY,AVFYPRVNET,AVFYPRVPBV,AVFYPRVPCF,AVFYPRVPE,AVFYPRVPEG,AVFYPRVPS,AVFYPRVROE,AVFYPRVSPS,AVFYPRVSALYOY,AVFYCURBVPS,AVFYCURDY,AVFYCURDPG,AVFYCUREPS,AVFYCURYOY,AVFYCURNET,AVFYCURPBV,AVFYCURPCF,AVFYCURPE,AVFYCURPEG,AVFYCURPS,AVFYCURROE,AVFYCURSPS,AVFYCURSALYOY,AVFYNXTBVPS,AVFYNXTDY,AVFYNXTDPG,AVFYNXTEPS,AVFYNXTYOY,AVFYNXTNET,AVFYNXTPBV,AVFYNXTPCF,AVFYNXTPE,AVFYNXTPEG,AVFYNXTPS,AVFYNXTROE,AVFYNXTSPS,AVFYNXTSALYOY,AVYTM_WGTAVG,AVCPN_WGTAVG,AVCURYLD_WGTAVG,AVMATURITY_WGTAVG,AVDUR_WGTAVG,AVMOD_DUR_WGTAVG,AVCONVEX_WGTAVG,AVFINUM_DISTINCT,AVPCT_FIXED,AVPCT_AT_MTY,AVMOODYRATING,AVSPRATING,PRODCAT,LEADFUT,MOODY,SP,BOND_RATINGS_RELATION,MATDATE,CURRENCY,CPNRATE,CONVOPT,BONDCREDITRATING,BOND_STK_SYMBOL,BOND_ISSUER,BOND_PAYMENT_FREQ,BOND_BID_OR_ASK_VALID,BOND_BID_SZ_VALUE,BOND_ASK_SZ_VALUE,BOND_BID_OR_ASK_SZ_VALUE,BOND_BID,BOND_ASK,BOND_BID_OR_ASK,BOND_BID_YIELD,BOND_ASK_YIELD,BOND_LAST_YIELD,BOND_BID_OR_ASK_YIELD,BOND_BID_CURR_YIELD,BOND_ASK_CURR_YIELD,BOND_BID_OR_ASK_CURR_YIELD,BOND_AMT_OUTSTANDING,BOND_SPREAD,BOND_CALL_PROT,BOND_DURATION,BOND_CONVEXITY,BOND_OAS,BOND_DV01,BOND_STK_MKTCAP,BOND_DEBT_OUTSTANDING,BOND_DEBT_2_BOOK_RATIO,BOND_DEBT_2_TAN_BOOK_RATIO,BOND_EQUITY_2_BOOK_RATIO,BOND_EQUITY_2_TAN_BOOK_RATIO,DEFAULTED,TRADING_FLAT,EXCHLISTED,CALLOPT,FDICINS,VARCPNRATE,BOND_GOVT_SUBTYPE,BOND_DEBT_OUTSTANDING_MUNI,BOND_US_STATE,INSURED,GENERAL_OBLIGATION,REVENUE,SUBJECT_TO_AMT,REFUNDED,NO_FEDERAL_TAX,BANK_QUALIFIED,BUILD_AMERICA,UNDCONID,

shortInterestChgAbove,shortInterestChgBelow,
shortOutstandingRatioAbove,shortOutstandingRatioBelow,
priceAbove,priceBelow,
usdPriceAbove,usdPriceBelow,
usdVolumeAbove,usdVolumeBelow,
avgVolumeAbove,avgVolumeBelow,
avgUsdVolumeAbove,avgUsdVolumeBelow,
ihNumSharesInsiderAbove,ihNumSharesInsiderBelow,
ihInsiderOfFloatPercAbove,ihInsiderOfFloatPercBelow,
iiNumSharesInstitutionalAbove,iiNumSharesInstitutionalBelow,
iiInstitutionalOfFloatPercAbove,iiInstitutionalOfFloatPercBelow,
marketCapAbove1e6,marketCapBelow1e6,
moodyRatingAbove,moodyRatingBelow,
spRatingAbove,spRatingBelow,
maturityDateAbove,maturityDateBelow,
couponRateAbove,couponRateBelow,
optVolumeAbove,optVolumeBelow,
optVolumePCRatioAbove,optVolumePCRatioBelow,
impVolatAbove,impVolatBelow,
impVolatOverHistAbove,impVolatOverHistBelow,
imbalanceAbove,imbalanceBelow,
displayImbalanceAdvRatioAbove,displayImbalanceAdvRatioBelow,
regulatoryImbalanceAbove,regulatoryImbalanceBelow,
displayRegulatoryImbAdvRatioAbove,displayRegulatoryImbAdvRatioBelow,
avgRatingAbove,avgRatingBelow,
numRatingsAbove,numRatingsBelow,
avgPriceTargetAbove,avgPriceTargetBelow,
numPriceTargetsAbove,numPriceTargetsBelow,
avgAnalystTarget2PriceRatioAbove,avgAnalystTarget2PriceRatioBelow,
dividendFrdAbove,dividendFrdBelow,
dividendYieldFrdAbove,dividendYieldFrdBelow,
dividendNextDateAbove,dividendNextDateBelow,
dividendNextAmountAbove,dividendNextAmountBelow,
histDividendFrdAbove,histDividendFrdBelow,
histDividendFrdYieldAbove,histDividendFrdYieldBelow,
minGrowthRate,maxGrowthRate,
minPeRatio,maxPeRatio,
minQuickRatio,maxQuickRatio,
minRetnOnEq,maxRetnOnEq,
minPrice2Bk,maxPrice2Bk,
minPrice2TanBk,maxPrice2TanBk,
firstTradeDateAbove,firstTradeDateBelow,
changePercAbove,changePercBelow,
afterHoursChangePercAbove,afterHoursChangePercBelow,
changeOpenPercAbove,changeOpenPercBelow,
openGapPercAbove,openGapPercBelow,
priceRangeAbove,priceRangeBelow,
tradeCountAbove,tradeCountBelow,
tradeRateAbove,tradeRateBelow,
volumeRateAbove,volumeRateBelow,
stVolume3minAbove,stVolume3minBelow,
stVolume5minAbove,stVolume5minBelow,
stVolume10minAbove,stVolume10minBelow,
sharesAvailableManyAbove,sharesAvailableManyBelow,
feeRateAbove,feeRateBelow,
rebateRateAbove,rebateRateBelow,
utilizationAbove,utilizationBelow,
bondNetBidAbove,bondNetBidBelow,
bondNetAskAbove,bondNetAskBelow,
bondNetBidOrAskAbove,bondNetBidOrAskBelow,
bondNetBidSizeValueAbove,bondNetBidSizeValueBelow,
bondNetAskSizeValueAbove,bondNetAskSizeValueBelow,
bondNetBidOrAskSizeValueAbove,bondNetBidOrAskSizeValueBelow,
bondNetBidYieldAbove,bondNetBidYieldBelow,
bondNetAskYieldAbove,bondNetAskYieldBelow,
bondNetBidOrAskYieldAbove,bondNetBidOrAskYieldBelow,bondNetSpreadAbove,bondNetSpreadBelow,bondAmtOutstandingAbove,bondAmtOutstandingBelow,bondNextCallDateAbove,bondNextCallDateBelow,bondDurationAbove,bondDurationBelow,
bondConvexityAbove,bondConvexityBelow,bondStkMarketCapAbove,bondStkMarketCapBelow,bondDebtOutstandingAbove,bondDebtOutstandingBelow,bondDebtOutstandingMuniAbove,bondDebtOutstandingMuniBelow,bondDebt2EquityRatioAbove,bondDebt2EquityRatioBelow,
bondDebt2BookRatioAbove,bondDebt2BookRatioBelow,bondDebt2TanBookRatioAbove,bondDebt2TanBookRatioBelow,bondEquity2BookRatioAbove,bondEquity2BookRatioBelow,bondEquity2TanBookRatioAbove,bondEquity2TanBookRatioBelow,
socialSentimentNetAbove,socialSentimentNetBelow,socialSentimentActivityAbove,socialSentimentActivityBelow,etfAssetsAbove,etfAssetsBelow,etfExpenseAbove,etfExpenseBelow,etfALTARAbove,etfALTARBelow,etfAvgALTARAbove,etfAvgALTARBelow,
etfTRytdAbove,etfTRytdBelow,etfTR1yrAbove,etfTR1yrBelow,etfTR5yrAbove,etfTR5yrBelow,etfTR10yrAbove,etfTR10yrBelow,etfTRIncepAbove,etfTRIncepBelow,etfTrackingDiffPctAbove,etfTrackingDiffPctBelow,etfBidAskPctAbove,etfBidAskPctBelow,
etfEmgAbove,etfEmgBelow,etfDevAbove,etfDevBelow,etf5yrEPSAbove,etf5yrEPSBelow,etfLeverageAbove,etfLeverageBelow,etfAssetTurnsAbove,etfAssetTurnsBelow,etf1moChngAbove,etf1moChngBelow,etf3moChngAbove,etf3moChngBelow,etfPayoutAbove,etfPayoutBelow,
etfFwd_PEAbove,etfFwd_PEBelow,etfFwd_PCFAbove,etfFwd_PCFBelow,etfFwd_YldAbove,etfFwd_YldBelow,etfBetaSPXAbove,etfBetaSPXBelow,etfLTGAbove,etfLTGBelow,etfSht_IntAbove,etfSht_IntBelow,etfRSIAbove,etfRSIBelow,etfYTM_WgtAvgAbove,etfYTM_WgtAvgBelow,
etfCPN_WgtAvgAbove,etfCPN_WgtAvgBelow,etfCurYld_WgtAvgAbove,etfCurYld_WgtAvgBelow,etfMaturity_WgtAvgAbove,etfMaturity_WgtAvgBelow,etfDUR_WgtAvgAbove,etfDUR_WgtAvgBelow,etfMOD_DUR_WgtAvgAbove,etfMOD_DUR_WgtAvgBelow,
etfCONVEX_WgtAvgAbove,etfCONVEX_WgtAvgBelow,etfNum_DistinctAbove,etfNum_DistinctBelow,etfComponent_CountAbove,etfComponent_CountBelow,etfPct_FixedAbove,etfPct_FixedBelow,etfPct_at_MtyAbove,etfPct_at_MtyBelow,etfMoodyRatingAbove,etfMoodyRatingBelow,
etfSPRatingAbove,etfSPRatingBelow,prevYrETFFYBVPerShareAbove,prevYrETFFYBVPerShareBelow,prevYrETFFYDividendYieldAbove,prevYrETFFYDividendYieldBelow,prevYrETFFYDPSAbove,prevYrETFFYDPSBelow,prevYrETFFYEPSAbove,prevYrETFFYEPSBelow,
prevYrETFFYEPSGrowthAbove,prevYrETFFYEPSGrowthBelow,prevYrETFFYNetMarginAbove,prevYrETFFYNetMarginBelow,prevYrETFFYPriceToBookValueAbove,prevYrETFFYPriceToBookValueBelow,prevYrETFFYPriceToCashFlowAbove,prevYrETFFYPriceToCashFlowBelow,
prevYrETFFYPriceToEarningsAbove,prevYrETFFYPriceToEarningsBelow,prevYrETFFYPriceToGrowthAbove,prevYrETFFYPriceToGrowthBelow,prevYrETFFYPriceToSalesAbove,prevYrETFFYPriceToSalesBelow,prevYrETFFYRoEAbove,prevYrETFFYRoEBelow,
prevYrETFFYSPSAbove,prevYrETFFYSPSBelow,prevYrETFFYSPSGrowthAbove,prevYrETFFYSPSGrowthBelow,currYrETFFYBVPerShareAbove,currYrETFFYBVPerShareBelow,currYrETFFYDividendYieldAbove,currYrETFFYDividendYieldBelow,currYrETFFYDPSAbove,currYrETFFYDPSBelow,
currYrETFFYEPSAbove,currYrETFFYEPSBelow,currYrETFFYEPSGrowthAbove,currYrETFFYEPSGrowthBelow,currYrETFFYNetMarginAbove,currYrETFFYNetMarginBelow,currYrETFFYPriceToBookValueAbove,currYrETFFYPriceToBookValueBelow,
currYrETFFYPriceToCashFlowAbove,currYrETFFYPriceToCashFlowBelow,currYrETFFYPriceToEarningsAbove,currYrETFFYPriceToEarningsBelow,currYrETFFYPriceToGrowthAbove,currYrETFFYPriceToGrowthBelow,currYrETFFYPriceToSalesAbove,currYrETFFYPriceToSalesBelow,
currYrETFFYRoEAbove,currYrETFFYRoEBelow,currYrETFFYSPSAbove,currYrETFFYSPSBelow,currYrETFFYSPSGrowthAbove,currYrETFFYSPSGrowthBelow,ford_researchAbove,ford_researchBelow,street_ratingsAbove,street_ratingsBelow,
valuengineAbove,valuengineBelow,sadif_analyticsAbove,sadif_analyticsBelow,market_edgeAbove,market_edgeBelow,validea_guruAbove,validea_guruBelow,market_graderAbove,market_graderBelow,argusAbove,argusBelow,morningstarAbove,morningstarBelow,
constructs_researchAbove,constructs_researchBelow,zacksAbove,zacksBelow,reutEPSChgPct_TTMAbove,reutEPSChgPct_TTMBelow,reutPENorm_MRYAbove,reutPENorm_MRYBelow,reutPayoutRatioPct_TTMAbove,reutPayoutRatioPct_TTMBelow,reutRev_TTMAbove,reutRev_TTMBelow,reutEBT_TTMAbove,reutEBT_TTMBelow,reutTotDebtTotEquityRatio_MRQ_Above,reutTotDebtTotEquityRatio_MRQ_Below,reutRevChgPct_TTMAbove,reutRevChgPct_TTMBelow,reutPrice2CashFlowPerShare_TTMAbove,reutPrice2CashFlowPerShare_TTMBelow,reutCurrentRatio_MRQAbove,reutCurrentRatio_MRQBelow,reutLongTermDebt2Equity_MRQAbove,reutLongTermDebt2Equity_MRQBelow,reutNetIncomeAvlble2Common_TTMAbove,reutNetIncomeAvlble2Common_TTMBelow,reutNetProfitMarginPct_TTMAbove,reutNetProfitMarginPct_TTMBelow,reutRevChgPctAbove,reutRevChgPctBelow,reutPrice2SalesRatio_TTMAbove,reutPrice2SalesRatio_TTMBelow,reutGrossMargin_TTMAbove,reutGrossMargin_TTMBelow,reutEBTNorm_MRYAbove,reutEBTNorm_MRYBelow,reutNetIncomeAvlble2CommonNorm_MRYAbove,reutNetIncomeAvlble2CommonNorm_MRYBelow,reutReturnOnAvgAssetsPct_TTMAbove,reutReturnOnAvgAssetsPct_TTMBelow,reutReturnOnInvestmentPct_TTMAbove,reutReturnOnInvestmentPct_TTMBelow,reutOperatingMargin_TTMAbove,reutOperatingMargin_TTMBelow,reutEPSChgPctAbove,reutEPSChgPctBelow,reutPretaxMarginPct_MRYAbove,reutPretaxMarginPct_MRYBelow,reutEBITD_TTMAbove,reutEBITD_TTMBelow,ibPriceYTDPctAbove,ibPriceYTDPctBelow,ibFrac52wkAbove,ibFrac52wkBelow,reutPretaxMargin_TTMAbove,reutPretaxMargin_TTMBelow,reutFreeCashFlow_TTMAbove,reutFreeCashFlow_TTMBelow,reutNetDebtIAbove,reutNetDebtIBelow,reutBetaAbove,reutBetaBelow,ibPriceYTDPctR_Above,ibPriceYTDPctR_Below,reutEnterpriseValueCurAbove,reutEnterpriseValueCurBelow,curEMA20Above,curEMA20Below,curEMA50Above,curEMA50Below,curEMA100Above,curEMA100Below,curEMA200Above,curEMA200Below,lastVsEMAChangeRatio20Above,lastVsEMAChangeRatio20Below,lastVsEMAChangeRatio50Above,lastVsEMAChangeRatio50Below,lastVsEMAChangeRatio100Above,lastVsEMAChangeRatio100Below,lastVsEMAChangeRatio200Above,lastVsEMAChangeRatio200Below,curMACDAbove,curMACDBelow,curMACDSignalAbove,curMACDSignalBelow,curMACDDistAbove,curMACDDistBelow,curPPOAbove,curPPOBelow,curPPOSignalAbove,curPPOSignalBelow,curPPODistAbove,curPPODistBelow,reutCurrentAssets_MRQAbove,reutCurrentAssets_MRQBelow,reutCurrentAssets_MRYAbove,reutCurrentAssets_MRYBelow,reutCashFlowPerShare_MRYAbove,reutCashFlowPerShare_MRYBelow,reutEBIT_MRQAbove,reutEBIT_MRQBelow,reutEBIT_MRYAbove,reutEBIT_MRYBelow,reutEPS_MRYAbove,reutEPS_MRYBelow,reutIssuanceOfDebt_MRQAbove,reutIssuanceOfDebt_MRQBelow,reutIssuanceOfDebt_MRYAbove,reutIssuanceOfDebt_MRYBelow,reutIssuanceOfStock_MRQAbove,reutIssuanceOfStock_MRQBelow,reutIssuanceOfStock_MRYAbove,reutIssuanceOfStock_MRYBelow,reutShortTermDebt_MRQAbove,reutShortTermDebt_MRQBelow,reutShortTermDebt_MRYAbove,reutShortTermDebt_MRYBelow,reutCurrentLiabilities_MRQAbove,reutCurrentLiabilities_MRQBelow,reutCurrentLiabilities_MRYAbove,reutCurrentLiabilities_MRYBelow,reutCashFromOperating_MRQAbove,reutCashFromOperating_MRQBelow,reutCashFromOperating_MRYAbove,reutCashFromOperating_MRYBelow,reutRevenuePerShare_MRYAbove,reutRevenuePerShare_MRYBelow,reutReturnOnAvgAssetsPct_MRYAbove,reutReturnOnAvgAssetsPct_MRYBelow,reutReturnOnInvestmentPct_MRYAbove,reutReturnOnInvestmentPct_MRYBelow,reutCapitalExpenditures_MRQAbove,reutCapitalExpenditures_MRQBelow,reutCapitalExpenditures_MRYAbove,reutCapitalExpenditures_MRYBelow,reutCashFromFinancing_MRQAbove,reutCashFromFinancing_MRQBelow,reutCashFromFinancing_MRYAbove,reutCashFromFinancing_MRYBelow,reutCashFromInvesting_MRQAbove,reutCashFromInvesting_MRQBelow,reutCashFromInvesting_MRYAbove,reutCashFromInvesting_MRYBelow,reutInterestExpense_MRQAbove,reutInterestExpense_MRQBelow,reutInterestExpense_MRYAbove,reutInterestExpense_MRYBelow,reutOperatingIncome_MRQAbove,reutOperatingIncome_MRQBelow,reutOperatingIncome_MRYAbove,reutOperatingIncome_MRYBelow,reutEnterpriseValueToEBITDAAbove,reutEnterpriseValueToEBITDABelow,ibPrice1WKPctAbove,ibPrice1WKPctBelow,ibPrice4WKPctAbove,ibPrice4WKPctBelow,ibPrice13WKPctAbove,ibPrice13WKPctBelow,ibPrice52WKPctAbove,ibPrice52WKPctBelow,reutYieldAbove,reutYieldBelow,reutCash_MRQAbove,reutCash_MRQBelow,reutEBITDA_MRQAbove,reutEBITDA_MRQBelow,reutPrice2SalesRatio_MRQAbove,reutPrice2SalesRatio_MRQBelow,reutPrice2CashFlowPerShare_MRQAbove,reutPrice2CashFlowPerShare_MRQBelow,reutTotalAssets_MRQAbove,reutTotalAssets_MRQBelow,reutTotalLiabilities_MRQAbove,reutTotalLiabilities_MRQBelow,reutTotalCommonEquity_MRQAbove,reutTotalCommonEquity_MRQBelow,reutTotalLongTermDebt_MRQAbove,reutTotalLongTermDebt_MRQBelow,reutBookValueTanPerShare_MRQAbove,reutBookValueTanPerShare_MRQBelow,reutCashFlowPerShare_TTMAbove,reutCashFlowPerShare_TTMBelow,reutEPS_TTMAbove,reutEPS_TTMBelow,reutRev2Share_TTMAbove,reutRev2Share_TTMBelow,sharesOutstandingAbove,sharesOutstandingBelow,esgScoreAbove,esgScoreBelow,esgCombinedScoreAbove,esgCombinedScoreBelow,esgControversiesScoreAbove,esgControversiesScoreBelow,esgResourceUseScoreAbove,esgResourceUseScoreBelow,esgEmissionsScoreAbove,esgEmissionsScoreBelow,esgEnvInvScoreAbove,esgEnvInvScoreBelow,esgWorkforceScoreAbove,esgWorkforceScoreBelow,esgHrScoreAbove,esgHrScoreBelow,esgCommunityScoreAbove,esgCommunityScoreBelow,esgProdRespScoreAbove,esgProdRespScoreBelow,esgManagementScoreAbove,esgManagementScoreBelow,esgShareholdersScoreAbove,esgShareholdersScoreBelow,esgStrategyScoreAbove,esgStrategyScoreBelow,esgEnvPillarScoreAbove,esgEnvPillarScoreBelow,esgSocialPillarScoreAbove,esgSocialPillarScoreBelow,esgCorpGovPillarScoreAbove,esgCorpGovPillarScoreBelow,ivRank13wAbove,ivRank13wBelow,ivRank26wAbove,ivRank26wBelow,ivRank52wAbove,ivRank52wBelow,ivPercntl13wAbove,ivPercntl13wBelow,ivPercntl26wAbove,ivPercntl26wBelow,ivPercntl52wAbove,ivPercntl52wBelow,hvRank13wAbove,hvRank13wBelow,HVRank26wAbove,HVRank26wBelow,HVRank52wAbove,HVRank52wBelow,HVPercntl13wAbove,HVPercntl13wBelow,HVPercntl26wAbove,HVPercntl26wBelow,HVPercntl52wAbove,HVPercntl52wBelow,volumeAbove,ratingsRelation,bondCreditRating,currencyLike,excludeConvertible,avgOptVolumeAbove,stkTypes,hasOptionsIs,leadFutOnly,prodCatIs,bondUSStateLike,,bondStkSymbolIs,bondIssuerLike,bondCallableIs,unshortableIs,haltedIs,shortSaleRestrictionIs,bondDefaultedIs,bondTradingFlatIs,bondExchListedIs,bondFdicInsIs,bondVarCouponRateIs,bondInsuredIs,bondGeneralObligationIs,bondRevenueIs,bondSubjectToAmtIs,bondRefundedIs,bondNoFederalTaxIs,bondBankQualifiedIs,bondBuildAmericaIs,bondValidNetBidOrAskOnly,bondPaymentFreqIs,bondStructRelated,bondAssetSubTypeLike,underConID,etfDistribFreqIs,btOptObj,btOptVendor,btNotional,btLeverageLong,btLeverageShort,btLongPosTopBottom,btLongPosRank,btShortPosTopBottom,btShortPosRank,btWeight,Rebalance,btRebalanceFrequency,btTestPeriod,btBenchmark,

message version = 1
 
     
     
     */
