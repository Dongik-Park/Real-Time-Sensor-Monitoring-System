using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NadaCommon;
using NadaCommon.Device.Omap;

namespace WindowForm_Dongik
{
    public enum OmapType : byte
    {
        None,
        Peak_Peak,
        Rms
    }
    public class OmapSensorReader : BaseSensorReader
    {
        private OmapReceiver receiver;
        private OmapModule module;
        private Queue<ExtractData> dataQueue = new Queue<ExtractData>(20);
        private OmapType omapType;
        private string ip;

        private bool flag;

        public OmapSensorReader(string name, DateTime time, SensorType sensorType, bool isActive, OmapType omapType, string ip)
            : base(name,time,sensorType,isActive)
        {
            this.ip = ip;
            this.module = new OmapModule(ip);
            this.receiver = new OmapReceiver();
            this.omapType = omapType;
            receiver.Initialize(module);

            switch (omapType)
            {
                case OmapType.Peak_Peak: receiver.DataProvide += OnPeakToPeakReceived; break;
                case       OmapType.Rms: receiver.DataProvide += OnRmsReceived;        break;
            }
            //receiver.DataProvide += OnDataReceived;
            receiver.Start();
        }
        public override SensorDataVO ReadSensorData()
        {
            lock (this.dataQueue)
            {
                if (this.dataQueue.Count < 1)
                    return new SensorDataVO(0, DateTime.Now,SensorType.None);
                ExtractData data = this.dataQueue.Dequeue();
                SensorDataVO vo = new SensorDataVO(
                            data.Data,
                            data.Time,
                            SensorType.Omap);
                return vo;
            }
        }
        private void OnPeakToPeakReceived(object data, DataType type)
        {
            if (type != NadaCommon.DataType.OmapWave)
                return;
            var waves = data as WaveData[];
            float[] list = new float[waves.Length];
            for (int i = 0; i < waves.Length; ++i)
            {
                float curMax = waves[i].AsyncData.Max();
                float curMin = waves[i].AsyncData.Min();
                list[i] = Math.Abs(curMax) + Math.Abs(curMin);
            }
            lock (this.dataQueue)
            {
                this.dataQueue.Enqueue(new ExtractData(){
                    Data = list.Max(),
                    Time = waves[0].DateTime
                });
            }
        }
        private void OnRmsReceived(object data, DataType type)
        {
            if (type != NadaCommon.DataType.OmapWave)
                return;
            var waves = data as WaveData[];
            float sum = 0;
            for (int i = 0; i < waves.Length; ++i)
            {
                foreach (float temp in waves[i].AsyncData)
                    sum += (temp * temp);
            }
            lock (this.dataQueue)
            {
                this.dataQueue.Enqueue(new ExtractData()
                {
                    Data = Math.Sqrt(sum / waves.Length),
                    Time = waves[0].DateTime
                });
            }
        }

        ~OmapSensorReader()
        {
            if (receiver != null)
            {
                receiver.Stop();
                receiver.Dispose();
            }
        }
        private class ExtractData
        {
            public ExtractData(){
                Data = double.MinValue;
            }
            public double Data { get; set; }
            public DateTime Time { get; set; }
        }
        public override string ToString()
        {
            return omapType.ToString();
        }
    }
}
