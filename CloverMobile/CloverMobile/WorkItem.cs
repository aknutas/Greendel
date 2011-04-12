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

namespace CloverMobile
{
    public class WorkItem
    {
        public string documentName { get; set; }
        public int deviceId { get; set; }
        public int sensorId { get; set; }

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
    }
}
