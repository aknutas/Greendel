﻿using System;
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
    public class Sensor
    {        
        public int sensorId { get; set; }
        public string sensorName { get; set; }
        public string sensorVarType { get; set; }
        public int? latestReading { get; set; }
    }

}