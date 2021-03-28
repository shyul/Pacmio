/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pacmio.Analysis;
using Xu;

namespace Pacmio
{
    public static class DecisionDataManager
    {
        public static Dictionary<Contract, IntraDayBarTableSet> BarTableSetLUT { get; } = new();




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
