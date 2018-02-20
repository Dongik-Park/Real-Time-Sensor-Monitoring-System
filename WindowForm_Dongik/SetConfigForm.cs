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
    public partial class SetConfigForm : Form
    {
        private SensorDAO dao = SensorDAO.Instance;
        private Dictionary<string, SensorDTO> data = null;
        public SetConfigForm()
        {
            InitializeComponent();
           // this.dao 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // 취소 button click
        private void ConfigCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // 저장 button click
        private void ConfigSaveBtn_Click(object sender, EventArgs e)
        {
            List<JsonObjectCollection> col = new List<JsonObjectCollection>();

            col.Add(GetTemperatureConfig());
            col.Add(GetCpuOccupiedConfig());
            col.Add(GetModbusConfig());
            col.Add(GetMemoryUsageConfig());

            dao.SaveConfigByJson(col.ToArray());
            MessageBox.Show("속성이 저장되었습니다.");
        }
        // Get temperature setting by JsonObjectCollection  
        private JsonObjectCollection GetTemperatureConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            string name = "temperature";
            col.Add(new JsonStringValue("name", name));
            col.Add(new JsonStringValue("date", dao.Sensors[name].AddDate.ToString("yyyy.MM.dd:HH:mm:ss")));
            if (this.TempActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));

            if (this.core1.Checked)
                col.Add(new JsonStringValue("core", "1"));
            else if (this.core2.Checked)
                col.Add(new JsonStringValue("core", "2"));
            else if (this.core3.Checked)
                col.Add(new JsonStringValue("core", "3"));
            else if (this.core4.Checked)
                col.Add(new JsonStringValue("core", "4"));
            return col;
        }
        // Get cpu occupied setting by JsonObjectCollection  
        private JsonObjectCollection GetCpuOccupiedConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            string name = "cpu occupied";
            col.Add(new JsonStringValue("name", name));
            col.Add(new JsonStringValue("date", dao.Sensors[name].AddDate.ToString("yyyy.MM.dd:HH:mm:ss")));
            if (this.CpuActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));

            if (this.cpu_total.Checked)
                col.Add(new JsonStringValue("process", "total"));
            else if (this.cpu_current.Checked)
                col.Add(new JsonStringValue("process", "current"));
            return col;
        }
        // Get modbus setting by JsonObjectCollection  
        private JsonObjectCollection GetModbusConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            string name = "modbus";
            col.Add(new JsonStringValue("name", name));
            col.Add(new JsonStringValue("date", dao.Sensors[name].AddDate.ToString("yyyy.MM.dd:HH:mm:ss")));
            if (this.ModbusActiveCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));
            col.Add(new JsonStringValue("ip",this.IPTextBox.Text));
            col.Add(new JsonStringValue("port", this.PortTextBox.Text));
            col.Add(new JsonStringValue("address", this.AddressTextBox.Text));
            col.Add(new JsonStringValue("size", this.SizeTextBox.Text));
            
            return col;
        }
        // Get memory usage setting by JsonObjectCollection  
        private JsonObjectCollection GetMemoryUsageConfig()
        {
            JsonObjectCollection col = new JsonObjectCollection();
            string name = "memory usage";
            col.Add(new JsonStringValue("name", name));
            col.Add(new JsonStringValue("date", dao.Sensors[name].AddDate.ToString("yyyy.MM.dd:HH:mm:ss")));
            if (this.MemoryUsageCheck.Checked)
                col.Add(new JsonStringValue("active", "1"));
            else
                col.Add(new JsonStringValue("active", "0"));

            return col;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void ModbusGroup_Enter(object sender, EventArgs e)
        {

        }

        private void ModbusActiveCheck_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
