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
        private GroupBox[] groups = new GroupBox[5];
        public SensorAddForm()
        {
            InitializeComponent();
        }
        private void GroupBoxControl(int index)
        {
            for (int i = 0; i < 5; ++i)
                if (i == index)
                    this.groups[i].Show();
                else
                    if (groups[i].Visible)
                        groups[i].Visible = false;
        }

        private void SensorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SensorType sensorType = SensorType.None;
            Enum.TryParse<SensorType>(SensorComboBox.SelectedValue.ToString(), out sensorType);
            switch (sensorType)
            {
                case  SensorType.Temperature : GroupBoxControl(0); break;
                case SensorType.Cpu_occupied : GroupBoxControl(1); break;
                case SensorType.Memory_usage : GroupBoxControl(2); break;
                case       SensorType.Modbus : GroupBoxControl(3); break;
                case         SensorType.Omap : GroupBoxControl(4); break;
                default                      : GroupBoxControl(5); break;
            }
        }

        private void SensorAddForm_Load(object sender, EventArgs e)
        {
            groups[0] = TemperatureGroup;
            groups[1] = CpuGroupBox;
            groups[2] = MemoyUsageGroup;
            groups[3] = ModbusGroupBox;
            groups[4] = OmapGroupBox;
            this.OmapTypeComboBox.DataSource = Enum.GetValues(typeof(OmapType));
            this.SensorComboBox.DataSource = Enum.GetValues(typeof(SensorType));
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
                MessageBox.Show("Name should be changed.");
                return;
            }

            string name = SensorNameText.Text.ToString();
            DateTime time = DateTime.Now;
            SensorType sensorType = SensorType.None;
            Enum.TryParse<SensorType>(SensorComboBox.SelectedValue.ToString(), out sensorType);

            if (sensorType == SensorType.None)
            {
                MessageBox.Show("Choose the sensor type.");
                return;
            }
                
            switch (sensorType)
            {
                case  SensorType.Temperature : InsertTemperatureSensor(name, time, sensorType, TempActiveCheck.Checked);   break;
                case SensorType.Cpu_occupied : InsertCpuOccupiedSensor(name, time, sensorType, CpuCheckBox.Checked);       break;
                case SensorType.Memory_usage : InsertMemoryUsageSensor(name, time, sensorType, MemoryActiveCheck.Checked); break;
                case       SensorType.Modbus : InsertModbusSensor(name, time, sensorType, ModbusActiveCheck.Checked);      break;
                case         SensorType.Omap : InsertOmapSensor(name, time, sensorType, OmapActiveCheck.Checked);          break;
                default                      : MessageBox.Show("Input sensor type."); return;
            }
            dbManager.dc.SubmitChanges();
            MessageBox.Show(this.SensorNameText.Text.ToString() + " has been made");
            SensorNameText.Text = "";
            this.SensorAddForm_Load(sender, e);
            this.Owner.Refresh();
        }

        private void InsertTemperatureSensor(string name, DateTime time, SensorType sensorType, bool isActive)
        {
            int coreSize = 5;
            CoreNumber [] coreIndex = new CoreNumber[coreSize];
            for(int i = 0; i < coreSize; ++i)
                coreIndex[i] = CoreNumber.None;

            if (this.CoreOneCheck.Checked){
                coreIndex[1] = CoreNumber.Core_1;
            }
            if (this.CoreTwoCheck.Checked){
                coreIndex[2] = CoreNumber.Core_2;
            }
            if (this.CoreThreeCheck.Checked) {
                coreIndex[3] = CoreNumber.Core_3;
            }
            if (this.CoreFourCheck.Checked){
                coreIndex[4] = CoreNumber.Core_4;
            }
            for(byte i = 1; i < coreSize; ++i){
                if (coreIndex[i] != CoreNumber.None)
                {
                    TempertaureSensorConfig tempConfig = new TempertaureSensorConfig()
                    {
                        Name = name,
                        MadeTime = time,
                        SensorType = sensorType,
                        IsActive = isActive,
                        CoreIndex = coreIndex[i]
                    };
                    System.Data.Linq.Table<BaseSensorConfig> table = dbManager.dc.BaseSensorConfigs;
                    table.InsertOnSubmit(tempConfig);
                }
            }

        }
        private void InsertCpuOccupiedSensor(string name, DateTime time, SensorType sensorType, bool isActive)
        {
            ProcessType processType = ProcessType.None;
            if (this.CpuTotalCheck.Checked)
                processType = ProcessType.Total;
            else if (this.CpuCurrentCheck.Checked)
                processType = ProcessType.Current;
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
        private void InsertMemoryUsageSensor(string name, DateTime time, SensorType sensorType, bool isActive)
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
        private void InsertModbusSensor(string name, DateTime time, SensorType sensorType, bool isActive)
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
        private void InsertOmapSensor(string name, DateTime time, SensorType sensorType, bool isActive)
        {
            OmapType type;
            Enum.TryParse<OmapType>(OmapTypeComboBox.SelectedValue.ToString(), out type);
            OmapSensorConfig omapConfig = new OmapSensorConfig()
            {
                Name = name,
                MadeTime = time,
                SensorType = sensorType,
                IsActive = isActive,
                Ip = this.OmapIpText.Text,
                OmapType = type
            };
            dbManager.dc.BaseSensorConfigs.InsertOnSubmit(omapConfig);
        }
    }
}
