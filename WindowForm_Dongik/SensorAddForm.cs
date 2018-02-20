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
        private SensorDAO dao = SensorDAO.Instance;
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
                case "temperature":  dao.AddSensorByJson(GetTemperatureConfig()); break;
                case "cpu occupied": dao.AddSensorByJson(GetCpuOccupiedConfig()); break;
                case "memory usage": dao.AddSensorByJson(GetMemoryUsageConfig()); break;
                case "modbus":       dao.AddSensorByJson(GetModbusConfig());      break;
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
            col.Add(new JsonStringValue("type", this.SensorComboBox.SelectedItem.ToString()));
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
            // Sensor Type
            col.Add(new JsonStringValue("type", this.SensorComboBox.SelectedItem.ToString()));
            // Sensor name 
            col.Add(new JsonStringValue("name", this.SensorNameText.Text.ToString()));
            // Sensor generating time
            col.Add(new JsonStringValue("date", cur.ToString("yyyy.MM.dd:HH:mm:ss")));
            // Whether sensor is active
            if (this.CpuCheckBox.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));
            // Select cpu process
            if (this.CpuTotalCheck.Checked)
                col.Add(new JsonStringValue("total", "1"));
            else
                col.Add(new JsonStringValue("total", "0"));
            if (this.CpuCurrentCheck.Checked)
                col.Add(new JsonStringValue("current", "1"));
            else 
                col.Add(new JsonStringValue("current", "0"));
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
            col.Add(new JsonStringValue("type", this.SensorComboBox.SelectedItem.ToString()));
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
            col.Add(new JsonStringValue("type", this.SensorComboBox.SelectedItem.ToString()));
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
