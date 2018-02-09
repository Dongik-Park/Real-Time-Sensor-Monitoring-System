using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public class BaseDTO
    {
        public DateTime Time { get; set; }
        public object Data { get; set; }

        //Constructor
        public BaseDTO()
        {
            this.Time = DateTime.Now;
            this.Data = 0;
        }

        public BaseDTO(DateTime Time)
        {
            this.Time = Time;
            this.Data = 0;
        }
        public BaseDTO(object Data)
        {
            this.Time = DateTime.Now;
            this.Data = Data;
        }
        public BaseDTO(DateTime Time, object Data)
        {
            this.Time = Time;
            this.Data = Data;
        }
    }
}
