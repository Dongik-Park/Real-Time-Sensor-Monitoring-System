using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public abstract class EssentialInfo
    {
        protected static bool isActivate { get; set; }

        public DateTime time { get; set; }
        public abstract dynamic GetDataFromSensor();
        public virtual void JsonRead() { }
        public virtual void JsonWrite() { }
    }
}
