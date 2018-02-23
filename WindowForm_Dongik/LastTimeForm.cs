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

        // Last Time DataGridview
        private Dictionary<string, DataGridView> gridViewList = new Dictionary<string, DataGridView>();

        // Last Time Tabpage
        private Dictionary<string, TabPage> tabList = new Dictionary<string, TabPage>();
        
        // Sensor Database manager
        private SensorDBManager sdm = SensorDBManager.Instance;

        private string[] checkeredList = null;

        double chartMaxTime = 0;
        double chartMinTime = 0;

        public LastTimeForm()
        {
            InitializeComponent();
        }
        public void BindingChartSeries(string[] sensors)
        {
            foreach (string sensor in sensors)
            {
                if (chart1.Series.IndexOf(sensor) != -1)
                {
                    Series series = chart1.Series[sensor];
                    SensorConfig config = sdm.dc.SensorConfigs.Where(t => t.Id == Convert.ToInt32(series.GetCustomProperty("id"))).First();
                    var results = sdm.dc.SensorDatas
                                        .Where(t => t.SensorConfigId == config.Id 
                                            && t.Time >= startTimePicker.Value 
                                            && t.Time <= lastTimePicker.Value)
                                        .Select(t => t) 
                                        .ToArray(); // In this line, sql query will be transfered
                    // Read Only chart - for loop is efficient
                    // Read & Write chart - datasource bind is efficient
                    foreach (var data in results)
                    {
                        series.Points.AddXY(data.Time, data.Data);
                        UpdateDataGridView(sensor, data);
                    }
                    
                }
            }
        }
        // 사용자가 원하는 센서 정보 목록을 입력하고 조회 버튼 클릭하였을때
        private void startBtn_Click(object sender, EventArgs e)
        {
            // Get currently checked items
            checkeredList = GetCheckedItems();
            // Load checked list
            LoadSeriesByChecked(checkeredList);
            // Load Tabpages
            //LoadTabPage(checkeredList);
            // Load DataGridView
            //LoadGridView(checkeredList);
            // Update chart series
            BindingChartSeries(checkeredList);
            // Update Data tab
            LoadDataTab(checkeredList);
        }
        // Load chart config
        private void LoadChart()
        {
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisX.LineColor = Color.Transparent;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
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
            var sensors = sdm.dc.SensorConfigs.Select(t => t);
            foreach (var sensor in sensors)
            {
                if (sensor.IsActive != 1 && !list.Contains(sensor.Name))
                     continue;
                switch (sensor.SensorType)
                {
                    case "temperature":
                        var tSensor = sdm.dc.TempertaureSensors.Where(t => t.SensorConfigId == sensor.Id).Select(t => t).First();
                        chart1.Series.Add(MakeSeries(sensor.Name, "Core" + tSensor.CoreIndex, sensor.Id));
                        break;
                    case "cpu occupied":
                        var cSensor = sdm.dc.CpuOccupiedSensors.Where(t => t.SensorConfigId == sensor.Id).Select(t => t).First();
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
                        var mSensor = sdm.dc.ModbusSensors.Where(t => t.SensorConfigId == sensor.Id).Select(t => t).First();
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
        // Load Data tab
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
        // Get currently checked items list
        private string[] GetCheckedItems()
        {
            string[] items = new string[checkedListBox1.CheckedItems.Count];

            for (int i = 0; i <= checkedListBox1.CheckedItems.Count - 1; ++i)
               items[i] = checkedListBox1.GetItemText(checkedListBox1.CheckedItems[i]);
           return items;
        }
        // Load checkedlist box
        private void LoadCheckedListBox()
        {
            List<SensorClass> lst = new List<SensorClass>();
            var sensorList = sdm.dc.SensorConfigs
                                    .Select(t => t)
                                    .ToArray();
            // Get sensors from Database 
            foreach (var sensor in sensorList)
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
            ((ListBox)checkedListBox1).DataSource = lst;
            ((ListBox)checkedListBox1).DisplayMember = "Name";
            ((ListBox)checkedListBox1).ValueMember = "IsChecked";


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
                IsChecked = true;
                Name = "name";
            }
            public SensorClass(string Name, bool IsChecked)
            {
                this.Name = Name;
                this.IsChecked = IsChecked;
            }
        }
        // Update real time sensor data grid view
        private void UpdateDataGridView(string sensorName, SensorData data)
        {
            // Should clear gridview before invocation!!!!!!!!!!!!!!!!!!!!!!!
            if (gridViewList.ContainsKey(sensorName))
            {
                string extraName = "";
                switch (sdm.dc.SensorConfigs.Where(t => t.Id == data.SensorConfigId).First().SensorType)
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
                //gridViewList[sensorName].Rows.Insert(0, new string[]{
                //        extraName,
                //        data.Time.ToString("MM월 dd일 HH:mm:ss"),
                //        data.Data.ToString()
                //    });
                Action updateAction = () =>
                {
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


        private void LastTimeForm_Load(object sender, EventArgs e)
        {
            // Load CheckedListBox
            LoadCheckedListBox();
            // Get currently checked items
            checkeredList = GetCheckedItems();
            // Load ChartConfig
            LoadChart();
            // Load Tabpages
            LoadTabPage(checkeredList);
            // Load DataGridView
            LoadGridView(checkeredList);
            // Update Data tab
            LoadDataTab(checkeredList);
            // Load Checked series
            LoadSeriesByChecked(checkeredList);
        }

        private void MainBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
