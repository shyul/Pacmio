using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Xu;
using Xu.Chart;
using Pacmio;
using Pacmio.IB;
using System.Threading;
using IbXmlScannerParameter;

namespace TestClient
{
    public static class DownloadBarTable
    {
        public static CancellationTokenSource DownloadCancellationTokenSource { get; set; }

        public static IProgress<int> DownloadProgress { get; set; }

        public static IProgress<float> DetailedProgress { get; set; }

        public static Period Period { get; set; } = new Period();

        public static List<string> SymbolList { get; private set; } = new List<string>();

        public static List<BarFreq> BarFreqs { get; private set; } = new List<BarFreq>();

        public static void Worker()
        {
            //var list = ContractList.Values.Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == "US" && SymbolList.Contains(n.Name));
            //var list = ContractList.GetList(SymbolList.Where(n => n.Length > 0), "US");
            var list = ContractList.GetOrFetch(SymbolList, "US", DownloadCancellationTokenSource, DetailedProgress);

            Console.WriteLine("list.Count() = " + list.Count());
            int total_downloads = BarFreqs.Count * list.Count();
            int pt = 0;
            foreach (BarFreq freq in BarFreqs) 
            {
                foreach (Contract c in list)
                {
                    BarTable bt = new BarTable(c, freq, BarType.Trades);
                    bt.Reset(Period, DownloadCancellationTokenSource, DetailedProgress);

                    while (!bt.ReadyForTickCalculation)
                    {
                        Thread.Sleep(100);
                        if (DownloadCancellationTokenSource.IsCancellationRequested) break;
                    }

                    bt.LoadJsonFileToFileData();
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.SaveFileDataToJsonFile();

                    pt++;
                    DownloadProgress?.Report((100.0f * pt / total_downloads).ToInt32());

                    if (DownloadCancellationTokenSource.IsCancellationRequested) break;
                }
                BusinessInfoList.Save();
                if (DownloadCancellationTokenSource.IsCancellationRequested) break;
            }
        }
    }
}
