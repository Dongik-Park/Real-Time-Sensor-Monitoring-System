using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using Newtonsoft.Json;

namespace WindowForm_Dongik
{
    public enum SensorType : byte
    {
        None = 0,
        Temperature,
        Cpu_occupied,
        Memory_usage,
        Modbus,
        Omap
    }
    public abstract class BaseSensorReader
    {
        // Member field
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public SensorType SensorType { get; set; }
        public bool IsActive { get; set; }

        public BaseSensorReader() { }
        // Base constructor
        public BaseSensorReader(string name, DateTime time, SensorType sensorType, bool isActive)
        {
            this.Name = name;
            this.Time = time;
            this.SensorType = sensorType;
            this.IsActive = IsActive;
        }
        // Abstract read sensor data method
        public abstract SensorDataVO ReadSensorData();

        // Override ToString
        public override string ToString()
        {
            return Name + "," + Time + "," + SensorType + "," + IsActive + ", it will not be binded";
        }

    }
}
