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
    public partial class SensorConfigForm_ver3 : Form
    {
        private SensorDAO dao = SensorDAO.Instance;
        private SensorDTO dto = new SensorDTO();
        private GroupBox[] groups = new GroupBox[4];
        public SensorConfigForm_ver3()
        {
            InitializeComponent();
        }
        public SensorConfigForm_ver3(SensorDTO dto) 
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
            this.SensorNameText.Text = dto.Name;
            this.SensorComboBox.Text = dto.Type;
            this.SensorComboBox.Enabled = false;
            switch (dto.Type)
            {
                case "temperature":  GroupBoxControl(0); TemperatureInfoLoad(); break;
                case "cpu occupied": GroupBoxControl(1); CpuOccupiedInfoLoad(); break;
                case "memory usage": GroupBoxControl(2); MemoryUsageInfoLoad(); break;
                case "modbus":       GroupBoxControl(3); ModbusInfoLoad();      break;
                default:             GroupBoxControl(4);                        break;
            }
        }
        private void TemperatureInfoLoad()
        {
            if (dto.IsActive == 1)
                this.TempActiveCheck.Checked = true;
            else
                this.TempActiveCheck.Checked = false;
            foreach (KeyValuePair<string, string> pair in dto.ExtraInfo)
                switch (pair.Key)
                {
                    //Checked is active
                    case "core1": if (pair.Value.Contains("1")) this.CoreOneCheck.Checked = true; break;
                    case "core2": if (pair.Value.Contains("1")) this.CoreTwoCheck.Checked = true; break;
                    case "core3": if (pair.Value.Contains("1")) this.CoreThreeCheck.Checked = true; break;
                    case "core4": if (pair.Value.Contains("1")) this.CoreFourCheck.Checked = true; break;
                }
        }
        private void MemoryUsageInfoLoad()
        {
            if (dto.IsActive == 1)
                this.MemoryActiveCheck.Checked = true;
            else
                this.MemoryActiveCheck.Checked = false;
        }
        private void CpuOccupiedInfoLoad()
        {
            if (dto.IsActive == 1)
                this.CpuCheckBox.Checked = true;
            else
                this.CpuCheckBox.Checked = false;
            foreach(KeyValuePair<string,string> pair in dto.ExtraInfo)
                switch (pair.Key)
                {
                    case "process": if (pair.Value.Contains("total")) this.CpuTotalCheck.Checked = true;
                                    else this.CpuTotalCheck.Checked = false; 
                                    break;
                }
        }
        private void ModbusInfoLoad()
        {
            if (dto.IsActive == 1)
                this.ModbusActiveCheck.Checked = true;
            else
                this.ModbusActiveCheck.Checked = false;
            foreach (KeyValuePair<string, string> pair in dto.ExtraInfo)
                switch (pair.Key)
                {
                    //Checked is active
                    case "ip":      this.IpTextBox.Text = pair.Value.ToString();      break;
                    case "port":    this.PortTextBox.Text = pair.Value.ToString();    break;
                    case "address": this.AddressTextBox.Text = pair.Value.ToString(); break;
                    case "size":    this.SizeTextBox.Text = pair.Value.ToString();    break;
                }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool NameChangedCheck()
        {
            string oldName = dto.Name;
            string newName = SensorNameText.Text.ToString();
            // User input different name
            if (!oldName.Equals(newName))
            {
                foreach(string name in dao.LoadSensorList())
                    if (name.Equals(newName))
                        return false;
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
                case "temperature":  dao.EditSensorByJson(this.dto, GetTemperatureConfig()); break;
                case "cpu occupied": dao.EditSensorByJson(this.dto, GetCpuOccupiedConfig()); break;
                case "memory usage": dao.EditSensorByJson(this.dto, GetMemoryUsageConfig()); break;
                case "modbus":       dao.EditSensorByJson(this.dto, GetModbusConfig());      break;
                default: MessageBox.Show("Input sensor type."); return; 
            }
            MessageBox.Show(this.SensorNameText.Text.ToString()+" has been made");  
            this.SensorAddForm_Load(sender, e);
            this.Owner.Refresh();
        }
        // Get temperature setting by JsonObjectCollection  
        private JsonObjectCollection GetTemperatureConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            DateTime cur = DateTime.Now;
            // Sensor ID
            col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
            // Sensor Type
            col.Add(new JsonStringValue("type", dto.Type));
            // Sensor name 
            col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
            // Sensor generating time
            col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
            // Whether sensor is active
            if (this.TempActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));
            // Select cpu cores 
            if (this.CoreOneCheck.Checked)
                col.Add(new JsonStringValue("core1", "1"));
            else
                col.Add(new JsonStringValue("core1", "0"));
            if (this.CoreTwoCheck.Checked)
                col.Add(new JsonStringValue("core2", "1"));                
            else
                col.Add(new JsonStringValue("core2", "0"));
            if (this.CoreThreeCheck.Checked)
                col.Add(new JsonStringValue("core3", "1"));
            else
                col.Add(new JsonStringValue("core3", "0"));
            if (this.CoreFourCheck.Checked)
                col.Add(new JsonStringValue("core4", "1"));
            else
                col.Add(new JsonStringValue("core4", "0"));
            return col;
        }
        // Get cpu occupied setting by JsonObjectCollection  
        private JsonObjectCollection GetCpuOccupiedConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            DateTime cur = DateTime.Now;
            // Sensor ID
            col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
            // Sensor name 
            col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
            // Sensor Type
            col.Add(new JsonStringValue("type", dto.Type));
            // Sensor generating time
            col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
            // Whether sensor is active
            if (this.CpuCheckBox.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));
            // Select cpu process
            if (this.CpuTotalCheck.Checked)
                col.Add(new JsonStringValue("process", "total"));
            else if (this.CpuCurrentCheck.Checked)
                col.Add(new JsonStringValue("process", "current"));
            return col;
        }
        // Get modbus setting by JsonObjectCollection  
        private JsonObjectCollection GetModbusConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            DateTime cur = DateTime.Now;
            // Sensor ID
            col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
            // Sensor name 
            col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
            // Sensor Type
            col.Add(new JsonStringValue("type", dto.Type));
            // Sensor generating time
            col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
            // Whether sensor is active
            if (this.ModbusActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));
            // Modbus informations
            col.Add(new JsonStringValue("ip", this.IpTextBox.Text));
            col.Add(new JsonStringValue("port", this.PortTextBox.Text));
            col.Add(new JsonStringValue("address", this.AddressTextBox.Text));
            col.Add(new JsonStringValue("size", this.SizeTextBox.Text));
            return col;
        }
        // Get memory usage setting by JsonObjectCollection  
        private JsonObjectCollection GetMemoryUsageConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            DateTime cur = DateTime.Now;
            // Sensor ID
            col.Add(new JsonStringValue("id", cur.ToString("yyyy.MM.dd:HH:mm:ss:") + this.SensorNameText.Text.ToString()));
            // Sensor name 
            col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
            // Sensor Type
            col.Add(new JsonStringValue("type", dto.Type));
            // Sensor generating time
            col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
            if (this.MemoryActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));

            return col;
        }

    }
}
