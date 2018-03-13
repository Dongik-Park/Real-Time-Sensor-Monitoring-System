# Real-Time-Sensor-Monitoring-System

This project is based on C#, Winforms, MS-SQL.

It has three basic functions.
1. Real-time PC(Core Temperatrue, CPU occupied, Memory Usage), Modbus, Vibration sensor monitoring. 
2. Sensor data save & search
3. Sensor configs manage

Check these three points when you using this project.
1. If you want to add new configs of sensor, your SensorReader should be inherited BaseSensorReader. 
2. i utilized linq-to-sql ORM to save sensor data on MS-SQL. So, you should check SensorData.dbml.
3. Actived sensors read, upload UI chart and DB - transaction occurs every 2 seconds. 

![image](https://user-images.githubusercontent.com/34857208/37350774-f7ef5ac2-271c-11e8-976d-144ed6fc43bb.png)
![image](https://user-images.githubusercontent.com/34857208/37350792-0163caca-271d-11e8-974d-ff9f2fda1183.png)
