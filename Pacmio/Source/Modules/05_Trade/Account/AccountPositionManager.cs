﻿/// ***************************************************************************
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

        public static AccountInfo[] Accounts
        {
            get
            {
                lock (AccountLUT) return AccountLUT.Values.ToArray();
            }
        }



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
                ac.EmergencyClose();
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
                PositionDataUpdated();
            }
        }

        public static PositionInfo[] Positions => Accounts.SelectMany(n => n.Positions is PositionInfo[] p ? p : new PositionInfo[] { }).Where(p => p.Quantity != 0).ToArray();

        public static double UnrealizedPnL
        {
            get => m_UnrealizedPnL;

            private set
            {
                if (Math.Abs(value - m_UnrealizedPnL) > 0.01)
                {
                    Console.WriteLine("Total Unrealized PnL = " + value.ToString("0.###"));
                }
                m_UnrealizedPnL = value;
            }
        }

        private static double m_UnrealizedPnL = 0;

        #endregion Data Requests

        #region Updates

        public static SimpleDataProvider AccountDataProvider { get; } = new SimpleDataProvider();
        
        public static SimpleDataProvider PositionDataProvider { get; } = new SimpleDataProvider();

        public static void PositionDataUpdated() 
        {
            UnrealizedPnL = Positions.Select(n => n.UnrealizedPnL).Sum();
            PositionDataProvider.Updated();
        }

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
                    ac.IsLive = false;
                    AccountLUT.CheckAdd(ac.AccountId, ac);
                }
            }

            AccountDataProvider.Updated();
        }

        #endregion File system
    }
}
