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
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace CloverMobile
{

    public partial class History : PhoneApplicationPage
    {
        private DispatcherTimer timer;
        private Controller controller;
        DataMaster model;
        public string currentFrequency { get; set; }
        public string newFrequency { get; set; }
        public int currentSensorId { get; set; }
        public History()
        {
            InitializeComponent();
  
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI: History page constructor called");

            // ** get sensors
            controller = Controller.getInstance;


            // ** set some default values
            currentSensorId = 1;
            currentFrequency = "hourly";

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            timer.Tick += new EventHandler(Timer_tick);
            timer.Start();

            frequencyListBox.Items.Add("hourly");
            frequencyListBox.Items.Add("daily");
            frequencyListBox.Items.Add("monthly");
            frequencyListBox.SelectedItem = currentFrequency;  


            System.Diagnostics.Debug.WriteLine("UI: History page loaded.");
            model = controller.getModel();
            foreach (Sensor s in model.currentSensors)
            {
                sensorsListBox.Items.Add(s.longName);
                controller.getSensorHistory(s.sensorId);
                if (currentSensorId == s.sensorId)
                {
                    currentSensorName.Text = s.longName;
                    sensorsListBox.SelectedItem = s.longName;
                    this.DataContext = s;
                }
            }
            //controller.getSensorHistory(currentSensorId);
            // ** ask the controller to get all sensors

            

        }
        // ** this button is for testing
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            // check if selected new sensor is same than the current
            // check if newFrequency is the same than the frequency already displayed
            
            //sensorsListBox.SelectedItem.ToString();
            //frequencyListBox.SelectedItem.ToString();
            //System.Diagnostics.Debug.WriteLine("UI: getting history time scale for a single sensor");
            //controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "daily", "2011-03-01", "2011-04-01");
            //string format = "yyyy:MM:dd:hh:mm";
            //time = timevalue.ToString(format);
            textBlock1.Text = startDatePicker.Value.ToString();
            //startDatePicker.DataContext.ToString();

        }
        private void nextChart_Click(object sender, RoutedEventArgs e)
        {
            currentSensorId++;
            if (currentSensorId > 4) // ** check if going outside the list
            {
                currentSensorId = 4;
                nextChartButton.IsEnabled = false;
                prevChartButton.IsEnabled = true;
            }
            else
            {
                SetGraphDataContext(currentSensorId);
                nextChartButton.IsEnabled = true;
                prevChartButton.IsEnabled = true;
                //controller.getSensorHistory(currentSensorId);
            }
        }
        private void prevChart_Click(object sender, RoutedEventArgs e)
        {
            currentSensorId--;
            if (currentSensorId < 1)
            {
                currentSensorId = 1;
                prevChartButton.IsEnabled = false;
                nextChartButton.IsEnabled = true;
            }
            else
            {
                SetGraphDataContext(currentSensorId);
                prevChartButton.IsEnabled = true;
                nextChartButton.IsEnabled = true;
                //controller.getSensorHistory(currentSensorId);
            }
        }
        private void SetGraphDataContext(int currentSensorId)
        {
            // ** set the datacontext
            foreach (Sensor s in model.currentSensors)
            {
                if (currentSensorId == s.sensorId)
                {
                    currentSensorName.Text = s.longName;
                    sensorsListBox.SelectedItem = s.longName;
                    this.DataContext = s;
                }
            }
        }
        private void Timer_tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI: timer.");
            controller.updateValueOfThisSensor(currentSensorId);
            
            //controller.getSensorsXML();
        }
    }
}