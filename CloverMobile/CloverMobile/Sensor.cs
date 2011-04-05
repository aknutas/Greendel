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
    public class Sensor
    {
        
        public int sensorId { get; set; }
        public string sensorName { get; set; }
        public string sensorVarType { get; set; }
        public int latestReading { get; set; }

        //public DateTime createdAt { get; set; }
        //public int latestReading { get; set; }
        //public string name { get; set; }
        //public DateTime updatedAt { get; set; }
    }
}
