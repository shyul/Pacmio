/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// Application Entry
/// ------------------------------------------------------------
/// 1. Register single thread application instance mutex
/// 2. Check Windows version (Windows 10)
/// 3. [Optional] Set Application DPI Aware
/// 4. Splash Screen for data loading 
/// 5. Auto save data when application terminates
/// 6. Release application instance mutex
/// 
/// ***************************************************************************

using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using Xu;
using Xu.WindowsNativeMethods;
using Pacmio;

namespace Pacmio.Utility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Loads the settings and environmental parameters of the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) // Remove args
        {
            if (InstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    BarTableForm btf = new BarTableForm();
                    btf.Show();

                    Root.Load();
                    Application.Run(new DataUtility());
                    Root.Save();
                }
                else
                {
                    MessageBox.Show("Windows 10 64-bit is required to run this application :)");
                }
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows if the pacmio window is registered.
                User32.PostMessage(HWND.BROADCAST, SHOW_PACMIO, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal static readonly string GUID = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true))[0] as GuidAttribute).Value;
        internal static readonly Mutex InstanceMutex = new Mutex(true, GUID);
        internal static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");
    }
}
