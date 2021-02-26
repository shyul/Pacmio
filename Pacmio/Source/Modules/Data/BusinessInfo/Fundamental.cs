/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public enum FundamentalDataType : int
    {
        [EnumMember, Description("Outstanding Shares")]
        ShareOut = 10,

        [EnumMember, Description("Floating Shares")]
        ShareFloat = 20,

        [EnumMember, Description("EPS")]
        EPS = 100,

        [EnumMember, Description("Total Revenue")]
        Revenue = 200,

        [EnumMember, Description("Number Employees")]
        NumberEmployees = 10000,

        [EnumMember, Description("Number Shareholders")]
        NumberShareholders = 20000,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public sealed class PeerInfo
    {
        [DataMember]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public string FullName { get; set; } = string.Empty;

        [DataMember]
        public HashSet<string> IndexConstituet { get; set; } = new HashSet<string>();

        [DataMember]
        public SortedDictionary<(string Type, string Code), int> Sectors { get; private set; } = new SortedDictionary<(string Type, string Code), int>();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public class FinancialStatement : IEquatable<FinancialStatement>
    {
        [DataMember]
        public FinancialStatementType Type { get; set; }

        [DataMember]
        public DataSourceType Source { get; set; }

        [DataMember] // SourceDate
        public DateTime Date { get; set; }

        [DataMember]
        public Period Period { get; set; } // <PeriodLength>52</PeriodLength><periodType Code="W">Weeks</periodType> Months 12, Weeks 52,  , Weeks 53, Months 3, Months 9, Months 6, Weeks 16, Weeks 12, Weeks 40, Weeks 28, Weeks 13, Weeks 14, Weeks 39, Weeks 26, Months 11, Weeks 36, Weeks 24, Months 2, Weeks 5, Weeks 48, Weeks 66, Months 15, Months 1, Months 10, Months 4, Months 8, Months 7, Months 5, Weeks 31, Weeks 51, Weeks 27, Weeks 17, Months 13, Weeks 25, Weeks 35

        //[DataMember]
        //public DateTime EndDate { get; set; }

        [DataMember]
        public string[] Params { get; set; }

        /*
        [DataMember]
        public string Source { get; set; } // 10-K, ARS, PROSPECTUS, 10-Q, Interim Report, 20-F, 6-K, 40-F, N-CSR, N-CSRS, 10-KT, PROSPECTUS/A, PRESS, 10-K/A, 8-K, PROSPECTUS PRELIM, Annual Audited Accounts, 10-Q/A, Interim Report/A, ARS/A, Yuho, N-CSRS/A, 20-F/A, 10-QT, N-30B-2, Tanshin, N-CSR/A, N-30D, 10KSB

        [DataMember]
        public string UpdateType { get; set; } // Updated Normal, Reclassified Normal, Restated Normal, Updated Calculated, Restated Calculated, Updated Special, Restated Special, Reclassified Special, Reclassified Calculated

        [DataMember]
        public string Auditor { get; set; } // BDO USA, LLP, PricewaterhouseCoopers LLP, Ernst & Young LLP, Deloitte & Touche LLP, , PricewaterhouseCoopers LLC, Ernst & Young AG, Richey, May & Co., P.C., KPMG LLP, KPMG Audit LLP, Ernst  & Young CPA, Ernst & Young, Grant Thornton LLP, Baker Tilly Virchow Krause, LLP, Price- Waterhouse & Co., PricewaterhouseCoopers Accountants N.V., Ernst & Young Accountants LLP, PricewaterhouseCoopers NV, Deloitte LLP, Price Waterhouse & Co SRL, PricewaterhouseCoopers Ltd, MOSS ADAMS LLP, KPMG Audit PLC, RSM US LLP, McGladrey LLP, Deloitte, Hadjipavlou, Sofianos & Cambanis S.A., A.V.DESHPANDE & COMPANY, ASA & Associates, Grant Thornton India, Pistrelli, Henry Martin Y Asociados S.R.L., Mancera, SC, Tait- Weller & Baker, Farmer, Fuqua & Huff, P.C., PricewaterhouseCoopers Zhong Tian, Ernst & Young Hua Ming, Deloitte Touche Tohmatsu LLP, Deloitte Touche Tohmatsu LLC, Ernst & Young Audit, Ernst & Young Inc., KPMG, Deloitte, Deloitte & Touche Ltd., PricewaterhouseCoopers SC, Ernst & Young S.L., Ernst & Young Associates LLP, PricewaterhouseCoopers Hongkong, Gaveglio Aparicio y Asociados Sociedad Civil de Responsabili, Paredes, Zaldivar, Burga & Asociados, KPMG Auditores Independentes, PricewaterhouseCoopers Auditores Independentes, Deloitte SL, Deloitte Touche Tohmatsu CPA LLP, Crowe Horwath LLP, Hein & Associates, Deloitte & Co SA, Deloitte SA, Ernst & Young Ltda., Ernst & Young Ltd, BDO China Shu Lun Pan CPAs, BDO, Deloitte & Touche, Deloitte Advisory SL, PricewaterhouseCoopers, Deloitte Auditores y Consultores Ltda, Deloitte Touche Tohmatsu Auditores Independentes, Deloitte Touche Tohmatsu, BKD, LLP, Ernst & Young Auditores Independentes, S.S., Ernst & Young Auditores Independentes S. S., Ernst & Young Auditores Independentes SC, UHY LLP, Deloitte Bedrijfsrevisoren, PricewaterhouseCoopers Bedijfsrevisoren, PricewaterhouseCoopers Ltda, Galaz, Yamazaki, Ruiz Urquiza, S.C., Ernst & Young Hua Ming LLP, Victor Burga, Ernst & Young ShinNihon LLC, Marcum LLP, KPMG Financial Risk & Actuarial Services Ltd, KSP Group, INC, Kabani & Co., Inc., Somekh Chaikin, CPAs, Ernst & Young (Hellas) SA, Ernst & Young et Autres, Marco Antonio Zaldivar, ParenteBeard LLC, PricewaterhouseCoopers SA, KPMG Cardenas Dosal, S.C., KPMG AG Wirtschaftsprufungsgesellschaft, KPMG AG, Plante & Moran PLLC, BDO LLP, KPMG Ltd, Deloitte AS, Ernst & Young AS, KPMG AS, KPMG Inc, Ernst & Young SpA, Reconta Ernst & Young SpA, PricewaterhouseCoopers Audit, KPMG Bedrijfsrevisoren, Deloitte Touche Tohmatsu CPA Ltd., Grant Thornton Al-Qatami,Al-Aiban & Partners, BDO RCS Auditores Independentes Sociedade Simples, Baker Tilly Brasil Auditores Independentes, McGladrey & Pullen, LLP, EisnerAmper LLP, KMPG, KPMG Auditores Consultores Ltda, Weaver & Tidwell, EKS&H LLLP, PricewaterhouseCoopers AS, Rubin Brown LLP, Malone Bailey, LLP, Amper- Politziner & Mattia, Withum- Smith & Brown, Dixon Hughes Goodman LLP, Hacker, Johnson & Smith PA, Zwick and Banyai, PLLC, CohnReznick LLP, Deloitte Haskins & Sells, Mc Lean, Deloitte Haskins & Sells LLP, BSR & Co., KPMG AZSA LLC, BSR & Co. LLP, Ernst & Young S.A., Kost, Forer, Gabbay & Kasierer, PricewaterhouseCoopers Inc, Somekh Chaikin KPMG., KPMG Accountants NV, McGOVERN, HURLEY, CUNNINGHAM, LLP, PricewaterhouseCoopers SpA, WithumSmith Brown PC, Ernst & Young LLC, ZAO Deloitte & Touche CIS, Accuity LLP, Marcum Bernstein & Pinchuk LLP, Samil PricewaterhouseCoopers, Arata Auditing Firm, Samil Accounting Corp., KPMG CPA, PricewaterhouseCoopers Oy, PKF O'Connor Davies, KPMG Samjong Accounting Corp, Brightman Almagor Zohar & Co, Deloitte Anjin LLC, WeiserMazars LLP, PricewaterhouseCoopers AG, Ernst & Young OOO, Crowe MacKay LLP, Mackay LLP, MacKay & Partners LLP, KPMG S.A., Deloitte & Associes, Moore Stephens CPA Limited, Moore Stephens, Deloitte Audit, Ernst & Young GmbH, PRICEWATERHOUSECOOPERS STATSAUTORISERET REVISIONSPARTNERSELS, PricewaterhouseCoopers LLP (USA), Whitley Penn LLP, Sycip- Gorres- Velayo & Co., Crowe Clark Whitehill LLP, PricewaterhouseCoopers CI LLP, MSPC, Arthur Andersen LLP, PwC Zhong Tian CPAs Limited Company Shanghai, China, Grant Thornton Auditores Independentes, Davidson & Co LLP, BDO Ziv Haft, BDO Ziv & Haft, PricewaterhouseCoopers Societe Cooperative, Ernst & Young AB, DRT Bagimsiz Denetim ve Serbest Muhasebeci Mali Musavirlik, Purwantono, Suherman & Surja, Pannell Kerr Forster of Texas, P.C., BDO RCS Auditores Independentes, PricewaterhouseCoopers SRL., BPM LLP, Burr, Pilger & Mayer, MN BLUM LLC, Aronson & Co, Rothstein- Kass & Co., KGS LLP, Frazier & Deeter, LLC, Kingery & Crouse. P.A., Kingery, Crouse & Hohl P.A., Blackman Kallick Bartelstein, LLP, Kesselman & Kesselman, GBH CPAs PC, PricewaterhouseCoopers Auditores SA, Lane- Gorman- Trubitt & Co., MNP LLP, PKF, Mc Elroy Quirk & Co., Swalm & Associates, BDO Canada LLP, Hein & Associates LLP, Not Available, Deloitte & Touche SA, Ernst & Young Bedrijfsrevisoren BCV, BKM Sowan Horan LLP, B F Borgers CPA PC, ARSHAK DAVTYAN, INC, Hansen- Barnett & Maxwell, KPMG Huazhen, Starkschenkein, LLP, Friedman LLP, Li & Company, PC, Haskell & White LLP, Mantyla McReynolds, Padgett, Stratemann & Co. LLP, PricewaterhouseCoopers International, Radin, Glass & Co. LLP, SQUAR MILNER LLP, Squar, Milner, Peterson, Miranda & Williamson, LLP, Mayer Hoffman & McCann P.C., Gibbons & Kawash, Lake and Associates CPAs LLC, Malin, Bergquist & Company, LLP, Calvetti, Ferguson & Wagner, P.C., Ernst & Young Terco, David & Company, Weinberg & Company, Cherry Bekaert LLP, Spicer, Jeffries LLP, Eide Bailly, L.L.P., Farber Hass Hurley LLP, OUM & CO. LLP, Sadler, Gibb & Associates, LLC, Mancerra, S.C., EFP Rotenberg LLP, Freed Maxick CPAs PC, A J Shah & Co., Haklai & Co. CPA, Hall & Co., Semple, Marchal & Cooper, LLP, RT LLP, GHP Horwath, P.C., Peterson Sullivan LLP, Rotenberg Meril Solomon Bertiger & Guttilla, P.C., WWC, P.C, CF & Co. L.L.P., Davidson & Company, Nichols, Cauley & Associates, LLC, Habif- Arogetti & Wynne, SingerLewak LLP, PBMares, LLP, Hoberman & Lesser,CPAs, LLP, Saturna Group LLP, Tanner LC, Manning Elliot LLP, KLJ & Associates LLP, Hay & Watson, Mc Gladrey- Et Al, Smythe Ratcliffe LLP, TJS Deemer Dana LLP, Thigpen, Jones, Seaton & Co., KPMG Audit Nord, Castillo Miranda Y CIA, S.C., Collins Barrow Toronto LLP, BDO AG, Gregory & Associates, LLC., RBSM LLP, Berman & Company P.A., Mallah- Furman- Ross & Co., Moore Stephens Lovelace, P.L., PMB Helin Donovan, LLP, Vavrinek- Trine- Day & Co., Home Health Care Forest Hills, Sherb & Co., LLP, D' Arelli Pruzansky, P.A., Porter Keadle Moore, LLP (LLC), CliftonLarsonAllen LLP, Hogantaylor LLP, Ernst & Young Global, Whitley Penn, AWC (CPA) Limited, Jonathon P. Reuben CPA, KCCW Accounting Corp, WOLF & COMPANY, P.C., Briggs & Veselka Co, MAZARS LLP, Armanino LLP, Holtz Rubenstein Reminick LLP, Dale Matheson Carr-Hilton LaBonte LLP, DeCoria, Maichel & Teague P.S., L.L. Bradford & Company, LLC, Samyn & Martin, LLC, Anton & Chia, LLP, Yount, Hyde, & Barbour P.C., Hannis T. Bourgeois, LLP, Elliott Davis LLC, Elliott Davis Decosimo, LLC, Miller Wachman LLP, Ham, Langston & Brezina, L.L.P., Johnson Lambert LLP, PricewaterhouseCoopers Audit sro, BDO China Shu Lun Pan Certified Public Accountants LLP, Frost, PLLC, Rose, Snyder & Jacobs, Comiskey & Co., ZH CPA LLP, Lurie, LLP, Lurie, Besikof, Lapidus & Co., Mazars CPA Limited, Clement C. W. Chan & Co., Boulay PLLP, Tabriztchi & Co, Deloitte Accountants B.V., S.R. Snodgrass, HJ & Associates, LLC, Horne LLP, Deloitte Statsautoriseret Revisionspartnerselskab, Ram Associates, Ramirez Jimenez International CPA's, Reznick Group, P.C., Morison Cogen, LLP, Salberg & Company, P.A., Whittlesey & Hadley, Unaudited, Gumbiner Savett Inc, Bharat Parikh & Associates, Murray A. Finkelman Chartered Accountant, Berry- Dunn- Mc Neil & Parker, Centurion ZD CPA Ltd, Crowe Horwath (HK) CPA Limited, Baker Newman & Noyes, Shatswell- Mac Leod & Co., Paritz & Company P.A., Mc Nair- Mc Lemore- Middlebrooks & Co., Stayner, Bates & Jensen, P.C., Haynie & Co., Goldstein Schechter Koch P.A., Goldstein Schechter koch Price Lucas Horwitz & Co., P.A., KMJ Corbin & Company LLP, Meaden & Moore, Jones Simkins, P.C., PKF Certified Public Accountants, KPMG Peat Marwick LLP, Stegman & Company, Davis, Kinard & Co., P.C., Dominic K.F Chan & Co., Ernst & Young Audit & associados - SROC, SA, Rosenfield and Company, PLLC, J. H. Cohn LLP, Grant Thornton, Wipfli LLP, Crowe- Chizek & Co., Armanino Mckenna LLP, MartinelliMick PLLC, PricewaterhouseCoopers AB, Blackman- Kallick & Company- Ltd., Deloitte Certified Public Accounts S.A, Goldman Kurland and Mohidin LLP, Schneider Downs & Co., KPMG Audit Limited, Haefele, Flanagan & Co., M&K CPAS PLLC, HUTCHINSON & BLOODGOOD LLP, MJF & Associates, APC, Ligget, Vogt & Webb, Webb & Company, Deloitte AG, Hancock Askew & Co., LLP, Lott (T.E.) & Company, Bonadio & Co, Beard Miller Company, Brown- Edwards & Company, Smith Elliott & Kearns, Monroe- Shine & Co. Inc., Silberstein Ungar, PLLC, Piercy, Bowler, Taylor & Kern, HJ Associates & Consultants, LLP, Boulay- Heutmaker- Zibell & Co., Brightman Almagor & Co., Rowles & Co, Wei, Wei & Co., LLP, Fiondella, Milone & LaSaracina, De Meo, Young, McGrath, Carr, Riggs & Ingram, LLC, dbbmckennon, WEINBERG & COMPANY PA, WELD ASIA  ASSOCIATES, Deloitte & Touche GmbH Wirtschaftspruef., Castaing & Hussey, Laporte, Sehrt, Romig, & Hand, TGM Group LLC, Trice Geary & Myers LLC, Mazars USA LLP, Scott and Company LLP, De Joya  Griffith & Company LLC, Abelovich, Polano & Asociados, Rosenberg, Rich, Baker, Berman & Company, Mercadien, P.C., CPAs, HHC LLP, Grant Thornton Audit Pty Ltd, Daszkal Bolton LLP, Warren Averett LLC, Warren, Averett, Kimbrough & Marino, LLC, Skoda Minotti, Tiao Ho United CPA Firm, Horwath & Company, (TW), Hoberman, Goldstein and Lesser, CPAs P.C., Mountjoy Chilton Medley, LLP, Postlethwaite & Netterville, KBL, LLP CPA, BDO Audit (WA) PTY LTD, Macias Gini and O Connell, LLP, Fahn, Kanne & Co., Cloud (J.D.) & Co., Hartley Moore Accountancy Corporation, PwC Wirtschaftsprufung Gmbh, BF Borgers CPA PC, D'Arcangelo & Co., Albert Wong & Co., Weinberg & Baer LLC, Cross, Fernandez & Riley LLP, LBB Associates, LTD. LLP, Somekh Chaikin, Anderson Bradshaw PLLC, HKCMCPA Company Limited, BDO Bedrijfsrevisoren Burg C.V., Stevenson & Company CPAS LLC, DKM Certified Public Accountants, Drake & Klein CPAs, Grassi & Co., CPAs, P.C., Ferlita, Walsh & Gonzalez, P.A., Cacciamatta Accountancy Corporation, Citrin, Cooperman & Company, Seligson & Giannattasio, LLP, Becton, Dickinson & Company, Richter LLP, RSM Richter LLP, Tarvaran Askelson & Company, LLP, Deloitte & Touche LLC, ThayerOneal CPAs, Smith Elliott Kearns & Company LLC, Akin, Doherty, Klein & Feuge, P.C., BDO China Dahua CPA Co., Ltd, Perry-Smith LLP, Ronald R. Chadwick, P.C., UHY Vocation HK CPA Limited, Scharf- Pera & Co., Seale and Beers, CPAs, Delap LLP, Deloitte Touche Tohmatsu Limited, Pritchett, Siler & Hardy, Mauldin & Jenkins, Hazlet Lewis & Bieter, Signed Richter LLP, Frazer Frost LLP, Adams & Co., Rehmann Robson, P.C., Arnett Carbis Toothman LLP, Arnett & Foster, PLLC, Montgomery Coscia Greilich LLP, Raich Ende Malter Lerner & Co., Maggart & Associates, P.C., McConnell & Jones, LLP, JLK Rosenberger LLP, D & H Group LLP, D+H Group LLP, Lumsden & McCormick, LLP, Peter Messineo, CPA, Moody, Famiglietti & Andronico, LLP, Clark Shaefer Hackett, Jones Simkins LLC, Burton, McCumber & Cortez, LLP, Moss Adams LLP Seattle, Washington, Horowitz and Ullman, P.C., Smythe LLP, PKF Littlejohn LLP, Subaru Audit Corp., Suttle & Stalnaker, PLLC, Schechter Dokken Kanter Andrews & Selcer, Ltd., Hegde & Associates

        [DataMember]
        public string AuditorOpinion { get; set; } // Unqualified
        */

        [DataMember]
        public Dictionary<string, double> Lines { get; set; }

        #region Equality

        public bool Equals(FinancialStatement other) => Type == other.Type && Date == other.Date && Period == other.Period && Source == other.Source;
        public static bool operator ==(FinancialStatement s1, FinancialStatement s2) => s1.Equals(s2);
        public static bool operator !=(FinancialStatement s1, FinancialStatement s2) => !s1.Equals(s2);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(FinancialStatement))
                return Equals((FinancialStatement)obj);
            else
                return false;
        }

        public override int GetHashCode() => Type.GetHashCode() ^ Date.GetHashCode() ^ Period.GetHashCode() ^ Params.GetHashCode();

        #endregion Equality
    }



    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract]
    public enum FinancialEventType : int
    {
        [EnumMember]
        Annual = 10,

        [EnumMember]
        Quarter = 20,

        [EnumMember]
        Defunct = 6666,
    }







    /// <summary>
    /// "INC" Income, "CAS" Cash, "BAL" Balance - INC, BAL, CAS, RAL
    /// https://www.accountingtools.com/articles/types-of-financial-statements.html
    /// </summary>
    [Serializable, DataContract]
    public enum FinancialStatementType : int
    {
        [EnumMember]
        Income = 10,

        [EnumMember]
        Balance = 20,

        [EnumMember]
        CashFlows = 30,

        [EnumMember]
        ChangesInEquity = 40
    }





    [Serializable, DataContract]
    public enum OwnerType : int
    {
        Other = -1,
        Insider = 1,
        Institute = 2,
        PrivateFund = 3,
    }

    [Serializable, DataContract]
    public struct OwnerInfo
    {
        [DataMember]
        public string Id { set; get; }

        [DataMember]
        public OwnerType Type { set; get; }

        [DataMember]
        public string Name { set; get; }
    }
}
