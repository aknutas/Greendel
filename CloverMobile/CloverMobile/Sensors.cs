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
using System.Collections.ObjectModel;

namespace CloverMobile
{
    public class Sensor
    {        
        public int sensorId { get; set; }
        public string sensorName { get; set; }
        public string longName { get; set; }
        public string sensorVarType { get; set; }
        public string latestReading { get; set; }
        public DateTime updatedAt { get; set; }
        
        static Random _r = new Random();
        public void addNewHistoryValue(DateTime time, double value)
        {
            //DateTime time = DateTime.Now;
            //string format = "h:mm:ss";
            //time.ToString(format); // Write to console
            //double f = (_r.NextDouble() * 15.0) - 1.0;
            _data.Add(new HistoryData(time, value)); /*{ time = time.ToString(format), value = f })*/
        }

        private ObservableCollection<HistoryData> _data = new ObservableCollection<HistoryData>()
        {
            //new SensorData() { time = "cat", value=5, /*val2=15, val3=12*/},
            //new SensorData() { time = "cat2", value=15.2, /*val2=1.5, val3=2.1*/},
            //new SensorData() { time = "cat3", value=25, /*val2=5, val3=2*/},
            //new SensorData() { time = "cat4", value=8.1, /*val2=1, val3=22*/},
        };

        //public ReadOnlyObservableCollection<SensorData> DataUnit { get; set; }
        public ObservableCollection<HistoryData> DataUnit { get { return _data; } }
    }
}
