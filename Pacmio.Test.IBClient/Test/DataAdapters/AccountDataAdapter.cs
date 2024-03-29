﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Pacmio;
using Pacmio.IB;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;

namespace TestClient
{
    public class AccountDataAdapter : IDataConsumer
    {
        public AccountDataAdapter(TreeView tr, ListBox lb)
        {
            TreeViewAccount = tr;
            ListBoxAccount = lb;
            AccountPositionManager.AccountDataProvider.AddDataConsumer(this);
            UpdateAccountList();
        }

        public void Dispose()
        {
            RemoveDataSource();
        }

        private TreeView TreeViewAccount { get; }

        private ListBox ListBoxAccount { get; }

        public void DataIsUpdated(IDataProvider provider)
        {
            Task.Run(() =>
            {
                TreeViewAccount?.Invoke(() =>
                {
                    UpdateAccountList();
                    TreeViewAccount.Invalidate(true);
                    ListBoxAccount.Invalidate(true);
                });
            });
        }

        public void UpdateAccountList()
        {
            ListBoxAccount.Items.Clear();
            TreeViewAccount.Nodes.Clear();

            foreach (var ac in AccountPositionManager.Accounts)
            {
                if (ac.IsLive) ListBoxAccount.Items.Add(ac.AccountId);

                string dayTradingString = "[ ";
                foreach (int d in ac.DayTradesRemaining)
                {
                    if (d == -1)
                    {
                        dayTradingString = "[ No DT Limit";
                        break;
                    }
                    else
                        dayTradingString += d + ", ";
                }
                dayTradingString = dayTradingString.Trim().TrimEnd(',') + " ]";

                TreeNode tr = new(ac.AccountId + ": " + ac.AccountType + " " + dayTradingString);

                foreach (PropertyInfo p in ac.GetType().GetProperties())
                {
                    string tagName = p.Name + " = ";
                    if (p.GetAttribute<DisplayNameAttribute>() is DisplayNameAttribute Result) tagName = Result.DisplayName + " = ";

                    if (p.PropertyType == typeof(double))
                    {
                        double v = (double)p.GetValue(ac);
                        if (!double.IsNaN(v))
                        {
                            tagName += v.ToString();
                            tr.Nodes.Add(tagName);
                        }
                    }
                }

                TreeViewAccount.Nodes.Add(tr);
            }

            TreeViewAccount.ExpandAll();
        }

        public void RemoveDataSource()
        {
            AccountPositionManager.AccountDataProvider.RemoveDataConsumer(this);
        }
    }
}
