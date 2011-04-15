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
    public class HistoryData
    {
            public string time { get; set; }
            public double value { get; set; }
            private string format;
            //public double val2 { get; set; }
            //public double val3 { get; set; }
            public HistoryData()
            { 
            
            }
            public HistoryData(DateTime timevalue, double number)
            {   
                // ** convert the datetime to proper form!
                format = "yy:mm:dd:hh:mm:ss";
                time = timevalue.ToString(format);
                value = number;
            }
    }
}
