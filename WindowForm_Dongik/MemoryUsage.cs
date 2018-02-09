using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
namespace WindowForm_Dongik
{
    public class MemoryUsage : EssentialInfo
    {
        public byte rate { get; set; }
        public double usageGB { get; set; }
        static PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes"); //"Working Set", Process.GetCurrentProcess().ProcessName

        //Constructor
        public MemoryUsage(DateTime currentTime)
        {
            this.time = currentTime;
            this.usageGB = 0;
            GetDataFromSensor();
        }
        public MemoryUsage(DateTime currentTime, double usageGB, byte rate)
        {
            this.time = currentTime;
            this.usageGB = usageGB;
            this.rate = rate;
        }
        //Override method
        public override dynamic GetDataFromSensor()
        {
            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            ulong memory = 0;
            foreach (ManagementObject item in searcher.Get())
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            float val = memCounter.NextValue();
            this.usageGB = ((memory / 1024) - val) / 1000;
            this.rate = (byte)(val / (memory / 1024) * 100);
            return new MemoryUsage(this.time,this.usageGB, this.rate);
        }
        public override void JsonRead()
        {

        }
        public override void JsonWrite()
        {

        }
    }
}
