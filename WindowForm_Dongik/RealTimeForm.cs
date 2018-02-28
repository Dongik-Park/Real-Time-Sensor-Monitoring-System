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
        private SensorDBManager dbManager = new SensorDBManager();

        // Real Time DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();

        // Real Time Tabpage
        private Dictionary<string, TabPage> tabList = new Dictionary<string, TabPage>();
        
        // Save start time
        private DateTime strTime = DateTime.Now;

        // Save finish time
        private DateTime fnsTime = DateTime.Now;

        // Real time data readers
        private BaseSensorReader[] dataReaders = null;

        // Sensor configs
        private BaseSensorConfig[] configs = null;

        // Sensor names for UI ( series, checkedList )
        private string[] sensorNames = null;

        // User checked list
        private string[] checkeredList = null;

        public RealTimeForm()
        {
            InitializeComponent();
        }

        // Component load template
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
            var tempDataReaders = new List<BaseSensorReader>();
            var tempConfigList = new List<BaseSensorConfig>();
            var tempSensorNames = new List<string>();
            foreach (var sensor in sensorList)
            {
                if (!sensor.IsActive)
                    continue;
                BaseSensorReader reader = SensorReaderFactory.CreateReader(sensor);
                if (reader == null)
                    continue;
                tempDataReaders.Add(reader);
                tempConfigList.Add(sensor);
                tempSensorNames.Add(sensor.Name + "_" + reader.ToString());
            }
            dataReaders = tempDataReaders.ToArray();
            configs     = tempConfigList.ToArray();
            sensorNames = tempSensorNames.ToArray();
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
            foreach (var sensor in sensorNames)
            {
                lst.Add(new SensorClass(sensor, true));
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
                if (sensorNames.Contains(sensor))
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
        public void AddDataToChart(string sensorName, SensorDataVO curData)
        {
            lock(this.chart1){
                DateTime timeStamp  = DateTime.Now;
                if (chart1.Series.IndexOf(sensorName) != -1)
                {
                    Series series = chart1.Series[sensorName];
                    AddDataToSeries(series, timeStamp, curData.CurTime, curData.Data);
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
            while (series.Points.Count > 0 && series.Points[0].XValue < removeBefore)
            {
                series.Points.RemoveAt(0);
            }
        }
        // Update real time sensor data grid view
        private void UpdateDataGridView(string sensorName, SensorDataVO data)
        {
            if (gridViewList.ContainsKey(sensorName))
            {
                int index = -1;
                for (int i = 0; i < sensorNames.Length; ++i)
                    if (sensorNames[i].Equals(sensorName)){
                        index = i;
                        break;
                    }
                string extraName = dataReaders[index].ToString();

                Action updateAction = () => {
                    gridViewList[sensorName].Rows.Insert(0, new string[]{
                        extraName,
                        data.CurTime.ToString("MM월 dd일 HH:mm:ss"),
                        data.Data.ToString()
                    });
                    if (gridViewList[sensorName].Rows.Count > 30)
                    {
                        int gridIndex = gridViewList[sensorName].Rows.Count - 1;
                        gridViewList[sensorName].Rows.Remove(gridViewList[sensorName].Rows[gridIndex]);
                    }
                };
                gridViewList[sensorName].BeginInvoke(updateAction);
            }
        }
        // Timer operating code 
        private void TimerOperating()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += new System.EventHandler(time_ticker);
            timer.Start();
        }
        // Update current time
        void time_ticker(object sender, System.EventArgs e)
        {
            // Express time
            this.label2.Text = System.DateTime.Now.ToString("HH:mm:ss");
            if (this.label2.Text.Equals("00:00:00"))
                this.label1.Text = System.DateTime.Now.ToString("yyyy년 MM월 dd일");
        }
        // Read current sensor data
        private void timer1_Tick(object sender, EventArgs e)
        {
            var curData = new List<SensorData>();
            // Update chart
            try
            {
                chart1.Series.SuspendUpdates();
                foreach (string sensor in checkeredList)
                {
                    if (chart1.Series.IndexOf(sensor) != -1)
                    {
                        int index = -1;
                        for (int i = 0; i < sensorNames.Length; ++i)
                            if (sensorNames[i].Equals(sensor)){
                                index = i;
                                break;
                            }
                        SensorDataVO readData = dataReaders[index].ReadSensorData();
                        if (readData.Type == SensorType.None)
                            continue;
                        SensorData addData = new SensorData()
                        {
                            Data = readData.Data,
                            Time = readData.CurTime,
                            SensorConfigId = configs[index].Id
                        };
                        curData.Add(addData);
                        AddDataToChart(sensor, readData);
                    }
                }
                if (chart1.Series[0] != null && chart1.Series[0].Points.Count > 0)
                {
                    chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(12).ToOADate();
                }
                chart1.Series.ResumeUpdates();
            }
            catch (Exception)
            {
                return;
            }

            // DB transaction
            try
            {
                dbManager.dc.SensorDatas.InsertAllOnSubmit(curData);
                dbManager.dc.SubmitChanges();
            }
            catch (Exception)
            {
                
            }
        }
        // When form closed
        private void RealTimeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.stopBtn_Click(sender, e);
            //this.Close();
        }
    }
}
