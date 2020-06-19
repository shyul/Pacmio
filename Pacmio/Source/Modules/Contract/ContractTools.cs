/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xu;
using System.Diagnostics.Contracts;

namespace Pacmio
{
    public static class ContractTools
    {
        public static IEnumerable<Stock> FindDuplicateStock(string countryCode)
        {
            IEnumerable<Stock> cList = ContractList.Values.AsParallel().Where(n => n is Stock s && s.Country == countryCode).Select(n => (Stock)n); // && s.Exchange != Exchange.OTCMKT && s.Exchange != Exchange.OTCBB);

            Dictionary<string, int> duplicate = new Dictionary<string, int>();

            foreach (Stock c in cList)
            {
                string name = c.Name;

                if (!duplicate.ContainsKey(name))
                    duplicate.Add(name, 1);
                else
                    duplicate[name]++;
            }

            return duplicate.Where(n => n.Value > 1).SelectMany(n => cList.Where(c => c.Name == n.Key));
        }

        public static HashSet<string> GetSymbolList(ref string text)
        {
            string[] symbolFields = text.CsvReadFields();
            HashSet<string> symbolItemList = new HashSet<string>();
            HashSet<string> symbolList = new HashSet<string>();
            foreach (string field in symbolFields)
            {
                string symbol = field.TrimCsvValueField();

                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    symbolItemList.CheckAdd("\"" + symbol + "\"");
                    symbolList.CheckAdd(symbol);
                }

            }
            text = symbolItemList.ToString(", ");
            return symbolList;
        }
    }
}
