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

namespace Pacmio
{
    public enum FinancialDataRequestType : int
    {
        [Description("Company Overview")]
        [IB.ApiCode("ReportSnapshot")]
        CompanyOverview,

        [Description("Financial Summary")]
        [IB.ApiCode("ReportsFinSummary")]
        FinancialSummary,

        [Description("Financial Statements")]
        [IB.ApiCode("ReportsFinStatements")]
        FinancialStatements,

        [Description("Analyst Estimates")]
        [IB.ApiCode("RESC")]
        AnalystEstimates,

        [Description("Financial Calendar")]
        [IB.ApiCode("CalendarReport")]
        Calendar,

        [Description("Ownership")]
        [IB.ApiCode("ReportsOwnership")]
        Ownership
    }
}
