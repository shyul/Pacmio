﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /// <summary>
        /// Send RequestFamilyCodes: (0)"80"-
        /// Received FamilyCodes: (0)"78"-(1)"3"-(2)"Uxxxxxx"-(3)""-(4)"Uxxxxxx"-(5)""-(6)"Uxxxxxx"-
        /// </summary>
        /// <param name="fields">respond sequence</param>
        private static void Parse_ManagedAccounts(string[] fields)
        {
            if (fields[1] == "1")
            {
                string[] accountCodes = fields[2].Split(',');

                foreach (string accountCode in accountCodes)
                {
                    if (accountCode.Length > 0)
                    {
                        AccountManager.GetOrAdd(accountCode);
                    }
                }

                AccountManager.UpdatedTime = DateTime.Now;
            }
        }

        private static void Parse_AccountValue(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            if (fields[1] == "1")
            {
                /*
                int msgVersion = ReadInt();
                string key = ReadString();
                string value = ReadString();
                string currency = ReadString();
                string accountName = null;
                if (msgVersion >= 2)
                    accountName = ReadString();
                eWrapper.updateAccountValue(key, value, currency, accountName);
                */
            }
        }

        private static void Parse_AccountUpdateTime(string[] fields)
        {
            if (fields[1] == "1")
            {
                AccountManager.UpdatedTime = DateTime.Now;
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            }
        }

        private static void Parse_AccountDownloadEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            if (fields[1] == "1")
            {
                /*
                int msgVersion = ReadInt();
                string account = ReadString();
                eWrapper.accountDownloadEnd(account);
                */
            }
        }
    }
}