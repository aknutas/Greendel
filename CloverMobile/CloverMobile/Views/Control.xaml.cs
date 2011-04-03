using System;
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
using System.Xml.Linq;
using System.IO;
using System.Windows.Threading;

namespace CloverMobile
{
    public partial class Control : PhoneApplicationPage
    {
        Controller controller;
        public Control()
        {
            InitializeComponent();
            controller = Controller.getInstance;
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            controlScreenAnimation.Begin();
        }

        private void sendChanges_Click(object sender, RoutedEventArgs e)
        {
            bool heating = false;
            bool lightning = false;
            // ** get values from radiobuttons
            if (radioButtonHeatingOn.IsChecked == true)
            {
                heating = true;

            }
            else if (radioButtonHeatingOff.IsChecked == true)
            {
                heating = false;
            }

            else if (radioButtonLightsOn.IsChecked == true)
            {
                lightning = true;
            }

            else if (radioButtonLightsOff.IsChecked == true)
            {
                lightning = false;

            }
            controller.sendHeatingAndLightning(heating, lightning);
        }
    }
}
