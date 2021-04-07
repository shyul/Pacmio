//// ***************************************************************************
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
    public abstract class AlertConfig : TopListConfig
    {
        public virtual int AlertCount { get; protected set; } = 0;

        #region Price Actions

        public int NewHigh { get => GetConfigIntQ("NHP"); set => SetConfigQ("NHP", value); }
        public int NewLow { get => GetConfigIntQ("NLP"); set => SetConfigQ("NLP", value); }

        public int PreMarketHigh { get => GetConfigIntQ("HPRE"); set => SetConfigQ("HPRE", value); }
        public int PreMarketLow { get => GetConfigIntQ("LPRE"); set => SetConfigQ("LPRE", value); }

        public double RunUp { get => GetConfigDoubleQ("RUN"); set => SetConfigQ("RUN", value); }
        public double RunDown { get => GetConfigDoubleQ("RDN"); set => SetConfigQ("RDN", value); }

        #endregion Price Actions



        public double StockTwitsSpikeRatio { get => GetConfigDoubleQ("STAS"); set => SetConfigQ("STAS", value); }

        public bool Halt { get => GetConfigBool("Sh_HALT"); set { SetConfig("Sh_HALT", value); } }
        public bool Resume { get => GetConfigBool("Sh_RESUME"); set { SetConfig("Sh_RESUME", value); } }
        public bool MarketLocked { get => GetConfigBool("Sh_ML"); set { SetConfig("Sh_ML", value); } }




        #region Volume

        public int BlockTrade { get => GetConfigIntQ("BP"); set => SetConfigQ("BP", value); }
        public double StrongVolumeRatio { get => GetConfigDoubleQ("SV"); set => SetConfigQ("SV", value); }

        #endregion Volume

        #region Hourly Time Frame



        #endregion Hourly Time Frame

        #region Daily Time Frame

        public bool CrossDailySMA200Above { get => GetConfigBool("Sh_CA200"); set { SetConfig("Sh_CA200", value); } }
        public bool CrossDailySMA200Below { get => GetConfigBool("Sh_CB200"); set { SetConfig("Sh_CB200", value); } }

        public bool CrossDailySMA50Above { get => GetConfigBool("Sh_CA50"); set { SetConfig("Sh_CA50", value); } }
        public bool CrossDailySMA50Below { get => GetConfigBool("Sh_CB50"); set { SetConfig("Sh_CB50", value); } }

        public bool CrossDailySMA20Above { get => GetConfigBool("Sh_CA20"); set { SetConfig("Sh_CA20", value); } }
        public bool CrossDailySMA20Below { get => GetConfigBool("Sh_CB20"); set { SetConfig("Sh_CB20", value); } }

        #endregion Daily Time Frame

        #region Intraday Time Frame

        public bool CrossAboveVWAP { get => GetConfigBool("Sh_CAVC"); set { SetConfig("Sh_CAVC", value); } }
        public bool CrossBelowVWAP { get => GetConfigBool("Sh_CBVC"); set { SetConfig("Sh_CBVC", value); } }

        public bool High5Min { get => GetConfigBool("Sh_IDH5"); set { SetConfig("Sh_IDH5", value); } }
        public bool Low5Min { get => GetConfigBool("Sh_IDL5"); set { SetConfig("Sh_IDL5", value); } }

        public bool High10Min { get => GetConfigBool("Sh_IDH10"); set { SetConfig("Sh_IDH10", value); } }
        public bool Low10Min { get => GetConfigBool("Sh_IDL10"); set { SetConfig("Sh_IDL10", value); } }

        public bool High15Min { get => GetConfigBool("Sh_IDH15"); set { SetConfig("Sh_IDH15", value); } }
        public bool Low15Min { get => GetConfigBool("Sh_IDL15"); set { SetConfig("Sh_IDL15", value); } }

        public bool High30Min { get => GetConfigBool("Sh_IDH30"); set { SetConfig("Sh_IDH30", value); } }
        public bool Low30Min { get => GetConfigBool("Sh_IDL30"); set { SetConfig("Sh_IDL30", value); } }

        public bool High60Min { get => GetConfigBool("Sh_IDH60"); set { SetConfig("Sh_IDH60", value); } }
        public bool Low60Min { get => GetConfigBool("Sh_IDL60"); set { SetConfig("Sh_IDL60", value); } }

        public bool Breakout1MinOpenRange { get => GetConfigBool("Sh_ORU1"); set { SetConfig("Sh_ORU1", value); } }
        public bool Breakdown1MinOpenRange { get => GetConfigBool("Sh_ORD1"); set { SetConfig("Sh_ORD1", value); } }

        public bool Breakout2MinOpenRange { get => GetConfigBool("Sh_ORU2"); set { SetConfig("Sh_ORU2", value); } }
        public bool Breakdown2MinOpenRange { get => GetConfigBool("Sh_ORD2"); set { SetConfig("Sh_ORD2", value); } }

        public bool Breakout5MinOpenRange { get => GetConfigBool("Sh_ORU5"); set { SetConfig("Sh_ORU5", value); } }
        public bool Breakdown5MinOpenRange { get => GetConfigBool("Sh_ORD5"); set { SetConfig("Sh_ORD5", value); } }

        public bool Breakout10MinOpenRange { get => GetConfigBool("Sh_ORU10"); set { SetConfig("Sh_ORU10", value); } }
        public bool Breakdown10MinOpenRange { get => GetConfigBool("Sh_ORD10"); set { SetConfig("Sh_ORD10", value); } }

        public bool Breakout15MinOpenRange { get => GetConfigBool("Sh_ORU15"); set { SetConfig("Sh_ORU15", value); } }
        public bool Breakdown15MinOpenRange { get => GetConfigBool("Sh_ORD15"); set { SetConfig("Sh_ORD15", value); } }

        public bool Breakout30MinOpenRange { get => GetConfigBool("Sh_ORU30"); set { SetConfig("Sh_ORU30", value); } }
        public bool Breakdown30MinOpenRange { get => GetConfigBool("Sh_ORD30"); set { SetConfig("Sh_ORD30", value); } }

        public bool Breakout60MinOpenRange { get => GetConfigBool("Sh_ORU60"); set { SetConfig("Sh_ORU60", value); } }
        public bool Breakdown60MinOpenRange { get => GetConfigBool("Sh_ORD60"); set { SetConfig("Sh_ORD60", value); } }

        #endregion Intraday Time Frame

        #region Utilities

        protected int GetConfigIntQ(string key)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (ConfigList.ContainsKey(keyOn))
            {
                return (ConfigList.ContainsKey(keyQ)) ? GetConfigInt(keyQ) : 0;
            }

            return -1;
        }

        protected void SetConfigQ(string key, int value)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (value < 0) // Remove  
            {
                if (ConfigList.ContainsKey(keyOn))
                    ConfigList.Remove(keyOn);
                if (ConfigList.ContainsKey(keyQ))
                    ConfigList.Remove(keyQ);
            }
            else
            {
                ConfigList[keyOn] = "on";
                if (value > 0) ConfigList[keyQ] = value.ToString();
            }
        }

        protected double GetConfigDoubleQ(string key)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (ConfigList.ContainsKey(keyOn))
            {
                return (ConfigList.ContainsKey(keyQ)) ? GetConfigDouble(keyQ) : 0;
            }

            return double.NaN;
        }

        protected void SetConfigQ(string key, double value)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (double.IsNaN(value)) // Remove  
            {
                if (ConfigList.ContainsKey(keyOn))
                    ConfigList.Remove(keyOn);
                if (ConfigList.ContainsKey(keyQ))
                    ConfigList.Remove(keyQ);
            }
            else
            {
                ConfigList[keyOn] = "on";
                if (value > 0) ConfigList[keyQ] = value.ToString("0.#######");
            }
        }

        #endregion Utilities
    }


}

/*
NHP | New high | Days, i.e. 52 week high = 365 | NHP:  'New high' ('Days, i.e. 52 week high = 365')
NLP | New low | Days, i.e. 52 week low = 365 | NLP:  'New low' ('Days, i.e. 52 week low = 365')
NHA | New high ask | Shares, minimum ask size | NHA:  'New high ask' ('Shares, minimum ask size')
NLB | New low bid | Shares, minimum bid size | NLB:  'New low bid' ('Shares, minimum bid size')
NHPF | New high (filtered) | Days, i.e. 52 week high = 365 | NHPF:  'New high (filtered)' ('Days, i.e. 52 week high = 365')
NLPF | New low (filtered) | Days, i.e. 52 week low = 365 | NLPF:  'New low (filtered)' ('Days, i.e. 52 week low = 365')
NHAF | New high ask (filtered) | Shares, minimum ask size | NHAF:  'New high ask (filtered)' ('Shares, minimum ask size')
NLBF | New low bid (filtered) | Shares, minimum bid size | NLBF:  'New low bid (filtered)' ('Shares, minimum bid size')
NHBF | New high bid (filtered) | Shares, minimum bid size | NHBF:  'New high bid (filtered)' ('Shares, minimum bid size')
NLAF | New low ask (filtered) | Shares, minimum ask size | NLAF:  'New low ask (filtered)' ('Shares, minimum ask size')
HPRE | Pre-market highs | Days | HPRE:  'Pre-market highs' ('Days')
LPRE | Pre-market lows | Days | LPRE:  'Pre-market lows' ('Days')
HPOST | Post-market highs | Days | HPOST:  'Post-market highs' ('Days')
LPOST | Post-market lows | Days | LPOST:  'Post-market lows' ('Days')
PFL75C | 75% pullback from lows (Close) | Dollars, initial move size | PFL75C:  '75% pullback from lows (Close)' ('Dollars, initial move size')
PFL25C | 25% pullback from lows (Close) | Dollars, initial move size | PFL25C:  '25% pullback from lows (Close)' ('Dollars, initial move size')
PFH75C | 75% pullback from highs (Close) | Dollars, initial move size | PFH75C:  '75% pullback from highs (Close)' ('Dollars, initial move size')
PFH25C | 25% pullback from highs (Close) | Dollars, initial move size | PFH25C:  '25% pullback from highs (Close)' ('Dollars, initial move size')
PFL75O | 75% pullback from lows (Open) | Dollars, initial move size | PFL75O:  '75% pullback from lows (Open)' ('Dollars, initial move size')
PFL25O | 25% pullback from lows (Open) | Dollars, initial move size | PFL25O:  '25% pullback from lows (Open)' ('Dollars, initial move size')
PFH75O | 75% pullback from highs (Open) | Dollars, initial move size | PFH75O:  '75% pullback from highs (Open)' ('Dollars, initial move size')
PFH25O | 25% pullback from highs (Open) | Dollars, initial move size | PFH25O:  '25% pullback from highs (Open)' ('Dollars, initial move size')
PFL75 | 75% pullback from lows | Dollars, initial move size | PFL75:  '75% pullback from lows' ('Dollars, initial move size')
PFL25 | 25% pullback from lows | Dollars, initial move size | PFL25:  '25% pullback from lows' ('Dollars, initial move size')
PFH75 | 75% pullback from highs | Dollars, initial move size | PFH75:  '75% pullback from highs' ('Dollars, initial move size')
PFH25 | 25% pullback from highs | Dollars, initial move size | PFH25:  '25% pullback from highs' ('Dollars, initial move size')
CMU | Check mark |  | CMU:  'Check mark'
CMD | Inverted check mark |  | CMD:  'Inverted check mark'
PUD | % up for the day | Minimum % up | PUD:  '% up for the day' ('Minimum % up')
PDD | % down for the day | Minimum % down | PDD:  '% down for the day' ('Minimum % down')
BBU | Standard deviation breakout | Standard deviations | BBU:  'Standard deviation breakout' ('Standard deviations')
BBD | Standard deviation breakdown | Standard deviations | BBD:  'Standard deviation breakdown' ('Standard deviations')
CDHR | Crossed daily highs resistance | Days, i.e. 52 week high = 365 | CDHR:  'Crossed daily highs resistance' ('Days, i.e. 52 week high = 365')
CDLS | Crossed daily lows support | Days, i.e. 52 week low = 365 | CDLS:  'Crossed daily lows support' ('Days, i.e. 52 week low = 365')
LBS | Large bid size | Shares, minimum bid size | LBS:  'Large bid size' ('Shares, minimum bid size')
LAS | Large ask size | Shares, minimum ask size | LAS:  'Large ask size' ('Shares, minimum ask size')
MC | Market crossed | Dollars | MC:  'Market crossed' ('Dollars')
MCU | Market crossed up | Dollars | MCU:  'Market crossed up' ('Dollars')
MCD | Market crossed down | Dollars | MCD:  'Market crossed down' ('Dollars')
ML | Market locked |  | ML:  'Market locked'
LSP | Large spread |  | LSP:  'Large spread'
TRA | Trading above | Recent count | TRA:  'Trading above' ('Recent count')
TRB | Trading below | Recent count | TRB:  'Trading below' ('Recent count')
TRAS | Trading above specialist | Recent count | TRAS:  'Trading above specialist' ('Recent count')
TRBS | Trading below specialist | Recent count | TRBS:  'Trading below specialist' ('Recent count')
SD | Offer stepping down |  | SD:  'Offer stepping down'
NYSEBI | NYSE buy imbalance | % of daily volume | NYSEBI:  'NYSE buy imbalance' ('% of daily volume')
NYSESI | NYSE sell imbalance | % of daily volume | NYSESI:  'NYSE sell imbalance' ('% of daily volume')
CAO | Crossed above open | Seconds, minimum since last crossing | CAO:  'Crossed above open' ('Seconds, minimum since last crossing')
CBO | Crossed below open | Seconds, minimum since last crossing | CBO:  'Crossed below open' ('Seconds, minimum since last crossing')
CAC | Crossed above close | Seconds, minimum since last crossing | CAC:  'Crossed above close' ('Seconds, minimum since last crossing')
CBC | Crossed below close | Seconds, minimum since last crossing | CBC:  'Crossed below close' ('Seconds, minimum since last crossing')
CAOC | Crossed above open (confirmed) |  | CAOC:  'Crossed above open (confirmed)'
CBOC | Crossed below open (confirmed) |  | CBOC:  'Crossed below open (confirmed)'
CACC | Crossed above close (confirmed) |  | CACC:  'Crossed above close (confirmed)'
CBCC | Crossed below close (confirmed) |  | CBCC:  'Crossed below close (confirmed)'
SBOO | Sector breakout (from open) | Minimum % up | SBOO:  'Sector breakout (from open)' ('Minimum % up')
SBDO | Sector breakdown (from open) | Minimum % down | SBDO:  'Sector breakdown (from open)' ('Minimum % down')
SBOC | Sector breakout (from close) | Minimum % up | SBOC:  'Sector breakout (from close)' ('Minimum % up')
SBDC | Sector breakdown (from close) | Minimum % down | SBDC:  'Sector breakdown (from close)' ('Minimum % down')
FDP | Positive market divergence | Minimum % up | FDP:  'Positive market divergence' ('Minimum % up')
FDN | Negative market divergence | Minimum % down | FDN:  'Negative market divergence' ('Minimum % down')
C | Consolidation | 2.0 - 10.0, 10 is the highest quality | C:  'Consolidation' ('2.0 - 10.0, 10 is the highest quality')
HRV | High relative volume | Ratio | HRV:  'High relative volume' ('Ratio')
VS1 | 1 minute volume spike | Ratio | VS1:  '1 minute volume spike' ('Ratio')
SV | Strong volume | Ratio | SV:  'Strong volume' ('Ratio')
UNOP | Unusual number of prints | Ratio | UNOP:  'Unusual number of prints' ('Ratio')
STAS | StockTwits activity spike | Ratio | STAS:  'StockTwits activity spike' ('Ratio')
HALT | Halt |  | HALT:  'Halt'
RESUME | Resume |  | RESUME:  'Resume'
RUN | Running up now | $ | RUN:  'Running up now' ('$')
RDN | Running down now | $ | RDN:  'Running down now' ('$')
RU | Running up | 1.0=most, 10.0=fewest | RU:  'Running up' ('1.0=most, 10.0=fewest')
RD | Running down | 1.0=most, 10.0=fewest | RD:  'Running down' ('1.0=most, 10.0=fewest')
RUI | Running up (intermediate) | Ratio | RUI:  'Running up (intermediate)' ('Ratio')
RDI | Running down (intermediate) | Ratio | RDI:  'Running down (intermediate)' ('Ratio')
RUC | Running up (confirmed) | 1.0=most, 10.0=fewest | RUC:  'Running up (confirmed)' ('1.0=most, 10.0=fewest')
RDC | Running down (confirmed) | 1.0=most, 10.0=fewest | RDC:  'Running down (confirmed)' ('1.0=most, 10.0=fewest')
CA200 | Crossed above 200 day moving average |  | CA200:  'Crossed above 200 day moving average'
CB200 | Crossed below 200 day moving average |  | CB200:  'Crossed below 200 day moving average'
CA50 | Crossed above 50 day moving average |  | CA50:  'Crossed above 50 day moving average'
CB50 | Crossed below 50 day moving average |  | CB50:  'Crossed below 50 day moving average'
CA20 | Crossed above 20 day moving average |  | CA20:  'Crossed above 20 day moving average'
CB20 | Crossed below 20 day moving average |  | CB20:  'Crossed below 20 day moving average'
CAVC | Crossed above VWAP |  | CAVC:  'Crossed above VWAP'
CBVC | Crossed below VWAP |  | CBVC:  'Crossed below VWAP'
VDU | Positive VWAP Divergence | Minimum % up | VDU:  'Positive VWAP Divergence' ('Minimum % up')
VDD | Negative VWAP Divergence | Minimum % down | VDD:  'Negative VWAP Divergence' ('Minimum % down')
GDR | Gap down reversal | Total retracement in dollars | GDR:  'Gap down reversal' ('Total retracement in dollars')
GUR | Gap up reversal | Total retracement in dollars | GUR:  'Gap up reversal' ('Total retracement in dollars')
FGUR | False gap up retracement | Percentage of gap filled, 0 - 100 | FGUR:  'False gap up retracement' ('Percentage of gap filled, 0 - 100')
FGDR | False gap down retracement | Percentage of gap filled, 0 - 100 | FGDR:  'False gap down retracement' ('Percentage of gap filled, 0 - 100')
CHBOC | Channel breakout (confirmed) | Same as running up confirmed. | CHBOC:  'Channel breakout (confirmed)' ('Same as running up confirmed.')
CHBDC | Channel breakdown (confirmed) | Same as running down confirmed. | CHBDC:  'Channel breakdown (confirmed)' ('Same as running down confirmed.')
CHBO | Channel breakout | 2.0 - 10.0, based on the channel | CHBO:  'Channel breakout' ('2.0 - 10.0, based on the channel')
CHBD | Channel breakdown | 2.0 - 10.0, based on the channel | CHBD:  'Channel breakdown' ('2.0 - 10.0, based on the channel')
CBO5 | 5 minute consolidation breakout | Dollars | CBO5:  '5 minute consolidation breakout' ('Dollars')
CBD5 | 5 minute consolidation breakdown | Dollars | CBD5:  '5 minute consolidation breakdown' ('Dollars')
CBO10 | 10 minute consolidation breakout | Dollars | CBO10:  '10 minute consolidation breakout' ('Dollars')
CBD10 | 10 minute consolidation breakdown | Dollars | CBD10:  '10 minute consolidation breakdown' ('Dollars')
CBO15 | 15 minute consolidation breakout | Dollars | CBO15:  '15 minute consolidation breakout' ('Dollars')
CBD15 | 15 minute consolidation breakdown | Dollars | CBD15:  '15 minute consolidation breakdown' ('Dollars')
CBO30 | 30 minute consolidation breakout | Dollars | CBO30:  '30 minute consolidation breakout' ('Dollars')
CBD30 | 30 minute consolidation breakdown | Dollars | CBD30:  '30 minute consolidation breakdown' ('Dollars')
CARC | Crossed above resistance (confirmed) | Hours of trading | CARC:  'Crossed above resistance (confirmed)' ('Hours of trading')
CBSC | Crossed below support (confirmed) | Hours of trading | CBSC:  'Crossed below support (confirmed)' ('Hours of trading')
CAR | Crossed above resistance | Hours of trading | CAR:  'Crossed above resistance' ('Hours of trading')
CBS | Crossed below support | Hours of trading | CBS:  'Crossed below support' ('Hours of trading')
BP | Block trade | Minimum number of shares | BP:  'Block trade' ('Minimum number of shares')
GBBOT | Broadening bottom | Hours of trading | GBBOT:  'Broadening bottom' ('Hours of trading')
GBTOP | Broadening top | Hours of trading | GBTOP:  'Broadening top' ('Hours of trading')
GTBOT | Triangle bottom | Hours of trading | GTBOT:  'Triangle bottom' ('Hours of trading')
GTTOP | Triangle top | Hours of trading | GTTOP:  'Triangle top' ('Hours of trading')
GRBOT | Rectangle bottom | Hours of trading | GRBOT:  'Rectangle bottom' ('Hours of trading')
GRTOP | Rectangle top | Hours of trading | GRTOP:  'Rectangle top' ('Hours of trading')
GDBOT | Double bottom | Hours of trading | GDBOT:  'Double bottom' ('Hours of trading')
GDTOP | Double top | Hours of trading | GDTOP:  'Double top' ('Hours of trading')
GHASI | Inverted head and shoulders | Hours of trading | GHASI:  'Inverted head and shoulders' ('Hours of trading')
GHAS | Head and shoulders | Hours of trading | GHAS:  'Head and shoulders' ('Hours of trading')
IDH5 | 5 minute high |  | IDH5:  '5 minute high'
IDL5 | 5 minute low |  | IDL5:  '5 minute low'
IDH10 | 10 minute high |  | IDH10:  '10 minute high'
IDL10 | 10 minute low |  | IDL10:  '10 minute low'
IDH15 | 15 minute high |  | IDH15:  '15 minute high'
IDL15 | 15 minute low |  | IDL15:  '15 minute low'
IDH30 | 30 minute high |  | IDH30:  '30 minute high'
IDL30 | 30 minute low |  | IDL30:  '30 minute low'
IDH60 | 60 minute high |  | IDH60:  '60 minute high'
IDL60 | 60 minute low |  | IDL60:  '60 minute low'
TSPU | Trailing stop, % up | % | TSPU:  'Trailing stop, % up' ('%')
TSPD | Trailing stop, % down | % | TSPD:  'Trailing stop, % down' ('%')
TSSU | Trailing stop, volatility up | Bars | TSSU:  'Trailing stop, volatility up' ('Bars')
TSSD | Trailing stop, volatility down | Bars | TSSD:  'Trailing stop, volatility down' ('Bars')
ORU1 | 1 minute opening range breakout |  | ORU1:  '1 minute opening range breakout'
ORD1 | 1 minute opening range breakdown |  | ORD1:  '1 minute opening range breakdown'
ORU2 | 2 minute opening range breakout |  | ORU2:  '2 minute opening range breakout'
ORD2 | 2 minute opening range breakdown |  | ORD2:  '2 minute opening range breakdown'
ORU5 | 5 minute opening range breakout |  | ORU5:  '5 minute opening range breakout'
ORD5 | 5 minute opening range breakdown |  | ORD5:  '5 minute opening range breakdown'
ORU10 | 10 minute opening range breakout |  | ORU10:  '10 minute opening range breakout'
ORD10 | 10 minute opening range breakdown |  | ORD10:  '10 minute opening range breakdown'
ORU15 | 15 minute opening range breakout |  | ORU15:  '15 minute opening range breakout'
ORD15 | 15 minute opening range breakdown |  | ORD15:  '15 minute opening range breakdown'
ORU30 | 30 minute opening range breakout |  | ORU30:  '30 minute opening range breakout'
ORD30 | 30 minute opening range breakdown |  | ORD30:  '30 minute opening range breakdown'
ORU60 | 60 minute opening range breakout |  | ORU60:  '60 minute opening range breakout'
ORD60 | 60 minute opening range breakdown |  | ORD60:  '60 minute opening range breakdown'
FU38 | Fibonacci 38% buy signal | Hours of trading | FU38:  'Fibonacci 38% buy signal' ('Hours of trading')
FD38 | Fibonacci 38% sell signal | Hours of trading | FD38:  'Fibonacci 38% sell signal' ('Hours of trading')
FU50 | Fibonacci 50% buy signal | Hours of trading | FU50:  'Fibonacci 50% buy signal' ('Hours of trading')
FD50 | Fibonacci 50% sell signal | Hours of trading | FD50:  'Fibonacci 50% sell signal' ('Hours of trading')
FU62 | Fibonacci 62% buy signal | Hours of trading | FU62:  'Fibonacci 62% buy signal' ('Hours of trading')
FD62 | Fibonacci 62% sell signal | Hours of trading | FD62:  'Fibonacci 62% sell signal' ('Hours of trading')
FU79 | Fibonacci 79% buy signal | Hours of trading | FU79:  'Fibonacci 79% buy signal' ('Hours of trading')
FD79 | Fibonacci 79% sell signal | Hours of trading | FD79:  'Fibonacci 79% sell signal' ('Hours of trading')
PEU5 | 5 minute linear regression up trend | Gain forecast, $ | PEU5:  '5 minute linear regression up trend' ('Gain forecast, $')
PED5 | 5 minute linear regression down trend | Gain forecast, $ | PED5:  '5 minute linear regression down trend' ('Gain forecast, $')
PEU15 | 15 minute linear regression up trend | Gain forecast, $ | PEU15:  '15 minute linear regression up trend' ('Gain forecast, $')
PED15 | 15 minute linear regression down trend | Gain forecast, $ | PED15:  '15 minute linear regression down trend' ('Gain forecast, $')
PEU30 | 30 minute linear regression up trend | Gain forecast, $ | PEU30:  '30 minute linear regression up trend' ('Gain forecast, $')
PED30 | 30 minute linear regression down trend | Gain forecast, $ | PED30:  '30 minute linear regression down trend' ('Gain forecast, $')
PEU90 | 90 minute linear regression up trend | Gain forecast, $ | PEU90:  '90 minute linear regression up trend' ('Gain forecast, $')
PED90 | 90 minute linear regression down trend | Gain forecast, $ | PED90:  '90 minute linear regression down trend' ('Gain forecast, $')
SMAU2 | Upward thrust (2 minute) | Suddenness, % | SMAU2:  'Upward thrust (2 minute)' ('Suddenness, %')
SMAD2 | Downward thrust (2 minute) | Suddenness, % | SMAD2:  'Downward thrust (2 minute)' ('Suddenness, %')
SMAU5 | Upward thrust (5 minute) | Suddenness, % | SMAU5:  'Upward thrust (5 minute)' ('Suddenness, %')
SMAD5 | Downward thrust (5 minute) | Suddenness, % | SMAD5:  'Downward thrust (5 minute)' ('Suddenness, %')
SMAU15 | Upward thrust (15 minute) | Suddenness, % | SMAU15:  'Upward thrust (15 minute)' ('Suddenness, %')
SMAD15 | Downward thrust (15 minute) | Suddenness, % | SMAD15:  'Downward thrust (15 minute)' ('Suddenness, %')
X5A8_1 | 5 period SMA crossed above 8 period SMA (1 minute) |  | X5A8_1:  '5 period SMA crossed above 8 period SMA (1 minute)'
X5B8_1 | 5 period SMA crossed below 8 period SMA (1 minute) |  | X5B8_1:  '5 period SMA crossed below 8 period SMA (1 minute)'
X5A8_2 | 5 period SMA crossed above 8 period SMA (2 minute) |  | X5A8_2:  '5 period SMA crossed above 8 period SMA (2 minute)'
X5B8_2 | 5 period SMA crossed below 8 period SMA (2 minute) |  | X5B8_2:  '5 period SMA crossed below 8 period SMA (2 minute)'
X5A8_4 | 5 period SMA crossed above 8 period SMA (4 minute) |  | X5A8_4:  '5 period SMA crossed above 8 period SMA (4 minute)'
X5B8_4 | 5 period SMA crossed below 8 period SMA (4 minute) |  | X5B8_4:  '5 period SMA crossed below 8 period SMA (4 minute)'
X5A8_5 | 5 period SMA crossed above 8 period SMA (5 minute) |  | X5A8_5:  '5 period SMA crossed above 8 period SMA (5 minute)'
X5B8_5 | 5 period SMA crossed below 8 period SMA (5 minute) |  | X5B8_5:  '5 period SMA crossed below 8 period SMA (5 minute)'
X5A8_10 | 5 period SMA crossed above 8 period SMA (10 minute) |  | X5A8_10:  '5 period SMA crossed above 8 period SMA (10 minute)'
X5B8_10 | 5 period SMA crossed below 8 period SMA (10 minute) |  | X5B8_10:  '5 period SMA crossed below 8 period SMA (10 minute)'
X5A8_20 | 5 period SMA crossed above 8 period SMA (20 minute) |  | X5A8_20:  '5 period SMA crossed above 8 period SMA (20 minute)'
X5B8_20 | 5 period SMA crossed below 8 period SMA (20 minute) |  | X5B8_20:  '5 period SMA crossed below 8 period SMA (20 minute)'
X5A8_30 | 5 period SMA crossed above 8 period SMA (30 minute) |  | X5A8_30:  '5 period SMA crossed above 8 period SMA (30 minute)'
X5B8_30 | 5 period SMA crossed below 8 period SMA (30 minute) |  | X5B8_30:  '5 period SMA crossed below 8 period SMA (30 minute)'
ECAY2 | 8 period SMA crossed above 20 period SMA (2 minute) |  | ECAY2:  '8 period SMA crossed above 20 period SMA (2 minute)'
ECBY2 | 8 period SMA crossed below 20 period SMA (2 minute) |  | ECBY2:  '8 period SMA crossed below 20 period SMA (2 minute)'
ECAY5 | 8 period SMA crossed above 20 period SMA (5 minute) |  | ECAY5:  '8 period SMA crossed above 20 period SMA (5 minute)'
ECBY5 | 8 period SMA crossed below 20 period SMA (5 minute) |  | ECBY5:  '8 period SMA crossed below 20 period SMA (5 minute)'
ECAY15 | 8 period SMA crossed above 20 period SMA (15 minute) |  | ECAY15:  '8 period SMA crossed above 20 period SMA (15 minute)'
ECBY15 | 8 period SMA crossed below 20 period SMA (15 minute) |  | ECBY15:  '8 period SMA crossed below 20 period SMA (15 minute)'
YCAD2 | 20 period SMA crossed above 200 period SMA (2 minute) |  | YCAD2:  '20 period SMA crossed above 200 period SMA (2 minute)'
YCBD2 | 20 period SMA crossed below 200 period SMA (2 minute) |  | YCBD2:  '20 period SMA crossed below 200 period SMA (2 minute)'
YCAD5 | 20 period SMA crossed above 200 period SMA (5 minute) |  | YCAD5:  '20 period SMA crossed above 200 period SMA (5 minute)'
YCBD5 | 20 period SMA crossed below 200 period SMA (5 minute) |  | YCBD5:  '20 period SMA crossed below 200 period SMA (5 minute)'
YCAD15 | 20 period SMA crossed above 200 period SMA (15 minute) |  | YCAD15:  '20 period SMA crossed above 200 period SMA (15 minute)'
YCBD15 | 20 period SMA crossed below 200 period SMA (15 minute) |  | YCBD15:  '20 period SMA crossed below 200 period SMA (15 minute)'
MDAS5 | 5 minute MACD crossed above signal |  | MDAS5:  '5 minute MACD crossed above signal'
MDBS5 | 5 minute MACD crossed below signal |  | MDBS5:  '5 minute MACD crossed below signal'
MDAZ5 | 5 minute MACD crossed above zero |  | MDAZ5:  '5 minute MACD crossed above zero'
MDBZ5 | 5 minute MACD crossed below zero |  | MDBZ5:  '5 minute MACD crossed below zero'
MDAS10 | 10 minute MACD crossed above signal |  | MDAS10:  '10 minute MACD crossed above signal'
MDBS10 | 10 minute MACD crossed below signal |  | MDBS10:  '10 minute MACD crossed below signal'
MDAZ10 | 10 minute MACD crossed above zero |  | MDAZ10:  '10 minute MACD crossed above zero'
MDBZ10 | 10 minute MACD crossed below zero |  | MDBZ10:  '10 minute MACD crossed below zero'
MDAS15 | 15 minute MACD crossed above signal |  | MDAS15:  '15 minute MACD crossed above signal'
MDBS15 | 15 minute MACD crossed below signal |  | MDBS15:  '15 minute MACD crossed below signal'
MDAZ15 | 15 minute MACD crossed above zero |  | MDAZ15:  '15 minute MACD crossed above zero'
MDBZ15 | 15 minute MACD crossed below zero |  | MDBZ15:  '15 minute MACD crossed below zero'
MDAS30 | 30 minute MACD crossed above signal |  | MDAS30:  '30 minute MACD crossed above signal'
MDBS30 | 30 minute MACD crossed below signal |  | MDBS30:  '30 minute MACD crossed below signal'
MDAZ30 | 30 minute MACD crossed above zero |  | MDAZ30:  '30 minute MACD crossed above zero'
MDBZ30 | 30 minute MACD crossed below zero |  | MDBZ30:  '30 minute MACD crossed below zero'
MDAS60 | 60 minute MACD crossed above signal |  | MDAS60:  '60 minute MACD crossed above signal'
MDBS60 | 60 minute MACD crossed below signal |  | MDBS60:  '60 minute MACD crossed below signal'
MDAZ60 | 60 minute MACD crossed above zero |  | MDAZ60:  '60 minute MACD crossed above zero'
MDBZ60 | 60 minute MACD crossed below zero |  | MDBZ60:  '60 minute MACD crossed below zero'
SC20_5 | 5 minute stochastic crossed above 20 |  | SC20_5:  '5 minute stochastic crossed above 20'
SC80_5 | 5 minute stochastic crossed below 80 |  | SC80_5:  '5 minute stochastic crossed below 80'
SC20_15 | 15 minute stochastic crossed above 20 |  | SC20_15:  '15 minute stochastic crossed above 20'
SC80_15 | 15 minute stochastic crossed below 80 |  | SC80_15:  '15 minute stochastic crossed below 80'
SC20_60 | 60 minute stochastic crossed above 20 |  | SC20_60:  '60 minute stochastic crossed above 20'
SC80_60 | 60 minute stochastic crossed below 80 |  | SC80_60:  '60 minute stochastic crossed below 80'
DOJ5 | 5 minute Doji |  | DOJ5:  '5 minute Doji'
DOJ10 | 10 minute Doji |  | DOJ10:  '10 minute Doji'
DOJ15 | 15 minute Doji |  | DOJ15:  '15 minute Doji'
DOJ30 | 30 minute Doji |  | DOJ30:  '30 minute Doji'
DOJ60 | 60 minute Doji |  | DOJ60:  '60 minute Doji'
HMR2 | 2 minute hammer | % | HMR2:  '2 minute hammer' ('%')
HMR5 | 5 minute hammer | % | HMR5:  '5 minute hammer' ('%')
HMR10 | 10 minute hammer | % | HMR10:  '10 minute hammer' ('%')
HMR15 | 15 minute hammer | % | HMR15:  '15 minute hammer' ('%')
HMR30 | 30 minute hammer | % | HMR30:  '30 minute hammer' ('%')
HMR60 | 60 minute hammer | % | HMR60:  '60 minute hammer' ('%')
HGM2 | 2 minute hanging man | % | HGM2:  '2 minute hanging man' ('%')
HGM5 | 5 minute hanging man | % | HGM5:  '5 minute hanging man' ('%')
HGM10 | 10 minute hanging man | % | HGM10:  '10 minute hanging man' ('%')
HGM15 | 15 minute hanging man | % | HGM15:  '15 minute hanging man' ('%')
HGM30 | 30 minute hanging man | % | HGM30:  '30 minute hanging man' ('%')
HGM60 | 60 minute hanging man | % | HGM60:  '60 minute hanging man' ('%')
NGU5 | 5 minute bullish engulfing | % | NGU5:  '5 minute bullish engulfing' ('%')
NGU10 | 10 minute bullish engulfing | % | NGU10:  '10 minute bullish engulfing' ('%')
NGU15 | 15 minute bullish engulfing | % | NGU15:  '15 minute bullish engulfing' ('%')
NGU30 | 30 minute bullish engulfing | % | NGU30:  '30 minute bullish engulfing' ('%')
NGD5 | 5 minute bearish engulfing | % | NGD5:  '5 minute bearish engulfing' ('%')
NGD10 | 10 minute bearish engulfing | % | NGD10:  '10 minute bearish engulfing' ('%')
NGD15 | 15 minute bearish engulfing | % | NGD15:  '15 minute bearish engulfing' ('%')
NGD30 | 30 minute bearish engulfing | % | NGD30:  '30 minute bearish engulfing' ('%')
PP5 | 5 minute piercing pattern | % | PP5:  '5 minute piercing pattern' ('%')
PP10 | 10 minute piercing pattern | % | PP10:  '10 minute piercing pattern' ('%')
PP15 | 15 minute piercing pattern | % | PP15:  '15 minute piercing pattern' ('%')
PP30 | 30 minute piercing pattern | % | PP30:  '30 minute piercing pattern' ('%')
DCC5 | 5 minute dark cloud cover | % | DCC5:  '5 minute dark cloud cover' ('%')
DCC10 | 10 minute dark cloud cover | % | DCC10:  '10 minute dark cloud cover' ('%')
DCC15 | 15 minute dark cloud cover | % | DCC15:  '15 minute dark cloud cover' ('%')
DCC30 | 30 minute dark cloud cover | % | DCC30:  '30 minute dark cloud cover' ('%')
BT2 | 2 minute bottoming tail | % | BT2:  '2 minute bottoming tail' ('%')
BT5 | 5 minute bottoming tail | % | BT5:  '5 minute bottoming tail' ('%')
BT10 | 10 minute bottoming tail | % | BT10:  '10 minute bottoming tail' ('%')
BT15 | 15 minute bottoming tail | % | BT15:  '15 minute bottoming tail' ('%')
BT30 | 30 minute bottoming tail | % | BT30:  '30 minute bottoming tail' ('%')
BT60 | 60 minute bottoming tail | % | BT60:  '60 minute bottoming tail' ('%')
TT2 | 2 minute topping tail | % | TT2:  '2 minute topping tail' ('%')
TT5 | 5 minute topping tail | % | TT5:  '5 minute topping tail' ('%')
TT10 | 10 minute topping tail | % | TT10:  '10 minute topping tail' ('%')
TT15 | 15 minute topping tail | % | TT15:  '15 minute topping tail' ('%')
TT30 | 30 minute topping tail | % | TT30:  '30 minute topping tail' ('%')
TT60 | 60 minute topping tail | % | TT60:  '60 minute topping tail' ('%')
NRBB5 | 5 minute narrow range buy bar | % | NRBB5:  '5 minute narrow range buy bar' ('%')
NRBB10 | 10 minute narrow range buy bar | % | NRBB10:  '10 minute narrow range buy bar' ('%')
NRBB15 | 15 minute narrow range buy bar | % | NRBB15:  '15 minute narrow range buy bar' ('%')
NRBB30 | 30 minute narrow range buy bar | % | NRBB30:  '30 minute narrow range buy bar' ('%')
NRSB5 | 5 minute narrow range sell bar | % | NRSB5:  '5 minute narrow range sell bar' ('%')
NRSB10 | 10 minute narrow range sell bar | % | NRSB10:  '10 minute narrow range sell bar' ('%')
NRSB15 | 15 minute narrow range sell bar | % | NRSB15:  '15 minute narrow range sell bar' ('%')
NRSB30 | 30 minute narrow range sell bar | % | NRSB30:  '30 minute narrow range sell bar' ('%')
GBR2 | 2 minute green bar reversal | Count | GBR2:  '2 minute green bar reversal' ('Count')
GBR5 | 5 minute green bar reversal | Count | GBR5:  '5 minute green bar reversal' ('Count')
GBR15 | 15 minute green bar reversal | Count | GBR15:  '15 minute green bar reversal' ('Count')
GBR60 | 60 minute green bar reversal | Count | GBR60:  '60 minute green bar reversal' ('Count')
RBR2 | 2 minute red bar reversal | Count | RBR2:  '2 minute red bar reversal' ('Count')
RBR5 | 5 minute red bar reversal | Count | RBR5:  '5 minute red bar reversal' ('Count')
RBR15 | 15 minute red bar reversal | Count | RBR15:  '15 minute red bar reversal' ('Count')
RBR60 | 60 minute red bar reversal | Count | RBR60:  '60 minute red bar reversal' ('Count')
C1U_2 | 2 minute 1-2-3 continuation buy signal |  | C1U_2:  '2 minute 1-2-3 continuation buy signal'
C1U_5 | 5 minute 1-2-3 continuation buy signal |  | C1U_5:  '5 minute 1-2-3 continuation buy signal'
C1U_15 | 15 minute 1-2-3 continuation buy signal |  | C1U_15:  '15 minute 1-2-3 continuation buy signal'
C1U_60 | 60 minute 1-2-3 continuation buy signal |  | C1U_60:  '60 minute 1-2-3 continuation buy signal'
C1D_2 | 2 minute 1-2-3 continuation sell signal |  | C1D_2:  '2 minute 1-2-3 continuation sell signal'
C1D_5 | 5 minute 1-2-3 continuation sell signal |  | C1D_5:  '5 minute 1-2-3 continuation sell signal'
C1D_15 | 15 minute 1-2-3 continuation sell signal |  | C1D_15:  '15 minute 1-2-3 continuation sell signal'
C1D_60 | 60 minute 1-2-3 continuation sell signal |  | C1D_60:  '60 minute 1-2-3 continuation sell signal'
C1U_2S | 2 minute 1-2-3 continuation buy setup |  | C1U_2S:  '2 minute 1-2-3 continuation buy setup'
C1U_5S | 5 minute 1-2-3 continuation buy setup |  | C1U_5S:  '5 minute 1-2-3 continuation buy setup'
C1U_15S | 15 minute 1-2-3 continuation buy setup |  | C1U_15S:  '15 minute 1-2-3 continuation buy setup'
C1U_60S | 60 minute 1-2-3 continuation buy setup |  | C1U_60S:  '60 minute 1-2-3 continuation buy setup'
C1D_2S | 2 minute 1-2-3 continuation sell setup |  | C1D_2S:  '2 minute 1-2-3 continuation sell setup'
C1D_5S | 5 minute 1-2-3 continuation sell setup |  | C1D_5S:  '5 minute 1-2-3 continuation sell setup'
C1D_15S | 15 minute 1-2-3 continuation sell setup |  | C1D_15S:  '15 minute 1-2-3 continuation sell setup'
C1D_60S | 60 minute 1-2-3 continuation sell setup |  | C1D_60S:  '60 minute 1-2-3 continuation sell setup'
OVO5U | Bullish opening power bar |  | OVO5U:  'Bullish opening power bar'
OVO5D | Bearish opening power bar |  | OVO5D:  'Bearish opening power bar'
NR7_1 | NR7 (1 minute) | Count | NR7_1:  'NR7 (1 minute)' ('Count')
NR7_2 | NR7 (2 minute) | Count | NR7_2:  'NR7 (2 minute)' ('Count')
NR7_5 | NR7 (5 minute) | Count | NR7_5:  'NR7 (5 minute)' ('Count')
NR7_10 | NR7 (10 minute) | Count | NR7_10:  'NR7 (10 minute)' ('Count')
NR7 | NR7 (15 minute) | Count | NR7:  'NR7 (15 minute)' ('Count')
NR7_30 | NR7 (30 minute) | Count | NR7_30:  'NR7 (30 minute)' ('Count')
WRB2 | 2 minute wide range bar | $ | WRB2:  '2 minute wide range bar' ('$')
WRB5 | 5 minute wide range bar | $ | WRB5:  '5 minute wide range bar' ('$')
WRB15 | 15 minute wide range bar | $ | WRB15:  '15 minute wide range bar' ('$')
HB | Heartbeat | Minutes | HB:  'Heartbeat' ('Minutes')
DEMO | Tests and Demonstrations |  | DEMO:  'Tests and Demonstrations'
*/
