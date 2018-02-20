using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public enum SensorTypes
    {
        Modbus,
        CpuTemp,
        MemUsage
    }
    public class SensorDTO
    {
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public byte IsActive { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public Dictionary<string, string> ExtraInfo { get; set; }

        public SensorTypes SensorType { get; set; }

        // 2월 19일 오전 10시 - 여기서 ExtraInfo 말고 string 타입으로 SensorCategory를 만들어 온도 이름 별로 DTO를 관리하지 말고
        // 속성에서 한 개 단위로 관리하는 것이 좋을 것 같다.


        public SensorDTO()
        {
            this.Name = "UnNamed";
            this.AddDate = DateTime.Now;
            this.IsActive = 0;
            this.ExtraInfo = new Dictionary<string, string>();
        }
        public SensorDTO(string Name)
        {
            this.Name = Name;
            this.AddDate = DateTime.Now;
            this.IsActive = 0;
        }
        public SensorDTO(string Name, DateTime AddDate)
        {
            this.Name = Name;
            this.AddDate = AddDate;
            this.IsActive = 0;
        }
        public SensorDTO(string Name, byte IsActive)
        {
            this.Name = Name;
            this.AddDate = DateTime.Now;
            this.IsActive = IsActive;
        }
        public SensorDTO(string Name, DateTime AddDate, byte IsActive)
        {
            this.Name = Name;
            this.AddDate = AddDate;
            this.IsActive = IsActive;
        }
        public SensorDTO(string Name, DateTime AddDate, byte IsActive, string Type, Dictionary<string, string> ExtraInfo)
        {
            this.Name = Name;
            this.AddDate = AddDate;
            this.IsActive = IsActive;
            this.Type = Type;
            this.ExtraInfo = ExtraInfo;
        }
    }
}
