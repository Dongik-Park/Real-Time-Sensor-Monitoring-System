using System;
using System.Threading;
using System.Management;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowForm_Dongik;
namespace UnitTest_ver1
{
    [TestClass]
    public class UnitTest1
    {
       // BaseDAO baseDAO = new BaseDAO();
        [TestMethod]
        public void TestMethod1()
        {/*
            int cnt = 0;
            baseDAO.AddSensor("temperature sensor");
            baseDAO.AddSensor("cpu occupied");
            baseDAO.AddSensor("memory usage");
            while (true)
            {
                baseDAO.AddData("temperature sensor", baseDAO.GetDataFromTemp());
                baseDAO.AddData("cpu occupied", baseDAO.GetDataFromCPU());
                baseDAO.AddData("memory usage", baseDAO.GetDataFromMem());
                if (++cnt == 5)
                    break;
                Thread.Sleep(1000);
            }
            string testSensor = "temperature sensor";
            DateTime d1 = DateTime.Now.AddMinutes(-1);
            DateTime d2 = DateTime.Now;
            this.baseDAO.SaveCurrentData(testSensor, d1, d2);
            //this.baseDAO.LoadDataByMin(testSensor, d1, d2.Minute);
            Dictionary<DateTime,BaseDTO> currentData = this.baseDAO.Dictionary[testSensor]
                        .Where(t => t.Key >= d1 && t.Key <= d2)
                        .OrderBy(t => t.Key.TimeOfDay)
                        .Select(t => t)
                        .ToDictionary(t => t.Key, t => new BaseDTO(t.Key, t.Value.Data));



            // Method 호출
            Dictionary<DateTime, BaseDTO> tempDoc = baseDAO.LoadDataByTime("temperature sensor", d1.ToString("yyyy.MM.dd:HH:mm:ss"), d2.ToString("yyyy.MM.dd:HH:mm:ss"));

            // 실제 호출
            Dictionary<DateTime, BaseDTO> tempDoc2 = baseDAO.Dictionary["temperature sensor"].Where(t => t.Key <= d2 && t.Key >= d1).Select(t => t).ToDictionary(t => t.Key, t => new BaseDTO(t.Key, t.Value.Data));

            //Assertion Portion
            //Assert.AreEqual(tempDoc, tempDoc2);

            Assert.IsTrue(DateTime.Now > DateTime.Now.AddSeconds(-1));
            Assert.IsTrue(DateTime.Now > DateTime.Now.AddMinutes(-1));
            Assert.IsTrue(DateTime.Now < DateTime.Now.AddYears(1));
            Assert.IsTrue(DateTime.Now < DateTime.Now.AddMonths(3));
            Assert.AreNotEqual(currentData, this.baseDAO.LoadDataByTime(testSensor,d1.ToString("yyyy.MM.dd:HH:mm:ss"),d2.ToString("yyyy.MM.dd:HH:mm:ss")));

            Assert.AreNotEqual(null, baseDAO.GetSensorData("temperature"));*/
        }
        // TDD , agile => TDD 무조건 좋게 나옴(unit test coding - 함수가 testable하게, 모든 함수가 기능별로 쪼개야함 ex) return 타입이 손쉬운), rename & extract method가 주 refactoring
        // Load -> component 구성
        // modu bus sensor
        // Dictionary - 이진 트리로 구성됨 ( hash table처럼 동작)
    }
}
