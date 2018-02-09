using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WindowForm_Dongik
{
    public class CpuOccupied : EssentialInfo
    {
        //Instance field
        private byte rate { get; set; }

        //Non-Instance field
        static List<CpuOccupied> cpuList = new List<CpuOccupied>();
        static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"); //Process.GetCurrentProcess().ProcessName

        //Constructor
        public CpuOccupied(DateTime currentTime)
        {
            this.time = currentTime;
            this.rate = 0;
            GetDataFromSensor();
        }

        //Override
        public override dynamic GetDataFromSensor()
        {
            this.rate = (byte)cpuCounter.NextValue();
            return this.rate;
        }
        public override void JsonRead()
        {

        }
        public override void JsonWrite()
        {
        }
    }
}
