using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WindowForm_Dongik
{
    public enum ProcessType : byte
    {
        None,
        Total,
        Current
    }
    public class CpuSensorReader : BaseSensorReader
    {
        // Member field
        private PerformanceCounter totCpuCounter;
        private PerformanceCounter curCpuCounter;
        public ProcessType ProcessType { get; set; }
        // Derived Constructor
        public CpuSensorReader(string name, DateTime time, SensorType sensorType, bool isActive, ProcessType processType) 
            : base(name,time,sensorType,isActive)
        {
            this.ProcessType = processType;
            totCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            curCpuCounter = new PerformanceCounter("Processor", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        }
        // Override read sensor data method
        public override SensorDataVO ReadSensorData()
        {
            double data = 0;
            switch (this.ProcessType)
            {
                case   ProcessType.Total : data = totCpuCounter.NextValue(); break;
                case ProcessType.Current : data = curCpuCounter.NextValue(); break;
            }
            return new SensorDataVO( 
                data,
                DateTime.Now,
                SensorType.Cpu_occupied);
        }
        // Override ToString 
        public override string ToString()
        {
            if (this.ProcessType == ProcessType.Total)
                //return base.ToString() + ", Total" 
                return "Total";
            return "Current";
        }
    }
}
