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
    /// SymbolListForm class.  Allows user to manage
    /// his symbol lists.
    /// </summary>
    public partial class SymbolListForm : Form
    {
        private ListManager.List _list;
        /// <summary>
        /// SymbolListForm constructor
        /// </summary>
        /// <param name="list">list of symbol objects</param>
        public SymbolListForm(ListManager.List list)
        {
            _list = list;
            InitializeComponent();
            CopyListToWindow();
        }

        private void CopyListToWindow()
        {
            listIdLabel.Text = "List ID #" + _list.Id;
            listNameTextBox.Text = _list.Name;
        }

        private void setListNameButton_Click(object sender, EventArgs e)
        {
            _list.Name = listNameTextBox.Text;
        }

        private void deleteListButton_Click(object sender, EventArgs e)
        {
            _list.DeleteList();
        }

        private void loadInfoButton_Click(object sender, EventArgs e)
        {
            CopyListToWindow();
        }

        private void addSingleSymbolButton_Click(object sender, EventArgs e)
        {
            _list.AddSymbol(singleSymbolTextBox.Text);
        }

        private void removeSingleSymbolButton_Click(object sender, EventArgs e)
        {
            _list.DeleteSymbol(singleSymbolTextBox.Text);
        }

        private void sendSymbolsButton_Click(object sender, EventArgs e)
        {
            string[] symbols = symbolListTextBox.Lines;
            _list.SetAllSymbols(new HashSet<string>(symbols));
        }

        private void requestSymbolsButton_Click(object sender, EventArgs e)
        {
            _list.RequestFromServer(OnListReturned);
            symbolListTextBox.Text = "*** Requested ***";
        }

        private void OnListReturned(ListManager.List list, HashSet<string> symbols)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { OnListReturned(list, symbols); });
            else
            {
                if (IsDisposed)
                    return;
                StringBuilder lines = new StringBuilder();
                foreach (string symbol in symbols)
                {
                    lines.Append(symbol);
                    lines.Append("\r\n");
                }
                symbolListTextBox.Text = lines.ToString();
            }
        }
    }
}
