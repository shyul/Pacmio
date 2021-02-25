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
        public StaticWatchList(string name)
        {
            Name = name;
            Contracts = new List<Contract>();
        }

        public StaticWatchList(string name, List<Contract> list)
        {
            Name = name;
            Contracts = list;
        }

        public StaticWatchList(string name, IEnumerable<string> list, string countryCode = "US", CancellationTokenSource cts = null, IProgress<float> progress = null) :
            this(name, ContractManager.GetOrFetch(list, countryCode, cts, progress).ToList())
        {
            Console.WriteLine("Count() = " + Contracts.Count());

            HashSet<string> existingSymbols = new HashSet<string>();
            var existingSymbolsArray = Contracts.Select(n => n.Name);

            foreach (string symbol in existingSymbolsArray)
            {
                existingSymbols.CheckAdd(symbol);
            }

            var non_existing_symbols = list.Where(n => !existingSymbols.Contains(n));

            foreach (string s in non_existing_symbols)
            {
                Console.WriteLine("Can't find: " + s);
            }
        }

        public StaticWatchList(string name, ref string csvList) : this(name, GetSymbolListFromCsv(ref csvList)) { }

        public void Add(Contract c)
        {
            if (Contracts is null) Contracts = new List<Contract>();
            lock (Contracts)
            {
                if (Contracts.CheckAdd(c))
                    DataIsUpdated();
            }
        }

        public void Remove(Contract c)
        {
            if (Contracts is null) return;
            lock (Contracts)
            {
                if (Contracts.CheckRemove(c))
                    DataIsUpdated();
            }
        }

        public override string ConfigurationString => CollectionTool.ToString(Contracts, ',');

        public override bool Equals(WatchList other) => other is StaticWatchList wt && Name == wt.Name;

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
