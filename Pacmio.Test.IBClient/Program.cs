﻿using System;
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
                    AccountManager.UpdatedHandler += (int status, DateTime time, string msg) =>
                    {
                        Console.WriteLine("TestClient [ " + time.ToString("HH:mm:ss") + " - " + status.ToString() + " ]: " + msg);
                    };

                    /*
                    Account c = new Account("DU");
                    // foreach(var p in typeof(Account).GetProperties()) 
                    foreach (PropertyInfo p in c.GetType().GetProperties())
                    {
                        string s = p.Name + "\t | \t" + p.PropertyType.Name;
                        (bool IsValid, DescriptionAttribute Result) = p.GetAttribute<DescriptionAttribute>();
                        if (IsValid)
                        {
                            s += "\t | \t" + Result.Description;
                        }
                        Console.WriteLine(s);
                    }
                    */

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
             
                    Root.Load();

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
        internal static readonly Mutex InstanceMutex = new Mutex(true, GUID);
        internal static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");

        public static string TitleText => Application.ProductName + " - Rev " + Application.ProductVersion;
    }
}
