using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    class SensorDTO
    {
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public byte IsActive { get; set; }

        public SensorDTO()
        {
            this.Name = "Unnamed";
            this.AddDate = DateTime.Now;
            this.IsActive = 0;
        }
        public SensorDTO(string name)
        {
            this.Name = name;
            this.AddDate = DateTime.Now;
            this.IsActive = 0;
        }
        public SensorDTO(string name, DateTime addDate)
        {
            this.Name = name;
            this.AddDate = addDate;
            this.IsActive = 0;
        }
        public SensorDTO(string name, byte isActive)
        {
            this.Name = name;
            this.AddDate = DateTime.Now;
            this.IsActive = isActive;
        }
        public SensorDTO(string name, DateTime addDate, byte isActive)
        {
            this.Name = name;
            this.AddDate = addDate;
            this.IsActive = isActive;
        }
    }
}
