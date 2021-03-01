/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public enum FundamentalRequestType : int
    {
        [Description("Company Overview")]
        [ApiCode("ReportSnapshot")]
        CompanyOverview,

        [Description("Financial Summary")]
        [ApiCode("ReportsFinSummary")]
        FinancialSummary,

        [Description("Financial Statements")]
        [ApiCode("ReportsFinStatements")]
        FinancialStatements,

        [Description("Analyst Estimates")]
        [ApiCode("RESC")]
        AnalystEstimates,

        [Description("Financial Calendar")]
        [ApiCode("CalendarReport")]
        Calendar,

        [Description("Ownership")]
        [ApiCode("ReportsOwnership")]
        Ownership
    }
}
