/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public static class BusinessInfoManager
    {
        /// <summary>
        /// Master List of Business Informations.
        /// </summary>
        private static ConcurrentDictionary<string, BusinessInfo> IsinToBusinessLUT { get; } = new ConcurrentDictionary<string, BusinessInfo>();

        public static BusinessInfo GetOrCreateBusinessInfo(string isin)
        {
            isin = isin.Trim();

            if (isin.Length < 11)
                return null;
            else
                lock (IsinToBusinessLUT)
                {
                    if (!IsinToBusinessLUT.ContainsKey(isin))
                    {
                        IsinToBusinessLUT[isin] = BusinessInfo.LoadFile(isin);
                    }

                    return IsinToBusinessLUT[isin];
                }
        }

        private static string IndustrySectorsFile => Root.ResourcePath + @"IndustrySectors.csv";

        public static Dictionary<(string Type, string Code), string> IndustrySectors { get; } = new Dictionary<(string Type, string Code), string>();

        public static void AddIndustrySector(string Type, string Code, string Text) => IndustrySectors[(Type, Code)] = Text;

        #region File system

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
                                IndustrySectors.CheckAdd((fields[0].TrimCsvValueField(), fields[1].TrimCsvValueField()), fields[2].TrimCsvValueField()); //.ReplaceEnd("T", ""));
                            }
                            else
                                throw new Exception("Error loading Industry Sectors File!");
                        }
                }
        }

        public static void Save()
        {
            // Save Industry Sectors
            StringBuilder sb = new StringBuilder("Type,Code,Title\n");

            lock (IndustrySectors)
            {
                IndustrySectors.AsParallel()
                .OrderBy(n => n.Key.Type)
                .ThenBy(n => n.Key.Code)
                .ToList()
                .ForEach(n => sb.AppendLine(n.Key.Type.CsvEncode() + "," + n.Key.Code.CsvEncode() + "," + n.Value.CsvEncode()));
            }

            sb.ToFile(IndustrySectorsFile);

            // Save Business Info
            lock (IsinToBusinessLUT)
            {
                Parallel.ForEach(IsinToBusinessLUT.Values.Where(n => n.IsModified), bi => bi.SaveFile());
            }
        }

        #endregion File system
    }
}
