
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace WindowForm_Dongik
{
    /*
        json 파일 형식 : yyyy년 MM월 dd일 HH시.json (1시간 단위)
        날짜 데이터 이동 형식 : yyyy.MM.dd:HH:mm:ss    
     */
    class TemperatureLoadManager
    {
        //저장 디렉토리
        private string directory = @"D:/Testfiles/Dongik_Json/";
        //임시 배열 리스트
        private List<Temperature> tempList = new List<Temperature>();


        //비트 연산자를 이용한 년,월,일,시,분,초,나노초 일치 결과 반환
        public byte HowMatched(DateTime startT, DateTime lastT)
        {
            byte result = 0;
            char []seps = {'.',':'};
            String[] values1 = startT.ToString().Split(seps);
            String[] values2 = lastT.ToString().Split(seps);

            if(values1.Length == values2.Length && values1.Length < 8)
                for( int i = 0; i < values1.Length; ++i)
                    if(values1[i] == values2[i])
                        result += (byte)Math.Pow(2, i);
                    else
                        break;
            return result;
        }

        //시작시간과 종료시간 입력 시
        public List<Temperature> LoadJsonByTime(string startT, string lastT)
        {
            DateTime startTime = DateTime.ParseExact(startT, "yyyy.MM.dd:HH:mm:ss", null);
            DateTime lastTime = DateTime.ParseExact(lastT, "yyyy.MM.dd:HH:mm:ss", null);

            switch (HowMatched(startTime, lastTime))
            {
                // 나노초
                case 127: break;
                // 초까지 일치
                case 63:  break;
                // 분까지 일치
                case 31: tempList = LoadJsonBySec(startTime, startTime.Minute, lastTime.Minute); break;
                // 시까지 일치
                case 15: tempList = LoadJsonByMin(startTime, startTime.Minute, lastTime.Minute); break;
                // 일까지 일치
                case 7:  tempList = LoadJsonByHour(startTime, lastTime, startTime.Hour, lastTime.Hour); break;
                // 월
                case 3: break;
                // 년
                case 1: break;
            }
            return tempList;
        }
        //초 단위 반환 - 최대 59 개의 객체
        public List<Temperature> LoadJsonBySec(DateTime currentTime, int s1, int s2)
        {
            List<Temperature> tempList = LoadJsonByMin(currentTime, currentTime.Minute, currentTime.Minute);
            tempList.OrderBy(Temperature=>Temperature.time.Second);
            int i1 = tempList.FindIndex(Temperature => Temperature.time.Second >= s1);
            int i2 = tempList.FindIndex(Temperature => Temperature.time.Second <= s2);
            return tempList.GetRange(i1,i2);
        }
        //1분(60초) 단위로 반환 System.DateTime.Now.ToString("yyyy.MM.dd:HH:mm"); 
        // - 59 * 60 + 59 = 최대 3599 개의 객체
        public List<Temperature> LoadJsonByMin(DateTime currentTime, int m1, int m2)
        {
            // DateTime currentTime = DateTime.ParseExact(time, "yyyy.MM.dd:HH:mm", null);
            string filePath = directory + currentTime.ToString("yyyy년 MM월 dd일 HH시") + ".json";
            using (StreamReader r = new StreamReader(filePath))
            {
                var json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Temperature>>(json);
                string min = currentTime.ToString("yyyy.MM.dd:HH:mm");
                foreach (var item in items)
                {
                    if(item.time.Minute >= m1 && item.time.Minute <=m2)
                      tempList.Add(new Temperature(item.temperature, item.time));
                }
            }
            return tempList;
        }

        //시간(3600초)단위로 반환 - 23 * 3600 + 3599 = 최대 86399 개의 객체
        public List<Temperature> LoadJsonByHour(DateTime startTime, DateTime lastTime, int h1, int h2)
        {
            //start시간 ~ 59분까지
            tempList.Concat(LoadJsonByMin(startTime, startTime.Minute, 59));
            //0~59분까지 반복
            while (startTime.AddHours(1) < lastTime)
            {
                startTime = startTime.AddHours(1);
                tempList.Concat(LoadJsonByMin(startTime, 00, 59));
            }
            //0~last시간
            tempList.Concat(LoadJsonByMin(lastTime, 0, lastTime.Minute));
            // tempList.OrderBy(Temperature => Temperature.time.Hour);
            return tempList;
        }






        //1시간(3600초) 단위로 반환 System.DateTime.Now.ToString("yyyy.MM.dd:HH:mm");
        public List<Temperature> LoadJsonByTime2(string startT, string lastT)
        {
            DateTime startTime = DateTime.ParseExact(startT, "yyyy.MM.dd:HH:mm", null);
            DateTime lastTime = DateTime.ParseExact(lastT, "yyyy.MM.dd:HH:mm", null);
            if (lastTime.Month > startTime.Month)
            {
            }
            else
            {
                // 일까지 같을 경우
                if (startTime.ToShortDateString().Equals(lastTime.ToShortDateString()) && lastTime.Hour > startTime.Hour)
                {
                    //시간이 다를 경우
                    if (lastTime.Hour > startTime.Hour)
                    {
                        //start시간 ~ 59분까지
                        tempList.Concat(LoadJsonByMin(startTime, startTime.Minute, 59));
                        //0~59분까지 반복
                        while (startTime.AddHours(1) < lastTime)
                        {
                            tempList.Concat(LoadJsonByMin(startTime, 00, 59));
                        }
                        //0~last시간
                        tempList.Concat(LoadJsonByMin(lastTime, lastTime.Minute, 59));
                    }
                    //시간이 같을 경우
                    else
                        tempList.Concat(LoadJsonByMin(startTime, startTime.Minute, lastTime.Minute));
                }
            }
            return tempList;
        }


        /*public List<Temperature> LoadJsonByTime(string startT, string lastT)
        {
            DateTime startTime = DateTime.ParseExact(startT, "yyyy.MM.dd:HH:mm", null);
            DateTime lastTime = DateTime.ParseExact(startT, "yyyy.MM.dd:HH:mm", null);
            TimeSpan result = lastTime - startTime;


            switch(startTime.Year){
                
            }
            if(startTime.Year == lastTime.Year){
                if(startTime.Month == lastTime.Month){
                    if(startTime.Day == lastTime.Day){
                        if(startTime.Hour == lastTime.Hour){
                            if(startTime.Minute == lastTime.Minute){ // 분까지 같은 경우
                                tempList.Concat(LoadJsonByMin(startTime.ToString()));
                            }
                            else{
                                while(startTime.Hour == lastTime.Hour){
                                    tempList.Concat(LoadJsonByMin(startTime.ToString()));
                                    startTime.AddHours(1);
                                }
                            }
                        }
                        else{
                            while(startTime.Hour == lastTime.Hour){
                                tempList.Concat(LoadJsonByMin(startTime.ToString()));
                                startTime.AddHours(1);
                            }
                        }
                    }
                }
            }
            
            //1시간 이상 차이날 경우
            if (result.Hours < 1) {
                if(res
                return LoadJsonByMin(startTime);
            }
        }*/

    }
}
