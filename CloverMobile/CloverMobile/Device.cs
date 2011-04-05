using System;
using System.Collections.Generic;
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
    public class Device
    {
        public int  deviceId { get; set; }
        public string deviceName { get; set; }
        public Location deviceLocation { get; set; }
        public List<Sensor> sensors;
        public List<Output> outputs;
    }
}
