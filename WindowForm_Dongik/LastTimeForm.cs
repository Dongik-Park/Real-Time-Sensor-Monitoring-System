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
        private SensorDBManager dbManager = new SensorDBManager();

        // Sensor configs
        private BaseSensorConfig[] configs = null;

        // Sensor names for UI ( series, checkedList )
        private string[] sensorNames = null;

        // User checked list
        private string[] checkeredList = null;

        public LastTimeForm()
        {
            InitializeComponent();
        }

        // Component load template
        private void LastTimeForm_Load(object sender, EventArgs e)
        {
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
        }

        // Load sensor data manager
        private void LoadSensorDataReaders()
        {
            var sensorList = dbManager.dc.BaseSensorConfigs.Select(t => t);
            var tempConfigList = new List<BaseSensorConfig>();
            var tempSensorNames = new List<string>();
            foreach (var sensor in sensorList)
            {
                if (!sensor.IsActive)
                    continue;
                tempConfigList.Add(sensor);
                string sensorName = "";
                switch(sensor.SensorType){
                    case  SensorType.Temperature : sensorName = sensor.Name + "_" + ((TempertaureSensorConfig)sensor).CoreIndex; break;
                    case SensorType.Cpu_occupied : sensorName = sensor.Name + "_" + ((CpuSensorConfig)sensor).ProcessType;       break;
                    case SensorType.Memory_usage : sensorName = sensor.Name + "_" ;                                              break;
                    case       SensorType.Modbus : sensorName = sensor.Name + "_" + ((ModbusSensorConfig)sensor).Address;        break;
                    case         SensorType.Omap : sensorName = sensor.Name + "_" + ((OmapSensorConfig)sensor).OmapType;         break;
                }
                tempSensorNames.Add(sensorName);
            }
            configs = tempConfigList.ToArray();
            sensorNames = tempSensorNames.ToArray();
        }
        // Load checkedlist box
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
                IsChecked = true;
                Name = "name";
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
            string[] items = new string[checkedListBox1.CheckedItems.Count];

            for (int i = 0; i <= checkedListBox1.CheckedItems.Count - 1; ++i)
                items[i] = checkedListBox1.GetItemText(checkedListBox1.CheckedItems[i]);
            return items;
        }
        // Load chart config
        private void LoadChart()
        {
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisX.LineColor = Color.Transparent;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
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
        // Make a series by name information
        private Series MakeSeries(string sensorName)
        {
            Series series = new Series(sensorName);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.XValueType = ChartValueType.DateTime;
            return series;
        }
        // When user click search
        private void searchBtn_Click(object sender, EventArgs e)
        {
            // Get currently checked items
            checkeredList = GetCheckedItems();
            // Load checked list
            LoadSeriesByChecked(checkeredList);
            // Update Data tab
            LoadDataTab(checkeredList);
            // Update chart series
            SearchSensorData(checkeredList);
        }
        // Load sensor data
        private void SearchSensorData(string[] sensors)
        {
            foreach (string sensor in sensors)
            {
                if (chart1.Series.IndexOf(sensor) != -1)
                {
                    Series series = chart1.Series[sensor];
                    int index = -1;
                    for (int i = 0; i < sensorNames.Length; ++i)
                        if (sensorNames[i].Equals(sensor))
                        {
                            index = i;
                            break;
                        }
                    int configId = configs[index].Id;
                    var results = dbManager.dc.SensorDatas
                                        .Where(t => t.SensorConfigId == configId
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
        // Update real time sensor data grid view
        private void UpdateDataGridView(string sensorName, SensorData data)
        {
            if (gridViewList.ContainsKey(sensorName))
            {
                int index = -1;
                for (int i = 0; i < sensorNames.Length; ++i)
                    if (sensorNames[i].Equals(sensorName))
                    {
                        index = i;
                        break;
                    }
                string extraName = sensorNames[index].ToString();
                Action updateAction = () =>
                {
                    gridViewList[sensorName].Rows.Insert(0, new string[]{
                        extraName,
                        data.Time.ToString("MM월 dd일 HH:mm:ss"),
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


    }
}
