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
    public partial class RealTimeForm : Form
    {
        // RealTime DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();

        // Control Thread
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);

        // 실시간 데이터를 관리할 dao 객체 ? 
        private BaseDAO dao = new BaseDAO();

        // Sensor DAO
        private SensorDAO sensorDAO = SensorDAO.Instance;

        // Sensor List
        private Dictionary<string, SensorDTO> sensors = null;

        // UI에 반영되는 Thread
        private Dictionary<string, Thread> threads = new Dictionary<string,Thread>();

        // UI thread flag
        private bool Flag = true;

        // Save start time
        private DateTime strTime = DateTime.Now;

        // Save finish time
        private DateTime fnsTime = DateTime.Now;

        private string[] checkeredList = null;

        public delegate void AddDataDelegate(string name);
        public AddDataDelegate addDataDel;

        public RealTimeForm()
        {
            InitializeComponent();
        }

        Task dataTask;

        private void RealTimeForm_ResizeEnd(object sender, EventArgs e)
        {

        }
        // component 구성
        private void RealTimeForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            SensorLoad();
            LoadCheckedListBox();
            LoadChart();
            checkeredList = GetCheckedItems();
            LoadSeries(checkeredList);
            LoadDataTab(checkeredList);
            //ThreadLoad();
            TimerOperating();
        }
        private void LoadChart()
        {

            // Predefine the viewing area of the chart
            DateTime minValue = DateTime.Now;
            DateTime maxValue = minValue.AddSeconds(12);

            //CPU ChartArea
            this.chart1.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
            this.chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            this.chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            this.chart1.ChartAreas[0].AxisX.LineColor = Color.Transparent;

            this.chart1.ChartAreas[0].AxisY.Maximum = 100;

            // Reset number of series in the chart.
            chart1.Series.Clear();
        }
        private void LoadSeries(string[] list)
        {
            // 여기서 센서 타입 별로 분기해서 basedao에 getData를 해야한다. 
            // 수정해야할 사항 : 하드코딩 으로 일일이 생성함...
            //                  모드버스 부분에서 모드버스에서 여러개 센서 받아올 경우 어떻게?
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
                        if(sensors[s].ExtraInfo.Count > 0)
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
                        else{
                            Series series = new Series(s);
                            series.ChartType = SeriesChartType.Line;
                            series.BorderWidth = 2;
                            //series.Color = Color.Green;
                            series.XValueType = ChartValueType.DateTime;
                            chart1.Series.Add(series);
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
        bool cancelRequest = false;
        // Load chart info
        private void ThreadLoad()
        {
            while (!cancelRequest)
            {
                Action action = () =>
                {
                    chart1.Series.SuspendUpdates();
                    foreach (string sensor in checkeredList)
                        AddData((string)sensor);
                    chart1.Series.ResumeUpdates();
                };
                chart1.BeginInvoke(action);
                Thread.Sleep(1000);
            }
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
                //if (obj.Name.Contains("temperature"))
                 //   checkedListBox1.SetItemChecked(i, true);
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

        // 사용자가 원하는 센서 정보 목록을 입력하고 조회 버튼 클릭하였을때
        private void startBtn_Click(object sender, EventArgs e)
        {

            this.strTime = DateTime.Now;
            // Disable all controls on the form
            this.startBtn.Enabled = false;
            // and only Enable the Stop button
            this.stopBtn.Enabled = true;

            foreach (string s in GetCheckedItems())
                if (!checkeredList.Contains(s))
                {
                    LoadSeries(GetCheckedItems());
                    LoadDataTab(GetCheckedItems());
                }
            checkeredList = GetCheckedItems();
            // -------------------------Timer code------------------------------
            //timer1.Start();
            // -------------------------Timer code------------------------------

            // -------------------------Thread code------------------------------

            cancelRequest = false;
            dataTask = Task.Factory.StartNew(ThreadLoad);
        }

        // Stop btn click
        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.fnsTime = DateTime.Now;
            // Disable all controls on the form
            this.startBtn.Enabled = true;
            // and only Enable the Stop button
            this.stopBtn.Enabled = false;

            cancelRequest = true;
            dataTask.Wait();
            // -------------------------Timer code------------------------------
            //timer1.Stop();
            // -------------------------Timer code------------------------------
        }


        /// Main loop for the thread that adds data to the chart.
        /// The main purpose of this function is to Invoke AddData
        /// function every 1000ms (1 second).
        public void AddDataThreadLoop(object name)
        {
            while (pauseEvent.WaitOne())
            {
                //chart1.Invoke(addDataDel);
                AddData((string)name);
                Thread.Sleep(1000);
            }
        }

        public void AddData(string sensorName)
        {
            lock(this.chart1){
                DateTime timeStamp  = DateTime.Now;
                AddNewPoint(timeStamp, sensorName);
            }
        }
        private void AddNewPoint(DateTime timeStamp, string sensorName)
        {
            // 1. Get sensor data
            Dictionary<string, BaseDTO> data = dao.GetSensorData(sensors[sensorName]);
            foreach (KeyValuePair<string, BaseDTO> dic in data)
            {
                string seriesName = "";
                if (dic.Key.Equals("NoExtra"))
                    seriesName = sensorName;
                else
                    seriesName = sensorName + ":" + dic.Key;
                if (chart1.Series.IndexOf(seriesName) != -1)
                {
                    AddDataToSeries(chart1.Series[seriesName], timeStamp, data[dic.Key].Time, data[dic.Key].Data);
                    UpdateDataGridView(data[dic.Key]);
                    //DataSaving(data[dic.Key]);
                }
            }

            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(12).ToOADate();
        }

        private void AddDataToSeries(Series series, DateTime removeTime,DateTime time, object data)
        {
            series.Points.AddXY(time.ToOADate(), data);
            
            // remove all points from the source series older than 1.5 minutes.
            double removeBefore = removeTime.AddSeconds((double)(10) * (-1)).ToOADate();

            //remove oldest values to maintain a constant number of data points
            while (series.Points[0].XValue < removeBefore)
            {
                series.Points.RemoveAt(0);
            }
        }
        /// The AddNewPoint function is called for each series in the chart when
        /// new points need to be added.  The new point will be placed at specified
        /// X axis (Date/Time) position with a Y value in a range +/- 1 from the previous
        /// data point's Y value, and not smaller than zero.
        public void AddNewPoint(DateTime timeStamp, Series ptSeries)
        {
            char[] seps = { ':' };
            String[] values = ptSeries.Name.Split(seps);
            // Get data by sensor name, not extra
            Dictionary<string,BaseDTO> data = dao.GetSensorData(sensors[values[0]]);

            // Add new data to file 
            //dao.AddData(ptSeries.Name, dto);
            data[values[1]].SensorName = values[0];
            data[values[1]].SensorCategory = values[1];
            DataSaving(data[values[1]]);

            // Add new data to DataGridView
           // UpdateDataGridView(data[values[1]]);

            // Add new data point to its series.
            foreach (KeyValuePair<string, BaseDTO> pair in data)
            {
                if (ptSeries.Name.Contains(pair.Key))
                    ptSeries.Points.AddXY(pair.Value.Time.ToOADate(), pair.Value.Data);
            }

           // ptSeries.Points.AddXY(dto.Time.ToOADate(), dto.Data);

            // remove all points from the source series older than 1.5 minutes.
            double removeBefore = timeStamp.AddSeconds((double)(10) * (-1)).ToOADate();

            //remove oldest values to maintain a constant number of data points
            while (ptSeries.Points[0].XValue < removeBefore)
            {
                ptSeries.Points.RemoveAt(0);
            }
            chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds(12).ToOADate();
            
            //chart1.ChartAreas["modbus"].AxisX.Minimum = ptSeries.Points[0].XValue;
            //chart1.ChartAreas["modbus"].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds(12).ToOADate();

        }
        private readonly object _syncRoot = new object();
        private void DataSaving(BaseDTO data)
        {
            lock (_syncRoot)
            {
                dao.FileSave(data);
            }
        }

        private void UpdateDataGridView(BaseDTO data)
        {
            if (gridViewList.ContainsKey(data.SensorName))
            {
                Action updateAction = () => {
                gridViewList[data.SensorName].Rows.Insert(0, new string[]{
                data.SensorCategory,
                data.Time.ToString("MM월 dd일 HH:mm:ss"),
                data.Data.ToString()});

                if (gridViewList[data.SensorName].Rows.Count > 30)
                {
                    int index = gridViewList[data.SensorName].Rows.Count - 1;
                    gridViewList[data.SensorName].Rows.Remove(gridViewList[data.SensorName].Rows[index]);
                }
                };
                gridViewList[data.SensorName].BeginInvoke(updateAction);
            }
        }

        //Timer operating code 
        private void TimerOperating()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += new System.EventHandler(time_ticker);
            timer.Start();
        }
        void time_ticker(object sender, System.EventArgs e)
        {
            this.label2.Text = System.DateTime.Now.ToString("HH:mm:ss");
            if (this.label2.Text.Equals("00:00:00"))
                this.label1.Text = System.DateTime.Now.ToString("yyyy년 MM월 dd일");
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            chart1.Series.SuspendUpdates();
            foreach(string sensor in checkeredList)
                AddData((string)sensor);
            chart1.Series.ResumeUpdates();
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainForm frm = new MainForm();
            frm.Visible = true;
        }

        private void RealTimeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopBtn_Click(null, null);
        }
    }
}
