using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TradeIdeas.TIProData;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// SymbollistMasterForm. Allows user to manage
    /// his symbol lists. He may edit a list, delete a list...etc
    /// </summary>
    public partial class SymbolListsMasterForm : Form
    {
        /// <summary>
        /// SymbolListMasterForm constructor
        /// </summary>
        public SymbolListsMasterForm()
        {
            InitializeComponent();
        }

        private void SymbolListsMasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void createByIdButton_Click(object sender, EventArgs e)
        {
            new SymbolListForm(Form1.Connection.ListManager.Find(Convert.ToInt32(idNumberTextBox.Text))).Show();
        }

        private void requestListOfListsButton_Click(object sender, EventArgs e)
        {
            requestListOfListsButton.Enabled = false;
            allListsListBox.Items.Clear();
            allListsListBox.Items.Add("[Waiting...]");
            Form1.Connection.ListManager.GetListOfLists(ListOfListsCallback);
        }

        private void ListOfListsCallback(List<ListManager.List> lists)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { ListOfListsCallback(lists); });
            else
            {
                if (IsDisposed)
                    return;
                requestListOfListsButton.Enabled = true;
                allListsListBox.Items.Clear();
                foreach (ListManager.List list in lists)
                {
                    allListsListBox.Items.Add(list);                    
                }
            }
        }

        private void allListsListBox_DoubleClick(object sender, EventArgs e)
        {
            Object selected = allListsListBox.SelectedItem;
            if (selected is ListManager.List)
                new SymbolListForm((ListManager.List)selected).Show();
        }

        private void deleteAllListsButton_Click(object sender, EventArgs e)
        {
            Form1.Connection.ListManager.DeleteAll();
        }

    }
}
