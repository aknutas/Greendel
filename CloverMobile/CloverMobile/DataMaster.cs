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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace CloverMobile
{
    public class DataMaster
    {
        private bool isUpdated;
        private XDocument dataDoc;
        private List<Output> allOutputs; 
        private List<Sensor> allSensors;
        private List<HistoryData> allHistoryData;

        public DataMaster()
        {
            allOutputs = new List<Output>();
            allSensors = new List<Sensor>();
            allHistoryData = new List<HistoryData>();
        }
        public List<HistoryData> getHistory()
        {
            return allHistoryData;
        }
        public List<Sensor> getSensors()
        {
            return allSensors;
        }
        public List<Output> getOutputs()
        {
            return allOutputs;
        }

        public void WriteHistory(XDocument xmlDoc)
        {
            allHistoryData.Clear();
            var historyValues = from historyValue in xmlDoc.Descendants("hash")
            select new HistoryData
            {
                powerConsumption0 = int.Parse(historyValue.Element("today").Value),
                powerConsumption1 = int.Parse(historyValue.Element("yesterday").Value),
                powerConsumption2 = int.Parse(historyValue.Element("daysago2").Value),
                powerConsumption3 = int.Parse(historyValue.Element("daysago3").Value),
            };
            foreach (HistoryData hd in historyValues)
            {
                allHistoryData.Add(hd);
            }
            isUpdated = true;
        }
        public void WriteOutputs(XDocument xmlDoc)
        {
            allOutputs.Clear();
            var dataUnitoutputs = from output in dataDoc.Descendants("output")
            select new Output
            {
                created = DateTime.Parse(output.Element("created-at").Value),
                deviceId = int.Parse(output.Element("device-id").Value),
                id = int.Parse(output.Element("id").Value),
                name = output.Element("name").Value.ToString(),
                state = bool.Parse(output.Element("state").Value),
                updatedAt = DateTime.Parse(output.Element("updated-at").Value),
            };
            foreach (Output o in dataUnitoutputs)
            {
                allOutputs.Add(o);
            }
            isUpdated = true;        
        }
        public void WriteSensors(XDocument xmlDoc)
        {
            allSensors.Clear();
            var sensorOutputs = from sensor in dataDoc.Descendants("sensor")
            select new Sensor
            {
                createdAt = DateTime.Parse(sensor.Element("created-at").Value),
                deviceId = int.Parse(sensor.Element("device-id").Value),
                id = int.Parse(sensor.Element("id").Value),
                latestReading = int.Parse(sensor.Element("latestreading").Value),
                name = sensor.Element("name").Value.ToString(),
                updatedAt = DateTime.Parse(sensor.Element("updated-at").Value),
            };
            foreach (Sensor s in sensorOutputs)
            {
                allSensors.Add(s);
            }
            isUpdated = true;
        }
    }
}
