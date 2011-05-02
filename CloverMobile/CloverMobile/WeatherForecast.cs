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
    public class WeatherForecast
    {
        public float temp { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        //public string unit { get; set; }
        public string description { get; set; }
        public int code { get; set; }
    }
}
