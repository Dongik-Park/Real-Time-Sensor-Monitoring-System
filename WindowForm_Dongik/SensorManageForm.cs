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
        private SensorDBManager dbManager = new SensorDBManager();
        public SensorManageForm()
        {
            InitializeComponent();
        }
        private void SetupDataGridView()
        {
            sensorConfigItemBindingSource.SuspendBinding();

            var list = new List<BaseSensorConfigItem>();

            foreach (var sensor in dbManager.dc.BaseSensorConfigs.Select(x => x))
            {
                //list.Add(sensor);
                switch(sensor.SensorType){
                    case SensorType.TEMPERATURE  : list.Add(new TempSensorConfigItem((TempertaureSensorConfig)sensor)); break;
                    case SensorType.CPU_OCCUPIED : list.Add(new CpuSensorConfigItem((CpuSensorConfig)sensor));          break;
                    case SensorType.MEMORY_USAGE : list.Add(new MemSensorConfigItem((MemorySensorConfig)sensor));       break;
                    case       SensorType.MODBUS : list.Add(new ModbusConfigItem((ModbusSensorConfig)sensor));          break;
                }
            }
            sensorConfigItemBindingSource.DataSource = list;
            dataGridView1.DataSource = sensorConfigItemBindingSource;
            dataGridView1.AllowUserToAddRows = false;
            sensorConfigItemBindingSource.ResumeBinding();
            dataGridView1.Refresh();
        }
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
                new SensorConfigEditForm(dbManager.dc.BaseSensorConfigs
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
            DialogResult result = new SensorAddForm().ShowDialog(this);

            if (result == DialogResult.Cancel)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = sensorConfigItemBindingSource;
                dataGridView1.Refresh();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            if (dataGridView1.SelectedCells.Count == 1)
            {
                BaseSensorConfigItem config = (BaseSensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem);
                propertyGrid1.SelectedObject = config;
            }
            else
            {
                var configs = new List<BaseSensorConfigItem>();
                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                {
                    BaseSensorConfigItem config = (BaseSensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[i].RowIndex].DataBoundItem);
                    configs.Add(config);
                }
                propertyGrid1.SelectedObjects = configs.ToArray();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
                return;
            if (e.ColumnIndex == 0)
            {
                object val = 0;
                BaseSensorConfigItem config = null;
                try
                {
                    var key = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    config = (BaseSensorConfigItem)(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem);
                    val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                }
                catch (Exception)
                {
                }

                try
                {
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
                }
                catch (Exception)
                {
                }
            }
            
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            {
                MessageBox.Show("1개의 행을 선택하세요.");
                return;
            }
            int delIndex = dataGridView1.SelectedCells[0].RowIndex;
            BaseSensorConfigItem delConfig = null;
            try
            {
                delConfig = (BaseSensorConfigItem)(dataGridView1.Rows[delIndex].DataBoundItem);
            }
            catch (Exception)
            {
            }

            DialogResult result = MessageBox.Show(delConfig.Name + "을 정말로 삭제하시겠습니까?","",MessageBoxButtons.YesNo);
            try
            {
                if (result == DialogResult.Yes)
                {
                    var delDTO = dbManager.dc.BaseSensorConfigs
                                    .Where(t => t.Id == delConfig.GetId())
                                    .Select(t => t);
                    foreach(var dto in delDTO)
                        dbManager.dc.BaseSensorConfigs.DeleteOnSubmit(dto);
                }
            }
            catch (Exception)
            {
            }
            dbManager.dc.SubmitChanges();
        }
    }

    public class BaseSensorConfigItem
    {
        private BaseSensorConfig config;
        public BaseSensorConfigItem(BaseSensorConfig config)
        {
            this.config = config;
        }

        public int GetId() { return config.Id; }
        [Category("Base")]
        public bool Active { get { return Convert.ToBoolean(config.IsActive); } set { config.IsActive = value; } }
        [Category("Base")]
        public string Name { get { return config.Name; } set { config.Name = value; } }
        [Category("Base")]
        public DateTime Time { get { return config.MadeTime; } set { config.MadeTime = value; } }
        [Category("Base")]
        public SensorType SensorType { get { return config.SensorType; } set { config.SensorType = value; } }
    }

    public class TempSensorConfigItem : BaseSensorConfigItem
    {
        private TempertaureSensorConfig tempSensor;
        public TempSensorConfigItem(TempertaureSensorConfig tempSensor)
            : base(tempSensor)
        {
            this.tempSensor = tempSensor;
        }
        [Category("Extra")]
        public CoreNumber CoreIndex { get { return tempSensor.CoreIndex; } set { tempSensor.CoreIndex = value; } }
    }

    public class MemSensorConfigItem : BaseSensorConfigItem
    {
        private MemorySensorConfig memSensor;
        public MemSensorConfigItem(MemorySensorConfig memSensor)
            : base(memSensor)
        {
            this.memSensor = memSensor;
        }
    }

    public class CpuSensorConfigItem : BaseSensorConfigItem
    {
        private CpuSensorConfig cpuSensor;
        public CpuSensorConfigItem(CpuSensorConfig cpuSensor)
            : base(cpuSensor)
        {
            this.cpuSensor = cpuSensor;
        }
        [Category("Extra")]
        public ProcessType Process { get { return cpuSensor.ProcessType; } set { cpuSensor.ProcessType = value; } }
    }
    public class ModbusConfigItem : BaseSensorConfigItem
    {
        private ModbusSensorConfig modSensor;
        public ModbusConfigItem(ModbusSensorConfig modSensor)
            : base(modSensor)
        {
            this.modSensor = modSensor;
        }
        [Category("Extra")]
        public ushort Address { get { return modSensor.Address; } set { modSensor.Address = value; } }
        [Category("Extra")]
        public int Port { get { return modSensor.Port; } set { modSensor.Port = value; } }
        [Category("Extra")]
        public string Ip { get { return modSensor.Ip; } set { modSensor.Ip = value; } }
    }
}
