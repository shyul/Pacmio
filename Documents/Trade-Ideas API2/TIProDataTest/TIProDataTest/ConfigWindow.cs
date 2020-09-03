using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;
using TradeIdeas.TIProData.Configuration;

// TODO  Add gui for symbol list disposition.

namespace WindowsFormsApplication1
{
    /// <summary>
    /// ConfigWindow class. Here, the user can configure his alert strategies
    /// A config string is generated and displayed in the top text box upon
    /// configuring a strategy displayed in the strategy tree view of the form
    /// </summary>
    public partial class ConfigWindow : Form
    {
        ConfigurationType _configurationType;

        /// <summary>
        /// Config Window constructor
        /// </summary>
        /// <param name="configurationType"> either "Alert" or "Toplist"</param>
        public ConfigWindow(ConfigurationType configurationType)
        {
            _configurationType = configurationType;
            InitializeComponent();
            switch (configurationType)
            {
                case ConfigurationType.Alerts:
                    tabControl1.TabPages.Remove(columnsTabPage);
                    tabControl1.TabPages.Remove(sortTabPage);
                    tabControl1.TabPages.Remove(timePeriodTabPage);
                    Text = "Alert Config Window";
                    break;
                case ConfigurationType.TopList:
                    tabControl1.TabPages.Remove(alertsTabPage);
                    Text = "Top List Config Window";
                    break;
            }
        }

        private ConfigurationWindowManager _configurationWindowManager;
        private PrepairedStrategy _strategy;
        private AlertStrategy _alertStrategy;
        private TopListStrategy _topListStrategy;

        private void SetStrategy(PrepairedStrategy strategy)
        {
            _strategy = strategy;
            _alertStrategy = strategy as AlertStrategy;
            _topListStrategy = strategy as TopListStrategy;
        }

        private void requestServerInfoButton_Click(object sender, EventArgs e)
        {
            if (_configurationWindowManager != null)
                _configurationWindowManager.Abandon();
            recentStatusLabel.Text = "Requested.";
            _configurationWindowManager = new ConfigurationWindowManager();
            _configurationWindowManager.LoadFromServer(Form1.Connection, _configurationType, OnLoaded, configStringTextBox.Text);
        }
        private void OnLoaded(ConfigurationWindowManager configurationWindowManager)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { OnLoaded(configurationWindowManager); });
            else
            {
                if (IsDisposed)
                    return;
                if (configurationWindowManager != _configurationWindowManager)
                {
                    recentStatusLabel.Text = "Ignoring unexpected response.";
                    return;
                }
                recentStatusLabel.Text = "Received response.";
                alertsListBox.Items.Clear();
                alertsListBox.Items.AddRange(_configurationWindowManager.AlertsInOrder.ToArray());
                filtersListBox.Items.Clear();
                filtersListBox.Items.AddRange(_configurationWindowManager.FiltersInOrder.ToArray());
                exchangesCheckedListBox.Items.Clear();
                exchangesCheckedListBox.Items.AddRange(_configurationWindowManager.ExchangesInOrder.ToArray());
                symbolListsCheckedListBox.Items.Clear();
                symbolListsCheckedListBox.Items.AddRange(_configurationWindowManager.SymbolListsInOrder.ToArray());
                allColumnsListBox.Items.Clear();
                allColumnsListBox.Items.AddRange(_configurationWindowManager.FiltersInOrder.ToArray());
                sortFieldListBox.Items.Clear();
                sortFieldListBox.Items.AddRange(_configurationWindowManager.FiltersInOrder.ToArray());
                categoriesComboBox.Items.Clear();
                categoriesComboBox.Items.Add("All alerts");
                categoriesComboBox.Items.AddRange(_configurationWindowManager.CategoriesAlerts);
                // This is hard coded for alerts.  These should be some way to see the CategoriesFilters list, too.
                categoriesComboBox.SelectedIndex = 0;
                AddStrategyNodes();
                if (_configurationWindowManager.CurrentSettings == null)
                    if (_configurationType == ConfigurationType.Alerts)
                        SetStrategy(new AlertStrategy());
                    else
                        SetStrategy(new TopListStrategy());
                else
                    if (_configurationType == ConfigurationType.Alerts)
                        SetStrategy(new AlertStrategy(_configurationWindowManager.CurrentSettings));
                    else
                        SetStrategy(new TopListStrategy(_configurationWindowManager.CurrentSettings));
                getFromConfigButton.Enabled = true;
            }
        }
        private void AddStrategyNodes()
        {
            strategyTreeView.Nodes.Clear();
            strategyDescriptionTextBox.Text = "";
            StrategyNode top = _configurationWindowManager.StrategyTree;
            if (top == null)
                return;
            if (top.IsFolder())
                foreach (StrategyNode node in top.Children)
                    AddStrategyNodes(node, strategyTreeView.Nodes);
            else
                // We should never get here.  The way our library parses the message from the
                // server, there has to be a folder on top.
                //AddStrategyNodes(top, null);
                Debug.Assert(false);
        }
        /// <summary>
        /// Populate each of the nodes of the strategy tree view on the Alert Configuration Window
        /// </summary>
        /// <param name="top"> Top node of strategy tree</param>
        /// <param name="into"></param>
        private void AddStrategyNodes(StrategyNode top, TreeNodeCollection into)
        {
            TreeNode newNode = new TreeNode(top.Name);
            switch (top.Icon)
            {
                case "+":
                    newNode.ImageIndex = 4;
                    break;
                case "-":
                    newNode.ImageIndex = 5;
                    break;
                case "*":
                    newNode.ImageIndex = 2;
                    break;
                case "folder":
                    newNode.ImageIndex = 3;
                    break;
                case ":)":
                    newNode.ImageIndex = 6;
                    break;
                default:
                    newNode.ImageIndex = 0;
                    break;
            }
            newNode.Tag = top;
            into.Add(newNode);
            if (top.IsFolder())
                foreach (StrategyNode node in top.Children)
                    AddStrategyNodes(node, newNode.Nodes);
        }

        private void ConfigWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_configurationWindowManager != null)
            {
                _configurationWindowManager.Abandon();
                _configurationWindowManager = null;
            }
        }

        private void getFromConfigButton_Click(object sender, EventArgs e)
        {
            configStringTextBox.Text = _strategy.MakeConfigString();
        }

        private void windowNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_strategy != null)
                _strategy.WindowName = windowNameTextBox.Text;
        }

        private void strategyTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            StrategyNode selected =
                strategyTreeView.SelectedNode.Tag as StrategyNode;
            StringBuilder description = new StringBuilder();
            if (selected != null)
            {
                description.Append('[');
                if (selected.IsFolder())
                    description.Append("Folder");
                else
                    description.Append(selected.PrepairedStrategy.OriginalConfigString);
                description.Append("] ");
                if (selected.Icon != "")
                {
                    description.Append("{icon: ");
                    description.Append(selected.Icon);
                    description.Append("} ");
                }
                description.Append(selected.Description);
            }
            strategyDescriptionTextBox.Text = description.ToString();
        }

        private void strategyTreeView_DoubleClick(object sender, EventArgs e)
        {
            StrategyNode selected =
                strategyTreeView.SelectedNode.Tag as StrategyNode;
            if (selected == null)
                return;
            if (selected.IsFolder())
                return;
            if (_configurationType == ConfigurationType.Alerts)
                SetStrategy(new AlertStrategy(selected.PrepairedStrategy));
            else
                SetStrategy(new TopListStrategy(selected.PrepairedStrategy));
        }

        private void alertsListBox_DoubleClick(object sender, EventArgs e)
        {
            Alert alert = alertsListBox.SelectedItem as Alert;
            if (alert == null)
                alertCodetextBox.Text = "";
            else
                alertCodetextBox.Text = alert.InternalCode;
        }

        private void filtersListBox_DoubleClick(object sender, EventArgs e)
        {
            Filter filter = filtersListBox.SelectedItem as Filter;
            if (filter == null)
                filterTextBox.Text = "";
            else
                filterTextBox.Text = filter.InternalCode;
        }

        private void addAlertButton_Click(object sender, EventArgs e)
        {
            Alert alert = _configurationWindowManager.FindAlert(alertCodetextBox.Text);
            if (alert == null)
                SystemSounds.Exclamation.Play();
            else
                _alertStrategy.Alerts.Add(alert);
        }

        private void removeAlertButton_Click(object sender, EventArgs e)
        {
            Alert alert = _configurationWindowManager.FindAlert(alertCodetextBox.Text);
            if (alert == null)
                SystemSounds.Exclamation.Play();
            else
                _alertStrategy.Alerts.Remove(alert);
        }

        private void setAlertQualityButton_Click(object sender, EventArgs e)
        {
            Alert alert = _configurationWindowManager.FindAlert(alertCodetextBox.Text);
            if (alert == null)
                SystemSounds.Exclamation.Play();
            else
                _alertStrategy.AlertQuality[alert] = alertQualityTextBox.Text;
        }

        private void setMinButton_Click(object sender, EventArgs e)
        {
            Filter filter = _configurationWindowManager.FindFilter(filterTextBox.Text);
            if (filter == null)
                SystemSounds.Exclamation.Play();
            else
                _strategy.MinFilters[filter] = filterValueTextBox.Text;
        }

        private void setMaxButton_Click(object sender, EventArgs e)
        {
            Filter filter = _configurationWindowManager.FindFilter(filterTextBox.Text);
            if (filter == null)
                SystemSounds.Exclamation.Play();
            else
                _strategy.MaxFilters[filter] = filterValueTextBox.Text;
        }

        private void readExchangesButton_Click(object sender, EventArgs e)
        {
            for (int i = exchangesCheckedListBox.Items.Count - 1; i >= 0; i--)
                exchangesCheckedListBox.SetItemChecked(i, _strategy.Exchanges.Contains(exchangesCheckedListBox.Items[i]));
        }

        private void setExchangesButton_Click(object sender, EventArgs e)
        {
            _strategy.Exchanges.Clear();
            foreach (Object item in exchangesCheckedListBox.CheckedItems)
                _strategy.Exchanges.Add(item as Exchange);
        }

        private void readSymbolListsButton_Click(object sender, EventArgs e)
        {
            for (int i = symbolListsCheckedListBox.Items.Count - 1; i >= 0; i--)
                symbolListsCheckedListBox.SetItemChecked(i, _strategy.SymbolLists.Contains(symbolListsCheckedListBox.Items[i]));
        }

        private void setSymbolListsButton_Click(object sender, EventArgs e)
        {
            _strategy.SymbolLists.Clear();
            foreach (Object item in symbolListsCheckedListBox.CheckedItems)
                _strategy.SymbolLists.Add(item as SymbolList);
        }

        private void flipButton_Click(object sender, EventArgs e)
        {
            if (_configurationType == ConfigurationType.Alerts)
                SetStrategy(_configurationWindowManager.Flip(_alertStrategy));
            else
                SetStrategy(_configurationWindowManager.Flip(_topListStrategy));
        }

        private void searchNowButton_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            HashSet<object> objects;
            if (categoriesComboBox.SelectedIndex > 0)
                objects = _configurationWindowManager.Search(searchTextTextBox.Text, categoriesComboBox.SelectedItem.ToString());
            else
                objects = _configurationWindowManager.Search(searchTextTextBox.Text);
            DateTime end = DateTime.Now;
            searchTimeLabel.Text = string.Format("LastSearchTime:  {0:N3} seconds", (end - start).TotalSeconds);
            searchResultsTextBox.Clear();
            foreach (object item in objects)
            {
                searchResultsTextBox.AppendText(item.ToString());
                searchResultsTextBox.AppendText("\r\n");
            }
        }

        private void readColumnsButton_Click(object sender, EventArgs e)
        {
            currentColumnsListBox.Items.Clear();
            currentColumnsListBox.Items.AddRange(_topListStrategy.Columns.ToArray());
        }

        private void setColumnsButton_Click(object sender, EventArgs e)
        {
            _topListStrategy.Columns.Clear();
            foreach (Filter column in currentColumnsListBox.Items)
                _topListStrategy.Columns.Add(column);
        }

        private void addColumnsButton_Click(object sender, EventArgs e)
        {
            foreach (Filter column in allColumnsListBox.SelectedItems)
                currentColumnsListBox.Items.Add(column);
        }

        private void removeColumnsButton_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection selected = currentColumnsListBox.SelectedIndices;
            List<int> toDelete = new List<int>(selected.Count);
            foreach (int i in selected)
                toDelete.Add(i);
            toDelete.Reverse();
            foreach (int i in toDelete)
                currentColumnsListBox.Items.RemoveAt(i);
        }

        private void sortTopRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortTopRadioButton.Checked)
                _topListStrategy.BiggestOnTop = true;
        }

        private void sortBottomRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortBottomRadioButton.Checked)
                _topListStrategy.BiggestOnTop = false;
        }

        private void sortNoneRadioButton_CheckedChanged(object sender, EventArgs e)
        {   // No real GUI would have this.  This is for testing.  Sometimes the selection
            // will be null because we don't provide a default. 
            if (sortNoneRadioButton.Checked)
                sortFieldListBox.ClearSelected();
        }

        private void sortFieldListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortNoneRadioButton.Checked)
                sortTopRadioButton.Checked = true;
            _topListStrategy.SortBy = sortFieldListBox.SelectedItem as Filter;
        }

        private void readTimePeriodButton_Click(object sender, EventArgs e)
        {
            historyCheckBox.Checked = _topListStrategy.History;
            try
            {
                historyDateTimePicker.Value = _topListStrategy.Time;
            }
            catch
            {
                historyDateTimePicker.Value = historyDateTimePicker.MinDate;
            }
            outsideMarketHoursCheckBox.Checked = _topListStrategy.OutsideMarketHours;
        }

        private void setTimePeriodButton_Click(object sender, EventArgs e)
        {
            _topListStrategy.History = historyCheckBox.Checked;
            _topListStrategy.Time = historyDateTimePicker.Value;
            _topListStrategy.Time = _topListStrategy.Time.AddMilliseconds(-_topListStrategy.Time.Millisecond);
            _topListStrategy.Time = _topListStrategy.Time.AddSeconds(-_topListStrategy.Time.Second);
            _topListStrategy.OutsideMarketHours = outsideMarketHoursCheckBox.Checked;

            //StringBuilder sb = new StringBuilder();

            Console.WriteLine(_topListStrategy.MakeConfigString());


        }

    }
}
