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
    public partial class SensorManageForm_ver2 : Form
    {
        private SensorDAO dao = SensorDAO.Instance;
        private Dictionary<string, SensorDTO> data = null;
        public SensorManageForm_ver2()
        {
            InitializeComponent();
        }
        private void SetupDataGridView()
        {
            this.dataGridView1.ColumnCount = 5;

            this.dataGridView1.Columns[0].Name = "No.";
            this.dataGridView1.Columns[1].Name = "Name";
            this.dataGridView1.Columns[2].Name = "Type";
            this.dataGridView1.Columns[3].Name = "Date";
            this.dataGridView1.Columns[4].Name = "State";
            this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        // 속성 Button click
        private void SetConfigBtn_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
            string sensorName = selectedRow.Cells["Name"].Value.ToString();

            int selectedCellCount = dataGridView1.SelectedCells.Count;
            if (selectedCellCount < 2)
            {
                SensorConfigForm_ver3 frm = new SensorConfigForm_ver3(data[sensorName]);
                frm.Owner = this;
                frm.Show(); 
            }
            else
                MessageBox.Show("Select just one row.");
        }

        private void SensorDataGridView()
        {
            data = dao.LoadConfigByJson();

            int i = 0;
            foreach (KeyValuePair<string, SensorDTO> dic in data)
            {
                // Build up each line one-by-one and then trim the end
                StringBuilder builder = new StringBuilder();
                if (dic.Value.IsActive == 1)
                    builder.Append("Active (");
                else
                    builder.Append("Inactive (");
                foreach (KeyValuePair<string, string> pair in dic.Value.ExtraInfo)
                {
                    if(!pair.Key.Contains("id"))
                    builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
                }
                string result = builder.ToString();
                // Remove the final delimiter
                result = result.TrimEnd(',') + ")";
                this.dataGridView1.Rows.Add(new string[] { 
                    (++i).ToString(), 
                    dic.Key, 
                    dic.Value.Type,
                    dic.Value.AddDate.ToString("yyyy.MM.dd:HH시"),
                    result
                });
            }
        }
        private void SensorManageForm_ver2_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            SensorDataGridView();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            MainForm frm = new MainForm();
            frm.Visible = true;
        }

        private void AddSensorBtn_Click(object sender, EventArgs e)
        {
            SensorAddForm frm = new SensorAddForm();
            frm.Owner = this;
            frm.Show();
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count <= 0)
            //    return;
            //var selectedKey = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString() ;
            //var dto = data[selectedKey];

            //propertyGrid1.SelectedObject = dto;
        }
    }
}
