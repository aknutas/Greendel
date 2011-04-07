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
        DispatcherTimer timer;
        Controller controller;
        DataMaster model;
        List<Output> outputs;
        List<Sensor> sensors;
        public Control()
        {
            InitializeComponent();
            controller = Controller.getInstance;  
            model = controller.getModel();
            controller.setActivePage(this);
            // ** after 10 seconds, ask the model for new information
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            timer.Tick += new EventHandler(Timer_tick);
            timer.Start();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            controlScreenAnimation.Begin();
        }

        private void sendChanges_Click(object sender, RoutedEventArgs e)
        {

            //controller.getXML();
            System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER");
            /*
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
             */ 
        }
        private void Timer_tick(object sender, EventArgs e)
        {
            //outputs = model.getOutputs();
            //sensors = model.getSensors();
            UpdateView();
        }
        public void UpdateView()
        {
            /*
            foreach (Sensor s in sensors)
            {
                // get values to ui
            }
            foreach (Output o in outputs)
            { 
                // get values to ui
            }
            */
        
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR USER INFO");
            controller.getUserXML();               
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR OUTPUTS");
            controller.getOutputsXML();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR SENSORS");
            controller.getSensorsXML();
        }


    }
}
