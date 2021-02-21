/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;

namespace Pacmio
{
    public class StaticWatchList : WatchList
    {
        public StaticWatchList(IEnumerable<Contract> list) => Contracts = list;

        public StaticWatchList(IEnumerable<string> list, string countryCode = "US", CancellationTokenSource cts = null, IProgress<float> progress = null) :
            this(ContractManager.GetOrFetch(list, countryCode, cts, progress))
        { }

        public StaticWatchList(ref string csvList) : this(GetSymbolListFromCsv(ref csvList)) { }

        public override string ConfigurationString => CollectionTool.ToString(Contracts, ',');

        public static HashSet<string> GetSymbolListFromCsv(ref string csvList)
        {
            string[] symbolFields = csvList.Replace('/', ',').CsvReadFields();
            HashSet<string> symbolItemList = new HashSet<string>();
            HashSet<string> symbolList = new HashSet<string>();
            foreach (string field in symbolFields)
            {
                string symbol = field.TrimCsvValueField().ToUpper();

                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    symbolItemList.CheckAdd("\"" + symbol + "\"");
                    symbolList.CheckAdd(symbol);
                }

            }
            csvList = symbolItemList.ToString(", ");
            return symbolList;
        }
    }
}
