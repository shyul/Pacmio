/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Program Assembly's Root Settings and Environment Parameters
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
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

        public static int DegreeOfParallelism = 24;

        #region Net Client

        public static void NetConnectUpdate(ConnectionStatus status, DateTime time, string message = "")
        {
            OnNetConnectedHandler?.Invoke(status, time, message);
        }

        public static event ConnectionStatusEventHandler OnNetConnectedHandler;

        public static bool NetConnected => IB.Client.Connected && TIProData.Client.Connected;

        public static void NetClientStart()
        {
            if (Settings is null) return;

            int timeout = 100;

            if (!TIProData.Client.Connected)
            {
                TIProData.Client.Connect();

                while (!TIProData.Client.Connected)
                {
                    Thread.Sleep(10);
                    timeout--;

                    if (timeout < 0)
                    {
                        TIProData.Client.Disconnect(); break;
                    }
                }
                //Thread.Sleep(500);
            }

            if (!IB.Client.Connected)
            {
                timeout = IB.Client.Timeout + 2000;
                int j = 0;

                OnNetConnectedHandler += (ConnectionStatus status, DateTime time, string msg) => { Console.WriteLine("IBClient [ " + time.ToString("HH:mm:ss") + " - " + status.ToString() + " ]: " + msg); };
                IB.Client.Connect(Settings.IBClientId, Settings.IBServerPort, Settings.IBServerAddress, Settings.IBTimeout);

                while (!IB.Client.Connected)
                {
                    Thread.Sleep(1);
                    j++;
                    if (j > timeout) break;
                }
            }
        }

        public static void NetClientStop()
        {
            if (IB.Client.Connected)
            {
                int timeout = IB.Client.Timeout + 2000;
                int j = 0;

                IB.Client.Disconnect.Start();

                while (IB.Client.Connected)
                {
                    Thread.Sleep(1);
                    j++;
                    if (j > timeout) break;
                }
            }

            if (TIProData.Client.Connected)
            {
                TIProData.Client.Disconnect();
            }
        }

        #endregion Net Client

        #region Background Tasks

        private static CancellationTokenSource BackgroundTaskCancellationTokenSource { get; set; }

        public static bool RequestWorkerCancel => BackgroundTaskCancellationTokenSource == null || BackgroundTaskCancellationTokenSource.IsCancellationRequested;

        public static void StartTask()
        {
            BackgroundTaskCancellationTokenSource = new CancellationTokenSource();
        }

        public static void StopTask()
        {
            if (!(BackgroundTaskCancellationTokenSource is null)) BackgroundTaskCancellationTokenSource.Cancel();
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

            // TODO: Set these from the Setting File
            UpperColor = Color.Green;
            LowerColor = Color.Red;

            // Build essential directories
            if (!Directory.Exists(ResourcePath)) Directory.CreateDirectory(ResourcePath);
            if (!Directory.Exists(CachePath)) Directory.CreateDirectory(CachePath);

            // Load Industry Sectors Code Dictionary
            IB.Client.Load();
            BusinessInfoList.Load();
            ContractList.Load();
            UnknownContractList.Load();
            AccountManager.Load();
            //TradeInfoManager.Load();
            //OrderManager.Load();

            StartTask();

            //TIProData.Client.Connect();
            //TIProData.Client.TestTopList();

            Form = new PacmioForm() { Text = TitleText, };
        }

        public static void Save()
        {
            StopTask();

            // Save settings
            Settings.SerializeJsonFile(SettingFile);

            IB.Client.Save();
            BusinessInfoList.Save();
            ContractList.Save();
            //TradeInfoManager.Save();
            //OrderManager.Save();
            AccountManager.Save();
            UnknownContractList.Save();
        }

        #endregion File Storage Utilities

        public static readonly ColorTheme Upper_Theme = new ColorTheme();

        public static readonly ColorTheme Upper_TextTheme = new ColorTheme();

        public static readonly ColorTheme Lower_Theme = new ColorTheme();

        public static readonly ColorTheme Lower_TextTheme = new ColorTheme();

        public static Color UpperColor
        {
            get
            {
                return Upper_Theme.ForeColor;
            }
            set
            {
                Upper_Theme.ForeColor = value;

                Upper_TextTheme.EdgeColor = value.Opaque(255);
                Upper_TextTheme.FillColor = Upper_TextTheme.EdgeColor.GetBrightness() < 0.6 ? Upper_TextTheme.EdgeColor.Brightness(0.85f) : Upper_TextTheme.EdgeColor.Brightness(-0.85f);
                Upper_TextTheme.ForeColor = Upper_TextTheme.EdgeColor;
            }
        }

        public static Color LowerColor
        {
            get
            {
                return Lower_Theme.ForeColor;
            }
            set
            {
                Lower_Theme.ForeColor = value;

                Lower_TextTheme.EdgeColor = value.Opaque(255);
                Lower_TextTheme.FillColor = Lower_TextTheme.EdgeColor.GetBrightness() < 0.6 ? Lower_TextTheme.EdgeColor.Brightness(0.85f) : Lower_TextTheme.EdgeColor.Brightness(-0.85f);
                Lower_TextTheme.ForeColor = Lower_TextTheme.EdgeColor;
            }
        }

        public static readonly string GUID = ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true))[0] as GuidAttribute).Value;
        public static readonly Mutex InstanceMutex = new Mutex(true, GUID);
        public static readonly int SHOW_PACMIO = User32.RegisterWindowMessage("SHOW_PACMIO");

        public static string TitleText => Application.ProductName + " - Rev " + Application.ProductVersion;
    }
}
