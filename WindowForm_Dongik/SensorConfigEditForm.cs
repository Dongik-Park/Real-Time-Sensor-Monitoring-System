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
    public partial class SensorConfigEditForm : Form
    {
        private SensorDBManager dbManager = new SensorDBManager();
        private BaseSensorConfig config = null;
        private GroupBox[] groups = new GroupBox[4];
        public SensorConfigEditForm()
        {
            InitializeComponent();
        }
        public SensorConfigEditForm(BaseSensorConfig config) 
        {
            InitializeComponent();
            this.config = config;
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
        }

        private void SensorAddForm_Load(object sender, EventArgs e)
        {
            groups[0] = TemperatureGroup;
            groups[1] = CpuGroupBox;
            groups[2] = MemoyUsageGroup;
            groups[3] = ModbusGroupBox;
            this.SensorNameText.Text = config.Name;
            //this.SensorComboBox.Text = config.SensorType;
            //this.SensorComboBox.Enabled = false;
            //switch (config.SensorType)
            //{
            //    case "temperature":  GroupBoxControl(0); TemperatureInfoLoad(); break;
            //    case "cpu occupied": GroupBoxControl(1); CpuOccupiedInfoLoad(); break;
            //    case "memory usage": GroupBoxControl(2); MemoryUsageInfoLoad(); break;
            //    case "modbus":       GroupBoxControl(3); ModbusInfoLoad();      break;
            //    default:             GroupBoxControl(4);                        break;
            //}
            this.SensorComboBox.Text = config.SensorType.ToString();
            this.SensorComboBox.Enabled = false;
            switch (config.SensorType)
            {
                case SensorType.Temperature  : GroupBoxControl(0); TemperatureInfoLoad(); break;
                case SensorType.Cpu_occupied : GroupBoxControl(1); CpuOccupiedInfoLoad(); break;
                case SensorType.Memory_usage : GroupBoxControl(2); MemoryUsageInfoLoad(); break;
                case SensorType.Modbus       : GroupBoxControl(3); ModbusInfoLoad(); break;
                default: GroupBoxControl(4); break;
            }
        }
        private void TemperatureInfoLoad()
        {
            if (config.IsActive)
                this.TempActiveCheck.Checked = true;
            else
                this.TempActiveCheck.Checked = false;
            //foreach (KeyValuePair<string, string> pair in config.ExtraInfo)
            //    switch (pair.Key)
            //    {
            //        //Checked is active
            //        case "core1": if (pair.Value.Contains("1")) this.CoreOneCheck.Checked = true; break;
            //        case "core2": if (pair.Value.Contains("1")) this.CoreTwoCheck.Checked = true; break;
            //        case "core3": if (pair.Value.Contains("1")) this.CoreThreeCheck.Checked = true; break;
            //        case "core4": if (pair.Value.Contains("1")) this.CoreFourCheck.Checked = true; break;
            //    }
        }
        private void MemoryUsageInfoLoad()
        {
            if (config.IsActive)
                this.MemoryActiveCheck.Checked = true;
            else
                this.MemoryActiveCheck.Checked = false;
        }
        private void CpuOccupiedInfoLoad()
        {
            if (config.IsActive)
                this.CpuCheckBox.Checked = true;
            else
                this.CpuCheckBox.Checked = false;
            //foreach(KeyValuePair<string,string> pair in config.ExtraInfo)
            //    switch (pair.Key)
            //    {
            //        case "process": if (pair.Value.Contains("total")) this.CpuTotalCheck.Checked = true;
            //                        else this.CpuTotalCheck.Checked = false; 
            //                        break;
            //    }
        }
        private void ModbusInfoLoad()
        {
            if (config.IsActive)
                this.ModbusActiveCheck.Checked = true;
            else
                this.ModbusActiveCheck.Checked = false;
            //foreach (KeyValuePair<string, string> pair in config.ExtraInfo)
            //    switch (pair.Key)
            //    {
            //        //Checked is active
            //        case "ip":      this.IpTextBox.Text = pair.Value.ToString();      break;
            //        case "port":    this.PortTextBox.Text = pair.Value.ToString();    break;
            //        case "address": this.AddressTextBox.Text = pair.Value.ToString(); break;
            //        case "size":    this.SizeTextBox.Text = pair.Value.ToString();    break;
            //    }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool NameChangedCheck()
        {
            string oldName = config.Name;
            string newName = SensorNameText.Text.ToString();
            // User input different name
            if (!oldName.Equals(newName))
            {
                if (dbManager.dc.BaseSensorConfigs.Where(t => t.Name == newName).First() != null)
                {
                    MessageBox.Show(newName+" already exists.");
                    return false;
                }
            }
            return true;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (SensorNameText.Text.Equals("") || SensorNameText.Text.Contains("/"))
            {
                MessageBox.Show("Name should be changed");
                return;
            }
            if (!NameChangedCheck())
            {
                MessageBox.Show("Name should be changed");
                return;
            }
            switch (this.SensorComboBox.SelectedItem.ToString())
            {
                //case "temperature":  dbManager.dc..EditSensorByJson(this.config, GetTemperatureConfig()); break;
                //case "cpu occupied": dao.EditSensorByJson(this.config, GetCpuOccupiedConfig()); break;
                //case "memory usage": dao.EditSensorByJson(this.config, GetMemoryUsageConfig()); break;
                //case "modbus":       dao.EditSensorByJson(this.config, GetModbusConfig());      break;
                default: MessageBox.Show("Input sensor type."); return; 
            }
            MessageBox.Show(this.SensorNameText.Text.ToString()+" has been made");  
            this.SensorAddForm_Load(sender, e);
            this.Owner.Refresh();
        }
        //// Get BaseSensorConfig setting
        //private BaseSensorConfig EditBaseSensorConfig()
        //{
        //    BaseSensorConfig curConfig = dbManager.dc.BaseSensorConfigs.Single(t => t.Id == config.Id);
        //    curConfig.Name = SensorNameText.Text.ToString();
        //    curConfig.IsActive = Convert.ToByte(TempActiveCheck.Checked);
        //    return curConfig;
        //}
        //private void EditGetTemperatureConfig(BaseSensorConfig curConfig)
        //{
        //    curConfig.TempertaureTables.CoreInd
        //}
        //// Get temperature setting by JsonObjectCollection  
        //private JsonObjectCollection GetTemperatureConfig()
        //{

        //    JsonObjectCollection col = new JsonObjectCollection();
        //    DateTime cur = DateTime.Now;
        //    // Sensor ID
        //    col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
        //    // Sensor Type
        //    col.Add(new JsonStringValue("type", config.Type));
        //    // Sensor name 
        //    col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
        //    // Sensor generating time
        //    col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
        //    // Whether sensor is active
        //    if (this.TempActiveCheck.Checked)
        //        col.Add(new JsonStringValue("active", "1"));
        //    else
        //        col.Add(new JsonStringValue("active", "0"));
        //    // Select cpu cores 
        //    if (this.CoreOneCheck.Checked)
        //        col.Add(new JsonStringValue("core1", "1"));
        //    else
        //        col.Add(new JsonStringValue("core1", "0"));
        //    if (this.CoreTwoCheck.Checked)
        //        col.Add(new JsonStringValue("core2", "1"));                
        //    else
        //        col.Add(new JsonStringValue("core2", "0"));
        //    if (this.CoreThreeCheck.Checked)
        //        col.Add(new JsonStringValue("core3", "1"));
        //    else
        //        col.Add(new JsonStringValue("core3", "0"));
        //    if (this.CoreFourCheck.Checked)
        //        col.Add(new JsonStringValue("core4", "1"));
        //    else
        //        col.Add(new JsonStringValue("core4", "0"));
        //    return col;
        //}
        //// Get cpu occupied setting by JsonObjectCollection  
        //private JsonObjectCollection GetCpuOccupiedConfig()
        //{
        //    JsonObjectCollection col = new JsonObjectCollection();
        //    DateTime cur = DateTime.Now;
        //    // Sensor ID
        //    col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
        //    // Sensor name 
        //    col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
        //    // Sensor Type
        //    col.Add(new JsonStringValue("type", config.Type));
        //    // Sensor generating time
        //    col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
        //    // Whether sensor is active
        //    if (this.CpuCheckBox.Checked)
        //        col.Add(new JsonStringValue("active", "1"));
        //    else
        //        col.Add(new JsonStringValue("active", "0"));
        //    // Select cpu process
        //    if (this.CpuTotalCheck.Checked)
        //        col.Add(new JsonStringValue("process", "total"));
        //    else if (this.CpuCurrentCheck.Checked)
        //        col.Add(new JsonStringValue("process", "current"));
        //    return col;
        //}
        //// Get modbus setting by JsonObjectCollection  
        //private JsonObjectCollection GetModbusConfig()
        //{
        //    JsonObjectCollection col = new JsonObjectCollection();
        //    DateTime cur = DateTime.Now;
        //    // Sensor ID
        //    col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
        //    // Sensor name 
        //    col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
        //    // Sensor Type
        //    col.Add(new JsonStringValue("type", config.Type));
        //    // Sensor generating time
        //    col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
        //    // Whether sensor is active
        //    if (this.ModbusActiveCheck.Checked)
        //        col.Add(new JsonStringValue("active", "1"));
        //    else
        //        col.Add(new JsonStringValue("active", "0"));
        //    // Modbus informations
        //    col.Add(new JsonStringValue("ip", this.IpTextBox.Text));
        //    col.Add(new JsonStringValue("port", this.PortTextBox.Text));
        //    col.Add(new JsonStringValue("address", this.AddressTextBox.Text));
        //    col.Add(new JsonStringValue("size", this.SizeTextBox.Text));
        //    return col;
        //}
        //// Get memory usage setting by JsonObjectCollection  
        //private JsonObjectCollection GetMemoryUsageConfig()
        //{
        //    JsonObjectCollection col = new JsonObjectCollection();
        //    DateTime cur = DateTime.Now;
        //    // Sensor ID
        //    col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
        //    // Sensor name 
        //    col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
        //    // Sensor Type
        //    col.Add(new JsonStringValue("type", config.Type));
        //    // Sensor generating time
        //    col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
        //    if (this.MemoryActiveCheck.Checked)
        //        col.Add(new JsonStringValue("active", "1"));
        //    else
        //        col.Add(new JsonStringValue("active", "0"));

        //    return col;
        //}

    }
}
