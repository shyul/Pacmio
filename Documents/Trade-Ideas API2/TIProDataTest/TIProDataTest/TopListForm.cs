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
    /// TopLIstForm class. Allows user to get top list data
    /// upon entering a TopList configuration string.
    /// </summary>
    public partial class TopListForm : Form
    {
        private BindingList<RowData> _boundList = new BindingList<RowData>();
        /// <summary>
        /// TopListForm Constructor
        /// </summary>
        public TopListForm()
        {
            InitializeComponent();

           /* Setting the data source allows us to use any type of object we
             * want to store the row data.  If we added the row directly using
             * dataGridView1.Rows.Add(myRow), myRow would have to be an array
             * of objects.  That means we'd have to parse and format the data
             * as we were adding it to the data.  That's not completely
           /* unreasonble, but I'd rather keep it the way it is. */
            dataGridView1.DataSource = _boundList;
        }

        TopList _topList;

        private void startButton_Click(object sender, EventArgs e)
        {
            if (null != _topList)
                _topList.Stop();
            _topList = Form1.Connection.TopListManager.GetTopList(configTextBox.Text);
            _topList.TopListStatus += new TopListStatus(_topList_TopListStatus);
            _topList.TopListData += new TopListData(_topList_TopListData);
            _topList.Start();
            stopButton.Enabled = true;
            resultsTextBox.AppendText("Start:  " + DateTime.Now + "\r\n");
        }

        void _topList_TopListStatus(TopList sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _topList_TopListStatus(sender); });
            else
            {
                if (sender != _topList)
                    resultsTextBox.AppendText("Ignoring TopListStatus.\r\n");
                else
                {   // As Text
                    resultsTextBox.AppendText(_topList.TopListInfo.ToString() + "\r\n");
                    // As Table
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Add(DataCell.GetSymbolColumn());
                    foreach (ColumnInfo columnInfo in _topList.TopListInfo.Columns)
                        dataGridView1.Columns.Add(DataCell.GetColumn(columnInfo));
                }
            }
        }

        private int rowCount;
        private int messageCount;
        void _topList_TopListData(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _topList_TopListData(rows, start, end, sender); });
            else
            {
                if (sender != _topList)
                    resultsTextBox.AppendText("Ignoring TopListData.\r\n");
                else
                {   // As Text
                    resultsTextBox.AppendText("TopListData " + DateTime.Now + "\r\n");
                    if (null != start)
                        resultsTextBox.AppendText("Start Time = " + start + "\r\n");
                    if (null != end)
                        resultsTextBox.AppendText("End Time = " + end + "\r\n");
                    rowCount += rows.Count;
                    rowCountLabel.Text = "Row Count:  " + rowCount;
                    messageCount++;
                    messageCountLabel.Text = "Message Count:  " + messageCount;
                    resultsTextBox.AppendText("Received " + rows.Count + " rows.\r\n");
                    foreach (RowData row in rows)
                    {
                        resultsTextBox.AppendText(row.ToString());
                        resultsTextBox.AppendText("\r\n");
                    }
                    // As Array
                    _boundList.Clear();
                    foreach (RowData row in rows)
                    {
                        _boundList.Add(row);
                    }
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _topList.Stop();
            _topList = null;
            stopButton.Enabled = false;
        }

        private void TopListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != _topList)
                _topList.Stop();
        }
    }

   
    /// <summary>
    /// Data Cell class
    /// This is good at doing the basic formatting.  Excellent proof of
    /// concept.  But it needs a lot of cleanup before the user sees it.
    /// At a bare minimum:
    /// 1)  Row headers disappear (or replaced by a number (1, 2, 3, etc.))
    /// 2)  Do not jump to the top of the list every time.  (TIQ had some
    ///     smart rules.  At a minimum just never jump.)
    /// 3)  Do not allow the user to resize a row.
    /// 4)  Replace the title with an icon, when appropriate.  (Keep the
    ///     tooltip.)  We could release without this, but I really don't
    ///     want to!
    /// 5)  Fix the way we display numbers that are too big.  Display an
    ///     elipsis to replace the entire number, like TI Pro does with
    ///     prices.  The current style is probably okay for the symbol
    ///     column.  The description column will be more complicated, and
    ///     probably use word wrap, although we don't need to deal with
    ///     that today.
    /// Probably more.  Make it look good.
    /// NOTE:  A newer version of this can be found in the SimpleTIPro
    /// project.
    /// </summary>
    public class DataCell : DataGridViewTextBoxCell
    {
        private enum Mode { Price, Number, String };
        private Mode _mode;
        private string _format;
        private string _internalCode;
        private string _wireName;
        /// <summary>
        /// This is required!  If you don't have a constructor with no arguments,
        ///  the code will fail at runtime with a confusing error message.  
        /// 
        /// Ideally _mode, _format, etc. would all be readonly.  But we need to
        ///  be able to use Clone().  base.Clone() calls this constructor, then
        ///  our version of Close() updates these member variables. 
        /// </summary>
        public DataCell()
        { 
        }
        public DataCell(string wireName)
        {
            _wireName = wireName;
            _mode = Mode.String;
        }
        public DataCell(ColumnInfo columnInfo)
        {
            // A reasonable default.
            _mode = Mode.String;
            if (columnInfo.Format == "p")
                _mode = Mode.Price;
            else
            {
                int digits;
                if (Int32.TryParse(columnInfo.Format, out digits))
                {
                    if (digits > 7)
                        digits = 7;
                    else if (digits < 0)
                        digits = 0;
                    _mode = Mode.Number;
                    _format = "N" + digits;
                }
            }
            _internalCode = columnInfo.InternalCode;  // This belongs somewhere else!
            _wireName = columnInfo.WireName;
        }
        public DataGridViewContentAlignment Alignment()
        {
            if (_mode == Mode.String)
                return DataGridViewContentAlignment.MiddleLeft;
            else
                return DataGridViewContentAlignment.MiddleRight;
        }
        protected override object GetFormattedValue(object value, int rowIndex, 
            ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, 
            TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            RowData rowData = ((BindingList<RowData>)DataGridView.DataSource)[rowIndex];
            //return "(" + _wireName + ", " + rowIndex + ")";
            // I was planning to cache this next part.  But it seems like there's so
            // much work just getting to here, and then more work rendering the string
            // on the screen, so this one conversion might not mean much!
            String cellData = rowData.GetAsString(_wireName);
            if (_mode == Mode.String)
                return cellData;
            // Convert the string we received from the server into a double, then format
            // it in the appropriate way.  It was tempting to have the server do the
            // formatting for us.  But then we might lose precision if we wanted to do
            // anything else with the value.  For example, sorting should work on all the
            // digits, not just the visible ones.  And maybe the user wants to change the
            // number of digits to display.
            double cellAsDouble;
            if (!Double.TryParse(cellData, out cellAsDouble))
                return "";
            if (_mode == Mode.Number)
                return cellAsDouble.ToString(_format);
            else
                // Mode.Price
                if (rowData.GetAsString("four_digits") == "1")
                    return cellAsDouble.ToString("N4");
                else
                    return cellAsDouble.ToString("N2");
        }
        public override object Clone()
        {
            DataCell result = (DataCell)base.Clone();
            result._mode = _mode;
            result._format = _format;
            result._internalCode = _internalCode;
            result._wireName = _wireName;
            return result;
        }
        public static DataGridViewColumn GetColumn(String wireName)
        {
            return GetColumn(new DataCell(wireName));
        }
        public static DataGridViewColumn GetColumn(ColumnInfo columnInfo)
        {
            DataGridViewColumn result = GetColumn(new DataCell(columnInfo));
            result.HeaderText = columnInfo.Description + " (" + columnInfo.Units + ')';
            // Ideally we'd have a size hint based on how the column would be used.  But we don't
            // have that information here.  Temporarily we're using the text description, but
            // that's a little silly, too.  Eventually most columns should use an icon in the
            // header, and the description does not matter.
            //result.Width = 12 + TextRenderer.MeasureText(result.HeaderText, result.DataGridView.Font).Width;
            // Can't set the width here because result.DataGridView is null.
            result.ToolTipText = result.HeaderText;
            return result;
        }
        private static DataGridViewColumn GetColumn(DataCell dataCell)
        {
            DataGridViewTextBoxColumn result = new DataGridViewTextBoxColumn();
            result.CellTemplate = dataCell;
            result.DefaultCellStyle = new DataGridViewCellStyle();
            result.DefaultCellStyle.Alignment = dataCell.Alignment();
            return result;
        }
        public static DataGridViewColumn GetSymbolColumn()
        {
            DataGridViewColumn result = GetColumn("symbol");
            result.HeaderText = "Symbol";
            // Need a better place to set the width.
            //result.Width = 12 + TextRenderer.MeasureText(result.HeaderText, result.DataGridView.Font).Width;
            // Can't set the width here because result.DataGridView is null.
            return result;
        }
    }
    //DataGridView.AutoSizeRowsMode Property could be useful for the description column in the alerts.
}
