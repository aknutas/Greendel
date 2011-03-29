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
        public DateTime createdAt { get; set; }
        public int deviceId { get; set; }
        public int id { get; set; }
        public int latestReading { get; set; }
        public int latestValue { get; set; }
        public string name { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
