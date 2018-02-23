using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowForm_Dongik
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void realTimeBtn_Click(object sender, EventArgs e)
        {
            new RealTimeForm().Show(this);
        }

        private void sensorBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new SensorManageForm())
                dialog.ShowDialog(this);
        }

        private void lastDataBtn_Click(object sender, EventArgs e)
        {
            new LastTimeForm().Show(this);
        }

    }
}
