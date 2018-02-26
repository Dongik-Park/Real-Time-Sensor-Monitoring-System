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
    public partial class SensorManageForm : Form
    {
        private SensorDBManager dbManager = SensorDBManager.Instance;
        //private Dictionary<string, SensorDTO> data = null;
        public SensorManageForm()
        {
            InitializeComponent();
        }
        private void SetupDataGridView()
        {
            sensorConfigItemBindingSource.SuspendBinding();

            var list = new List<SensorConfigItem>();

            foreach (var sensor in dbManager.dc.SensorConfigs.Select(x => x))
            {
                switch(sensor.SensorType){
                    case  "temperature" : list.Add(new TempSensorConfigItem(sensor.Id)); break;
                    case "cpu occupied" : list.Add(new CpuSensorConfigItem(sensor.Id));  break;
                    case "memory usage" : list.Add(new MemSensorConfigItem(sensor.Id));  break;
                    case      "modbus"  : list.Add(new ModbusConfigITem(sensor.Id));     break;
                }
            }

            //sensorConfigItemBindingSource.DataSource = dbManager.dc.SensorConfigs
            //                                                //.Select(x => x)
            //                                                .ToList();
            sensorConfigItemBindingSource.DataSource = list;
            dataGridView1.DataSource = sensorConfigItemBindingSource;
            dataGridView1.AllowUserToAddRows = false;
            sensorConfigItemBindingSource.ResumeBinding();
            dataGridView1.Refresh();
        }
        // 속성 Button click
        private void SetConfigBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
            string sensorName = selectedRow.Cells[1].Value.ToString();

            int selectedCellCount = dataGridView1.SelectedCells.Count;
            if (selectedCellCount < 2)
            {
                new SensorConfigEditForm(dbManager.dc.SensorConfigs
                                                   .Where(t=>t.Name==sensorName)
                                                   .First()).Show(this);
            }
            else
                MessageBox.Show("Select just one row.");
        }
        private void SensorManageForm_ver2_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddSensorBtn_Click(object sender, EventArgs e)
        {
            new SensorAddForm().Show(this);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            if (dataGridView1.SelectedCells.Count == 1)
            {
                //SensorConfig config = (SensorConfig)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem;
                SensorConfigItem config = (SensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem);
                //var key = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                //SensorConfig config = dbManager.dc.SensorConfigs.Where(t => t.Name == key).First();
                propertyGrid1.SelectedObject = config;
                //switch (config.SensorType)
                //{
                //    case "temperature": propertyGrid1.SelectedObject  = new TempSensorConfigItem(config.GetId()); break;
                //    case "cpu occupied": propertyGrid1.SelectedObject = new CpuSensorConfigItem(config.GetId()); break;
                //    case "memory usage": propertyGrid1.SelectedObject = new MemSensorConfigItem(config.GetId()); break;
                //    case "modbus": propertyGrid1.SelectedObject = new ModbusConfigITem(config.GetId()); break;
                //}
            }
            else
            {
                var configs = new List<SensorConfigItem>();
                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                {
                    SensorConfigItem config = (SensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[i].RowIndex].DataBoundItem);
                    configs.Add(config);
                    //switch (config.SensorType)
                    //{
                    //    case "temperature" : configs.Add(new TempSensorConfigItem(config.Id)); break;
                    //    case "cpu occupied": configs.Add(new CpuSensorConfigItem(config.Id)); break;
                    //    case "memory usage": configs.Add(new MemSensorConfigItem(config.Id)); break;
                    //    case "modbus"      : configs.Add(new ModbusConfigITem(config.Id)); break;
                    //}
                }
                //var key = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                //SensorConfig config = dbManager.dc.SensorConfigs.Where(t => t.Name == key).First();
                //propertyGrid1.SelectedObject = dataGridView1.
                propertyGrid1.SelectedObjects = configs.ToArray();
                
            }
        }
        //List<SensorConfigItem> configs = new List<SensorConfigItem>();

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
                return;
            if (e.ColumnIndex == 0)
            {
                var key = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                SensorConfigItem config = (SensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem);
                object val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if ((byte)val == 1)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    config.Active = false;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    config.Active = true;
                }
                dbManager.dc.SubmitChanges();
                //SetupDataGridView();
            }
        }
    }
    //[TypeConverter(typeof(StreetAddressConverter))]
    public class SensorConfigItem
    {
        private SensorConfig config;
        public SensorConfigItem(int sensorId)
        {
            this.config = SensorDBManager.Instance.dc.SensorConfigs
                                                     .Where(t => t.Id == sensorId)
                                                     .First();
        }
        
        public int GetId() { return config.Id; }
        [Category("Base")]
        public bool Active { get { return Convert.ToBoolean(config.IsActive); } set { config.IsActive = Convert.ToByte(value); } }
        [Category("Base")]
        public string Name { get { return config.Name; } set { config.Name = value; } }
        [Category("Base")]
        public DateTime Time { get { return config.MadeTime; } set { config.MadeTime = value; } }
        [Category("Base")]
        public string SensorType { get { return config.SensorType; } set { config.SensorType = value; } }

        // Return as a comma-delimited string.
        public override string ToString()
        {
            return Active + "," + Name + "," + Time + "," + SensorType;
        }
    }
    class StreetAddressConverter : TypeConverter
    {
        // Convert the StreetAddress to a string.
        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(string)) return value.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
        // Convert from a string.
        public override object ConvertFrom(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value)
        {
            if (value.GetType() == typeof(string))
            {
                // Split the string separated by commas.
                string txt = (string)(value);
                string[] fields = txt.Split(new char[] { ',' });

                try
                {
                    return new SensorConfigItem(Convert.ToInt32(fields[0]))
                    {
                        Active = Convert.ToBoolean(fields[1]),
                        Name = fields[2],
                        Time = Convert.ToDateTime(fields[3]),
                        SensorType = fields[4]
                    };
                }
                catch
                {
                    throw new InvalidCastException(
                        "Cannot convert the string '" +
                        value.ToString() + "' into a StreetAddress");
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }





    public class TempSensorConfigItem : SensorConfigItem
    {
        private TempertaureSensor tempSensor;
        public TempSensorConfigItem(int sensorId) : base(sensorId)
        {
            this.tempSensor = SensorDBManager.Instance.dc.TempertaureSensors
                                                     .Where(t => t.SensorConfigId == sensorId)
                                                     .First();
        }
        [Category("Extra")]
        public byte CoreIndex { get { return tempSensor.CoreIndex; } set { tempSensor.CoreIndex = value; } }
    }

    public class MemSensorConfigItem : SensorConfigItem
    {
        private MemoryUsageSensor memSensor;
        public MemSensorConfigItem(int sensorId) : base(sensorId)
        {
            this.memSensor = SensorDBManager.Instance.dc.MemoryUsageSensors
                                                        .Where(t => t.SensorConfigId == sensorId)
                                                        .First();
        }
    }

    public class CpuSensorConfigItem : SensorConfigItem
    {
        private CpuOccupiedSensor cpuSensor;
        public CpuSensorConfigItem(int sensorId) : base(sensorId)
        {
            this.cpuSensor = SensorDBManager.Instance.dc.CpuOccupiedSensors
                                                        .Where(t => t.SensorConfigId == sensorId)
                                                        .First();
        }
        [Category("Extra")]
        public byte Process { get { return cpuSensor.ProcessType; } set { cpuSensor.ProcessType = value; } }
    }
    public class ModbusConfigITem : SensorConfigItem
    {
        private ModbusSensor modSensor;
        public ModbusConfigITem(int sensorId)
            : base(sensorId)
        {
            this.modSensor = SensorDBManager.Instance.dc.ModbusSensors
                                                        .Where(t => t.SensorConfigId == sensorId)
                                                        .First();
        }
        [Category("Extra")]
        public ushort Address { get { return modSensor.Address; } set { modSensor.Address = value; } }
        [Category("Extra")]
        public int Port { get { return modSensor.Port; } set { modSensor.Port = value; } }
        [Category("Extra")]
        public string Ip { get { return modSensor.Ip; } set { modSensor.Ip = value; } }
    }
}
