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
    public partial class SetComfigForm_ver2 : Form
    {
        private SensorDTO dto = null;
        private GroupBox[] groups = new GroupBox[4];

        public SetComfigForm_ver2(SensorDTO dto)
        {
            InitializeComponent();
            this.dto = dto;
        }

        private void GroupBoxControl(int index)
        {
            for (int i = 0; i < 4; ++i)
                if (i == index)
                    this.groups[i].Show();
                else
                    if (groups[i].Visible)
                        groups[i].Visible = false;
        }
        private void SetComfigForm_ver2_Load(object sender, EventArgs e)
        {
            groups[0] = TemperatureGroup;
            groups[1] = CpuGroupBox;
            groups[2] = MemoyUsageGroup;
            groups[3] = ModbusGroupBox;
            switch (dto.Type)
            {
                case "temperature":  GroupBoxControl(0); break;
                case "cpu occupied": GroupBoxControl(1); break;
                case "memory usage": GroupBoxControl(2); break;
                case "modbus":       GroupBoxControl(3); break;
                default:             GroupBoxControl(4); break;
            }
        }
        private void TemperatureInfoLoad()
        {
            if (dto.IsActive == 1)
                this.CpuCheckBox.Checked = true;
            else
                this.CpuCheckBox.Checked = false;
           // if(dto.
        }
    }
}
