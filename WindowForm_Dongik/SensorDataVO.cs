using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public class SensorDataVO
    {
        public double Data { get; set; }
        public DateTime CurTime { get; set; }
        public SensorType Type { get; set; }

        public SensorDataVO(double data, DateTime curTime, SensorType type)
        {
            this.Data = data;
            this.CurTime = curTime;
            this.Type = type;
        }
    }
}
