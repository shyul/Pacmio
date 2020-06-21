/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// https://interactivebrokers.github.io/tws-api/tick_types.html
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /// <summary>
        /// (0)"84"-(1)"90010000"-(2)"BRFG"-(3)"BRFG$0bc6e1ef"
        /// </summary>
        /// <param name="value"></param>
        public static void RequestNewsArticle(string providerCode, string articleId, ICollection<(string, string)> newsArticleOptions = null)
        {
            if (Connected)
            {
                (int requestId, string typeStr) = RegisterRequest(RequestType.RequestNewsArticle);

                SendRequest(new string[] {
                    typeStr, // 84
                    requestId.Param(),
                    providerCode,
                    articleId,
                    newsArticleOptions.Param()
                }); ;
            }
        }

        /// <summary>
        /// (0)"83"-(1)"90010000"-(2)"0"-(3)"<html><body><p><strong>IBM (IBM -6%)</strong> is trading lower following its Q3 earnings report last night. It was a $0.02 beat with a slight revenue miss ($18.03 bln vs $18.29 bln consensus). Revs were down 3.9% yr/yr. IBM also reaffirmed full-year non-GAAP EPS guidance of at least $12.80 and reaffirmed free cash flow of approximately $12 bln. Gross margin slipped a bit to 46.2% from 47.0% in Q2 and 46.9% in the prior year period. </p><p>Global Business Services (GBS) revenue was up over 2%, led by consulting primarily for next-generation application offerings and an application modernization for the cloud. However, the Global Technology Services (GTS) segment saw revenue decline 4% as IBM exits lower margin offerings. Also, IBM was hurt by lower-than-expected volumes in certain markets and some multinational clients. Systems revenue was down 14%, primarily reflecting lower mainframe sales.  </p><p>Every quarter, the market is always scrutinizing IBM's cloud performance. Cloud &amp; Cognitive Software revenue rose 6.4% to $5.3 bln, a nice acceleration from +3.2% in Q2. This was led by security, IoT, data and AI platforms, and hybrid cloud. Cloud and data platforms, were up 17% while cognitive applications were up 4%. Cloud revenue of $5.0 bln in Q3 was up 11%. A key metric investors watch is cloud revenue over past 12 months and that came in at $20 bln, up from $19.5 bln in Q2. </p><p>It's important to mention that IBM finally closed on its purchase of Red Hat on July 9. As we have mentioned before, IBM made a huge cloud bet with this deal. The rationale for the purchase has been that the bulk (80%) of client business has yet to move to the cloud, held back by the proprietary nature of today's cloud market. IBM's huge scale in combination with the largest open source software provider (Red Hat) seems well poised to tackle this opportunity.  </p><p>This was the first quarter with Red Hat in the fold. As such, it was good to see IBM reaffirm and not lower guidance now that Red Hat is included and IBM is digging into it. IBM provided post-Red Hat guidance in early August and it's sticking with that, so that's good to see. </p><p>A big problem for IBM is that it has historically been a hardware-based, large mainframe type business, but the world is changing. Enterprise customers have been moving to the cloud and network virtualization. Also, competition has gotten more intense over the years as younger upstarts have been built from the ground up on cloud, mobility, virtualization etc. For example, Amazon (AMZN) with its AWS offering, and Microsoft's (MSFT) Azure have been very successful in cloud computing and IBM has been trying to play catch up.  </p><p>In sum, cloud revenue was decent, but mainframe sales and traditional services revenue were down. Total revenue was a miss and gross margin saw some compression. After a $0.09 beat last quarter, a $0.02 beat this time is a bit disappointing. All of this is weighing on the stock today. </p><p>Investors understand that it's going to take a while for IBM to catch up with its cloud computing rivals. The Red Hat acquisition seems like a great step in that direction, but the full benefit of this combination will take some time to be fully realized. It seems investors are willing to give IBM the benefit of the doubt for now but with five consecutive revenue declines, we sense that their patience is being tested. </p><BR><BR>Copyright (C) 2019 Briefing.com</body></html>"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_NewsArticle(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
        }
    }

    /*
    Received Error: (0)"4"-(1)"2"-(2)"90020000"-(3)"321"-(4)"Error validating request:-'cg' : cause - Not subscribed for 'BZ' provider"
    Send RequestHistoricalNews: (0)"86"-(1)"90020000"-(2)"8314"-(3)"BRFG"-(4)"2020-01-28 16:21:07.0"-(5)"2020-01-29 16:21:07.0"-(6)"5"
    Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2106"-(4)"HMDS data farm connection is OK:ushmds"
    Received HistoricalNews: (0)"86"-(1)"90020000"-(2)"2019-10-17 14:46:06.0"-(3)"BRFG"-(4)"BRFG$0bc6e1ef"-(5)"{K:1.00}IBM is wearing a Red Hat but investors are disappointed with Q3 results"
    Received HistoricalNews: (0)"86"-(1)"90020000"-(2)"2019-07-18 19:36:21.0"-(3)"BRFG"-(4)"BRFG$0b37971a"-(5)"{K:1.00}IBM trades higher on Q2 results as investors await Red Hat guidance"
    Received HistoricalNews: (0)"86"-(1)"90020000"-(2)"2019-04-17 15:11:32.0"-(3)"BRFG"-(4)"BRFG$0a7cf715"-(5)"{K:1.00}IBM trades lower on Q1 revenue miss but co reaffirmed FY19 guidance"
    Received HistoricalNews: (0)"86"-(1)"90020000"-(2)"2019-01-23 14:24:27.0"-(3)"BRFG"-(4)"BRFG$09cb908d"-(5)"{K:1.00}IBM trades up nicely on Q4 results; first time they grew full year revs and EPS in six years"
    Received HistoricalNews: (0)"86"-(1)"90020000"-(2)"2018-10-29 13:56:15.0"-(3)"BRFG"-(4)"BRFG$092d3a90"-(5)"{K:1.00}IBM takes bold step in acquiring Red Hat in big hybrid cloud deal"
    Received HistoricalNewsEnd: (0)"87"-(1)"90020000"-(2)"1"

    Send RequestNewsArticle: (0)"84"-(1)"90010000"-(2)"BRFG"-(3)"BRFG$0bc6e1ef"
    Received NewsArticle: (0)"83"-(1)"90010000"-(2)"0"-(3)"<html><body><p><strong>IBM (IBM -6%)</strong> is trading lower following its Q3 earnings report last night. It was a $0.02 beat with a slight revenue miss ($18.03 bln vs $18.29 bln consensus). Revs were down 3.9% yr/yr. IBM also reaffirmed full-year non-GAAP EPS guidance of at least $12.80 and reaffirmed free cash flow of approximately $12 bln. Gross margin slipped a bit to 46.2% from 47.0% in Q2 and 46.9% in the prior year period. </p><p>Global Business Services (GBS) revenue was up over 2%, led by consulting primarily for next-generation application offerings and an application modernization for the cloud. However, the Global Technology Services (GTS) segment saw revenue decline 4% as IBM exits lower margin offerings. Also, IBM was hurt by lower-than-expected volumes in certain markets and some multinational clients. Systems revenue was down 14%, primarily reflecting lower mainframe sales.  </p><p>Every quarter, the market is always scrutinizing IBM's cloud performance. Cloud &amp; Cognitive Software revenue rose 6.4% to $5.3 bln, a nice acceleration from +3.2% in Q2. This was led by security, IoT, data and AI platforms, and hybrid cloud. Cloud and data platforms, were up 17% while cognitive applications were up 4%. Cloud revenue of $5.0 bln in Q3 was up 11%. A key metric investors watch is cloud revenue over past 12 months and that came in at $20 bln, up from $19.5 bln in Q2. </p><p>It's important to mention that IBM finally closed on its purchase of Red Hat on July 9. As we have mentioned before, IBM made a huge cloud bet with this deal. The rationale for the purchase has been that the bulk (80%) of client business has yet to move to the cloud, held back by the proprietary nature of today's cloud market. IBM's huge scale in combination with the largest open source software provider (Red Hat) seems well poised to tackle this opportunity.  </p><p>This was the first quarter with Red Hat in the fold. As such, it was good to see IBM reaffirm and not lower guidance now that Red Hat is included and IBM is digging into it. IBM provided post-Red Hat guidance in early August and it's sticking with that, so that's good to see. </p><p>A big problem for IBM is that it has historically been a hardware-based, large mainframe type business, but the world is changing. Enterprise customers have been moving to the cloud and network virtualization. Also, competition has gotten more intense over the years as younger upstarts have been built from the ground up on cloud, mobility, virtualization etc. For example, Amazon (AMZN) with its AWS offering, and Microsoft's (MSFT) Azure have been very successful in cloud computing and IBM has been trying to play catch up.  </p><p>In sum, cloud revenue was decent, but mainframe sales and traditional services revenue were down. Total revenue was a miss and gross margin saw some compression. After a $0.09 beat last quarter, a $0.02 beat this time is a bit disappointing. All of this is weighing on the stock today. </p><p>Investors understand that it's going to take a while for IBM to catch up with its cloud computing rivals. The Red Hat acquisition seems like a great step in that direction, but the full benefit of this combination will take some time to be fully realized. It seems investors are willing to give IBM the benefit of the doubt for now but with five consecutive revenue declines, we sense that their patience is being tested. </p><BR><BR>Copyright (C) 2019 Briefing.com</body></html>"

    Unknown Message: 84: (0)"84"-(1)"2"-(2)"1578657239000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2d715e"-(5)"Bernstein initiated Facebook (FB) coverage with Outperform"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"2"-(2)"1580131054000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c40ad1f"-(5)"Stifel reiterated Facebook (FB) coverage with Buy and target $250"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"2"-(2)"1580297481000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c446c2c"-(5)"Raymond James upgraded Facebook (FB) to Strong Buy with target $270"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"2"-(2)"1580327839000"-(3)"BRFG"-(4)"BRFG$0c44f74e"-(5)"Facebook hits record high ahead of earnings this afternoon: Q4 Preview"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"2"-(2)"1580383924000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c45c81a"-(5)"Pivotal Research Group downgraded Facebook (FB) to Hold with target $215"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"1"-(2)"1571916546000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bcf0621"-(5)"Mizuho reiterated Xilinx (XLNX) coverage with Buy and target $125"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"1"-(2)"1571916598000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bcede23"-(5)"BMO Capital Markets reiterated Xilinx (XLNX) coverage with Market Perform and target $95"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"1"-(2)"1571916684000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bcede91"-(5)"Barclays reiterated Xilinx (XLNX) coverage with Equal Weight and target $91"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"1"-(2)"1578659832000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2d719a"-(5)"BofA/Merrill downgraded Xilinx (XLNX) to Underperform"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"1"-(2)"1579175694000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3484fb"-(5)"Mizuho downgraded Xilinx (XLNX) to Neutral with target $106"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"11"-(2)"1569502028000"-(3)"BRFUPDN"-(4)"BRFUPDN$0baca33b"-(5)"Morgan Stanley downgraded NIO (NIO) to Equal-Weight"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"11"-(2)"1569502335000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bac8e95"-(5)"Wolfe Research downgraded NIO (NIO) to Peer Perform"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"11"-(2)"1570192133000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bb723d8"-(5)"Goldman downgraded NIO (NIO) to Neutral"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"11"-(2)"1575378914000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c01830d"-(5)"Piper Jaffray initiated NIO (NIO) coverage with Neutral"-(6)"K:0.37"
    Unknown Message: 84: (0)"84"-(1)"11"-(2)"1577792421000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2186b2"-(5)"BofA/Merrill upgraded NIO (NIO) to Neutral with target $3.8"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"5"-(2)"1571836044000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bcd61c5"-(5)"Credit Suisse reiterated Alphabet A (GOOGL) coverage with Outperform and target $1700"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"5"-(2)"1576852581000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c181939"-(5)"Cleveland Research resumed Alphabet A (GOOGL) coverage with Neutral"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"5"-(2)"1579269264000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c35f70f"-(5)"UBS reiterated Alphabet A (GOOGL) coverage with Buy and target $1675"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"5"-(2)"1580131207000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c40ad21"-(5)"Mizuho reiterated Alphabet A (GOOGL) coverage with Buy and target $1650"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"5"-(2)"1580221713000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c42a3ed"-(5)"Aegis Capital reiterated Alphabet A (GOOGL) coverage with Buy and target $1800"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"3"-(2)"1580292514000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c4433f5"-(5)"Cowen reiterated Apple (AAPL) coverage with Outperform and target $370"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"3"-(2)"1580301279000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c4448f0"-(5)"Monness Crespi & Hardt reiterated Apple (AAPL) coverage with Buy and target $370"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"3"-(2)"1580302468000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c444b6e"-(5)"Maxim Group upgraded Apple (AAPL) to Hold"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"3"-(2)"1580303569000"-(3)"BRFG"-(4)"BRFG$0c446a14"-(5)"Apple shines with fiscal Q1 earnings results"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"3"-(2)"1580383657000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c45fa57"-(5)"DZ Bank upgraded Apple (AAPL) to Buy with target $355"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"4"-(2)"1567770581000"-(3)"BRFUPDN"-(4)"BRFUPDN$0b9487c4"-(5)"Telsey Advisory Group reiterated lululemon athletica (LULU) coverage with Outperform and target $220"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"4"-(2)"1569240684000"-(3)"BRFUPDN"-(4)"BRFUPDN$0ba7b115"-(5)"Piper Jaffray initiated lululemon athletica (LULU) coverage with Overweight and target $227"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"4"-(2)"1572259162000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bd2c877"-(5)"Citigroup downgraded lululemon athletica (LULU) to Neutral with target $205"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"4"-(2)"1573016401000"-(3)"BRFUPDN"-(4)"BRFUPDN$0be0aba3"-(5)"Raymond James initiated lululemon athletica (LULU) coverage with Strong Buy and target $275"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"4"-(2)"1574681535000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bf85916"-(5)"Barclays initiated lululemon athletica (LULU) coverage with Overweight and target $257"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"15"-(2)"1553011096000"-(3)"BRFG"-(4)"BRFG$0a3f4fb0"-(5)"Looking Ahead - March 20, 2019 - FOMC Decision and Press Conference"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"15"-(2)"1554396626000"-(3)"BRFG"-(4)"BRFG$0a6391be"-(5)"Looking Ahead - April 5, 2019 - Employment Situation Report"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"15"-(2)"1556643188000"-(3)"BRFG"-(4)"BRFG$0a96120d"-(5)"Looking Ahead - May 1, 2019 - FOMC Decision and Press Conference"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"15"-(2)"1556812688000"-(3)"BRFG"-(4)"BRFG$0a9b14b8"-(5)"Looking Ahead - May 3, 2019 - Employment Situation Report"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"15"-(2)"1557500067000"-(3)"BRFG"-(4)"BRFG$0aae57aa"-(5)"Looking Ahead - May 13, 2019 - Trade talk aftermath"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"12"-(2)"1558349103000"-(3)"BRFUPDN"-(4)"BRFUPDN$0ac092f8"-(5)"Robert W. Baird downgraded Keysight (KEYS) to Neutral with target $82"-(6)"K:-0.96"
    Unknown Message: 84: (0)"84"-(1)"12"-(2)"1563795966000"-(3)"BRFUPDN"-(4)"BRFUPDN$0b3c8c64"-(5)"Goldman downgraded Keysight (KEYS) to Neutral"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"12"-(2)"1565270642000"-(3)"BRFUPDN"-(4)"BRFUPDN$0b645cb3"-(5)"Barclays initiated Keysight (KEYS) coverage with Overweight and target $103"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"12"-(2)"1566469823000"-(3)"BRFUPDN"-(4)"BRFUPDN$0b80983a"-(5)"Robert W. Baird upgraded Keysight (KEYS) to Outperform with target $100"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"12"-(2)"1571371221000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bc7f33f"-(5)"Susquehanna initiated Keysight (KEYS) coverage with Positive and target $115"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"6"-(2)"1540312954000"-(3)"BRFUPDN"-(4)"BRFUPDN$09216beb"-(5)"Stifel resumed Natl Instruments (NATI) coverage with Hold"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"6"-(2)"1540568068000"-(3)"BRFUPDN"-(4)"BRFUPDN$09295a68"-(5)"Deutsche Bank upgraded Natl Instruments (NATI) to Buy with target $50"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"6"-(2)"1556707395000"-(3)"BRFUPDN"-(4)"BRFUPDN$0a97645d"-(5)"Deutsche Bank downgraded Natl Instruments (NATI) to Hold"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"6"-(2)"1571371215000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bc7f33c"-(5)"Susquehanna initiated Natl Instruments (NATI) coverage with Neutral"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"6"-(2)"1578572806000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2bbd77"-(5)"Robert W. Baird upgraded Natl Instruments (NATI) to Outperform with target $50"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"9"-(2)"1553512603000"-(3)"BRFUPDN"-(4)"BRFUPDN$0a4a69b9"-(5)"Morgan Stanley upgraded New Oriental Education & Technology (EDU) to Overweight"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"9"-(2)"1555417431000"-(3)"BRFUPDN"-(4)"BRFUPDN$0a793d85"-(5)"Deutsche Bank upgraded New Oriental Education & Technology (EDU) to Buy"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"9"-(2)"1556119344000"-(3)"BRFUPDN"-(4)"BRFUPDN$0a88ea27"-(5)"The Benchmark Company reiterated New Oriental Education & Technology (EDU) coverage with Buy and target $105"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"9"-(2)"1571315971000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bc66a18"-(5)"JP Morgan resumed New Oriental Education & Technology (EDU) coverage with Overweight and target $143"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"9"-(2)"1579611043000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c394a8a"-(5)"CLSA downgraded New Oriental Education & Technology (EDU) to Outperform"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"13"-(2)"1571397880000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bc7f343"-(5)"UBS downgraded Agilent (A) to Neutral with target $82"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"13"-(2)"1573794010000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bed94a6"-(5)"Stifel initiated Agilent (A) coverage with Hold"-(6)"K:0.98"
    Unknown Message: 84: (0)"84"-(1)"13"-(2)"1574770926000"-(3)"BRFUPDN"-(4)"BRFUPDN$0bfaaa19"-(5)"Needham reiterated Agilent (A) coverage with Buy and target $85"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"13"-(2)"1578396413000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c287fe3"-(5)"Citigroup initiated Agilent (A) coverage with Neutral and target $85"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"13"-(2)"1578482921000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c2a1b39"-(5)"Wells Fargo initiated Agilent (A) coverage with Overweight and target $100"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"8"-(2)"1579175493000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3484f4"-(5)"Morgan Stanley downgraded Tesla (TSLA) to Underweight with target $360"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"8"-(2)"1579180053000"-(3)"BRFG"-(4)"BRFG$0c34829e"-(5)"Tesla stock losing power for multiple reasons"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"8"-(2)"1579696875000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3b41fb"-(5)"Wedbush reiterated Tesla (TSLA) coverage with Neutral and target $550"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"8"-(2)"1579780361000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3d10f5"-(5)"Exane BNP Paribas downgraded Tesla (TSLA) to Neutral with target $555"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"8"-(2)"1579783831000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3d1106"-(5)"UBS resumed Tesla (TSLA) coverage with Sell and target $410"-(6)"K:n/a"
    Unknown Message: 84: (0)"84"-(1)"7"-(2)"1575977711000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c0a6f4e"-(5)"Needham downgraded Netflix (NFLX) to Underperform"-(6)"K:-1.00"
    Unknown Message: 84: (0)"84"-(1)"7"-(2)"1576760185000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c167f1f"-(5)"Pivotal Research Group reiterated Netflix (NFLX) coverage with Buy and target $425"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"7"-(2)"1579696575000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c3b205a"-(5)"Monness Crespi & Hardt reiterated Netflix (NFLX) coverage with Buy and target $400"-(6)"K:1.00"
    Unknown Message: 84: (0)"84"-(1)"7"-(2)"1579697201000"-(3)"BRFG"-(4)"BRFG$0c3b3f46"-(5)"Mixed reviews for Netflix's fourth quarter report"-(6)"K:0.88"
    Unknown Message: 84: (0)"84"-(1)"7"-(2)"1580131130000"-(3)"BRFUPDN"-(4)"BRFUPDN$0c40ad20"-(5)"Citigroup reiterated Netflix (NFLX) coverage with Neutral and target $350"-(6)"K:n/a"
    */
}
