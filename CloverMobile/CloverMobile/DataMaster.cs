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

namespace CloverMobile
{
    public class DataMaster
    {
        private List<Output> outputs; 
        private List<Sensor> sensors;

        public DataMaster()
        {
            outputs = new List<Output>();
            sensors = new List<Sensor>();
        }

        public void writeNewOutputs(List<Output> outputs)
        {
            
        
        }
        public void writeNewSensors(List<Output> outputs)
        {


        }
    }
}
