/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using Xu;
using Pacmio;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TestConsole
{
    [Serializable, DataContract]
    public abstract class Test
    {
        [DataMember]
        public virtual string Name { get; set; } = string.Empty;
    }

    [Serializable, DataContract]
    public class Test2 : Test
    {
        [DataMember]
        public virtual string Age { get; set; } = string.Empty;
    }


    [Serializable, DataContract]
    public class Test3 : Test
    {
        [DataMember]
        public virtual string Address { get; set; } = string.Empty;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MultiPeriod<DataSource> m = new MultiPeriod<DataSource>();

            m.Add(new DateTime(2000, 2, 10), new DateTime(2000, 2, 15), DataSource.IB);
            m.Add(new DateTime(2000, 2, 15), new DateTime(2000, 3, 20), DataSource.Google);
            m.Add(new DateTime(2000, 3, 16), new DateTime(2000, 3, 21), DataSource.IB);
            m.Add(new DateTime(1999, 7, 10), new DateTime(1998, 3, 21), DataSource.IB);

            MultiPeriod<DataSource> m2 = new MultiPeriod<DataSource>();

            m2.Add(new DateTime(2000, 2, 11), new DateTime(2002, 2, 15), DataSource.Quandl);
            m2.Add(new DateTime(2001, 2, 15), new DateTime(2002, 2, 20), DataSource.Google);
            m2.Add(new DateTime(2001, 2, 16), new DateTime(2002, 2, 21), DataSource.IB);

            //m.Merge(m2);

            Console.WriteLine("#### Test  MultiPeriod ");
            foreach (var item in m.Reverse()) 
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
            Console.WriteLine("#### Extra Data");
            //Console.WriteLine(m[new DateTime(2000, 2, 15)]);
            Console.WriteLine(m.LastStreak);
            Console.WriteLine("#### Test  MultiPeriod End");


            string dbNum = "-1.0E-4";

            double test = dbNum.ToDouble(); //; double.Parse(dbNum);

            Console.WriteLine("Number test: " + test.ToString());

            HashSet<Test> Tests = new HashSet<Test>();

            Tests.Add(new Test2());
            Tests.Add(new Test3());

            Tests.SerializeBinaryFile("D:\\Test.bin");


            Console.WriteLine("Start...");
            double s = 1233423121.12312;
            Console.WriteLine(s.ToSINumberString().String);

            // https://msdn.microsoft.com/en-us/library/system.gc.gettotalmemory%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
            Console.WriteLine("Total Memory: {0}", GC.GetTotalMemory(false));

            ConcurrentDictionary<int, double[]> TechData = new ConcurrentDictionary<int, double[]>
            {
                [1] = new double[] { 123424, 212354 }
            };
            Console.WriteLine(TechData[1][0].ToString());

            /*
            string testfilename = @"C:\Users\uxli\Pacmio Workplace\Historical Data\US\$AAPL";
            BarTable bt = Util.DeserializeBinaryFile<BarTable>(testfilename);
            bt.ExportCSV(@"E:\AAPL.csv");
            */

            Period pd = new Period(new DateTime(1992, 12, 2), new DateTime(1999, 3, 10));

            DateTime t = new DateTime(2018, 6, 3, 18, 13, 56);
            Frequency f1 = new Frequency(TimeUnit.Hours, 2);




            //if (pd < t) Console.WriteLine("yeah"); else Console.WriteLine("nah");
            //DateTime t2 = f1.Align(t, -2);
            //Console.WriteLine("t2 = " + t2.ToString("MM-dd-yyyy HH:mm:ss"));

            Period p2 = f1.AlignPeriod(t, 0);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i + ": p2 = " + p2.ToString());
                p2 -= f1;
            }


            Console.WriteLine("Compare Period: " + pd.CompareTo(t));

            //ImportSymbol("D:\\StockData\\EOD_20180605.csv", "E:\\list.csv");


            string str = "asd   2314H12.11e123  Helloe";

            double ss1 = str.ToDouble(-1);


            string timeStr = "2015-05-16";

            (bool valid0, DateTime t0) = timeStr.ToDateTime("yyyy-M-d");

            Console.WriteLine("t0 = " + t0 + " Kind = " + t0.Kind);

            Console.WriteLine("ss1 = " + ss1);

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }


        public static void ImportSymbol(string fileName, string outFileName)
        {
            FileInfo info = new FileInfo(fileName);
            int i = 0, j = 0;
            long totalLineCount = info.Length / 123;
            string LastSymbolName = string.Empty;

            Dictionary<string, string> RawSymbolList = new Dictionary<string, string>();

            File.Delete(outFileName);

            StreamWriter outFile = new StreamWriter(outFileName);

            using (StreamReader strQuandlEOD = new StreamReader(fileName))
            {
                while (!strQuandlEOD.EndOfStream)
                {
                    string line = strQuandlEOD.ReadLine();
                    string[] values = line.Trim().Split(',');
                    string CurrentSymbolName = values[0];
                    if (values.Length == 14 && Regex.IsMatch(CurrentSymbolName, @"^[a-zA-Z0-9]+$"))
                    {
                        i++;

                        if (!RawSymbolList.ContainsKey(CurrentSymbolName))
                        {
                            RawSymbolList.Add(CurrentSymbolName, string.Empty);
                            outFile.WriteLine(CurrentSymbolName);
                        }


                        if (CurrentSymbolName != LastSymbolName)
                        {
                            j++;
                            Console.WriteLine("Progress: {0}% / Symbol({1}): {2}", (i * 100.0 / totalLineCount).ToString("0.00"), j.ToString(), LastSymbolName);
                        }
                        LastSymbolName = CurrentSymbolName;
                    }

                }
            }
            /*
            for (int k = 0; k < RawSymbolList.Count; k++)
            {
                sb.AppendLine(RawSymbolList[k]);

            }
            */
            outFile.Close();
        }
    }
}
