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
        public Dictionary<string, Dictionary<string, BaseDTO>> Dictionary { get; set; }
        private Dictionary<string, List<BaseDTO>> tempDic = new Dictionary<string, List<BaseDTO>>();
        //Directory info
        private const string directory = @"D:/Testfiles/Dongik_Json/";

        //Constructor
        public BaseDAO()
        {
            if (this.Dictionary == null)
                this.Dictionary = new Dictionary<string, Dictionary<string, BaseDTO>>();
        }
        public BaseDAO(Dictionary<string, Dictionary<string, BaseDTO>> Dictionary)
        {
            this.Dictionary = Dictionary;
        }

        //Custom method
        public void AddSensor(string name)
        {
            if(!this.Dictionary.ContainsKey(name))
              this.Dictionary[name] = new Dictionary<string, BaseDTO>();
        }
        public void AddData(string name, BaseDTO dto)
        {
            AddSensor(name);
            if (dto == null)
                return;
            if (!this.Dictionary[name].ContainsKey(dto.SensorCategory))
                this.Dictionary[name].Add(dto.SensorCategory, dto);
        }

        /************************************************************************
         ************************   IO Method   *******************************
         ************************************************************************/
        public void FileSave(BaseDTO baseDTO)
        {
            // 1. Set file total path
            string filepath = directory + "/" +
                                baseDTO.SensorName + "/" +
                                    baseDTO.Time.ToString("yyyy년 MM월 dd일 HH시") + ".json"; // 시간 단위로 저장
            //FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            
            // FileStream을 써야하지 않을까?
            if (File.Exists(filepath))
                File.AppendAllText(filepath, GetLine(baseDTO));
            else
                File.WriteAllText(filepath, GetLine(baseDTO));
        }
        enum Time { Sec, Min, Hour, Day, Month, Year }
        private string GetLine(BaseDTO dto)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            builder.Append(dto.SensorCategory).Append(",")
                    .Append(dto.Time.ToString("yyyy.MM.dd:HH:mm:ss")).Append(",")
                      .Append(dto.Data).Append("\n");
            return builder.ToString();
        }
        private string GetLine(Dictionary<string, BaseDTO> dictionary)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, BaseDTO> pair in dictionary)
            {
                builder.Append(pair.Value.SensorCategory).Append(",")
                       .Append(pair.Value.Time.ToString("yyyy.MM.dd:HH:mm:ss"))
                       .Append(",").Append(pair.Value.Data).Append("\n");
            }
            string result = builder.ToString();
            // Remove the final delimiter
            result = result.TrimEnd('\n');
            return result;
        }

        private Dictionary<string, List<BaseDTO>> GetDict(string filepath, Time eTime, DateTime reqTime, int addVal)
        {
            string s = "";
            if(File.Exists(filepath))
               s = File.ReadAllText(filepath);
            return GetDict2(eTime, reqTime, addVal, s);
        }

        private Dictionary<string, List<BaseDTO>> GetDict2(Time eTime, DateTime reqTime, int addVal, string s)
        {
            Dictionary<string, List<BaseDTO>> d = new Dictionary<string, List<BaseDTO>>();
            if (s.Length < 2)
                return d;
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
            for (int i = 0; i < tokens.Length; i += 3)
            {
                string category = tokens[i];
                string timeT = tokens[i + 1];
                string data = tokens[i + 2];

                // Parse the int (this can throw)
                DateTime time = DateTime.ParseExact(timeT, "yyyy.MM.dd:HH:mm:ss", null);
                // DateTime time = Convert.ToDateTime(timeT);
                if (reqTime <= time && time <= compareTime)
                {
                    BaseDTO dto = new BaseDTO(category,time, data);
                    // Fill the value in the sorted dictionary
                    if (!d.ContainsKey(category))
                    {
                        d[category] = new List<BaseDTO>();
                    }
                    d[category].Add(dto);
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
        public Dictionary<string,List<BaseDTO>> LoadDataByTime(string name, string startT, string lastT)
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
                case 7: 
                // 월
                case 3: 
                // 년
                case 1: LoadDataByHour(name, startTime, lastTime); break;
            }
            return this.tempDic;
        }
        //Return by 1 sec unit 
        // Max instance => 59
        public void LoadDataBySec(string name, DateTime reqTime, int s2)
        {
            string filePath = directory + "/" + name + "/" + reqTime.ToString("yyyy년 MM월 dd일 HH시") + ".json";
            foreach (KeyValuePair<string, List<BaseDTO>> t in GetDict(filePath, Time.Sec, reqTime, s2 - reqTime.Second))
                this.tempDic.Add(t.Key, t.Value);
        }

        //Return by 1 minute(60sec) unit 
        // Max instance => 59 * 60 + 59 = 3599
        public void LoadDataByMin(string name, DateTime reqTime, int m2)
        {
            string filePath = directory + "/" + name + "/" + reqTime.ToString("yyyy년 MM월 dd일 HH시") + ".json";
            foreach (KeyValuePair<string, List<BaseDTO>> t in GetDict(filePath, Time.Min, reqTime, m2 - reqTime.Minute))
            {
                if (this.tempDic.ContainsKey(t.Key))
                    this.tempDic[t.Key].AddRange(t.Value);
                else
                    this.tempDic.Add(t.Key, t.Value);
            }
        }

        // Return by 1 hour(3600sec) unit 
        // Max instance => 23 * 3600 + 3599 = 86399
        public void LoadDataByHour(string name, DateTime startTime, DateTime lastTime)
        {
            // start min ~ 59 min
            LoadDataByMin(name, startTime, 59);
            // Repeat until 0~59 min
            //while (startTime.AddHours(1) < lastTime && startTime.AddHours(1).Hour < lastTime.Hour)
            while (startTime.AddHours(1) < lastTime)
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
                //SaveDataByHour(name, loop.AddHours(i));
                loop = startTime;
            }
        }
        /*private void SaveDataByHour(string name, DateTime reqTime)
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
                              where t.Value.Time.ToString("yyyy-MM-dd:HH").Equals(reqTime.ToString("yyyy-MM-dd:HH"))
                              select t;
                // 4. Among current instanced datas, concat requested time datas to local dictionary
                foreach (KeyValuePair<string, BaseDTO> e in tempVar)
                    this.tempDic.Add(e.Key, e.Value);
                // 5. Write datas to file
                File.WriteAllText(filepath, GetLine(this.tempDic));
            }
        }*/

        /************************************************************************
         *******************   GetSensor Data Method   **************************
         ************************************************************************/
        public PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes"); //"Working Set", Process.GetCurrentProcess().ProcessName
        static ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

        PerformanceCounter totCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"); 
        PerformanceCounter curCpuCounter = new PerformanceCounter("Processor", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        //List<BaseDTO> result = new List<BaseDTO>();
        ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

        //--------------Memory----------------
        public BaseDTO GetDataFromMem(SensorDTO sensorDTO)
        {
            return GetDataFromMem2(sensorDTO)["NoExtra"];
        }

        public Dictionary<string, BaseDTO> GetDataFromMem2(SensorDTO sensorDTO)
        {
            Dictionary<string, BaseDTO> cpuData = new Dictionary<string, BaseDTO>();
            ulong memory = 0;
            double usageGB = 0;
            byte rate = 0;
            DateTime cur = DateTime.Now;
            foreach (ManagementObject item in searcher.Get())
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            float val = memCounter.NextValue();
            usageGB = ((memory / 1024) - val) / 1000;
            rate = (byte)(val / (memory / 1024) * 100);

            cpuData.Add("NoExtra", new BaseDTO(sensorDTO.Name, "usage", cur, usageGB));
            //cpuData.Add("usage", new BaseDTO(sensorDTO.Name, "usage", cur, usageGB));
            //cpuData.Add("rate", new BaseDTO(sensorDTO.Name,"rate",cur, rate));

            return cpuData;
        }
        
        //----------------CPU-----------------
        public BaseDTO GetDataFromCPU(SensorDTO sensorDTO)
        {
            return GetDataFromCPU2(sensorDTO)["NoExtra"];
        }
        public Dictionary<string, BaseDTO> GetDataFromCPU2(SensorDTO sensorDTO)
        {
            Dictionary<string, BaseDTO> cpuData = new Dictionary<string, BaseDTO>();
            byte rate = 0;
            string category = "";
            /*if (sensorDTO.ExtraInfo["total"].Contains("1"))
            {
                cpuCounter.InstanceName = "_Total";
                category = "total";
            }
            else
            {
                cpuCounter.InstanceName = Process.GetCurrentProcess().ProcessName;
                category = "current";
            }*/
            rate = (byte)totCpuCounter.NextValue();
                /*switch (sensorDTO.ExtraInfo["process"])
                {
                    case "total": rate = (byte)cpuCounter.NextValue(); break;
                    case "current": rate = (byte)(new PerformanceCounter("Processor",
                                                                        "% Processor Time",
                                                                         Process.GetCurrentProcess().ProcessName)).NextValue();
                        break;
                }*/
            cpuData.Add("NoExtra", new BaseDTO(sensorDTO.Name, category, DateTime.Now, rate));
            return cpuData;
        }
       
        //------------Temperature-------------
        public BaseDTO GetDataFromTemp(SensorDTO sensorDTO)
        {
            BaseDTO dto = null;
            Dictionary<string,BaseDTO> dic = GetDataFromTemp2(sensorDTO);
            foreach (KeyValuePair<string, string> d in sensorDTO.ExtraInfo)
                if (dic.ContainsKey(d.Key))
                    dto = dic[d.Key];
            return dto;
        }
        public Dictionary<string, BaseDTO> GetDataFromTemp2(SensorDTO sensorDTO)
        {
            object temperature = 0;

            Dictionary<string, BaseDTO> cpuData = new Dictionary<string, BaseDTO>();
            int i = 1; 
            string tempStr = "";
            DateTime cur = DateTime.Now;
            foreach(ManagementObject col in searcher2.Get()){
                tempStr = "core" + i++;
                if(sensorDTO.ExtraInfo.ContainsKey(tempStr) && sensorDTO.ExtraInfo[tempStr].Equals("1")){
                    Double temp = Convert.ToDouble(col["CurrentTemperature"].ToString());
                    temp = (temp - 2732) / 10.0;
                    cpuData.Add(tempStr, new BaseDTO(sensorDTO.Name,tempStr, cur, temp));
                }
            }
            /*
            foreach (ManagementObject obj in searcher2.Get())
            {
                Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                result.Add(new BaseDTO(DateTime.Now, temp));
                // Console.WriteLine(" Time : " + DateTime.Now.ToLongTimeString() + " 현재온도 : " + temp + "℃");
            }
            temperature = result.Max(BaseDTO => BaseDTO.Data);*/

            return cpuData;
        }
        //---------------Modbus----------------
        public Dictionary<string, BaseDTO> GetDataFromModBus(SensorDTO sensorDTO)
        {
            Dictionary<string, BaseDTO> cpuData = new Dictionary<string, BaseDTO>();
            BaseDTO[] dtos = null;

            string ip = sensorDTO.ExtraInfo["ip"];
            int port = Convert.ToInt32(sensorDTO.ExtraInfo["port"]);
            ushort addr = Convert.ToUInt16(sensorDTO.ExtraInfo["address"]);
            ushort size = Convert.ToUInt16(sensorDTO.ExtraInfo["size"]);

            using (TcpClient client = new TcpClient(ip, port))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);
                var readed = master.ReadHoldingRegisters(1, addr, size);
                dtos = new BaseDTO[readed.Length];
                DateTime datetime = DateTime.Now;
                for (int i = 0; i < dtos.Length; ++i)
                {
                    cpuData.Add((addr + i) + "", new BaseDTO(sensorDTO.Name,(addr+i)+"",datetime, readed[i]));
                }
            }
            return cpuData;
        }
        public Dictionary<string, BaseDTO> GetSensorData(SensorDTO sensor)
        {
            Dictionary<string, BaseDTO> o = null;
            switch (sensor.Type)
            {
                case  "temperature": o = GetDataFromTemp2(sensor);    break;
                case "cpu occupied": o = GetDataFromCPU2(sensor);     break;
                case "memory usage": o = GetDataFromMem2(sensor);     break;
                case       "modbus": o = GetDataFromModBus(sensor);   break;
            }
            return o;
        }
    }
}
