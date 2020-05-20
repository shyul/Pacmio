using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Pacmio;

namespace TestClient
{
    public partial class TestFreqAlign : Form
    {
        public TestFreqAlign()
        {
            InitializeComponent();
            ComboBoxTimeUnit.Items.AddRange(typeof(TimeUnit).GetEnumNames());
            ComboBoxTimeUnit.SelectedIndex = ComboBoxTimeUnit.FindStringExact(TimeUnit.Weeks.ToString());

        }

        private void BtnAlign_Click(object sender, EventArgs e)
        {
            Frequency freq = new Frequency(ComboBoxTimeUnit.Text.ParseEnum<TimeUnit>(), (int)NumUpDownFreq.Value);

            DateTimePicker.Value = freq.Align(DateTimePicker.Value, (int)NumAlign.Value);
        }
    }
}
