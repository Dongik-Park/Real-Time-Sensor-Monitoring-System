using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowForm_Dongik
{
    class SensorDAO
    {
        private string filepath = @"D:/Testfiles/Dongik_Json/Sensors/sensors.txt";
        private string filepath_tot = @"D:/Testfiles/Dongik_Json/Sensors/sensors_tot.txt";
        public Dictionary<string, SensorDTO> Sensors = new Dictionary<string, SensorDTO>();

        public SensorDAO()
        {
            //this.AddSensor("temperature");
            //this.AddSensor("cpu occupied");
            //this.AddSensor("memory usage");
            //this.AddSensor("modbus_1");
        }

        public void AddSensor(string name)
        {
            this.Sensors.Add(name, new SensorDTO(name, 1));
        }
        public void SaveSensors()
        {
            this.SaveSensors(this.filepath);
        }
        public void SaveSensors(string filepath)
        {
            File.WriteAllText(filepath, GetLine(this.Sensors));
        }
        private string GetLine(Dictionary<string, SensorDTO> dictionary)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, SensorDTO> pair in dictionary)
            {
                builder.Append(pair.Key).Append(",").Append(pair.Value.AddDate.ToString("yyyy.MM.dd:HH:mm:ss")).Append(',').Append(pair.Value.IsActive).Append("\n");
            }
            string result = builder.ToString();
            // Remove the final delimiter
            result = result.TrimEnd('\n');
            return result;
        }
        public Dictionary<string, SensorDTO> LoadTotalSensors()
        {
            Dictionary<string, SensorDTO> returnDIc = new Dictionary<string, SensorDTO>();
            string s = File.ReadAllText(filepath_tot);// Divide all pairs (remove empty strings)
            string[] tokens = s.Split(new char[] { ',', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            // Walk through each item
            for (int i = 0; i < tokens.Length; i += 2)
            {
                string name = tokens[i];
                string timeT = tokens[i + 1];
                // Parse the int (this can throw)
                DateTime time = DateTime.ParseExact(timeT, "yyyy.MM.dd:HH:mm:ss", null);
                SensorDTO data = new SensorDTO(name, time);
                // Fill the value in the sorted dictionary
                if (returnDIc.ContainsKey(name))
                {
                    returnDIc[name] = data;
                }
                else
                {
                    returnDIc.Add(name, data);
                }
            }
            return returnDIc;
        }
        public Dictionary<string, SensorDTO> LoadSensors()
        {
            return LoadSensors(this.filepath);
        }
        public Dictionary<string, SensorDTO> LoadSensors(string filepath)
        {
            string s = File.ReadAllText(filepath);// Divide all pairs (remove empty strings)
            string[] tokens = s.Split(new char[] { ',', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            // Walk through each item
            for (int i = 0; i < tokens.Length; i += 3)
            {
                string name = tokens[i];
                string timeT = tokens[i + 1];
                string active = tokens[i + 2];
                byte isActive = 1;
                if (active.Contains("0"))
                    isActive = 0;
                // Parse the int (this can throw)
                DateTime time = DateTime.ParseExact(timeT, "yyyy.MM.dd:HH:mm:ss", null);
                SensorDTO data = new SensorDTO(name, time, isActive);
                // Fill the value in the sorted dictionary
                if (this.Sensors.ContainsKey(name))
                {
                    this.Sensors[name] = data;
                }
                else
                {
                    this.Sensors.Add(name, data);
                }
            }
            return this.Sensors;
        }
    }
}
