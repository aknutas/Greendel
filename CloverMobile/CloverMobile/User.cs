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
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string realName { get; set; }
        public Device myDevice { get; set; }
        
       
        public User()
        {
            
            
        
        }
    }
    public class OutputInformation
    {

        public List<Output> myOutputs { get; set; }
        public OutputInformation()
        {

            myOutputs = new List<Output>();
        }
    
    }
}
