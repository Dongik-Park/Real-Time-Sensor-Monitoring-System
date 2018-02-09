using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
namespace WindowForm_Dongik
{
    public class Temperature : EssentialInfo
    {
        //Non-static field
        static List<Temperature> tempList = new List<Temperature>();

        //Instance field
        public double temperature { get; set; }

        // Construcotr
        public Temperature(double temperature)
        {
            this.temperature = temperature;
        }
        public Temperature(DateTime currentTime)
        {
            // 현재 시간 
            this.time = currentTime;
            GetDataFromSensor();            
        }
        public Temperature(double temperature, DateTime currentTime)
        {
            this.temperature = temperature;
            this.time = currentTime;
        }
        //Override Method
        public override dynamic GetDataFromSensor()
        {
            // 온도 추출 후 경우 중 가장 큰 온도를 현재 온도로 지정
            List<Temperature> result = new List<Temperature>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject obj in searcher.Get())
            {
                Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                result.Add(new Temperature(temp));
                //Console.WriteLine(" Time : " + DateTime.Now.ToLongTimeString() + " 현재온도 : " + temp + "℃");
            }
            this.temperature = result.Max(Temperature => Temperature.temperature);
            return this.temperature;
        }
        public override void JsonRead()
        {
        }
        public override void JsonWrite()
        { 
        }
    }
}
