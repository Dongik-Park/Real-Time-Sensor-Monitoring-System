using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace WindowForm_Dongik
{
    /*
        json 파일 형식 : yyyy년 MM월 dd일 HH시.json (1시간 단위)
        날짜 데이터 이동 형식 : yyyy.MM.dd:HH:mm:ss    
     */
    class TemperatureSaveManager
    {
        //저장 디렉토리
        private string directory = @"D:/Testfiles/Dongik_Json/";

        //임시 배열 리스트
        private List<Temperature> tempList = null;
        
        //동일한 시간을 가진 데이터만 들어온다.
        public void SaveJsonByHour(List<Temperature> tempList)
        {
            lock (tempList)
            {
                // 0. 시간 데이터 검사
                int hour = tempList[0].time.Hour;
                foreach (Temperature t in tempList)
                    if (hour != t.time.Hour)
                    {
                        Console.WriteLine("TemperatureSaveManager.SaveJsonByHour() - 시간오류");
                        return;
                    }
                // 1.파일 존재 시 데이터 읽기 
                string filepath = directory + tempList[0].time.ToString("yyyy년 MM월 dd일 HH시") + ".json"; // 시간 단위로 저장
                // 2.기존에 파일이 존재할 경우 데이터를 읽어온다.
                if (File.Exists(filepath))
                {
                    using (StreamReader r = new StreamReader(filepath))
                    {
                        var json = r.ReadToEnd();
                        var items = JsonConvert.DeserializeObject<List<Temperature>>(json);
                        foreach (var item in items)
                        {
                            tempList.Add(new Temperature(item.temperature, item.time));
                        }
                    }
                }
                // 3.파일에 데이터 작성
                using (StreamWriter file = File.CreateText(filepath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    // 4.시간 순서로 데이터 삽입
                    serializer.Serialize(file, tempList.OrderBy(x => x.time.TimeOfDay).ToList());
                }
            }
        }



        /*
            1. JSON Data 저장 정책
            2. DAO, DTO
            3. CSV
            4. 원하는 CPU DATA 값으로 테스트 할 수 있는지 ( 추후 )
         * 
         * 
         *  1. JSON 저장 완성 후(백업한 다음) 추후 CSV로 변경
         */




        // 현재 instance데이터를 파일로 저장한다.
        public void SaveCurrentData(List<Temperature> tempList)
        {
            // 1. 저장할 인스턴스 List 복사
            this.tempList = tempList;
            // 2. 파일 저장 스레드 생성
            Thread saveThread = new Thread(new ThreadStart(StartSaving));
            // 3. 스레드 시작
            saveThread.Start();
            // 4. 스레드 종료 대기
            saveThread.Join();
        }
        // 스레드 루틴
        private void StartSaving()
        {
            // 1.시간 순서로 정렬(nlng)
            this.tempList.OrderBy(Temperature => Temperature.time);
            // 2.최소 & 최대 시간 추출
            DateTime startTime = this.tempList[0].time;
            DateTime lastTime = this.tempList[tempList.Count-1].time;
            DateTime loop = startTime;
            // 3. 시간 단위로 명령 수행
            for (int i = 0; loop.AddHours(i).Hour <= lastTime.Hour; ++i)
            {
                loop = startTime;
                // 4. 현재 loop의 시간 별 데이터 검색 
                var list = from temp in this.tempList
                           where temp.time.Year == loop.Year && temp.time.Month == loop.Month && temp.time.Hour == loop.Hour
                           orderby temp.time
                           select temp;
                // 5. 파일 루틴 수행 메소드 호출
                SaveJsonByHour(list.ToList());
            }
        }




        /*public void SaveJsonByHour2(List<Temperature> tempList)
        {
            //IEnumerable<int> dayList = tempList.Select(Temperature => Temperature.time.Day).Distinct();
            //dayList.
            //List<DateTime> hourList = tempList.Select(Temperature=>Temperature.time.Hour).Distinct();
            //1.제일 오래된 시간의 인덱스의 시간 파일 작성
            DateTime minHour = tempList[0].time;
            foreach (Temperature t in tempList)
            {
                if (DateTime.Compare(minHour, t.time) < 0)
                    minHour = t.time;
            }
            //2.파일 존재 시 데이터 읽기 
            string filepath = minHour.ToString("yyyy년 MM월 dd일 HH시") + ".json"; // 시간 단위로 저장
            if(File.Exists(filepath)){
                using (StreamReader r = new StreamReader(filepath))
                {
                    var json = r.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<List<Temperature>>(json);
                    foreach (var item in items)
                    {
                           tempList.Add(new Temperature(item.temperature, item.time));
                    }
                }
            }
            //3.시간 순서로 정렬(nlng)
            tempList.OrderBy(Temperature => Temperature.time);
            //4.시간이 변하는 구간까지 검색
            var list = from temp in tempList
                       where temp.time.Year==minHour.Year && temp.time.Month==minHour.Month && temp.time.Hour == minHour.Hour
                       orderby temp.time
                       select temp; 
            using (StreamWriter file = File.CreateText(@"D:\path.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, list.ToArray());
            }



            foreach (Temperature t in tempList)
            {
            }
            string readResult = string.Empty;
            string writeResult = string.Empty;
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                readResult = jobj.ToString();
                foreach (var item in jobj.Properties())
                {
                    item.Value = item.Value.ToString().Replace("v1", "v2");
                }
                writeResult = jobj.ToString();
                Console.WriteLine(writeResult);
            }
            Console.WriteLine(readResult);
            File.WriteAllText(filepath, writeResult);
        }*/
    }
}
