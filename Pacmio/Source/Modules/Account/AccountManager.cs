/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public static class AccountManager
    {
        private static HashSet<Account> m_List { get; } = new HashSet<Account>();

        public static IEnumerable<Account> List => m_List;

        public static int Count => m_List.Count;

        public static Account Get(string accountCode)
        {
            var res = m_List.Where(n => n.AccountCode == accountCode);

            if (res.Count() > 0) return res.First();
            else return null;
        }

        public static Account GetOrAdd(string accountCode)
        {
            var res = m_List.Where(n => n.AccountCode == accountCode);

            if (res.Count() > 0) return res.First();
            else
            {
                Account ac = new Account(accountCode);
                m_List.Add(ac);
                return ac;
            }
        }

        #region Data Requests

        public static object PositionLockObject { get; } = new object();

        public static void Request_AccountSummary() => IB.Client.SendRequest_AccountSummary();

        public static void Request_Postion() => IB.Client.SendRequest_Postion();

        #region Updates

        public static DateTime UpdatedTime { get; set; }

        public static event StatusEventHandler UpdatedHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdatedTime = DateTime.Now;
            UpdatedHandler?.Invoke(statusCode, UpdatedTime, message);
        }

        #endregion Updates

        #endregion Data Requests

        #region File system

        private static string FileName => Root.ResourcePath + "Accounts.Json";

        public static void Save()
        {
            lock (m_List)
            {
                m_List.ToArray().SerializeJsonFile(FileName);
            }
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                var list = Serialization.DeserializeJsonFile<Account[]>(FileName);
                foreach (Account ac in list)
                {
                    ac.Setup();
                    m_List.CheckAdd(ac);
                }
            }

            Update(1, "Account file loaded.");
        }

        #endregion File system
    }
}
