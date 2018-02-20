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

        private SensorDAO dao = SensorDAO.Instance;

        public SensorManageForm()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SetupDataGridView()
        {
            this.dataGridView1.ColumnCount = 3;
            this.dataGridView2.ColumnCount = 3;

            this.dataGridView1.Columns[0].Name = "No.";
            this.dataGridView1.Columns[1].Name = "Name.";
            this.dataGridView1.Columns[2].Name = "Date";
            this.dataGridView2.Columns[0].Name = "No.";
            this.dataGridView2.Columns[1].Name = "Name.";
            this.dataGridView2.Columns[2].Name = "Date";
            //this.dataGridView1.Columns[3].Name = "State";

            DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
            dgvButton.FlatStyle = FlatStyle.Flat;
            dgvButton.HeaderText = "State";
            dgvButton.Name = "StateBtn";
            dgvButton.UseColumnTextForButtonValue = true;
            dgvButton.Text = "활성화";

            this.dataGridView1.Columns.Add(dgvButton);

        }
        private void TotSensorDataGridView()
        {
            var list = from sensors in dao.LoadTotalSensors()
                       orderby sensors.Value.AddDate.TimeOfDay
                       select sensors;
            int i = 0;
            foreach (KeyValuePair<string, SensorDTO> s in list)
                this.dataGridView2.Rows.Add(new string[] { (++i).ToString(), s.Key, s.Value.AddDate.ToString("yyyy.MM.dd:HH시") });
        }

        private void SensorDataGridView()
        {
            var list = from sensors in dao.LoadConfigByJson()
                       orderby sensors.Value.AddDate.TimeOfDay
                       select sensors;
            int i = 0;
            foreach (KeyValuePair<string, SensorDTO> s in list)
            {
                if (s.Value.AddDate.ToString("yyyy.MM.dd:HH시").Length > 0)
                  this.dataGridView1.Rows.Add(new string[] { (++i).ToString(), s.Key, s.Value.AddDate.ToString("yyyy.MM.dd:HH시") });
            }
        }
        private void SensorManageForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            //데이터 로딩
            SensorDataGridView();
            //전체 센서 목록 불러오기
            TotSensorDataGridView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns.Contains("button") && e.ColumnIndex == dataGridView1.Columns["button"].Index)//Specify which column contains Button in DGV
            {
                MessageBox.Show("Row " + (e.RowIndex + 1) + " Of " + (e.ColumnIndex + 1) + " th Column button clicked ");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // 추가 버튼 클릭
        private void AddBtn_Click(object sender, EventArgs e)
        {
            //int index = this.dataGridView2.CurrentCell.RowIndex;
            //string name = this.dataGridView2.Rows[index].Cells[1].FormattedValue.ToString();
            //this.dao.AddSensor(name);
            //this.dataGridView1.Rows.Add(new string[]{
            //    (this.dataGridView1.Rows.Count + 1)+"" ,
            //    name,
            //    this.dao.Sensors[name].AddDate.ToString("yyyy.MM.dd:HH시")
            //});
            //MessageBox.Show(name+"이 추가되었습니다.");

            SetConfigForm frm = new SetConfigForm();
            frm.Owner = this;
            frm.Show(); 

        }
        // 삭제 버튼 클릭
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int index = this.dataGridView1.CurrentCell.RowIndex;
            string name = this.dataGridView1.Rows[index].Cells[1].FormattedValue.ToString();
            DialogResult result = MessageBox.Show(name+"을 정말로 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                this.dao.Sensors.Remove(name);
                this.dataGridView1.Rows.RemoveAt(index);
            }
            else if (result == DialogResult.No)
            {
                this.DialogResult = DialogResult.Abort;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("센서 정보가 저장됩니다.", "YesOrNo",MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                //this.dao.SaveSensors();
            }
            else if (result == DialogResult.No)
            {
                this.DialogResult = DialogResult.Abort;
            }
        }
    }
}
