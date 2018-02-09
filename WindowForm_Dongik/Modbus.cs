using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using NModbus;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    class Modbus
    {

        /*public BaseDTO[] Read(ushort sI, ushort size)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 502))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);
                var readed = master.ReadHoldingRegisters(1, sI, size);
                BaseDTO[] dtos = new BaseDTO[readed.Length];
                
            }

        }*/
    }
}
