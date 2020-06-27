/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    using IndustrySectorTable = Dictionary<(string Type, string Code), string>;

    public static class BusinessInfoList
    {
        /// <summary>
        /// Master List of Business Informations.
        /// </summary>
        private static readonly Dictionary<string, BusinessInfo> List = new Dictionary<string, BusinessInfo>();

        private static string BusinessFileName(string isin)
        {
            string path = Root.ResourcePath + "BusinessData\\" + isin.Substring(0, 2) + "\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path + isin;
        }

        public static BusinessInfo GetOrAdd(string isin)
        {
            isin = isin.Trim();

            if (isin.Length < 11)
                return null;
            else if (!List.ContainsKey(isin))
            {
                string fileName = BusinessFileName(isin);
                BusinessInfo bi = File.Exists(fileName) ? Serialization.DeserializeJsonFile<BusinessInfo>(fileName) : new BusinessInfo(isin);
                List.Add(isin, bi);
                return bi;
            }
            else
                return List[isin];
        }

        /// <summary>
        /// 
        /// </summary>
        public static IndustrySectorTable IndustrySectors = new IndustrySectorTable();

        public static void AddIndustrySector(string Type, string Code, string Text)
        {
            IndustrySectors[(Type, Code)] = Text;
        }

        #region File system

        // S&P 500, Dow Industry, S&P 600 Small Cap, S&P 400 Mid Cap, Dow Transportation, Dow Utility
        //public static MarketIndexTable Indices = new MarketIndexTable();

        private static string IndustrySectorsFile => Root.ResourcePath + @"IndustrySectors.csv";



        public static void Load()
        {
            lock (IndustrySectors)
                if (File.Exists(IndustrySectorsFile))
                {
                    using var fs = new FileStream(IndustrySectorsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using StreamReader sr = new StreamReader(fs);
                    string[] headers = sr.ReadLine().Split(',');

                    if (headers.Length == 3)
                        while (!sr.EndOfStream)
                        {
                            string[] fields = sr.CsvReadFields();

                            if (fields.Length == 3)
                            {
                                IndustrySectors.CheckAdd((fields[0].TrimCsvValueField(), fields[1].TrimCsvValueField()), fields[2].TrimCsvValueField());
                            }
                            else
                                throw new Exception("Error loading Industry Sectors File!");
                        }
                }
        }

        public static void Save()
        {
            int pt = 0;

            // Save Industry Sectors
            StringBuilder sb = new StringBuilder("Type,Code,Title\n");

            lock (IndustrySectors)
            {
                var sorted = IndustrySectors
                    .OrderBy(n => n.Key.Type)
                    .ThenBy(n => n.Key.Code);

                for (int i = 0; i < sorted.Count(); i++)
                {
                    var item = sorted.ElementAt(0);
                    sb.AppendLine(item.Key.Type.CsvEncode() + "," + item.Key.Code.CsvEncode() + "," + item.Value.CsvEncode());
                }
            }

            sb.ToFile(IndustrySectorsFile);

            // Save Business Info
            lock (List)
            {
                Parallel.ForEach(List.Values, bi =>
                {
                    if (bi.IsModified) bi.SerializeJsonFile(BusinessFileName(bi.ISIN));
                    pt++;
                });
            }
        }

        #endregion File system
    }
}
