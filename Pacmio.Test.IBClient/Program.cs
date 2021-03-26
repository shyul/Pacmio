using Pacmio;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Xu.WindowsNativeMethods;
using System.Collections.Generic;
using Xu;


namespace TestClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (InstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    //List<int> source = new() { 1, 2, 3, 3 };
                    //var result = source.GetPair();
                    //Console.WriteLine(result.ToString(", "));

                    List<int> source = new() { 137801, 2, 3, 3, 1, 6, 10000, 100, 203, -1 };

                    var Range = new Range<int>(source);
                    Console.WriteLine(Range.ToString());



                    Root.Load();
                    Console.WriteLine("Quandl is connected? " + Quandl.Connected);

                    Application.Run(new MainForm());

                    Root.Save();
                }
                else
                {
                    MessageBox.Show("Windows 10 64-bit is required to run this application :)");
                }

                InstanceMutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows if the pacmio window is registered.
                User32.PostMessage(HWND.BROADCAST, SHOW_PACMIO, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal static readonly string GUID = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true))[0] as GuidAttribute).Value;
        internal static readonly Mutex InstanceMutex = new(true, GUID);
        internal static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");

        public static string TitleText => Application.ProductName + " - Rev " + Application.ProductVersion;
    }
}
