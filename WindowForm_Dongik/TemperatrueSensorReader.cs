using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace WindowForm_Dongik
{
    public enum CoreNumber : byte
    {
        None   = 0,
        Core_1 = 1, 
        Core_2 = 2,
        Core_3 = 3,
        Core_4 = 4
    }
    public class TemperatrueSensorReader : BaseSensorReader
    {
        // Member field
        private ManagementObjectSearcher searcher;
        public CoreNumber CoreIndex { get; set; }
        // Derived Constructor
        public TemperatrueSensorReader() { }
        public TemperatrueSensorReader(string name, DateTime time, SensorType sensorType, bool isActive,
                                        CoreNumber coreIndex)
            : base(name,time,sensorType,isActive)
        {
            this.CoreIndex = coreIndex;
            searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
        }
        // Override read sensor data method
        public override SensorDataVO ReadSensorData()
        {
            int cnt = 0;
            double data = 0;
            foreach (ManagementObject obj in searcher.Get())
            {

                if (++cnt == (int)CoreIndex)
                    data = Convert.ToDouble(obj["CurrentTemperature"].ToString());
            }

            //ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher2.Get().GetEnumerator();
            //// Move enum to matched config
            //for(int i = 1; i < temp.CoreIndex; ++i)
            //    enumerator.MoveNext(); // Move가 일어나지 않음(?)
            //double data = Convert.ToDouble(enumerator.Current["CurrentTemperature"].ToString());


            data = (data - 2732) / 10.0;

            return new SensorDataVO(
                data,
                DateTime.Now,
                SensorType.Temperature);
        }
        // Override ToString
        public override string ToString()
        {
            //return base.ToString() + ", Core" + temp.CoreIndex;
            return CoreIndex.ToString();
        }
    }
}
