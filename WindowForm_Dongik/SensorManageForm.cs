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
            sensorConfigBindingSource.SuspendBinding();
            sensorConfigBindingSource.DataSource = dbManager.dc.SensorConfigs
                                                            .Select(x => x)
                                                            .ToList();
            dataGridView1.DataSource = sensorConfigBindingSource;
            dataGridView1.AllowUserToAddRows = false;
            sensorConfigBindingSource.ResumeBinding();
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
                SensorConfig config = (SensorConfig)(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem);
                //var key = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                //SensorConfig config = dbManager.dc.SensorConfigs.Where(t => t.Name == key).First();
                propertyGrid1.SelectedObject = config;
                switch (config.SensorType)
                {
                    case "temperature": propertyGrid1.SelectedObject = config.TempertaureTables; break;
                    case "cpu occupied": propertyGrid1.SelectedObject = config.CpuOccupiedTables; break;
                    case "memory usage": propertyGrid1.SelectedObject = config.MemoryUsageTables; break;
                    case "modbus": propertyGrid1.SelectedObject = config.ModbusTables; break;
                }
            }
            else
            {
                var configs = new List<SensorConfig>();
                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                    configs.Add((SensorConfig)(dataGridView1.Rows[dataGridView1.SelectedCells[i].RowIndex].DataBoundItem));
                //var key = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                //SensorConfig config = dbManager.dc.SensorConfigs.Where(t => t.Name == key).First();
                propertyGrid1.SelectedObjects = configs.ToArray();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var key = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                SensorConfig config = dbManager.dc.SensorConfigs.Where(t => t.Name == key).First();
                object val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if ((byte)val == 1)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    config.IsActive = 0;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    config.IsActive = 1;
                }
                dbManager.dc.SubmitChanges();
                //SetupDataGridView();
            }
        }
        List<SensorConfigItem> items;
    }

    public class SensorConfigItem
    {
        SensorConfig config;
        public string Name { get { return config.Name; } set { config.Name = value; } }
    }

    public class TempSensorConfigItem : SensorConfigItem
    {
        TempertaureSensor myConfig;
    }
}
