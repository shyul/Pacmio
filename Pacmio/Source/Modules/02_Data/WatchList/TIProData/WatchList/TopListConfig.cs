/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API: 
/// 1. https://www.trade-ideas.com/Help.html
/// 2. https://pro.trade-ideas.com/professional/tidocumentation/
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;

namespace Pacmio.TIProData
{
    public abstract class TopListConfig : DynamicWatchList
    {
        public override bool IsRunning { get => m_IsRunning && Client.Connected; protected set => m_IsRunning = value; }
        protected bool m_IsRunning = false;

        public virtual int MessageCount { get; protected set; } = 0;

        public override string ConfigurationString
        {
            get
            {
                if (Name.Length > 1) ConfigList["WN"] = Name;

                SetConfig("X_NYSE", Exchanges.Contains(Exchange.NYSE));
                SetConfig("XN", Exchanges.Contains(Exchange.NASDAQ));
                SetConfig("X_ARCA", Exchanges.Contains(Exchange.ARCA));
                SetConfig("X_AMEX", Exchanges.Contains(Exchange.AMEX));
                SetConfig("X_BATS", Exchanges.Contains(Exchange.BATS));
                SetConfig("X_PINK", Exchanges.Contains(Exchange.OTCMKT));

                for (int i = 0; i < ShowColumns.Count; i++)
                {
                    SetConfig("show" + i, ShowColumns[i]);
                }

                string extraConfig = ExtraConfig;
                if (extraConfig.Length > 0 && !extraConfig.EndsWith("&")) extraConfig += "&";
                return extraConfig + string.Join("&", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());
            }
        }

        public Dictionary<string, ColumnInfo> Columns { get; } = new Dictionary<string, ColumnInfo>();

        public void ConfigColumns(IList<ColumnInfo> list)
        {
            lock (Columns)
            {
                Columns.Clear();
                foreach (ColumnInfo c in list)
                {
                    Columns[c.WireName] = c;
                    Console.WriteLine(c.WireName + " | " + c.Description + " | " + c.Format + " | " + c.Units + " | " + c.InternalCode + " | " + c.Graphics + " | " + c.TextHeader + " | " + c.PreferredWidth);
                }
            }
        }

        public List<Contract> GetContractList(List<RowData> rows, string symbolColumnName = "symbol")
        {
            List<Contract> List = new();
            lock (Columns)
            {
                foreach (RowData row in rows)
                {
                    Console.WriteLine(row.ToString());

                    if (row.GetContract(symbolColumnName) is Stock stk)
                    {
                        // See if the stk has live data subscription !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! before override the Last and volume
                        if ((!IsSnapshot) && stk.MarketData is MarketData md && (!md.IsLive))
                        {
                            md.Status = MarketDataStatus.Delayed;

                            md.LastPrice = row.GetAsString("c_Price") is string s && s.Length > 0 ? s.ToDouble() : double.NaN; // c_Price
                            md.Volume = row.GetAsString("c_TV") is string s1 && s1.Length > 0 ? s1.ToDouble() : double.NaN;
                            // Price

                            // Volume

                            // 

                            // Social

                            // Float Short

                            // Float
                        }

                        List.Add(stk);

                        /*
                        if (list.Count() == 1 && row.GetAsString("c_D_Name") is string fullname && fullname.Length > 0)
                            stk.FullName = fullname;*/



                        string rowString = stk.ToString() + " >> " + stk.ISIN + " >> " + stk.FullName + " >> ";

                        foreach (string key in Columns.Keys)
                        {
                            ColumnInfo ci = Columns[key];

                            if (row.GetValue(ci) is string val)
                            {
                                rowString += ci.Description + "=" + val + "; ";

                                if (key == "c_D_Name")
                                {
                                    stk.FullName = val;
                                }
                            }
                        }

                        if (row.GetAsString("DESCRIPTION") is string desc && desc.Length > 0)
                        {
                            rowString += " | desc = " + desc;
                        }

                        if (row.GetAsString("ALT_DESCRIPTION") is string adesc && adesc.Length > 0)
                        {
                            rowString += " | adesc = " + adesc;
                        }

                        Console.WriteLine(rowString);

                    }
                }
                // Trigger the list is updated event!!
            }

            return List;
        }

        #region Utilities

        public (double Min, double Max) GetConfigRangeDouble(string key) => (GetConfigDouble("Min" + key), GetConfigDouble("Max" + key));
        public void SetConfigRange(string key, (double Min, double Max) value) { SetConfig("Min" + key, value.Min); SetConfig("Max" + key, value.Max); }
        public void SetConfigPercent(string key, (double Min, double Max) value)
        {
            double min = Math.Min(value.Min, value.Max);
            if (min < 0) min = 0; else if (min > 100) min = 100;

            double max = Math.Max(value.Min, value.Max);
            if (max < 0) max = 0; else if (max > 100) max = 100;

            SetConfig("Min" + key, min); SetConfig("Max" + key, max);
        }

        public (int Min, int Max) GetConfigRangeInt(string key) => (GetConfigInt("Min" + key), GetConfigInt("Max" + key));
        public void SetConfigRange(string key, (int Min, int Max) value) { SetConfig("Min" + key, value.Min); SetConfig("Max" + key, value.Max); }

        #endregion Utilities

        public override string Name { get => GetConfigString("WN"); set => SetConfig("WN", value); }

        public List<string> ShowColumns { get; } = new List<string>();

        public List<Exchange> Exchanges { get; } = new List<Exchange>() { Exchange.NYSE, Exchange.NASDAQ, Exchange.ARCA, Exchange.AMEX, Exchange.BATS };

        public int Count { get => GetConfigInt("count"); set => SetConfig("count", value); }

        public override (double Min, double Max) Price { get => GetConfigRangeDouble("Price"); set => SetConfigRange("Price", value); }

        public override (double Min, double Max) MarketCap { get => GetConfigRangeDouble("MCap"); set => SetConfigRange("MCap", value); }

        public (double Min, double Max) Volume { get => GetConfigRangeDouble("TV"); set => SetConfigRange("TV", value); }
        public (double Min, double Max) RelativeVolume { get => GetConfigRangeDouble("RV"); set => SetConfigRange("RV", value); }
        public (double Min, double Max) Volume5Days { get => GetConfigRangeDouble("Vol5D"); set => SetConfigRange("Vol5D", value); }
        public (double Min, double Max) Float { get => GetConfigRangeDouble("Float"); set => SetConfigRange("Float", value); }
        public (double Min, double Max) ShortFloatPercent { get => GetConfigRangeDouble("SFloat"); set => SetConfigRange("SFloat", value); }

        public (double Min, double Max) Gain { get => GetConfigRangeDouble("FCD"); set => SetConfigRange("FCD", value); }
        public override (double Min, double Max) GainPercent { get => GetConfigRangeDouble("FCP"); set => SetConfigRange("FCP", value); }

        public (double Min, double Max) Gap { get => GetConfigRangeDouble("GUD"); set => SetConfigRange("GUD", value); }
        public override (double Min, double Max) GapPercent { get => GetConfigRangeDouble("GUP"); set => SetConfigRange("GUP", value); }
        public (double Min, double Max) GapBars { get => GetConfigRangeDouble("GUR"); set => SetConfigRange("GUR", value); }

        /// <summary>
        /// These filters refer to Wilder’s Relative Strength Index (RSI), using the standard value of 14 periods.
        /// The server recomputes this value every 1, 2, 5, 15, or 60 minutes, at the same time as new bars or candlesticks would appear on a 1, 2, 5, 15, or 60 minute stock chart.
        /// These filters do not use pre- or post-market data.
        /// RSI1 | 1 Minute RSI | 0 - 100 | MinRSI1 | MaxRSI1 | RSI1:  '1 Minute RSI' ('0 - 100')
        /// RSI2 | 2 Minute RSI | 0 - 100 | MinRSI2 | MaxRSI2 | RSI2:  '2 Minute RSI' ('0 - 100')
        /// RSI5 | 5 Minute RSI | 0 - 100 | MinRSI5 | MaxRSI5 | RSI5:  '5 Minute RSI' ('0 - 100')
        /// RSI15 | 15 Minute RSI | 0 - 100 | MinRSI15 | MaxRSI15 | RSI15:  '15 Minute RSI' ('0 - 100')
        /// RSI60 | 60 Minute RSI | 0 - 100 | MinRSI60 | MaxRSI60 | RSI60:  '60 Minute RSI' ('0 - 100')
        /// </summary>
        public (double Min, double Max) RSI1Min { get => GetConfigRangeDouble("RSI1"); set => SetConfigPercent("RSI1", value); }
        public (double Min, double Max) RSI2Min { get => GetConfigRangeDouble("RSI2"); set => SetConfigPercent("RSI2", value); }
        public (double Min, double Max) RSI5Min { get => GetConfigRangeDouble("RSI5"); set => SetConfigPercent("RSI5", value); }
        public (double Min, double Max) RSI15Min { get => GetConfigRangeDouble("RSI15"); set => SetConfigPercent("RSI15", value); }
        public (double Min, double Max) RSI60Min { get => GetConfigRangeDouble("RSI60"); set => SetConfigPercent("RSI60", value); }

        /// <summary>
        /// These filters refer to Wilder’s Relative Strength Index (RSI), using the standard value of 14 periods.  The server recomputes this value every night, after the close.
        /// These filters are only available for stocks with sufficient history; if we do not have at least 14 days of history, the server will not report an RSI for that stock.
        /// Available historical information of up to one year is factored into the RSI using Wilder’s Smoothing.
        /// DRSI | Daily RSI | 0 - 100 | MinDRSI | MaxDRSI | DRSI:  'Daily RSI' ('0 - 100')
        /// </summary>
        public (double Min, double Max) RSI { get => GetConfigRangeDouble("DRSI"); set => SetConfigPercent("DRSI", value); }

        /// <summary>
        /// These filters look at the Average Directional Index, or ADX, for a stock.  These look at a daily chart and use a 14 period smoothing factor.
        /// The ADX is traditionally used to determine if a stock is trending or not.Values less than 25 indicate a sideways or choppy motion.
        /// Values between 30 and 50 typically indicate a strong trend.Values can be anywhere between 0 and 100, but very large values are unusual, 
        /// and they denote stocks doing very unusual things.
        /// ADX | Average Directional Index | % | MinADX | MaxADX | ADX:  'Average Directional Index' ('%')
        /// </summary>
        public (double Min, double Max) ADX { get => GetConfigRangeDouble("ADX"); set => SetConfigRange("ADX", value); }

        public (double Min, double Max) AverageTrueRange { get => GetConfigRangeDouble("ATR"); set => SetConfigRange("ATR", value); }

        public (double Min, double Max) Volatility { get => GetConfigRangeDouble("VWV"); set => SetConfigRange("VWV", value); }

        public (double Min, double Max) VolatilityPercent { get => GetConfigRangeDouble("VWVP"); set => SetConfigRange("VWVP", value); }
        public (double Min, double Max) Wiggle { get => GetConfigRangeDouble("Wiggle"); set => SetConfigRange("Wiggle", value); }



    }

}

/*
Price | Price | $ | MinPrice | MaxPrice | Price:  'Price' ('$')
Spread | Spread | Pennies | MinSpread | MaxSpread | Spread:  'Spread' ('Pennies')
BS | Bid Size | Shares | MinBS | MaxBS | BS:  'Bid Size' ('Shares')
AS | Ask Size | Shares | MinAS | MaxAS | AS:  'Ask Size' ('Shares')
BAR | Bid / Ask Ratio | Ratio | MinBAR | MaxBAR | BAR:  'Bid / Ask Ratio' ('Ratio')
DNbbo | Distance from Inside Market | % | MinDNbbo | MaxDNbbo | DNbbo:  'Distance from Inside Market' ('%')
Prints | Average Number of Prints | Prints / Day | MinPrints | MaxPrints | Prints:  'Average Number of Prints' ('Prints / Day')
Vol5D | Average Daily Volume (5D) | Shares / Day | MinVol5D | MaxVol5D | Vol5D:  'Average Daily Volume (5D)' ('Shares / Day')
Vol | Average Daily Volume (10D) | Shares / Day | MinVol | MaxVol | Vol:  'Average Daily Volume (10D)' ('Shares / Day')
Vol3M | Average Daily Volume (3M) | Shares / Day | MinVol3M | MaxVol3M | Vol3M:  'Average Daily Volume (3M)' ('Shares / Day')
DV | Dollar Volume | Dollars/Share x Shares/Day | MinDV | MaxDV | DV:  'Dollar Volume' ('Dollars/Share x Shares/Day')
RV | Relative Volume | Ratio | MinRV | MaxRV | RV:  'Relative Volume' ('Ratio')
TV | Volume Today | Shares | MinTV | MaxTV | TV:  'Volume Today' ('Shares')
PV | Volume Today | % | MinPV | MaxPV | PV:  'Volume Today' ('%')
YPV | Volume Yesterday | % | MinYPV | MaxYPV | YPV:  'Volume Yesterday' ('%')
Vol1 | Volume 1 Minute | % | MinVol1 | MaxVol1 | Vol1:  'Volume 1 Minute' ('%')
Vol5 | Volume 5 Minute | % | MinVol5 | MaxVol5 | Vol5:  'Volume 5 Minute' ('%')
Vol10 | Volume 10 Minute | % | MinVol10 | MaxVol10 | Vol10:  'Volume 10 Minute' ('%')
Vol15 | Volume 15 Minute | % | MinVol15 | MaxVol15 | Vol15:  'Volume 15 Minute' ('%')
Vol30 | Volume 30 Minute | % | MinVol30 | MaxVol30 | Vol30:  'Volume 30 Minute' ('%')
PMVol | Post Market Volume | Shares | MinPMVol | MaxPMVol | PMVol:  'Post Market Volume' ('Shares')
STH | StockTwits Average Activity | Mentions / Day | MinSTH | MaxSTH | STH:  'StockTwits Average Activity' ('Mentions / Day')
STP | StockTwits Relative Activity | % | MinSTP | MaxSTP | STP:  'StockTwits Relative Activity' ('%')
VWV | Volatility | $ | MinVWV | MaxVWV | VWV:  'Volatility' ('$')
VWVP | Volatility | % | MinVWVP | MaxVWVP | VWVP:  'Volatility' ('%')
YSD | Yearly Standard Deviation | $ | MinYSD | MaxYSD | YSD:  'Yearly Standard Deviation' ('$')
Wiggle | Wiggle | $ | MinWiggle | MaxWiggle | Wiggle:  'Wiggle' ('$')
ATR | Average True Range | $ | MinATR | MaxATR | ATR:  'Average True Range' ('$')
TRangeD | Today's Range | $ | MinTRangeD | MaxTRangeD | TRangeD:  'Today's Range' ('$')
TRangeP | Today's Range | % | MinTRangeP | MaxTRangeP | TRangeP:  'Today's Range' ('%')
Range2 | 2 Minute Range | $ | MinRange2 | MaxRange2 | Range2:  '2 Minute Range' ('$')
Range2P | 2 Minute Range | % | MinRange2P | MaxRange2P | Range2P:  '2 Minute Range' ('%')
Range5 | 5 Minute Range | $ | MinRange5 | MaxRange5 | Range5:  '5 Minute Range' ('$')
Range5P | 5 Minute Range | % | MinRange5P | MaxRange5P | Range5P:  '5 Minute Range' ('%')
Range15 | 15 Minute Range | $ | MinRange15 | MaxRange15 | Range15:  '15 Minute Range' ('$')
Range15P | 15 Minute Range | % | MinRange15P | MaxRange15P | Range15P:  '15 Minute Range' ('%')
Range30 | 30 Minute Range | $ | MinRange30 | MaxRange30 | Range30:  '30 Minute Range' ('$')
Range30P | 30 Minute Range | % | MinRange30P | MaxRange30P | Range30P:  '30 Minute Range' ('%')
Range60 | 60 Minute Range | $ | MinRange60 | MaxRange60 | Range60:  '60 Minute Range' ('$')
Range60P | 60 Minute Range | % | MinRange60P | MaxRange60P | Range60P:  '60 Minute Range' ('%')
Range120 | 120 Minute Range | $ | MinRange120 | MaxRange120 | Range120:  '120 Minute Range' ('$')
Range120P | 120 Minute Range | % | MinRange120P | MaxRange120P | Range120P:  '120 Minute Range' ('%')
Range5D | 5 Day Range | $ | MinRange5D | MaxRange5D | Range5D:  '5 Day Range' ('$')
Range5DP | 5 Day Range | % | MinRange5DP | MaxRange5DP | Range5DP:  '5 Day Range' ('%')
Range10D | 10 Day Range | $ | MinRange10D | MaxRange10D | Range10D:  '10 Day Range' ('$')
Range10DP | 10 Day Range | % | MinRange10DP | MaxRange10DP | Range10DP:  '10 Day Range' ('%')
Range20D | 20 Day Range | $ | MinRange20D | MaxRange20D | Range20D:  '20 Day Range' ('$')
Range20DP | 20 Day Range | % | MinRange20DP | MaxRange20DP | Range20DP:  '20 Day Range' ('%')
PCR | Put/Call Ratio | Ratio | MinPCR | MaxPCR | PCR:  'Put/Call Ratio' ('Ratio')
PCAV | Options Volume | Contracts | MinPCAV | MaxPCAV | PCAV:  'Options Volume' ('Contracts')
PCTV | Options Volume Today | Contracts | MinPCTV | MaxPCTV | PCTV:  'Options Volume Today' ('Contracts')
PTV | Put Volume Today | Contracts | MinPTV | MaxPTV | PTV:  'Put Volume Today' ('Contracts')
CTV | Call Volume Today | Contracts | MinCTV | MaxCTV | CTV:  'Call Volume Today' ('Contracts')
PCPV | Options Volume Today | % | MinPCPV | MaxPCPV | PCPV:  'Options Volume Today' ('%')
GUD | Gap | $ | MinGUD | MaxGUD | GUD:  'Gap' ('$')
GUP | Gap | % | MinGUP | MaxGUP | GUP:  'Gap' ('%')
GUR | Gap | Bars | MinGUR | MaxGUR | GUR:  'Gap' ('Bars')
POORP | Position of Open | % | MinPOORP | MaxPOORP | POORP:  'Position of Open' ('%')
Dec | Decimal | $ | MinDec | MaxDec | Dec:  'Decimal' ('$')
Up1 | Consecutive Candles | 1 minute candles | MinUp1 | MaxUp1 | Up1:  'Consecutive Candles' ('1 minute candles')
Up2 | Consecutive Candles | 2 minute candles | MinUp2 | MaxUp2 | Up2:  'Consecutive Candles' ('2 minute candles')
Up5 | Consecutive Candles | 5 minute candles | MinUp5 | MaxUp5 | Up5:  'Consecutive Candles' ('5 minute candles')
Up10 | Consecutive Candles | 10 minute candles | MinUp10 | MaxUp10 | Up10:  'Consecutive Candles' ('10 minute candles')
Up15 | Consecutive Candles | 15 minute candles | MinUp15 | MaxUp15 | Up15:  'Consecutive Candles' ('15 minute candles')
Up30 | Consecutive Candles | 30 minute candles | MinUp30 | MaxUp30 | Up30:  'Consecutive Candles' ('30 minute candles')
Up60 | Consecutive Candles | 60 minute candles | MinUp60 | MaxUp60 | Up60:  'Consecutive Candles' ('60 minute candles')
Up | Consecutive Days | Days | MinUp | MaxUp | Up:  'Consecutive Days' ('Days')
DUp1 | Change 1 Minute | $ | MinDUp1 | MaxDUp1 | DUp1:  'Change 1 Minute' ('$')
PUp1 | Change 1 Minute | % | MinPUp1 | MaxPUp1 | PUp1:  'Change 1 Minute' ('%')
DUp2 | Change 2 Minute | $ | MinDUp2 | MaxDUp2 | DUp2:  'Change 2 Minute' ('$')
PUp2 | Change 2 Minute | % | MinPUp2 | MaxPUp2 | PUp2:  'Change 2 Minute' ('%')
DUp5 | Change 5 Minute | $ | MinDUp5 | MaxDUp5 | DUp5:  'Change 5 Minute' ('$')
PUp5 | Change 5 Minute | % | MinPUp5 | MaxPUp5 | PUp5:  'Change 5 Minute' ('%')
DUp10 | Change 10 Minute | $ | MinDUp10 | MaxDUp10 | DUp10:  'Change 10 Minute' ('$')
PUp10 | Change 10 Minute | % | MinPUp10 | MaxPUp10 | PUp10:  'Change 10 Minute' ('%')
DUp15 | Change 15 Minute | $ | MinDUp15 | MaxDUp15 | DUp15:  'Change 15 Minute' ('$')
PUp15 | Change 15 Minute | % | MinPUp15 | MaxPUp15 | PUp15:  'Change 15 Minute' ('%')
DUp30 | Change 30 Minute | $ | MinDUp30 | MaxDUp30 | DUp30:  'Change 30 Minute' ('$')
PUp30 | Change 30 Minute | % | MinPUp30 | MaxPUp30 | PUp30:  'Change 30 Minute' ('%')
DUp60 | Change 60 Minute | $ | MinDUp60 | MaxDUp60 | DUp60:  'Change 60 Minute' ('$')
PUp60 | Change 60 Minute | % | MinPUp60 | MaxPUp60 | PUp60:  'Change 60 Minute' ('%')
DUp120 | Change 120 Minute | $ | MinDUp120 | MaxDUp120 | DUp120:  'Change 120 Minute' ('$')
PUp120 | Change 120 Minute | % | MinPUp120 | MaxPUp120 | PUp120:  'Change 120 Minute' ('%')
Qqqq5 | NASDAQ Change 5 Minute | % | MinQqqq5 | MaxQqqq5 | Qqqq5:  'NASDAQ Change 5 Minute' ('%')
Qqqq10 | NASDAQ Change 10 Minute | % | MinQqqq10 | MaxQqqq10 | Qqqq10:  'NASDAQ Change 10 Minute' ('%')
Qqqq15 | NASDAQ Change 15 Minute | % | MinQqqq15 | MaxQqqq15 | Qqqq15:  'NASDAQ Change 15 Minute' ('%')
Qqqq30 | NASDAQ Change 30 Minute | % | MinQqqq30 | MaxQqqq30 | Qqqq30:  'NASDAQ Change 30 Minute' ('%')
QqqqD | NASDAQ Change Today | % | MinQqqqD | MaxQqqqD | QqqqD:  'NASDAQ Change Today' ('%')
Spy5 | S&P Change 5 Minute | % | MinSpy5 | MaxSpy5 | Spy5:  'S&P Change 5 Minute' ('%')
Spy10 | S&P Change 10 Minute | % | MinSpy10 | MaxSpy10 | Spy10:  'S&P Change 10 Minute' ('%')
Spy15 | S&P Change 15 Minute | % | MinSpy15 | MaxSpy15 | Spy15:  'S&P Change 15 Minute' ('%')
Spy30 | S&P Change 30 Minute | % | MinSpy30 | MaxSpy30 | Spy30:  'S&P Change 30 Minute' ('%')
SpyD | S&P Change Today | % | MinSpyD | MaxSpyD | SpyD:  'S&P Change Today' ('%')
Dia5 | Dow Change 5 Minute | % | MinDia5 | MaxDia5 | Dia5:  'Dow Change 5 Minute' ('%')
Dia10 | Dow Change 10 Minute | % | MinDia10 | MaxDia10 | Dia10:  'Dow Change 10 Minute' ('%')
Dia15 | Dow Change 15 Minute | % | MinDia15 | MaxDia15 | Dia15:  'Dow Change 15 Minute' ('%')
Dia30 | Dow Change 30 Minute | % | MinDia30 | MaxDia30 | Dia30:  'Dow Change 30 Minute' ('%')
DiaD | Dow Change Today | % | MinDiaD | MaxDiaD | DiaD:  'Dow Change Today' ('%')
PivotR2 | Distance from Pivot R2 | % | MinPivotR2 | MaxPivotR2 | PivotR2:  'Distance from Pivot R2' ('%')
PivotR1 | Distance from Pivot R1 | % | MinPivotR1 | MaxPivotR1 | PivotR1:  'Distance from Pivot R1' ('%')
Pivot | Distance from Pivot | % | MinPivot | MaxPivot | Pivot:  'Distance from Pivot' ('%')
PivotS1 | Distance from Pivot S1 | % | MinPivotS1 | MaxPivotS1 | PivotS1:  'Distance from Pivot S1' ('%')
PivotS2 | Distance from Pivot S2 | % | MinPivotS2 | MaxPivotS2 | PivotS2:  'Distance from Pivot S2' ('%')
VWAP | Distance from VWAP 1 | % | MinVWAP | MaxVWAP | VWAP:  'Distance from VWAP 1' ('%')
DVWAP2 | Distance from VWAP 2 | % | MinDVWAP2 | MaxDVWAP2 | DVWAP2:  'Distance from VWAP 2' ('%')
DVWAP3 | Distance from VWAP 3 | % | MinDVWAP3 | MaxDVWAP3 | DVWAP3:  'Distance from VWAP 3' ('%')
DVWAP4 | Distance from VWAP 4 | % | MinDVWAP4 | MaxDVWAP4 | DVWAP4:  'Distance from VWAP 4' ('%')
DVWAP5 | Distance from VWAP 5 | % | MinDVWAP5 | MaxDVWAP5 | DVWAP5:  'Distance from VWAP 5' ('%')
VWAP1 | VWAP 1 | $ | MinVWAP1 | MaxVWAP1 | VWAP1:  'VWAP 1' ('$')
VWAP2 | VWAP 2 | $ | MinVWAP2 | MaxVWAP2 | VWAP2:  'VWAP 2' ('$')
VWAP3 | VWAP 3 | $ | MinVWAP3 | MaxVWAP3 | VWAP3:  'VWAP 3' ('$')
VWAP4 | VWAP 4 | $ | MinVWAP4 | MaxVWAP4 | VWAP4:  'VWAP 4' ('$')
VWAP5 | VWAP 5 | $ | MinVWAP5 | MaxVWAP5 | VWAP5:  'VWAP 5' ('$')
VWAPStop | VWAP Stop | $ | MinVWAPStop | MaxVWAPStop | VWAPStop:  'VWAP Stop' ('$')

FCD | Change from the Close | $ | MinFCD | MaxFCD | FCD:  'Change from the Close' ('$')
FCP | Change from the Close | % | MinFCP | MaxFCP | FCP:  'Change from the Close' ('%')
FCR | Change from the Close | Bars | MinFCR | MaxFCR | FCR:  'Change from the Close' ('Bars')

FOD | Change from the Open | $ | MinFOD | MaxFOD | FOD:  'Change from the Open' ('$')
FOP | Change from the Open | % | MinFOP | MaxFOP | FOP:  'Change from the Open' ('%')
FOR | Change from the Open | Bars | MinFOR | MaxFOR | FOR:  'Change from the Open' ('Bars')
FOW | Change from the Open | % of Average True Range | MinFOW | MaxFOW | FOW:  'Change from the Open' ('% of Average True Range')
PostD | Change Post Market | $ | MinPostD | MaxPostD | PostD:  'Change Post Market' ('$')
PostP | Change Post Market | % | MinPostP | MaxPostP | PostP:  'Change Post Market' ('%')
FCDP | Change Previous Day | $ | MinFCDP | MaxFCDP | FCDP:  'Change Previous Day' ('$')
FCPP | Change Previous Day | % | MinFCPP | MaxFCPP | FCPP:  'Change Previous Day' ('%')
U5DD | Change in 5 Days | $ | MinU5DD | MaxU5DD | U5DD:  'Change in 5 Days' ('$')
U5DP | Change in 5 Days | % | MinU5DP | MaxU5DP | U5DP:  'Change in 5 Days' ('%')
U10DD | Change in 10 Days | $ | MinU10DD | MaxU10DD | U10DD:  'Change in 10 Days' ('$')
U10DP | Change in 10 Days | % | MinU10DP | MaxU10DP | U10DP:  'Change in 10 Days' ('%')
U20DD | Change in 20 Days | $ | MinU20DD | MaxU20DD | U20DD:  'Change in 20 Days' ('$')
U20DP | Change in 20 Days | % | MinU20DP | MaxU20DP | U20DP:  'Change in 20 Days' ('%')
UYD | Change in 1 Year | $ | MinUYD | MaxUYD | UYD:  'Change in 1 Year' ('$')
UYP | Change in 1 Year | % | MinUYP | MaxUYP | UYP:  'Change in 1 Year' ('%')
UpJan1D | Change Since January 1 | $ | MinUpJan1D | MaxUpJan1D | UpJan1D:  'Change Since January 1' ('$')
UpJan1P | Change Since January 1 | % | MinUpJan1P | MaxUpJan1P | UpJan1P:  'Change Since January 1' ('%')
BB | Standard Deviation | Standard Deviations | MinBB | MaxBB | BB:  'Standard Deviation' ('Standard Deviations')
R5M | Position in 5 minute range | % | MinR5M | MaxR5M | R5M:  'Position in 5 minute range' ('%')
R15M | Position in 15 minute range | % | MinR15M | MaxR15M | R15M:  'Position in 15 minute range' ('%')
R30M | Position in 30 minute range | % | MinR30M | MaxR30M | R30M:  'Position in 30 minute range' ('%')
R60M | Position in 60 minute range | % | MinR60M | MaxR60M | R60M:  'Position in 60 minute range' ('%')
BelowHigh | Below High | $ | MinBelowHigh | MaxBelowHigh | BelowHigh:  'Below High' ('$')
AboveLow | Above Low | $ | MinAboveLow | MaxAboveLow | AboveLow:  'Above Low' ('$')
BelowHighPre | Below Pre-Market High | $ | MinBelowHighPre | MaxBelowHighPre | BelowHighPre:  'Below Pre-Market High' ('$')
AboveLowPre | Above Pre-Market Low | $ | MinAboveLowPre | MaxAboveLowPre | AboveLowPre:  'Above Pre-Market Low' ('$')
RD | Position in Range | % | MinRD | MaxRD | RD:  'Position in Range' ('%')
RPD | Position in Previous Day's Range | % | MinRPD | MaxRPD | RPD:  'Position in Previous Day's Range' ('%')
RPM | Position in Pre-Market Range | % | MinRPM | MaxRPM | RPM:  'Position in Pre-Market Range' ('%')
R5D | Position in 5 Day Range | % | MinR5D | MaxR5D | R5D:  'Position in 5 Day Range' ('%')
R10D | Position in 10 Day Range | % | MinR10D | MaxR10D | R10D:  'Position in 10 Day Range' ('%')
R20D | Position in 20 Day Range | % | MinR20D | MaxR20D | R20D:  'Position in 20 Day Range' ('%')
R3MO | Position in 3 Month Range | % | MinR3MO | MaxR3MO | R3MO:  'Position in 3 Month Range' ('%')
R6MO | Position in 6 Month Range | % | MinR6MO | MaxR6MO | R6MO:  'Position in 6 Month Range' ('%')
R9MO | Position in 9 Month Range | % | MinR9MO | MaxR9MO | R9MO:  'Position in 9 Month Range' ('%')
RY | Position in Year Range | % | MinRY | MaxRY | RY:  'Position in Year Range' ('%')
R2Y | Position in 2 Year Range | % | MinR2Y | MaxR2Y | R2Y:  'Position in 2 Year Range' ('%')
RL | Position in Lifetime Range | % | MinRL | MaxRL | RL:  'Position in Lifetime Range' ('%')





Boll5 | Position in Bollinger Bands (5 Minute) | % | MinBoll5 | MaxBoll5 | Boll5:  'Position in Bollinger Bands (5 Minute)' ('%')


Boll15 | Position in Bollinger Bands (15 Minute) | % | MinBoll15 | MaxBoll15 | Boll15:  'Position in Bollinger Bands (15 Minute)' ('%')
Boll60 | Position in Bollinger Bands (60 Minute) | % | MinBoll60 | MaxBoll60 | Boll60:  'Position in Bollinger Bands (60 Minute)' ('%')
Boll | Position in Bollinger Bands (Daily) | % | MinBoll | MaxBoll | Boll:  'Position in Bollinger Bands (Daily)' ('%')
RC | Range Contraction | Days | MinRC | MaxRC | RC:  'Range Contraction' ('Days')
LR130 | Linear Regression Divergence | 0 - 1 | MinLR130 | MaxLR130 | LR130:  'Linear Regression Divergence' ('0 - 1')

PDIMDI | Directional Indicator | % | MinPDIMDI | MaxPDIMDI | PDIMDI:  'Directional Indicator' ('%')
MA200P | Change from 200 Day SMA | % | MinMA200P | MaxMA200P | MA200P:  'Change from 200 Day SMA' ('%')
MA200R | Change from 200 Day SMA | Bars | MinMA200R | MaxMA200R | MA200R:  'Change from 200 Day SMA' ('Bars')
SMA200 | 200 Day SMA | $ | MinSMA200 | MaxSMA200 | SMA200:  '200 Day SMA' ('$')
MA50P | Change from 50 Day SMA | % | MinMA50P | MaxMA50P | MA50P:  'Change from 50 Day SMA' ('%')
MA50R | Change from 50 Day SMA | Bars | MinMA50R | MaxMA50R | MA50R:  'Change from 50 Day SMA' ('Bars')
MA20P | Change from 20 Day SMA | % | MinMA20P | MaxMA20P | MA20P:  'Change from 20 Day SMA' ('%')
MA20R | Change from 20 Day SMA | Bars | MinMA20R | MaxMA20R | MA20R:  'Change from 20 Day SMA' ('Bars')
MA10P | Change from 10 Day SMA | % | MinMA10P | MaxMA10P | MA10P:  'Change from 10 Day SMA' ('%')
MA10R | Change from 10 Day SMA | Bars | MinMA10R | MaxMA10R | MA10R:  'Change from 10 Day SMA' ('Bars')
MA8P | Change from 8 Day SMA | % | MinMA8P | MaxMA8P | MA8P:  'Change from 8 Day SMA' ('%')
MA8R | Change from 8 Day SMA | Bars | MinMA8R | MaxMA8R | MA8R:  'Change from 8 Day SMA' ('Bars')
2SmaLa5 | Change from 5 Period SMA (2m) | % | Min2SmaLa5 | Max2SmaLa5 | 2SmaLa5:  'Change from 5 Period SMA (2m)' ('%')
5SmaLa5 | Change from 5 Period SMA (5m) | % | Min5SmaLa5 | Max5SmaLa5 | 5SmaLa5:  'Change from 5 Period SMA (5m)' ('%')
15SmaLa5 | Change from 5 Period SMA (15m) | % | Min15SmaLa5 | Max15SmaLa5 | 15SmaLa5:  'Change from 5 Period SMA (15m)' ('%')
2SmaLa8 | Change from 8 Period SMA (2m) | % | Min2SmaLa8 | Max2SmaLa8 | 2SmaLa8:  'Change from 8 Period SMA (2m)' ('%')
5SmaLa8 | Change from 8 Period SMA (5m) | % | Min5SmaLa8 | Max5SmaLa8 | 5SmaLa8:  'Change from 8 Period SMA (5m)' ('%')
15SmaLa8 | Change from 8 Period SMA (15m) | % | Min15SmaLa8 | Max15SmaLa8 | 15SmaLa8:  'Change from 8 Period SMA (15m)' ('%')
60SmaLa8 | Change from 8 Period SMA (60m) | % | Min60SmaLa8 | Max60SmaLa8 | 60SmaLa8:  'Change from 8 Period SMA (60m)' ('%')
2SmaLa10 | Change from 10 Period SMA (2m) | % | Min2SmaLa10 | Max2SmaLa10 | 2SmaLa10:  'Change from 10 Period SMA (2m)' ('%')
5SmaLa10 | Change from 10 Period SMA (5m) | % | Min5SmaLa10 | Max5SmaLa10 | 5SmaLa10:  'Change from 10 Period SMA (5m)' ('%')
15SmaLa10 | Change from 10 Period SMA (15m) | % | Min15SmaLa10 | Max15SmaLa10 | 15SmaLa10:  'Change from 10 Period SMA (15m)' ('%')
60SmaLa10 | Change from 10 Period SMA (60m) | % | Min60SmaLa10 | Max60SmaLa10 | 60SmaLa10:  'Change from 10 Period SMA (60m)' ('%')
2SmaLa20 | Change from 20 Period SMA (2m) | % | Min2SmaLa20 | Max2SmaLa20 | 2SmaLa20:  'Change from 20 Period SMA (2m)' ('%')
5SmaLa20 | Change from 20 Period SMA (5m) | % | Min5SmaLa20 | Max5SmaLa20 | 5SmaLa20:  'Change from 20 Period SMA (5m)' ('%')
15SmaLa20 | Change from 20 Period SMA (15m) | % | Min15SmaLa20 | Max15SmaLa20 | 15SmaLa20:  'Change from 20 Period SMA (15m)' ('%')
60SmaLa20 | Change from 20 Period SMA (60m) | % | Min60SmaLa20 | Max60SmaLa20 | 60SmaLa20:  'Change from 20 Period SMA (60m)' ('%')
2SmaLa200 | Change from 200 Period SMA (2m) | % | Min2SmaLa200 | Max2SmaLa200 | 2SmaLa200:  'Change from 200 Period SMA (2m)' ('%')
5SmaLa200 | Change from 200 Period SMA (5m) | % | Min5SmaLa200 | Max5SmaLa200 | 5SmaLa200:  'Change from 200 Period SMA (5m)' ('%')
15SmaLa130 | Change from 130 Period SMA (15m) | % | Min15SmaLa130 | Max15SmaLa130 | 15SmaLa130:  'Change from 130 Period SMA (15m)' ('%')
15SmaLa200 | Change from 200 Period SMA (15m) | % | Min15SmaLa200 | Max15SmaLa200 | 15SmaLa200:  'Change from 200 Period SMA (15m)' ('%')
60SmaLa200 | Change from 200 Period SMA (60m) | % | Min60SmaLa200 | Max60SmaLa200 | 60SmaLa200:  'Change from 200 Period SMA (60m)' ('%')
2Sma8a20 | 8 vs. 20 Period SMA (2m) | % | Min2Sma8a20 | Max2Sma8a20 | 2Sma8a20:  '8 vs. 20 Period SMA (2m)' ('%')
5Sma8a20 | 8 vs. 20 Period SMA (5m) | % | Min5Sma8a20 | Max5Sma8a20 | 5Sma8a20:  '8 vs. 20 Period SMA (5m)' ('%')
15Sma8a20 | 8 vs. 20 Period SMA (15m) | % | Min15Sma8a20 | Max15Sma8a20 | 15Sma8a20:  '8 vs. 20 Period SMA (15m)' ('%')
60Sma8a20 | 8 vs. 20 Period SMA (60m) | % | Min60Sma8a20 | Max60Sma8a20 | 60Sma8a20:  '8 vs. 20 Period SMA (60m)' ('%')
2Sma20a200 | 20 vs. 200 Period SMA (2m) | % | Min2Sma20a200 | Max2Sma20a200 | 2Sma20a200:  '20 vs. 200 Period SMA (2m)' ('%')
5Sma20a200 | 20 vs. 200 Period SMA (5m) | % | Min5Sma20a200 | Max5Sma20a200 | 5Sma20a200:  '20 vs. 200 Period SMA (5m)' ('%')
15Sma20a200 | 20 vs. 200 Period SMA (15m) | % | Min15Sma20a200 | Max15Sma20a200 | 15Sma20a200:  '20 vs. 200 Period SMA (15m)' ('%')
60Sma20a200 | 20 vs. 200 Period SMA (60m) | % | Min60Sma20a200 | Max60Sma20a200 | 60Sma20a200:  '20 vs. 200 Period SMA (60m)' ('%')
ConDays | Consolidation | Days | MinConDays | MaxConDays | ConDays:  'Consolidation' ('Days')
RCon | Position in Consolidation | % | MinRCon | MaxRCon | RCon:  'Position in Consolidation' ('%')
SmartStopD | Smart Stop | $ | MinSmartStopD | MaxSmartStopD | SmartStopD:  'Smart Stop' ('$')
SmartStopP | Smart Stop | % | MinSmartStopP | MaxSmartStopP | SmartStopP:  'Smart Stop' ('%')
SCR | Stock Composite Rating | 30 - 100 | MinSCR | MaxSCR | SCR:  'Stock Composite Rating' ('30 - 100')
MCap | Market Cap | $ | MinMCap | MaxMCap | MCap:  'Market Cap' ('$')
ShOut | Shares Outstanding | Shares | MinShOut | MaxShOut | ShOut:  'Shares Outstanding' ('Shares')
DTC | Days to Cover | Days | MinDTC | MaxDTC | DTC:  'Days to Cover' ('Days')
ShortG | Short Growth | % | MinShortG | MaxShortG | ShortG:  'Short Growth' ('%')
SFloat | Short Float | % | MinSFloat | MaxSFloat | SFloat:  'Short Float' ('%')
Float | Float | Shares | MinFloat | MaxFloat | Float:  'Float' ('Shares')
Insider | Held by Insiders | % | MinInsider | MaxInsider | Insider:  'Held by Insiders' ('%')
Institution | Held by Institutions | % | MinInstitution | MaxInstitution | Institution:  'Held by Institutions' ('%')
Cash | Cash | $ | MinCash | MaxCash | Cash:  'Cash' ('$')
Assets | Current Assets | $ | MinAssets | MaxAssets | Assets:  'Current Assets' ('$')
Debt | Current Debt | $ | MinDebt | MaxDebt | Debt:  'Current Debt' ('$')
CashDebt | Cash / Debt Ratio | Ratio | MinCashDebt | MaxCashDebt | CashDebt:  'Cash / Debt Ratio' ('Ratio')
Income | Income | $ / Year | MinIncome | MaxIncome | Income:  'Income' ('$ / Year')
IncomeDebt | Income / Debt Ratio | Ratio | MinIncomeDebt | MaxIncomeDebt | IncomeDebt:  'Income / Debt Ratio' ('Ratio')
Revenue | Revenue | $ / Year | MinRevenue | MaxRevenue | Revenue:  'Revenue' ('$ / Year')
QRevG | Quarterly Revenue Growth | % | MinQRevG | MaxQRevG | QRevG:  'Quarterly Revenue Growth' ('%')
Value | Enterprise Value | $ | MinValue | MaxValue | Value:  'Enterprise Value' ('$')
ValueMCap | Enterprise Value / Market Cap Ratio | Ratio | MinValueMCap | MaxValueMCap | ValueMCap:  'Enterprise Value / Market Cap Ratio' ('Ratio')
EPS | EPS | $ / Share | MinEPS | MaxEPS | EPS:  'EPS' ('$ / Share')
EstAEPSG | Estimated Annual EPS Growth | Ratio | MinEstAEPSG | MaxEstAEPSG | EstAEPSG:  'Estimated Annual EPS Growth' ('Ratio')
EstQEPSG | Estimated Quarterly EPS Growth | Ratio | MinEstQEPSG | MaxEstQEPSG | EstQEPSG:  'Estimated Quarterly EPS Growth' ('Ratio')
QEarnG | Quarterly Earnings Growth | % | MinQEarnG | MaxQEarnG | QEarnG:  'Quarterly Earnings Growth' ('%')
PERatio | Price / Earnings Ratio | Ratio | MinPERatio | MaxPERatio | PERatio:  'Price / Earnings Ratio' ('Ratio')
PEG | PEG Ratio | Ratio | MinPEG | MaxPEG | PEG:  'PEG Ratio' ('Ratio')
EarningD | Earnings Date | Days | MinEarningD | MaxEarningD | EarningD:  'Earnings Date' ('Days')
Dividend | Dividend | $ | MinDividend | MaxDividend | Dividend:  'Dividend' ('$')
Beta | Beta | Ratio | MinBeta | MaxBeta | Beta:  'Beta' ('Ratio')
Time | Time of Day | Minutes after the open | MinTime | MaxTime | Time:  'Time of Day' ('Minutes after the open')
Count | Count | Alerts | MinCount | MaxCount | Count:  'Count' ('Alerts')
*/
