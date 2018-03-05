using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public class SensorReaderFactory
    {
        public static BaseSensorReader CreateReader(BaseSensorConfig config)
        {
            BaseSensorReader manager = null;
            switch (config.SensorType)
            {
                case SensorType.Temperature: manager = new TemperatrueSensorReader(
                                                            config.Name,
                                                            config.MadeTime,
                                                            config.SensorType,
                                                            config.IsActive,
                                                            ((TempertaureSensorConfig)config).CoreIndex); break;
                case SensorType.Cpu_occupied: manager = new CpuSensorReader(
                                                            config.Name,
                                                            config.MadeTime,
                                                            config.SensorType,
                                                            config.IsActive, 
                                                            ((CpuSensorConfig)config).ProcessType); break;
                case SensorType.Memory_usage: manager = new MemorySensorReader(
                                                            config.Name,
                                                            config.MadeTime,
                                                            config.SensorType,
                                                            config.IsActive); break;
                case SensorType.Modbus: manager = new ModbusSensorReader(
                                                            config.Name,
                                                            config.MadeTime,
                                                            config.SensorType,
                                                            config.IsActive,
                                                            ((ModbusSensorConfig)config).Ip,
                                                            ((ModbusSensorConfig)config).Port,
                                                            ((ModbusSensorConfig)config).Address); break;
                case SensorType.Omap: manager = new OmapSensorReader(
                                                           config.Name,
                                                           config.MadeTime,
                                                           config.SensorType,
                                                           config.IsActive,
                                                           ((OmapSensorConfig)config).OmapType,
                                                           ((OmapSensorConfig)config).Ip); break;

            }

            //manager = FastMapper.TypeAdapter.Adapt<BaseSensorConfig, TemperatrueSensorReader>(config);
            return manager;
        }
    }
}
