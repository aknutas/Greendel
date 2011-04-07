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
    public class Output
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string hasChanged { get; set; }

        //public DateTime created { get; set; }
        //public string name { get; set; }
        //public bool state { get; set; }
        //public DateTime updatedAt { get; set; }
    }
}
