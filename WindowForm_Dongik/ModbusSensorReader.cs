using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using NModbus;

namespace WindowForm_Dongik
{
    class ModbusSensorReader : BaseSensorReader
    {
        // Member field
        private TcpClient client;
        private ModbusFactory factory;
        private IModbusMaster master;
        public string Ip { get; set; }
        public int Port { get; set; }
        public ushort Address { get; set; }
        private const ushort Size = 1;
        // Derived Constructor
        public ModbusSensorReader(string name, DateTime time, SensorType sensorType, bool isActive, 
                                    string ip, int port, ushort address) 
            : base(name,time,sensorType,isActive)
        {
            this.Ip = ip;
            this.Port = port;
            this.Address = address;
        }
        // Override read sensor data method
        public override SensorDataVO ReadSensorData()
        {
            client = new TcpClient(Ip, Port);
            factory = new ModbusFactory();

            master = factory.CreateMaster(client);
            double data = 0;
            using (client)
            {
                var readed = master.ReadHoldingRegisters(1, Address, Size);
                data = readed[0];
            }
            client.Close();

            return new SensorDataVO( 
                data,
                DateTime.Now,
                SensorType.Modbus);
        }
        // Override ToString 
        public override string ToString()
        {
            //return base.ToString() + "," + modSensor.Address;
            return "Address:" + Address;
        }
    }
}
