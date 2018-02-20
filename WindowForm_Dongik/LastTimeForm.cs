using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace WindowForm_Dongik
{
    public partial class LastTimeForm : Form
    {
        // LastTimeData
        public Dictionary<string, Dictionary<string, List<BaseDTO>>> Dictionary { get; set; }

        // RealTime DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();

        // 실시간 데이터를 관리할 dao 객체 ? 
        private BaseDAO dao = new BaseDAO();

        // Sensor DAO
        private SensorDAO sensorDAO = SensorDAO.Instance;

        // Sensor List
        private Dictionary<string, SensorDTO> sensors = null;

        private string[] checkeredList = null;

        double chartMaxTime = 0;
        double chartMinTime = 0;

        public LastTimeForm()
        {
            InitializeComponent();
            if (this.Dictionary == null)
                this.Dictionary = new Dictionary<string, Dictionary<string, List<BaseDTO>>>();
        }
        // Get last data
        private void GetLastData(string[] sensors)
        {
            foreach (string sensor in sensors)
            {
                Dictionary<string,List<BaseDTO>> list = dao.LoadDataByTime(sensor,
                                                                 startTimePicker.Value.ToString("yyyy.MM.dd:HH:mm:ss"),
                                                                 lastTimePicker.Value.ToString("yyyy.MM.dd:HH:mm:ss"));
                foreach (KeyValuePair<string, List<BaseDTO>> l in list)
                {
                    if (!this.Dictionary.ContainsKey(sensor))
                        this.Dictionary.Add(sensor, new Dictionary<string, List<BaseDTO>>());
                    this.Dictionary[sensor].Add(l.Key,l.Value);
                }
            }
        }
        // Update chart series
        private void UpdateChartSeries()
        {
            foreach (KeyValuePair<string, Dictionary<string, List<BaseDTO>>> dic in this.Dictionary)
            {
                foreach (KeyValuePair<string, List<BaseDTO>> dic2 in dic.Value)
                {
                    string seriesName = dic.Key + ":" + dic2.Key;
                    if (chart1.Series.IndexOf(seriesName)!=-1)
                    {
                        foreach (BaseDTO item in dic2.Value)
                        {
                            item.SensorName = dic.Key;
                            AddNewPoint(item, chart1.Series[seriesName]);
                            UpdateDataGridView(item);
                        }
                    }
                }
            }
            chartMinTime = chart1.ChartAreas[0].AxisX.Minimum;
            chartMaxTime = chart1.ChartAreas[0].AxisX.Maximum;
        }
        // 사용자가 원하는 센서 정보 목록을 입력하고 조회 버튼 클릭하였을때
        private void startBtn_Click(object sender, EventArgs e)
        {
            this.Dictionary.Clear();

            checkeredList = GetCheckedItems();

            // Load checked list
            LoadSeries(checkeredList);
            // Update Data tab
            LoadDataTab(checkeredList);
            // Get last data
            GetLastData(checkeredList);
            // Update chart series
            UpdateChartSeries();
        }

        private void LoadChart()
        {
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //this.chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            //this.chart1.ChartAreas[0].AxisX.Interval = 1;
            this.chart1.ChartAreas[0].AxisX.LineColor = Color.Transparent;

            this.chart1.ChartAreas[0].AxisY.Maximum = 100;
        }
        private void LoadSeries(string[] list)
        {
            // Reset number of series in the chart.
            chart1.Series.Clear();
            foreach (string s in list)
            {
                //Series newSeries = new Series();
                //newSeries.Name = s.Value.Name;
                if (chart1.Series.IndexOf(s) != -1)
                    continue;
                switch(sensors[s].Type){
                    case  "temperature":
                    case "cpu occupied":
                    case "memory usage":
                        foreach (KeyValuePair<string, string> s2 in sensors[s].ExtraInfo)
                        {
                            if (s2.Value.Contains("1"))
                            {
                                Series series = new Series(s + ":" + s2.Key);
                                series.ChartType = SeriesChartType.Line;
                                series.BorderWidth = 2;
                                //series.Color = Color.Green;
                                series.XValueType = ChartValueType.DateTime;
                                chart1.Series.Add(series);
                            }
                        } 
                        break;
                    case "modbus":
                        int address = Convert.ToInt32(sensors[s].ExtraInfo["address"]);
                        int size = Convert.ToInt32(sensors[s].ExtraInfo["size"]);
                        //int loop = Math.Abs(address - size);
                        for (int i = 0; i < size; ++i)
                        {
                            Series series = new Series(s + ":" + (address+i));
                            series.ChartType = SeriesChartType.Line;
                            series.BorderWidth = 2;
                            //series.Color = Color.Yellow;
                            series.XValueType = ChartValueType.DateTime;
                            chart1.Series.Add(series);
                        }
                        break;
                }
            }
        }
        private void LoadDataTab(string[] list)
        {
            SensorDataTab.TabPages.Clear();
            gridViewList.Clear();
            foreach (string sensor in list)
            {
                DataGridView dataGridView = new DataGridView()
                {
                    Name = sensor + "_GridView",
                    Dock = DockStyle.Fill
                };
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.ColumnCount = 3;
                dataGridView.Columns[0].Name = "Name";
                dataGridView.Columns[1].Name = "Time";
                dataGridView.Columns[2].Name = "Data";
                dataGridView.AllowUserToAddRows = false;
                gridViewList.Add(sensor, dataGridView);

                TabPage tabPage = new TabPage(sensor);
                tabPage.Controls.Add(dataGridView);
                SensorDataTab.TabPages.Add(tabPage);
            }
        }
        // Sensor datas load
        private void SensorLoad()
        {
            sensors = sensorDAO.LoadConfigByJson();
        }
        // Get currently checked items list
        private string[] GetCheckedItems()
        {
            string[] items = new string[this.checkedListBox1.CheckedItems.Count];

            for (int i = 0; i <= this.checkedListBox1.CheckedItems.Count - 1; ++i)
               items[i] = this.checkedListBox1.GetItemText(this.checkedListBox1.CheckedItems[i]);
           return items;
        }
        // CheckedListBox load
        private void LoadCheckedListBox()
        {
            List<SensorClass> lst = new List<SensorClass>();

            // dao에 추가되어 있는 key의 센서 값들 불러와 추가
            foreach (string dic in sensors.Keys)
                lst.Add(new SensorClass(dic, true));

            ((ListBox)this.checkedListBox1).DataSource = lst;
            ((ListBox)this.checkedListBox1).DisplayMember = "Name";
            ((ListBox)this.checkedListBox1).ValueMember = "IsChecked";


            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                SensorClass obj = (SensorClass)checkedListBox1.Items[i];
                checkedListBox1.SetItemChecked(i, obj.IsChecked);
            }
        }
        // CheckedListBox item class
        class SensorClass
        {
            public bool IsChecked { get; set; }
            public string Name { get; set; }
            public SensorClass()
            {
                this.IsChecked = true;
                Name = "name";
            }
            public SensorClass(string Name, bool IsChecked)
            {
                this.Name = Name;
                this.IsChecked = IsChecked;
            }
        }
        /// The AddNewPoint function is called for each series in the chart when
        /// new points need to be added.  The new point will be placed at specified
        /// X axis (Date/Time) position with a Y value in a range +/- 1 from the previous
        /// data point's Y value, and not smaller than zero.
        public void AddNewPoint(BaseDTO dto, Series ptSeries)
        {
            char[] seps = { ':' };
            String[] values = ptSeries.Name.Split(seps);
            
            // Add new data to DataGridView
           // UpdateDataGridView(data[values[1]]);

            // Add new data point to its series.
            ptSeries.Points.AddXY(dto.Time.ToOADate(), dto.Data);

        }
        private void UpdateDataGridView(BaseDTO data)
        {
            if (gridViewList.ContainsKey(data.SensorName))
            {
                gridViewList[data.SensorName].Rows.Insert(0, new string[]{
                data.SensorCategory,
                data.Time.ToString("MM월 dd일 HH:mm:ss"),
                data.Data.ToString()});
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LastTimeForm_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            SensorLoad();
            LoadCheckedListBox();
            LoadChart();
            LoadDataTab(GetCheckedItems());
            // create a line chart series
            LoadSeries(GetCheckedItems());
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainForm frm = new MainForm();
            frm.Visible = true;
        }

    }
}
