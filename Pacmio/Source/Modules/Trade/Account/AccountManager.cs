/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using Xu;
using System.Web.SessionState;

namespace Pacmio
{
    /// <summary>
    /// Master List of all Accounts in the program
    /// </summary>
    public static class AccountManager
    {
        public static HashSet<Account> List { get; set; }

        public static Account Get(string accountCode)
        {
            if (List == null) List = new HashSet<Account>();

            var res = List.Where(n => n.AccountCode == accountCode);

            if (res.Count() > 0) return res.First();
            else return null;
        }

        public static T GetOrAdd<T>(T ac) where T : Account
        {
            if (List == null) List = new HashSet<Account>();

            lock (List)
            {
                var list = List.Where(n => n.AccountCode == ac.AccountCode).ToList();

                if (list.Count > 0)
                    if (list.First().GetType() == typeof(T))
                        return (T)list.First();
                    else
                        foreach (var acx in list)
                            List.Remove(acx);

                List.Add(ac);
                ac.Reset();

                Update(1, "Account added and updated");
                return ac;
            }
        }

        public static void CloseAllPositions()
        {
            CancelAllOrders();
            foreach (Account ac in List)
                ac.CloseAllPositions();
        }

        public static void CancelAllOrders() => Root.IBClient.SendRequest_GlobalCancel();

        #region Updates

        public static DateTime UpdatedTime { get; set; }

        public static event StatusEventHandler UpdatedHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdatedTime = DateTime.Now;
            UpdatedHandler?.Invoke(statusCode, UpdatedTime, message);
        }

        #endregion Updates

        #region File system

        private static string FileName => Root.ResourcePath + "Accounts.Json";

        public static void Save()
        {
            lock (List)
                List?.SerializeJsonFile(FileName);
        }

        public static void Load()
        {
            if (File.Exists(FileName))
                List = Serialization.DeserializeJsonFile<HashSet<Account>>(FileName);
            else
                List = new HashSet<Account>();

            foreach (var ac in List)
            {
                ac.Reset();
            }

            Update(1, "Account file loaded.");
        }

        #endregion File system
    }
}
