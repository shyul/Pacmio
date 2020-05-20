namespace Pacmio.Utility
{
    partial class MarketDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.marketDataGrid_MDT = new System.Windows.Forms.DataGridView();
            this.marketDataContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marketDataTypeTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bidSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bidPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preOpenBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preOpenAsk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.askPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.askSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.closeTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.highTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.futuresOpenInterestTickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.marketDataGrid_MDT)).BeginInit();
            this.SuspendLayout();
            // 
            // marketDataGrid_MDT
            // 
            this.marketDataGrid_MDT.AllowUserToAddRows = false;
            this.marketDataGrid_MDT.AllowUserToDeleteRows = false;
            this.marketDataGrid_MDT.AllowUserToOrderColumns = true;
            this.marketDataGrid_MDT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.marketDataGrid_MDT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.marketDataContract,
            this.marketDataTypeTickerColumn,
            this.bidSize,
            this.bidPrice,
            this.preOpenBid,
            this.preOpenAsk,
            this.askPrice,
            this.askSize,
            this.lastTickerColumn,
            this.lastPrice,
            this.volume,
            this.closeTickerColumn,
            this.openTickerColumn,
            this.highTickerColumn,
            this.lowTickerColumn,
            this.futuresOpenInterestTickerColumn});
            this.marketDataGrid_MDT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketDataGrid_MDT.Location = new System.Drawing.Point(0, 0);
            this.marketDataGrid_MDT.Name = "marketDataGrid_MDT";
            this.marketDataGrid_MDT.ReadOnly = true;
            this.marketDataGrid_MDT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.marketDataGrid_MDT.Size = new System.Drawing.Size(1860, 1037);
            this.marketDataGrid_MDT.TabIndex = 27;
            this.marketDataGrid_MDT.Visible = false;
            // 
            // marketDataContract
            // 
            this.marketDataContract.HeaderText = "Description";
            this.marketDataContract.Name = "marketDataContract";
            this.marketDataContract.ReadOnly = true;
            this.marketDataContract.Width = 200;
            // 
            // marketDataTypeTickerColumn
            // 
            this.marketDataTypeTickerColumn.HeaderText = "Mkt Data Type";
            this.marketDataTypeTickerColumn.Name = "marketDataTypeTickerColumn";
            this.marketDataTypeTickerColumn.ReadOnly = true;
            this.marketDataTypeTickerColumn.Width = 150;
            // 
            // bidSize
            // 
            this.bidSize.HeaderText = "Bid Size";
            this.bidSize.Name = "bidSize";
            this.bidSize.ReadOnly = true;
            // 
            // bidPrice
            // 
            this.bidPrice.HeaderText = "Bid";
            this.bidPrice.Name = "bidPrice";
            this.bidPrice.ReadOnly = true;
            // 
            // preOpenBid
            // 
            this.preOpenBid.HeaderText = "Pre-Open Bid";
            this.preOpenBid.Name = "preOpenBid";
            this.preOpenBid.ReadOnly = true;
            // 
            // preOpenAsk
            // 
            this.preOpenAsk.HeaderText = "Pre-Open Ask";
            this.preOpenAsk.Name = "preOpenAsk";
            this.preOpenAsk.ReadOnly = true;
            // 
            // askPrice
            // 
            this.askPrice.HeaderText = "Ask";
            this.askPrice.Name = "askPrice";
            this.askPrice.ReadOnly = true;
            // 
            // askSize
            // 
            this.askSize.HeaderText = "Ask Size";
            this.askSize.Name = "askSize";
            this.askSize.ReadOnly = true;
            // 
            // lastTickerColumn
            // 
            this.lastTickerColumn.HeaderText = "Last";
            this.lastTickerColumn.Name = "lastTickerColumn";
            this.lastTickerColumn.ReadOnly = true;
            // 
            // lastPrice
            // 
            this.lastPrice.HeaderText = "Last Size";
            this.lastPrice.Name = "lastPrice";
            this.lastPrice.ReadOnly = true;
            // 
            // volume
            // 
            this.volume.HeaderText = "Volume";
            this.volume.Name = "volume";
            this.volume.ReadOnly = true;
            // 
            // closeTickerColumn
            // 
            this.closeTickerColumn.HeaderText = "Close";
            this.closeTickerColumn.Name = "closeTickerColumn";
            this.closeTickerColumn.ReadOnly = true;
            this.closeTickerColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.closeTickerColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // openTickerColumn
            // 
            this.openTickerColumn.HeaderText = "Open";
            this.openTickerColumn.Name = "openTickerColumn";
            this.openTickerColumn.ReadOnly = true;
            // 
            // highTickerColumn
            // 
            this.highTickerColumn.HeaderText = "High";
            this.highTickerColumn.Name = "highTickerColumn";
            this.highTickerColumn.ReadOnly = true;
            // 
            // lowTickerColumn
            // 
            this.lowTickerColumn.HeaderText = "Low";
            this.lowTickerColumn.Name = "lowTickerColumn";
            this.lowTickerColumn.ReadOnly = true;
            // 
            // futuresOpenInterestTickerColumn
            // 
            this.futuresOpenInterestTickerColumn.HeaderText = "Fut Open Int";
            this.futuresOpenInterestTickerColumn.Name = "futuresOpenInterestTickerColumn";
            this.futuresOpenInterestTickerColumn.ReadOnly = true;
            // 
            // MarketDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1860, 1037);
            this.Controls.Add(this.marketDataGrid_MDT);
            this.Name = "MarketDataForm";
            this.Text = "Market Data";
            ((System.ComponentModel.ISupportInitialize)(this.marketDataGrid_MDT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView marketDataGrid_MDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn marketDataContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn marketDataTypeTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bidSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn bidPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn preOpenBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn preOpenAsk;
        private System.Windows.Forms.DataGridViewTextBoxColumn askPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn askSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn closeTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn openTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn highTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowTickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn futuresOpenInterestTickerColumn;
    }
}