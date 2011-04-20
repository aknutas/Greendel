using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CloverMobile
{
    public class WorkItem
    {
        public string documentName { get; set; }
        public int deviceId { get; set; }
        public int sensorId { get; set; }
        public string historyInfoType { get; set; }
        public string frequency { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int pointsToGet { get; set; }

        public WorkItem(string workName)
        {
            documentName = workName;
            deviceId = 0;
            sensorId = 0;
        }
        public WorkItem(string workName, int deviceID)
        {
            documentName = workName;
            deviceId = deviceID;
            sensorId = 0;
        }
        public WorkItem(string workName, int deviceID, int sensorID)
        {
            documentName = workName;
            sensorId = sensorID;
            deviceId = deviceID;
        }
        public WorkItem(string workName, int deviceID, int sensorID, string type, string freq, string startDate, string endDate)
        {
            documentName = workName;
            deviceId = deviceID;
            sensorId = sensorID;
            historyInfoType = type;
            frequency = freq;
            start = startDate;
            end = endDate;     
        }
        public WorkItem(string workName, int deviceID, int sensorID, int points)
        {
            documentName = workName;
            sensorId = sensorID;
            deviceId = deviceID;
            pointsToGet = points;
        }
    }
}
