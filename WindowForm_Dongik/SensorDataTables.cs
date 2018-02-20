using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowForm_Dongik
{
    public partial class SensorDataTables : UserControl
    {
        public int SelectedTabIndex
        {
            get { return SensorDataTab.SelectedIndex; }
            set { SensorDataTab.SelectedIndex = value; }
        }
        public SensorDataTables()
        {
            InitializeComponent();
        }
    }
}
