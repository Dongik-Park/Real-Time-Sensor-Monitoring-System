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
           this.Visible = false;
           RealTimeForm frm = new RealTimeForm(); 
    	   frm.Owner = this; 
    	   frm.Show(); 
        }

        private void sensorBtn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            SensorManageForm frm = new SensorManageForm();
            frm.Owner = this;
            frm.Show();
        }

    }
}
