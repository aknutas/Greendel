﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;

namespace CloverMobile
{

    public partial class History : PhoneApplicationPage
    {
        Controller controller; 

        public History()
        {
            InitializeComponent();
            controller = Controller.getInstance;
            // ** connect the web service here   
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            historyScreenLoad.Begin();

        }   

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

        }
    }
}