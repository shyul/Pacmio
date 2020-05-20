using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using Xu;
using Xu.WindowsNativeMethods;
using Pacmio;

namespace Pacmio.TradeCAD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Loads the settings and environmental parameters of the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Root.InstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    // NativeMethods.SetProcessDPIAware();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Root.Load();

                    // It is going to be the splash widget
                    // ChartForm cf = new ChartForm("Test Bar Table");
                    //Application.Run(new PacMain() { IsRibbonShrink = true, Text = TitleText, Width = 1500, Height = 1200 });

                    Application.Run(new RemoteControlForm());

                    Root.Save();
                }
                else
                {
                    MessageBox.Show("Windows 10 64-bit is required to run this application :)");
                }

                Root.InstanceMutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows if the pacmio window is registered.
                User32.PostMessage(HWND.BROADCAST, Root.SHOW_PACMIO, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
