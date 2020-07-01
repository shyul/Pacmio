/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Program Assembly's Root Settings and Environment Parameters
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.WindowsNativeMethods;

namespace Pacmio
{
    public static class Root
    {
        public static PacmioForm Form { get; private set; }

        public static PacSettings Settings { get; private set; }

        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        public static string SettingFile => AppPath + Assembly.GetExecutingAssembly().GetName().Name + ".Json";

        public static string ResourcePath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Pacmio\\";

        public static string CachePath => (Settings != null) ? Settings.CachePath : Path.GetTempPath() + "Pacmio\\";

        #region IB Client

        public static void IBConnectUpdate(ConnectionStatus status, DateTime time, string message = "")
        {
            OnIBConnectedHandler?.Invoke(status, time, message);
        }

        public static event ConnectionStatusEventHandler OnIBConnectedHandler;

        public static bool IBConnected => IB.Client.Connected;

        public static void IBClientStart()
        {
            if (Settings is null) return;

            if (!IB.Client.Connected)
            {
                int timeout = IB.Client.Timeout + 2000;
                int j = 0;

                OnIBConnectedHandler += (ConnectionStatus status, DateTime time, string msg) => { Console.WriteLine("IBClient [ " + time.ToString("HH:mm:ss") + " - " + status.ToString() + " ]: " + msg); };
                IB.Client.Connect(Settings.IBClientId, Settings.IBServerPort, Settings.IBServerAddress, Settings.IBTimeout);

                while (!IB.Client.Connected)
                {
                    Thread.Sleep(1);
                    j++;
                    if (j > timeout) break;
                }
            }
        }

        public static void IBClientStop()
        {
            if (IBConnected)
            {
                int timeout = IB.Client.Timeout + 2000;
                int j = 0;

                IB.Client.Disconnect.Start();

                while (IBConnected)
                {
                    Thread.Sleep(1);
                    j++;
                    if (j > timeout) break;
                }
            }
        }

        #endregion IB Client

        #region Background Tasks

        private static CancellationTokenSource BackgroundTaskCancellationTokenSource { get; set; }

        public static bool RequestWorkerCancel => BackgroundTaskCancellationTokenSource == null || BackgroundTaskCancellationTokenSource.IsCancellationRequested;

        public static void StartTask()
        {
            BackgroundTaskCancellationTokenSource = new CancellationTokenSource();
            TableList.StartTask(BackgroundTaskCancellationTokenSource);
        }

        public static void StopTask()
        {
            if (!(BackgroundTaskCancellationTokenSource is null)) BackgroundTaskCancellationTokenSource.Cancel();
            TableList.StopTask();
        }

        #endregion Background Tasks

        #region File Storage Utilities

        public static OpenFileDialog OpenFile { get; set; } = new OpenFileDialog() { Filter = "Comma-separated values file (*.csv) | *.csv", FilterIndex = 1, RestoreDirectory = true };
        public static SaveFileDialog SaveFile { get; set; } = new SaveFileDialog() { Filter = "Comma-separated values file (*.csv) | *.csv", FilterIndex = 1, RestoreDirectory = true };

        public static void Load()
        {
            // Load Application General Settings 
            if (File.Exists(SettingFile))
                Settings = Serialization.DeserializeJsonFile<PacSettings>(SettingFile);
            else
                Settings = new PacSettings();

            BarChartSet.UpperColor = Color.Green;
            BarChartSet.LowerColor = Color.Red;

            // Build essential directories
            if (!Directory.Exists(ResourcePath)) Directory.CreateDirectory(ResourcePath);
            if (!Directory.Exists(CachePath)) Directory.CreateDirectory(CachePath);

            // Load Industry Sectors Code Dictionary
            BusinessInfoList.Load();
            ContractList.Load();
            UnknownItemList.Load();
            AccountManager.Load();
            OrderManager.Load();

            StartTask();

            Form = new PacmioForm() { IsRibbonShrink = true, Text = TitleText, Width = 1500, Height = 1200 };
        }

        public static void Save()
        {
            StopTask();

            // Save settings
            Settings.SerializeJsonFile(SettingFile);

            TableList.Save();
            BusinessInfoList.Save();
            ContractList.Save();
            OrderManager.Save();
            AccountManager.Save();
            UnknownItemList.Save();
        }

        #endregion File Storage Utilities

        public static readonly string GUID = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true))[0] as GuidAttribute).Value;
        public static readonly Mutex InstanceMutex = new Mutex(true, GUID);
        public static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");

        public static string TitleText => Application.ProductName + " - Rev " + Application.ProductVersion;
    }
}
