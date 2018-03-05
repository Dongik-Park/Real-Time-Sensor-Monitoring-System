using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    public class SensorDBManager
    {
        public SensorDataContext dc = null;

        // Singleton pattern
        //static SensorDBManager instance;
        //
        //public static SensorDBManager Instance { get { return instance; } }
        //
        //static SensorDBManager()
        //{
        //    instance = new SensorDBManager();
        //}
        //
        //private SensorDBManager()
        //{
        //    if (!dc.DatabaseExists())
        //        dc.CreateDatabase();
        //}

        public SensorDBManager()
        {
            dc = new SensorDataContext(@"Data Source=localhost;
                                                       Initial Catalog=DongikDB_3; 
                                                       Integrated Security=True");
            if (!dc.DatabaseExists())
                dc.CreateDatabase();

        }
    }
}
