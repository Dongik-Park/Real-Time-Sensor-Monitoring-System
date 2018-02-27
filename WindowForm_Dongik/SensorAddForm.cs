using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Json;
namespace WindowForm_Dongik
{
    public partial class SensorAddForm : Form
    {
        private SensorDBManager dbManager = new SensorDBManager();
        private GroupBox[] groups = new GroupBox[4];
        public SensorAddForm()
        {
            InitializeComponent();
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

        private void SensorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.SensorComboBox.SelectedItem.ToString())
            {
                case "temperature":  GroupBoxControl(0); break;
                case "cpu occupied": GroupBoxControl(1); break;
                case "memory usage": GroupBoxControl(2); break;
                case "modbus":       GroupBoxControl(3); break;
                default:             GroupBoxControl(4); break;
            }
            //this.Invalidate();
        }

        private void SensorAddForm_Load(object sender, EventArgs e)
        {
            groups[0] = TemperatureGroup;
            groups[1] = CpuGroupBox;
            groups[2] = MemoyUsageGroup;
            groups[3] = ModbusGroupBox;
            this.SensorNameText.Text = "";
            this.SensorComboBox.Text = this.SensorComboBox.Items[0].ToString();
            this.TemperatureGroup.Hide();
            this.CpuGroupBox.Hide();
            this.ModbusGroupBox.Hide();
            this.MemoyUsageGroup.Hide();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (SensorNameText.Text.Equals("") || SensorNameText.Text.Contains("/"))
            {
                MessageBox.Show("Name should be changed");
                return;
            }
            string name = SensorNameText.Text.ToString();
            DateTime time = DateTime.Now;
            string sensorType = SensorComboBox.SelectedItem.ToString();
            switch (this.SensorComboBox.SelectedItem.ToString())
            {
                case  "temperature": InsertTemperatureSensor(name, time,sensorType,TempActiveCheck.Checked);     break;
                case "cpu occupied": InsertCpuOccupiedSensor(name, time, sensorType, CpuCheckBox.Checked);       break;
                case "memory usage": InsertMemoryUsageSensor(name, time, sensorType, MemoryActiveCheck.Checked); break;
                case       "modbus": InsertModbusSensor(name, time, sensorType, ModbusActiveCheck.Checked);      break;
                           default : MessageBox.Show("Input sensor type."); return;
            }
            dbManager.dc.SubmitChanges();
            MessageBox.Show(this.SensorNameText.Text.ToString()+" has been made");  
            this.SensorAddForm_Load(sender, e);
            this.Owner.Refresh();
        }

        private void InsertTemperatureSensor(string name, DateTime time, string sensorType, bool isActive)
        {
            int coreSize = 5;
            bool [] coreIndex = new bool[coreSize];
            for(int i = 0; i < coreSize; ++i)
                coreIndex[i] = false;

            if (this.CoreOneCheck.Checked){
                coreIndex[1] = true;
            }
            if (this.CoreTwoCheck.Checked){
                coreIndex[2] = true;
            }
            if (this.CoreThreeCheck.Checked) {
                coreIndex[3] = true;
            }
            if (this.CoreFourCheck.Checked){
                coreIndex[4] = true;
            }
            for(byte i = 1; i < coreSize; ++i){
                if (coreIndex[i])
                {
                    TempertaureSensorConfig tempConfig = new TempertaureSensorConfig()
                    {
                        Name = name,
                        MadeTime = time,
                        SensorType = sensorType,
                        IsActive = isActive,
                        CoreIndex = i
                    };
                    System.Data.Linq.Table<BaseSensorConfig> table = dbManager.dc.BaseSensorConfigs;
                    table.InsertOnSubmit(tempConfig);
                }
            }

        }
        private void InsertCpuOccupiedSensor(string name, DateTime time, string sensorType, bool isActive)
        {
            byte processType = 1;
            //if (this.CpuTotalCheck.Checked)
            //    processType = 1;
            if (this.CpuCurrentCheck.Checked)
                processType = 0;
            CpuSensorConfig cpuConfig = new CpuSensorConfig()
            {
                Name = name,
                MadeTime = time,
                SensorType = sensorType,
                IsActive = isActive,
                ProcessType = processType
            };
            dbManager.dc.BaseSensorConfigs.InsertOnSubmit(cpuConfig);
        }
        private void InsertMemoryUsageSensor(string name, DateTime time, string sensorType, bool isActive)
        {
            MemorySensorConfig memConfig = new MemorySensorConfig()
            {
                Name = name,
                MadeTime = time,
                SensorType = sensorType,
                IsActive = isActive
            };
            dbManager.dc.BaseSensorConfigs.InsertOnSubmit(memConfig);
        }
        private void InsertModbusSensor(string name, DateTime time, string sensorType, bool isActive)
        {
            ModbusSensorConfig modConfig = new ModbusSensorConfig()
            {
                Name = name,
                MadeTime = time,
                SensorType = sensorType,
                IsActive = isActive,
                Ip = this.IpTextBox.Text,
                Port = Convert.ToInt32(this.PortTextBox.Text),
                Address = Convert.ToUInt16(this.AddressTextBox.Text)
            };
            dbManager.dc.BaseSensorConfigs.InsertOnSubmit(modConfig);
        }
    }
}
