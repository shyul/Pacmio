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
        public static HashSet<Account> List { get; } = new HashSet<Account>();

        public static int Count => List.Count;

        public static Account Get(string accountCode)
        {
            var res = List.Where(n => n.AccountCode == accountCode);

            if (res.Count() > 0) return res.First();
            else return null;
        }

        public static Account GetOrAdd(string accountCode)
        {
            var res = List.Where(n => n.AccountCode == accountCode);

            if (res.Count() > 0) return res.First();
            else
            {
                Account ac = new Account(accountCode);
                List.Add(ac);
                return ac;
            }
        }

        #region Data Requests

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
            lock (List)
            {
                List.ToArray().SerializeJsonFile(FileName);
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
                    List.CheckAdd(ac);
                }
            }

            Update(1, "Account file loaded.");
        }

        #endregion File system
    }
}
