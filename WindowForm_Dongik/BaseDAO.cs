using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Management;
using Newtonsoft.Json;
using System.Net.Sockets;
using NModbus;
namespace WindowForm_Dongik
{
    public class BaseDAO
    {
        //Instance Field
        public Dictionary<string, Dictionary<DateTime, BaseDTO>> Dictionary { get; set; }
        private Dictionary<DateTime, BaseDTO> tempDic = new Dictionary<DateTime, BaseDTO>();
        //Directory info
        private const string directory = @"D:/Testfiles/Dongik_Json/";

        //Constructor
        public BaseDAO()
        {
            if (this.Dictionary == null)
                this.Dictionary = new Dictionary<string, Dictionary<DateTime, BaseDTO>>();
        }
        public BaseDAO(Dictionary<string, Dictionary<DateTime, BaseDTO>> Dictionary)
        {
            this.Dictionary = Dictionary;
        }

        //Custom method
        public void AddSensor(string name)
        {
            if(!this.Dictionary.ContainsKey(name))
              this.Dictionary[name] = new Dictionary<DateTime, BaseDTO>();
        }
        public void AddData(string name, BaseDTO dto)
        {
            AddSensor(name);
            if (dto == null)
                return;
            if (!this.Dictionary[name].ContainsKey(dto.Time))
                 this.Dictionary[name].Add(dto.Time, dto);
        }

        /************************************************************************
         ************************   IO Method   *******************************
         ************************************************************************/
        enum Time { Sec, Min, Hour, Day, Month, Year }
        private string GetLine(Dictionary<DateTime, BaseDTO> dictionary)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<DateTime, BaseDTO> pair in dictionary)
            {
                builder.Append(pair.Key.ToString("yyyy.MM.dd:HH:mm:ss")).Append(",").Append(pair.Value.Data).Append("\n");
            }
            string result = builder.ToString();
            // Remove the final delimiter
            result = result.TrimEnd('\n');
            return result;
        }

        private Dictionary<DateTime, BaseDTO> GetDict(string filepath, Time eTime, DateTime reqTime, int addVal)
        {
            string s = File.ReadAllText(filepath);
            return GetDict2(eTime, reqTime, addVal, s);
        }

        private Dictionary<DateTime, BaseDTO> GetDict2(Time eTime, DateTime reqTime, int addVal, string s)
        {
            Dictionary<DateTime, BaseDTO> d = new Dictionary<DateTime, BaseDTO>();

            // Divide all pairs (remove empty strings)
            string[] tokens = s.Split(new char[] { ',', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            // Extract Time info
            DateTime compareTime = reqTime;
            switch (eTime)
            {
                case Time.Sec: compareTime = compareTime.AddSeconds(addVal); break;
                case Time.Min: compareTime = compareTime.AddMinutes(addVal); break;
                case Time.Hour: compareTime = compareTime.AddHours(addVal); break;
                case Time.Day: compareTime = compareTime.AddDays(addVal); break;
                case Time.Month: compareTime = compareTime.AddMonths(addVal); break;
                case Time.Year: compareTime = compareTime.AddYears(addVal); break;
            }
            // Walk through each item
            for (int i = 0; i < tokens.Length; i += 2)
            {
                string timeT = tokens[i];
                string freq = tokens[i + 1];

                // Parse the int (this can throw)
                DateTime time = DateTime.ParseExact(timeT, "yyyy.MM.dd:HH:mm:ss", null);
                // DateTime time = Convert.ToDateTime(timeT);
                if (reqTime <= time && time <= compareTime)
                {
                    BaseDTO data = new BaseDTO(time, freq);
                    // Fill the value in the sorted dictionary
                    if (d.ContainsKey(time))
                    {
                        d[time] = data;
                    }
                    else
                    {
                        d.Add(time, data);
                    }
                }
            }
            return d;
        }

        /************************************************************************
         ************************   Load Method   *******************************
         ************************************************************************/

        // Return (year,month,day,hour,min,sec) matched result using bit operator
        public byte HowMatched(DateTime startT, DateTime lastT)
        {
            byte result = 0;
            char[] seps = { '.', ':' };
            String[] values1 = startT.ToString("yyyy.MM.dd:HH:mm:ss").Split(seps);
            String[] values2 = lastT.ToString("yyyy.MM.dd:HH:mm:ss").Split(seps);

            if (values1.Length == values2.Length && values1.Length < 8)
                for (int i = 0; i < values1.Length; ++i)
                    if (values1[i] == values2[i])
                        result += (byte)Math.Pow(2, i);
                    else
                        break;
            return result;
        }

        // Load datas that user requested
        public Dictionary<DateTime,BaseDTO> LoadDataByTime(string name, string startT, string lastT)
        {
            this.tempDic.Clear();
            DateTime startTime = DateTime.ParseExact(startT, "yyyy.MM.dd:HH:mm:ss", null);
            DateTime lastTime = DateTime.ParseExact(lastT, "yyyy.MM.dd:HH:mm:ss", null);

            switch (HowMatched(startTime, lastTime))
            {
                // 나노초
                case 127: break;
                // 초까지 일치
                case 63: break;
                // 분까지 일치
                case 31: LoadDataBySec(name, startTime, lastTime.Minute); break;
                // 시까지 일치
                case 15: LoadDataByMin(name, startTime, lastTime.Minute); break;
                // 일까지 일치
                case 7: LoadDataByHour(name, startTime, lastTime, startTime.Hour, lastTime.Hour); break;
                // 월
                case 3: break;
                // 년
                case 1: break;
            }
            return this.tempDic;
        }
        //Return by 1 sec unit 
        // Max instance => 59
        public void LoadDataBySec(string name, DateTime reqTime, int s2)
        {
            string filePath = directory + "/" + name + "/" + reqTime.ToString("yyyy년 MM월 dd일 HH시") + ".json";
            foreach (KeyValuePair<DateTime, BaseDTO> t in GetDict(filePath, Time.Sec, reqTime, s2 - reqTime.Second))
                this.tempDic.Add(t.Key, t.Value);
        }

        //Return by 1 minute(60sec) unit 
        // Max instance => 59 * 60 + 59 = 3599
        public void LoadDataByMin(string name, DateTime reqTime, int m2)
        {
            string filePath = directory + "/" + name + "/" + reqTime.ToString("yyyy년 MM월 dd일 HH시") + ".json";
            foreach (KeyValuePair<DateTime, BaseDTO> t in GetDict(filePath, Time.Min, reqTime, m2 - reqTime.Minute))
                this.tempDic.Add(t.Key, t.Value);
        }

        // Return by 1 hour(3600sec) unit 
        // Max instance => 23 * 3600 + 3599 = 86399
        public void LoadDataByHour(string name, DateTime startTime, DateTime lastTime, int h1, int h2)
        {
            // start min ~ 59 min
            LoadDataByMin(name, startTime, 59);
            // Repeat until 0~59 min
            while (startTime.AddHours(1) < lastTime && startTime.AddHours(1).Hour < lastTime.Hour)
            //while (startTime.AddHours(1) < lastTime)
            {
                startTime = startTime.AddHours(1);
                LoadDataByMin(name, startTime.AddMinutes(-startTime.Minute), 59);
            }
            // 0 ~ last min
            LoadDataByMin(name, lastTime.AddMinutes(-lastTime.Minute), lastTime.Minute);
        }

        /************************************************************************
         ************************   Save Method   *******************************
         ************************************************************************/
        // Save user requested datas that currently instanced to file
        public void SaveCurrentData(string name, DateTime startTime, DateTime lastTime)
        {
            StartSaving(name, startTime, lastTime);
        }
        private void StartSaving(string name, DateTime startTime, DateTime lastTime)
        {
            DateTime loop = startTime;
            // 3. 시간 단위로 명령 수행
            for (int i = 0; loop.AddHours(i) <= lastTime || loop.AddHours(i).Hour == lastTime.Hour; ++i)
            {
                SaveDataByHour(name, loop.AddHours(i));
                loop = startTime;
            }
        }
        private void SaveDataByHour(string name, DateTime reqTime)
        {
            this.tempDic.Clear();
            lock (this.Dictionary[name])
            {
                // 1. Set file total path
                string filepath = directory +"/" + name + "/" + reqTime.ToString("yyyy년 MM월 dd일 HH시") + ".json"; // 시간 단위로 저장
                // 2. If file has existed, read it to local dictionary
                if (!Directory.Exists(directory + "/" + name))
                    Directory.CreateDirectory(directory + "/" + name);
                if (File.Exists(filepath))
                    LoadDataByMin(name, reqTime.AddMinutes(-reqTime.Minute), 59);
                // 3. Extracts requested datas.
                var tempVar = from t in this.Dictionary[name]
                              where t.Key.ToString("yyyy-MM-dd:HH").Equals(reqTime.ToString("yyyy-MM-dd:HH"))
                              select t;
                // 4. Among current instanced datas, concat requested time datas to local dictionary
                foreach (KeyValuePair<DateTime, BaseDTO> e in tempVar)
                    this.tempDic.Add(e.Key, e.Value);
                // 5. Write datas to file
                File.WriteAllText(filepath, GetLine(this.tempDic));
            }
        }

        /************************************************************************
         *******************   GetSensor Data Method   **************************
         ************************************************************************/
        public PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes"); //"Working Set", Process.GetCurrentProcess().ProcessName
        static ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"); //Process.GetCurrentProcess().ProcessName
        List<BaseDTO> result = new List<BaseDTO>();
        ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
        public BaseDTO GetDataFromMem()
        {

            ulong memory = 0;
            double usageGB = 0;
            foreach (ManagementObject item in searcher.Get())
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            float val = memCounter.NextValue();
            usageGB = ((memory / 1024) - val) / 1000;
            return new BaseDTO(DateTime.Now, usageGB);
        }
        public BaseDTO GetDataFromCPU()
        {
            byte rate = 0;
            rate = (byte)cpuCounter.NextValue();
            return new BaseDTO(DateTime.Now, rate);
        }
        public BaseDTO GetDataFromTemp()
        {
            object temperature = 0;
            // 온도 추출 후 경우 중 가장 큰 온도를 현재 온도로 지정
            foreach (ManagementObject obj in searcher2.Get())
            {
                Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                result.Add(new BaseDTO(DateTime.Now, temp));
                // Console.WriteLine(" Time : " + DateTime.Now.ToLongTimeString() + " 현재온도 : " + temp + "℃");
            }
            temperature = result.Max(BaseDTO => BaseDTO.Data);
            return new BaseDTO(DateTime.Now, temperature);
        }
        public BaseDTO[] GetDataFromModeBus(ushort sI, ushort size)
        {
            BaseDTO[] dtos = null;
            using (TcpClient client = new TcpClient("127.0.0.1", 502))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);
                var readed = master.ReadHoldingRegisters(1, sI, size);
                dtos = new BaseDTO[readed.Length];
                DateTime datetime = DateTime.Now;
                for (int i = 0; i < dtos.Length; ++i)
                {
                    dtos[i] = new BaseDTO(datetime, readed[i]);
                }
            }
            return dtos;
        }
        public BaseDTO GetSensorData(string sensor)
        {
            BaseDTO o = null;
            switch (sensor)
            {
                case "temperature": o = GetDataFromTemp(); break;
                case "cpu occupied": o = GetDataFromCPU();  break;
                case "memory usage": o = GetDataFromMem(); break;
                case "modbus_1": o = GetDataFromModeBus(0, 1)[0]; break;
            }
            return o;
        }
    }
}
