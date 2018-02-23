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
        private SensorDBManager dbManager = SensorDBManager.Instance;
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
            switch (this.SensorComboBox.SelectedItem.ToString())
            {
                case "temperature":  InsertTemperatureSensor(); break;
                case "cpu occupied": InsertCpuOccupiedSensor(); break;
                case "memory usage": InsertMemoryUsageSensor(); break;
                case "modbus":       InsertModbusSensor();      break;
                default: MessageBox.Show("Input sensor type."); return;
            }
            MessageBox.Show(this.SensorNameText.Text.ToString()+" has been made");  
            this.SensorAddForm_Load(sender, e);
            this.Owner.Refresh();
        }
        private SensorConfig InsertSensorConfig(bool isActive)
        {
            SensorConfig newConfig = new SensorConfig()
            {
                Name = this.SensorNameText.Text.ToString(),
                MadeTime = DateTime.Now,
                SensorType = this.SensorComboBox.SelectedItem.ToString(),
                IsActive = 1
            };
            if (!isActive)
                newConfig.IsActive = 0;
            dbManager.dc.SensorConfigs.InsertOnSubmit(newConfig);
            return newConfig;
        }
        private void InsertTemperatureSensor()
        {
            if (this.CoreOneCheck.Checked)
            {
                SensorConfig newConfig = InsertSensorConfig(this.TempActiveCheck.Checked);
                newConfig.TempertaureTables = new TempertaureSensor() { CoreIndex = 1 };
                dbManager.dc.TempertaureSensors.InsertOnSubmit(newConfig.TempertaureTables);
                dbManager.dc.SubmitChanges();
            }
            if (this.CoreTwoCheck.Checked)
            {
                SensorConfig newConfig = InsertSensorConfig(this.TempActiveCheck.Checked);
                newConfig.TempertaureTables = new TempertaureSensor() { CoreIndex = 2 };
                dbManager.dc.TempertaureSensors.InsertOnSubmit(newConfig.TempertaureTables);
                dbManager.dc.SubmitChanges();
            }
            if (this.CoreThreeCheck.Checked)
            {
                SensorConfig newConfig = InsertSensorConfig(this.TempActiveCheck.Checked);
                newConfig.TempertaureTables = new TempertaureSensor() { CoreIndex = 3 };
                dbManager.dc.TempertaureSensors.InsertOnSubmit(newConfig.TempertaureTables);
                dbManager.dc.SubmitChanges();
            }
            if (this.CoreFourCheck.Checked)
            {
                SensorConfig newConfig = InsertSensorConfig(this.TempActiveCheck.Checked);
                newConfig.TempertaureTables = new TempertaureSensor() { CoreIndex = 4 };
                dbManager.dc.TempertaureSensors.InsertOnSubmit(newConfig.TempertaureTables);
                dbManager.dc.SubmitChanges();
            }

        }
        private void InsertCpuOccupiedSensor()
        {
            SensorConfig newConfig = InsertSensorConfig(this.CpuCheckBox.Checked);
            if (this.CpuTotalCheck.Checked)   newConfig.CpuOccupiedTables = new CpuOccupiedSensor() { ProcessType = 1 };
            if (this.CpuCurrentCheck.Checked) newConfig.CpuOccupiedTables = new CpuOccupiedSensor() { ProcessType = 0 };
            dbManager.dc.CpuOccupiedSensors.InsertOnSubmit(newConfig.CpuOccupiedTables);
        }
        private void InsertMemoryUsageSensor()
        {
            SensorConfig newConfig = InsertSensorConfig(this.MemoryActiveCheck.Checked);
            newConfig.MemoryUsageTables = new MemoryUsageSensor() { };
            dbManager.dc.MemoryUsageSensors.InsertOnSubmit(newConfig.MemoryUsageTables);
        }
        private void InsertModbusSensor()
        {
            SensorConfig newConfig = InsertSensorConfig(this.ModbusActiveCheck.Checked);
            newConfig.ModbusTables = new ModbusSensor()
            {
                Ip = this.IpTextBox.Text,
                Port = Convert.ToInt32(this.PortTextBox.Text),
                Address = Convert.ToInt32(this.AddressTextBox.Text)
            };
            dbManager.dc.ModbusSensors.InsertOnSubmit(newConfig.ModbusTables);
        }
    }
}
