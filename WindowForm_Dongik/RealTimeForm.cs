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
        // 실시간 데이터를 관리할 dao 객체 ? 
        private static BaseDAO dao = new BaseDAO();

        // UI에 반영되는 Thread
        private Dictionary<string, Thread> threads = new Dictionary<string,Thread>();

        // UI thread flag
        private bool isActive = true;

        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;

        public RealTimeForm()
        {
            InitializeComponent();
        }

        // 사용자가 원하는 센서 정보 목록을 입력하고 조회 버튼 클릭하였을때
        private void startBtn_Click(object sender, EventArgs e)
        { 
            // Disable all controls on the form
            this.startBtn.Enabled = false;
            // and only Enable the Stop button
            this.stopBtn.Enabled = true;

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

            //Modbus ChartArea
            //this.chart1.ChartAreas.Add("modbus");
            //this.chart1.ChartAreas["modbus"].AxisX.Minimum = minValue.ToOADate();
            //this.chart1.ChartAreas["modbus"].AxisX.Maximum = maxValue.ToOADate();
            //this.chart1.ChartAreas["modbus"].AxisX.LabelStyle.Format = "HH:mm:ss";
            //this.chart1.ChartAreas["modbus"].AxisX.Interval = 1;

            // Reset number of series in the chart.
            chart1.Series.Clear();

            // create a line chart series
            string[] lists = GetCheckedItems();
            //string[] lists = { "temperature", "cpu occupied", "memory usage", "modbus_1" };
            foreach (string s in lists)
            {
                Series newSeries = new Series(s);
                newSeries.ChartType = SeriesChartType.Line;
                newSeries.BorderWidth = 2;
                //newSeries.Color = Color.OrangeRed;
                newSeries.XValueType = ChartValueType.DateTime;
                chart1.Series.Add(newSeries);
            }

            // start worker threads.
            if (this.threads["cpu"].IsAlive == true)
            {
                this.threads["cpu"].Resume();
            }
            else
            {
                this.threads["cpu"].Start();
            }
            // 원하는 센서 데이터 정보 스레드 생성
            //CpuSensorThread();
        }

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
            TimerOperating();
        }
        // Sensor datas load
        private void SensorLoad()
        {
            dao.AddSensor("temperature");
            dao.AddSensor("cpu occupied");
            dao.AddSensor("memory usage");
            dao.AddSensor("modbus_1");
            //dao.AddSensor("memory usage");
        }
        // 비동기 스레드
        private async void Run()
        {
            // 비동기로 Worker Thread에서 도는 task1
            //await Task.Run(CpuSensorThread());
            //await Task.Run(() => CpuSensorThread());
        }

        // Load chart info
        private void LoadChart()
        {
            // create the Adding Data Thread but do not start until start button clicked
            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);

            //Thread load 임시 데이터
            this.threads.Add("cpu",new Thread(addDataThreadStart));

            //this.threads.Add("modbus", new Thread(addDataThreadStart));

            // create a delegate for adding data
            addDataDel += new AddDataDelegate(AddData);

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
            foreach (string dic in dao.Dictionary.Keys)
                lst.Add(new SensorClass(dic, false));

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

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        // Stop btn click
        private void stopBtn_Click(object sender, EventArgs e)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            // Disable all controls on the form
            this.startBtn.Enabled = true;
            // and only Enable the Stop button
            this.stopBtn.Enabled = false;
            foreach (string thread in this.threads.Keys)
                this.threads[thread].Abort();
        }


        /// Main loop for the thread that adds data to the chart.
        /// The main purpose of this function is to Invoke AddData
        /// function every 1000ms (1 second).
        private void AddDataThreadLoop()
        {
            while (true)
            {
                chart1.Invoke(addDataDel);

                Thread.Sleep(1000);
            }
        }

        public void AddData()
        {
            DateTime timeStamp = DateTime.Now;
            
            foreach (Series ptSeries in chart1.Series)
            {
                AddNewPoint(timeStamp, ptSeries);
            }
            chart1.Invalidate();
        }
        /// The AddNewPoint function is called for each series in the chart when
        /// new points need to be added.  The new point will be placed at specified
        /// X axis (Date/Time) position with a Y value in a range +/- 1 from the previous
        /// data point's Y value, and not smaller than zero.
        public void AddNewPoint(DateTime timeStamp, Series ptSeries)
        {
            BaseDTO dto = dao.GetSensorData(ptSeries.Name);
            // Add new data point to its series.
            ptSeries.Points.AddXY(dto.Time.ToOADate(), dto.Data);

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

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
        /// Clean up any resources being used.
       /* protected override void Dispose(bool disposing)
        {
            foreach (string thread in this.threads.Keys)
            {
                if ((this.threads[thread].ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
                {
                    this.threads[thread].Resume();
                }
                this.threads[thread].Abort();

                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }
        }   */

        // -------------------------------------------------------------------------------Custom method start ----------------------------------------------------------------------
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
        // -------------------------------------------------------------------------------Custom method end ------------------------------------------------------------------------
    }
}
