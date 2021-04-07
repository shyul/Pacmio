﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public static class SimulationManager
    {
        public static void Run(BarTableSet bts, IndicatorSet inds, OrderRule odr)
        {


            foreach (var item in inds.Where(n => n.freq >= BarFreq.Daily).OrderByDescending(n => n.freq))
            {
                BarTable bt = bts[item.freq, item.type];
                bt.CalculateRefresh(item.bas);
            }



        }

        #region Search for tradeable contracts



        public static IEnumerable<Contract> Search(Indicator ind, Period pd)
        {
            return Search(ContractManager.Values.Where(n => n is Stock s && s.Country == "US" && s.Status == ContractStatus.Alive).OrderBy(n => n.Name).Select(n => n as Stock).ToList(), ind, pd);
        }

        public static IEnumerable<Contract> Search(IEnumerable<Contract> clist, Indicator ind, Period pd) 
        {
            return new List<Contract>();
        }

        #endregion Search for tradeable contracts
    }

    public class OrderRule
    {


    }


    /// public void Simulate(BarTableSet bts, IndicatorSet inds, OrderRule)
    /// {
    ///      1) Run Indicators (includeded in the Strategy) ->
    ///      1.5) Run higher timeframe indicator first
    ///      1.6) Yield MultiPeriod for intested intraday bars
    ///      2) Run Intraday Indicators
    ///      3) Yield all scores ->
    ///      4) Run OrderRules identify all of the scores
    ///      5) Yield OrderDatum Only
    ///
    /// }
    /// 


    public static class DecisionDataManager
    {
        public static Dictionary<Contract, BarTableSet> BarTableSetLUT { get; } = new();




        /*
#region Watch List

/// <summary>
/// Acquire from WatchListManager
/// </summary>
public WatchList WatchList
{
    get => m_WatchList;

    set
    {
        if (m_WatchList is WatchList w) w.RemoveDataConsumer(this);
        m_WatchList = value;
        m_WatchList.AddDataConsumer(this);
    }
}

private WatchList m_WatchList = null;

public List<Contract> ContractList { get; private set; }

public void DataIsUpdated(IDataProvider provider)
{
    if (provider is WatchList w)
    {
        if (ContractList is List<Contract>)
        {
            var list_to_remove = ContractList.Where(n => !w.Contracts.Contains(n));
            list_to_remove.RunEach(n => n.MarketData.RemoveDataConsumer(this));
        }

        ContractList = w.Contracts.ToList();
        ContractList.ForEach(n => n.MarketData.AddDataConsumer(this));
    }
}

#endregion Watch List
*/

    }
}