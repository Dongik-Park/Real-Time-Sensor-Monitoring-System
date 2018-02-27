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
        // Sensor Database manager
        // 얘를 싱글톤으로 쓰지 않는 이유는 Linq2sql에서 DB connection을 유동적으로 관리하기 때문에 괜찮다.
        // Linq2sql에서 singleton을 사용하지 말라고 권고한다.
        private SensorDBManager dbManager = new SensorDBManager();

        // Real Time DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();

        // Real Time Tabpage
        private Dictionary<string, TabPage> tabList = new Dictionary<string, TabPage>();
        
        // Save start time
        private DateTime strTime = DateTime.Now;

        // Save finish time
        private DateTime fnsTime = DateTime.Now;

        // User checked list
        private string[] checkeredList = null;

        // Real time data manage object
        //private RealTimeDataManager sensorReaders = new RealTimeDataManager();
        private Dictionary<string, BaseSensorReader> sensorReaders = new Dictionary<string, BaseSensorReader>();

        public RealTimeForm()
        {
            InitializeComponent();
        }

        // component 구성
        private void RealTimeForm_Load(object sender, EventArgs e)
        {
            stopBtn.Enabled = false;
            // Load Sensor data readers
            LoadSensorDataReaders();
            // Load ChartConfig
            LoadChart();
            // Load CheckedListBox
            LoadCheckedListBox();
            // Get currently checked items
            checkeredList = GetCheckedItems();
            // Load Tabpages
            LoadTabPage(checkeredList);
            // Load DataGridView
            LoadGridView(checkeredList);
            // Update Data tab
            LoadDataTab(checkeredList);
            // Load Checked series
            LoadSeriesByChecked(checkeredList);
            // Print current time
            TimerOperating();
        }
        // Load sensor data manager
        private void LoadSensorDataReaders()
        {
            var sensorList = dbManager.dc.BaseSensorConfigs.Select(t => t);
            foreach (var sensor in sensorList)
            {
                if (!sensor.IsActive)
                    continue;
                BaseSensorReader reader = SensorReaderFactory.GetManager(sensor);
                if (reader == null)
                    continue;
                //string[] tokens = reader.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //string fullName = reader.config.Name + "_" + tokens[tokens.Length - 1];
                string fullName = reader.Name + "_" + reader.ToString();
                this.sensorReaders.Add(fullName, reader);
            }
        }
        // Load chart config
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
        // CheckedListBox load
        private void LoadCheckedListBox()
        {
            List<SensorClass> lst = new List<SensorClass>();
            // Get sensors from Database 
            foreach (var sensor in sensorReaders)
            {
                lst.Add(new SensorClass(sensor.Key, true));
            }
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
                Name = "Input sensor name";
            }
            public SensorClass(string Name, bool IsChecked)
            {
                this.Name = Name;
                this.IsChecked = IsChecked;
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
        // Make tabpages by checkered list
        private void LoadTabPage(string[] list)
        {
            foreach (string sensor in list)
            {
                if (!tabList.ContainsKey(sensor))
                {
                    TabPage newPage = new TabPage(sensor);
                    newPage.Name = sensor;
                    tabList.Add(sensor, newPage);
                }
            }
        }
        // Load dataGridView
        private void LoadGridView(string[] list)
        {
            foreach (string sensor in list)
            {
                if (!gridViewList.ContainsKey(sensor))
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
                }
            }
        }
        // Make a series by name information
        private Series MakeSeries(string sensorName)
        {
            Series series = new Series(sensorName);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.XValueType = ChartValueType.DateTime;
            return series;
        }
        // Make serieses by checkered list
        private void LoadSeriesByChecked(string[] list)
        {
            if (list == null)
                return;
            chart1.Series.Clear();
            foreach (var sensor in list)
            {
                if (sensorReaders.ContainsKey(sensor))
                {
                    Series newSeries = MakeSeries(sensor);
                    chart1.Series.Add(newSeries);
                }
            }
        }
        // Make tabs by chechered list
        private void LoadDataTab(string[] list)
        {
            //SensorDataTab.TabPages.Clear();
            foreach (TabPage tabPage in SensorDataTab.TabPages)
            {
                if (!list.Contains(tabPage.Text))
                    SensorDataTab.TabPages.Remove(tabPage);
            }
            foreach (string sensor in list)
            {
                if (!SensorDataTab.TabPages.ContainsKey(sensor))
                {
                    tabList[sensor].Controls.Add(gridViewList[sensor]);
                    SensorDataTab.TabPages.Add(tabList[sensor]);
                }
            }
        }
        // Start button click event
        private void startBtn_Click(object sender, EventArgs e)
        {
            this.strTime = DateTime.Now;
            // Disable all controls on the form
            this.startBtn.Enabled = false;
            // and only Enable the Stop button
            this.stopBtn.Enabled = true;
            // Reload checkered list
            checkeredList = GetCheckedItems();
            // Reload Series
            LoadSeriesByChecked(GetCheckedItems());
            // Reload data tab
            LoadDataTab(GetCheckedItems());
            // Timer start
            timer1.Start();
        }
        // Stop btn click
        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.fnsTime = DateTime.Now;
            // Disable all controls on the form
            this.startBtn.Enabled = true;
            // and only Enable the Stop button
            this.stopBtn.Enabled = false;
            timer1.Stop();
        }
        // Add real time data to chart
        public void AddDataToChart(string sensorName, SensorData curData)
        {
            lock(this.chart1){
                DateTime timeStamp  = DateTime.Now;
                if (chart1.Series.IndexOf(sensorName) != -1)
                {
                    Series series = chart1.Series[sensorName];
                    AddDataToSeries(series, timeStamp, curData.Time, curData.Data);
                    UpdateDataGridView(sensorName, curData);
                }
            }
        }
        // Add real time sensor data to series
        private void AddDataToSeries(Series series, DateTime removeTime, DateTime time, object data)
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
        // Update real time sensor data grid view
        private void UpdateDataGridView(string sensorName, SensorData data)
        {
            if (gridViewList.ContainsKey(sensorName))
            {
                string[] tokens = sensorName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string extraName = tokens[tokens.Length - 1];
                Action updateAction = () => {
                    gridViewList[sensorName].Rows.Insert(0, new string[]{
                        extraName,
                        data.Time.ToString("MM월 dd일 HH:mm:ss"),
                        data.Data.ToString()
                    });
                    if (gridViewList[sensorName].Rows.Count > 30)
                    {
                        int index = gridViewList[sensorName].Rows.Count - 1;
                        gridViewList[sensorName].Rows.Remove(gridViewList[sensorName].Rows[index]);
                    }
                };
                gridViewList[sensorName].BeginInvoke(updateAction);
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
            // Express time
            this.label2.Text = System.DateTime.Now.ToString("HH:mm:ss");
            if (this.label2.Text.Equals("00:00:00"))
                this.label1.Text = System.DateTime.Now.ToString("yyyy년 MM월 dd일");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var curData = new List<SensorData>();
            try
            {
                // Update chart
                chart1.Series.SuspendUpdates();
                foreach (string sensor in checkeredList)
                {
                    if (chart1.Series.IndexOf(sensor) != -1)
                    {
                        object data = sensorReaders[sensor].ReadSensorData();
                        SensorData sensorData = new SensorData() { 
                            Data = (double)data, 
                            Time = DateTime.Now , 
                            SensorConfigId = sensorReaders[sensor].Id
                        };
                        curData.Add(sensorData);
                        AddDataToChart(sensor, sensorData);
                    }
                }
                chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(12).ToOADate();
                chart1.Series.ResumeUpdates();
            }
            catch (Exception)
            {
                
            }

            // DB 배치처리
            try
            {
                dbManager.dc.SensorDatas.InsertAllOnSubmit(curData);
                dbManager.dc.SubmitChanges();
            }
            catch (Exception)
            {
                
            }
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RealTimeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopBtn_Click(null, null);
        }
    }
}
