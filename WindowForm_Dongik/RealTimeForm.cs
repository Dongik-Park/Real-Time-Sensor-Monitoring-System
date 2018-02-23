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
        private SensorDBManager sdm = SensorDBManager.Instance;

        // RealTime DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();
        
        // Save start time
        private DateTime strTime = DateTime.Now;

        // Save finish time
        private DateTime fnsTime = DateTime.Now;

        // User checked list
        private string[] checkeredList = null;

        // Real time data manage object
        private RealTimeDataManager rtManager = new RealTimeDataManager();

        public RealTimeForm()
        {
            InitializeComponent();
        }

        // component 구성
        private void RealTimeForm_Load(object sender, EventArgs e)
        {
            stopBtn.Enabled = false;
            LoadChart();
            LoadCheckedListBox();
            checkeredList = GetCheckedItems();
            LoadSeriesByChecked(checkeredList);
            LoadDataTab(checkeredList);
            TimerOperating();
        }
        // CheckedListBox load
        private void LoadCheckedListBox()
        {
            List<SensorClass> lst = new List<SensorClass>();
            // Get sensors from Database 
            foreach (var sensor in sdm.dc.SensorConfigs.Select(t => t))
            {
                if (sensor.IsActive != 1)
                    continue;
                string fullName = "";
                switch (sensor.SensorType)
                {
                    case "temperature":  
                        fullName = sensor.Name + "_Core" + sdm.dc.TempertaureSensors.Where(t => t.SensorConfigId == sensor.Id).First().CoreIndex; 
                        break;
                    case "cpu occupied":
                        if (sdm.dc.CpuOccupiedSensors.Where(t => t.SensorConfigId == sensor.Id).First().ProcessType == 1)
                            fullName = sensor.Name + "_Total";
                        else
                            fullName = sensor.Name + "_Current";
                        break;
                    case "memory usage": fullName = sensor.Name; 
                        break;
                    case "modbus":      
                        fullName = sensor.Name + "_" + sdm.dc.ModbusSensors.Where(t => t.SensorConfigId == sensor.Id).First().Address; 
                        break;
                }
                lst.Add(new SensorClass(fullName, true));
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
        // Make a series by name information
        private Series MakeSeries(string sensorName, string extraName, int id)
        {
            string fullName = "";
            if (extraName.Equals(""))
                fullName = sensorName;
            else
                fullName = sensorName + "_" + extraName;
            Series series = new Series(fullName);
            series.SetCustomProperty("id", id + "");
            series.SetCustomProperty("extra", extraName);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.XValueType = ChartValueType.DateTime;
            return series;
        }
        // Make serieses by checkered list
        private void LoadSeriesByChecked(string[] list)
        {
            chart1.Series.Clear();
            // Get sensors from Database 
            var sensorList = sdm.dc.SensorConfigs
                                    .Select(t => t)
                                    .ToArray();
            foreach (var sensor in sensorList)
            {
                if (sensor.IsActive != 1 && !list.Contains(sensor.Name))
                    continue;
                switch (sensor.SensorType)
                {
                    case "temperature":
                        var tSensor = sdm.dc.TempertaureSensors
                                            .Where(t => t.SensorConfigId == sensor.Id)
                                            .Select(t => t)
                                            .First();
                        chart1.Series.Add(MakeSeries(sensor.Name, "Core"+tSensor.CoreIndex,sensor.Id));
                        break;
                    case "cpu occupied":
                        var cSensor = sdm.dc.CpuOccupiedSensors
                                            .Where(t => t.SensorConfigId == sensor.Id)
                                            .Select(t => t)
                                            .First();
                        if (cSensor.ProcessType == 1)
                            chart1.Series.Add(MakeSeries(sensor.Name, "Total", sensor.Id));
                        else 
                            chart1.Series.Add(MakeSeries(sensor.Name, "Current", sensor.Id));
                        break;
                    case "memory usage":
                        chart1.Series.Add(MakeSeries(sensor.Name, "", sensor.Id));
                        break;
                    case "modbus":
                        int size = 0;
                        var mSensor = sdm.dc.ModbusSensors
                                            .Where(t => t.SensorConfigId == sensor.Id)
                                            .Select(t => t)
                                            .First();
                            chart1.Series.Add(MakeSeries(sensor.Name, mSensor.Address + "", sensor.Id));
                        //while (size <= mSensor.Size)
                        //{
                        //    chart1.Series.Add(MakeSeries(sensor.Name, (mSensor.Address + size) + "", sensor.Id));
                        //    ++size;
                        //}
                        break;
                }
            }
        }
        // Make tabs by chechered list
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
        // Get currently checked items list
        private string[] GetCheckedItems()
        {
            string[] items = new string[this.checkedListBox1.CheckedItems.Count];
            for (int i = 0; i <= this.checkedListBox1.CheckedItems.Count - 1; ++i)
               items[i] = this.checkedListBox1.GetItemText(this.checkedListBox1.CheckedItems[i]);
           return items;
        }
        // Start button click event
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
                    LoadSeriesByChecked(GetCheckedItems());
                    LoadDataTab(GetCheckedItems());
                }
            checkeredList = GetCheckedItems();
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
        public void AddData(string sensorName)
        {
            lock(this.chart1){
                DateTime timeStamp  = DateTime.Now;
                AddNewPoint(timeStamp, sensorName);
            }
        }
        // Template of Add real time sensor data
        private void AddNewPoint(DateTime timeStamp, string sensorName)
        {
            if (chart1.Series.IndexOf(sensorName) != -1)
            {
                Series series = chart1.Series[sensorName];
                SensorData curData = rtManager.GetSensorData(Convert.ToInt32(series.GetCustomProperty("id")));
                AddDataToSeries(series, timeStamp, curData.Time, curData.Data);
                UpdateDataGridView(sensorName, curData);
                InsertSensorData(curData);
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
                string extraName = "";
                SensorConfig config = sdm.dc.SensorConfigs.Where(t => t.Id == data.SensorConfigId).First();
                switch (config.SensorType)
                {
                    case "temperature":
                        var tSensor = sdm.dc.TempertaureSensors.Where(t => t.SensorConfigId == data.SensorConfigId).Select(t => t).First();
                        extraName = "Core" + tSensor.CoreIndex;
                        break;
                    case "cpu occupied":
                        var cSensor = sdm.dc.CpuOccupiedSensors.Where(t => t.SensorConfigId == data.SensorConfigId).Select(t => t).First();
                        if (cSensor.ProcessType == 1)
                            extraName = "Total process";
                        else
                            extraName = "Current process";
                        break;
                    case "memory usage":
                        extraName = "Memory Usage";
                        break;
                    case "modbus":
                        int size = 0;
                        var mSensor = sdm.dc.ModbusSensors.Where(t => t.SensorConfigId == data.SensorConfigId).Select(t => t).First();
                        extraName = "Address:" + mSensor.Address;
                        //while (size <= mSensor.Size)
                        //{
                        //    chart1.Series.Add(MakeSeries(sensor.Name, (mSensor.Address + size) + "", sensor.Id));
                        //    ++size;
                        //}
                        break;
                }
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
        // Insert current sensor data to database
        private void InsertSensorData(SensorData curData)
        {
            sdm.dc.SensorDatas.InsertOnSubmit(curData);
            sdm.dc.SubmitChanges();
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
            // Update chart
            chart1.Series.SuspendUpdates();
            foreach (string sensor in checkeredList)
            {
                if(chart1.Series.IndexOf(sensor) != -1)
                 AddData((string)sensor);
            }
            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(12).ToOADate();
            chart1.Series.ResumeUpdates();
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
