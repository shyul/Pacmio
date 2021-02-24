/// ***************************************************************************
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
    public class WatchListDataAdapter : IDataConsumer
    {
        public WatchListDataAdapter(CheckedListBox checkedListBox)
        {
            CheckedListBox = checkedListBox;
            WatchListManager.DataProvider.AddDataConsumer(this);
        }

        private CheckedListBox CheckedListBox;

        public void DataIsUpdated(IDataProvider provider)
        {
            Task.Run(() =>
            {
                CheckedListBox?.Invoke(() =>
                {
                    CheckedListBox.Items.Clear();
                    foreach (WatchList wt in WatchListManager.List)
                    {
                        string name = wt.Name;
                        bool isRunning = wt is DynamicWatchList dwt ? dwt.IsRunning : false;

                        CheckedListBox.Items.Add(name, isRunning);
                    }

                    CheckedListBox.Invalidate(true);
                });
            });
        }

        public void Dispose()
        {
            RemoveDataSource();
        }

        public void RemoveDataSource()
        {
            WatchListManager.DataProvider.RemoveDataConsumer(this);
        }
    }

}
