/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Master List of all Accounts in the program
    /// </summary>
    public static class AccountPositionManager
    {
        private static Dictionary<string, AccountInfo> AccountLUT { get; } = new Dictionary<string, AccountInfo>();

        public static IEnumerable<AccountInfo> List => AccountLUT.Values;

        public static int Count => AccountLUT.Count;

        public static AccountInfo GetAccountById(string accountId)
        {
            if (accountId is string code)
            {
                lock (AccountLUT)
                {
                    return AccountLUT.ContainsKey(code) ? AccountLUT[code] : null;
                }
            }
            else
                throw new Exception("Account Code has to be a valid string.");
        }

        public static AccountInfo GetOrCreateAccountById(string accountId)
        {
            if (accountId is string code)
            {
                lock (AccountLUT)
                {
                    if (!AccountLUT.ContainsKey(code)) AccountLUT.Add(code, new AccountInfo(code));
                    return AccountLUT[code];
                }
            }
            else
                throw new Exception("Account Code has to be a valid string.");
        }

        public static void Request_AccountSummary()
        {
            IB.Client.SendRequest_AccountSummary();
        }

        public static void EmergencyCloseAllPositions()
        {
            foreach (var ac in AccountLUT.Values)
            {
                ac.EmergencyCloseAllPositions();
            }
        }

        #region Data Requests

        public static void Request_Position()
        {
            IB.Client.SendRequest_Position();
        }

        internal static void ResetAllPositionRefreshStatus()
        {
            lock (AccountLUT)
            {
                foreach (AccountInfo ac in AccountLUT.Values) ac.ResetAllPositionRefreshStatus();
            }
        }

        internal static void ResetNonRefreshedPositions()
        {
            lock (AccountLUT)
            {
                foreach (AccountInfo ac in AccountLUT.Values) ac.ResetNonRefreshedPositions();
            }
        }

        #endregion Data Requests

        #region Updates

        public static DateTime UpdateTime { get; set; }

        public static event StatusEventHandler OnUpdateHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdateTime = DateTime.Now;
            OnUpdateHandler?.Invoke(statusCode, UpdateTime, message);
        }

        public static SimpleDataProvider AccountDataProvider { get; } = new SimpleDataProvider();

        public static SimpleDataProvider PositionDataProvider { get; } = new SimpleDataProvider();


        // Refresh Account Info

        // Refresh Position Info

        #endregion Updates

        #region File system

        private static string FileName => Root.ResourcePath + "Accounts.Json";

        public static void Save()
        {
            lock (AccountLUT)
            {
                AccountLUT.Values.ToArray().SerializeJsonFile(FileName);
            }
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                var list = Serialization.DeserializeJsonFile<AccountInfo[]>(FileName);
                foreach (AccountInfo ac in list)
                {
                    AccountLUT.CheckAdd(ac.AccountId, ac);
                }
            }

            Update(1, "Account file loaded.");
        }

        #endregion File system
    }
}
