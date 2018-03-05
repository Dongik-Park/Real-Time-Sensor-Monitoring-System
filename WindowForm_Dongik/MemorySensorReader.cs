using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace WindowForm_Dongik
{
    public class MemorySensorReader : BaseSensorReader
    {
        // Member field
        private PerformanceCounter memCounter;
        private ManagementObjectSearcher searcher;
        // Derived Constructor
        public MemorySensorReader(string name, DateTime time, SensorType sensorType, bool isActive)
            : base(name, time, sensorType, isActive)
        {
            memCounter = new PerformanceCounter("Memory", "Available MBytes"); //"Working Set", Process.GetCurrentProcess().ProcessName
            searcher = new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_OperatingSystem"));
        }
        // Override read sensor data method
        public override SensorDataVO ReadSensorData()
        {
            ulong memory = 0;
            double usageGB = 0;
            //byte rate = 0;
            DateTime cur = DateTime.Now;
            foreach (ManagementObject item in searcher.Get())
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            float val = memCounter.NextValue();
            usageGB = ((memory / 1024) - val) / 1000;
            //rate = (byte)(val / (memory / 1024) * 100);
            return new SensorDataVO(
                usageGB, 
                DateTime.Now, 
                SensorType.Memory_usage);
        }
        // Override ToString
        public override string ToString()
        {
            //return base.ToString();
            return "";
        }
    }
}
