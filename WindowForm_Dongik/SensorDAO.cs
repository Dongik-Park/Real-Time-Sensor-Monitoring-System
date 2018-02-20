using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Json;

namespace WindowForm_Dongik
{
    class SensorDAO
    {
        private string fileDirectory = @"D:/Testfiles/Dongik_Json";
        private string filepath_tot = @"D:/Testfiles/Dongik_Json/Sensors/sensors_tot.txt";

        public Dictionary<string, SensorDTO> Sensors = new Dictionary<string, SensorDTO>();

        static SensorDAO instance;
        public static SensorDAO Instance { get { return instance; } }
        
        static SensorDAO()
        {
            instance = new SensorDAO();
        }

        private SensorDAO()
        {
            //this.AddSensor("temperature");
            //this.AddSensor("cpu occupied");
            //this.AddSensor("memory usage");
            //this.AddSensor("modbus");
        }

        public void AddSensor(string name)
        {
            this.Sensors.Add(name, new SensorDTO(name, 1));
        }

        /************************************************************************
         ************************   IO Method   *******************************
         ************************************************************************/
        // Copy source directory data to destination directory
        private void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
                file.Delete();
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryMove(subdir.FullName, temppath, copySubDirs);
                }
            }
            Directory.Delete(sourceDirName);
        }
        public bool AddSensorByJson(JsonObjectCollection col)
        {
            string destDirName = fileDirectory + "/" + col["name"].GetValue();
            // 추가할 경우
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.WriteAllText(destDirName + "/" + col["name"].GetValue() + "_config.txt", 
                                    col.ToString());
                return true;
            }
            else
                return false;
        }
        public bool EditSensorByJson(SensorDTO oldInfo, JsonObjectCollection col)
        {
            string sourceDirName = fileDirectory + "/" + oldInfo.Name;
            string destDirName = fileDirectory + "/" + col["name"].GetValue();


            //string tempDirName = fileDirectory + "/TEMP";
            if (oldInfo.Name.Equals(col["name"].GetValue()))
            {
                File.WriteAllText(destDirName + "/" + col["name"].GetValue() + "_config.txt",
                                    col.ToString());
                return true;
            }
            else { //directory name has been changed
                //DirectoryMove(sourceDirName, destDirName, true);
                Directory.Move(sourceDirName, destDirName);            
                return true;
            }
        }
        

        public void SaveConfigByJson(JsonObjectCollection[] cols)
        {
            foreach (JsonObjectCollection col in cols)
            {
                File.WriteAllText(fileDirectory + "/" + col["name"].GetValue() + "/" + col["name"].GetValue() + "_config.txt", col.ToString());
            }
        }
        public string[] LoadSensorList()
        {
            string[] subdirectoryEntries = Directory.GetDirectories(fileDirectory);
            string[] list = new string[subdirectoryEntries.Length];
            for (int i = 0; i < list.Length; ++i)
                list[i] = subdirectoryEntries[i].Substring(fileDirectory.Length + 1);
            return list;
        }
        public Dictionary<string, SensorDTO> LoadConfigByJson()
        {
            string[] list = LoadSensorList();
            Dictionary<string, SensorDTO> returnVal = new Dictionary<string, SensorDTO>();
            for (int i = 0; i < list.Length; ++i)
            {
                if(!File.Exists((fileDirectory + "/" + list[i] + "/" + list[i] + "_config.txt")))
                    continue;

                string str = File.ReadAllText(fileDirectory + "/"+ list[i] + "/" + list[i] + "_config.txt");
                
                JsonTextParser jsonText = new JsonTextParser();
                JsonObject temp = jsonText.Parse(str);
                JsonObjectCollection col = (JsonObjectCollection)temp;
                SensorDTO dto = new SensorDTO();
                foreach (JsonObject o in col)
                {
                    switch (o.Name.ToString())
                    {
                        case "name": dto.Name = o.GetValue().ToString(); break;
                        case "date":
                            DateTime time = DateTime.ParseExact(o.GetValue().ToString(), "yyyy.MM.dd:HH:mm:ss", null);
                            dto.AddDate = time;
                            break;
                        case "active":
                            byte active = Convert.ToByte(o.GetValue().ToString());
                            dto.IsActive = active;
                            break;
                        case "type" :
                            dto.Type = o.GetValue().ToString();
                            break;
                        case "id" :
                            dto.Id = o.GetValue().ToString();
                            break;
                        default:
                            //if(!o.Name.ToString().Equals("id"))
                             dto.ExtraInfo.Add(o.Name, o.GetValue().ToString());
                            break;
                    }
                }
                returnVal.Add(list[i], dto);
            }
            return returnVal;
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
