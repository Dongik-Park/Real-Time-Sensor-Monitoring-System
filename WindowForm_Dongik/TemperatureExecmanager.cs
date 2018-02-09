using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowForm_Dongik
{
    class TemperatureExecmanager
    {
        private List<Temperature> tempList = new List<Temperature>();
        public List<Temperature> TempList
        {
            get
            {
                return this.tempList;
            }
            set
            {
                this.tempList = value;
            }
        }
        private TemperatureSaveManager sManager = null;

        // Constructor
        public TemperatureExecmanager()
        {
            if (this.tempList == null)
                this.tempList = new List<Temperature>();
        }
        public TemperatureExecmanager(TemperatureSaveManager sManager)
        {
            this.sManager = sManager;
        }
        public TemperatureExecmanager(List<Temperature> tempList)
        {
            this.tempList.Concat(tempList);
        }
        // Destructor
        ~TemperatureExecmanager()
        {
            //소멸전 데이터 존재 시 백업
            this.sManager.SaveCurrentData(this.tempList);
        }

        // Custom method
        // 목록 추가 + IO 별도
        public void AddTemperature(Temperature temp)
        {
            if (this.tempList.Count < 5)
            {
                this.tempList.Add(temp);
            }
            else
            {
                this.sManager.SaveCurrentData(this.tempList);
                this.tempList.Clear();
                this.tempList.Add(temp);
            }
        }

    }
}
