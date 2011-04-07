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

namespace CloverMobile
{
    public class SensorInformation
    {
        public List<Sensor> mySensors { get; set; }


        public SensorInformation()
        {
            mySensors = new List<Sensor>();
        }
    }
}
