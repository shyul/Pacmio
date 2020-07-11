using Pacmio;
using System;
using System.Linq;
using System.Windows.Forms;
using Xu;

namespace TestClient
{
    public partial class TestMultiPeriodDataSource : Form
    {
        public MultiPeriod<DataSource> d = new MultiPeriod<DataSource>();

        public DateTime GetDataSourceStartTime(DateTime endTime, DataSource source)
        {
            var res = d.Where(n => n.Value <= source && n.Key.Contains(endTime));
            if (res.Count() > 0) return res.Last().Key.Start;
            else return endTime;
        }

        public TestMultiPeriodDataSource()
        {
            InitializeComponent();
            Text = Program.TitleText;
            ComboBoxDataSource.Items.AddRange(typeof(DataSource).GetEnumNames());
            ComboBoxDataSource.SelectedIndex = ComboBoxDataSource.FindStringExact(DataSource.Quandl.ToString());
            GridView.ColumnCount = 3;
            GridView.Columns[0].Name = "Start time";
            GridView.Columns[1].Name = "End time";
            GridView.Columns[2].Name = "Data source";
            GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            RefreshGrid();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            d.Add(DateTimePickerStart.Value, DateTimePickerEnd.Value, ComboBoxDataSource.Text.ParseEnum<DataSource>());
            RefreshGrid();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            d.Remove(DateTimePickerStart.Value, DateTimePickerEnd.Value);
            RefreshGrid();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            d.Clear();
            RefreshGrid();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            GridView.Rows.Clear();
            foreach (var item in d)
            {
                string[] row = new string[] { item.Key.Start.ToString(), item.Key.Stop.ToString(), item.Value.ToString() };
                GridView.Rows.Add(row);
            }
        }

        private void BtnGetStart_Click(object sender, EventArgs e)
        {
            DateTimePickerGetStart.Value = GetDataSourceStartTime(DateTimePickerGetStart.Value, ComboBoxDataSource.Text.ParseEnum<DataSource>());
        }
    }
}
