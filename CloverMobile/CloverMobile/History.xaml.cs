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
        //private DispatcherTimer timer;
        private Controller controller;
        private DataMaster model;
        private DateTime currentTime;
        public string currentFrequency { get; set; }
        public string newFrequency { get; set; }
        public int currentSensorId { get; set; }
        public History()
        {
            
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("UI.History: History page constructor called");
            // ** get controller instance and give our reference
            controller = Controller.getInstance;
            controller.setActivePage(this);
            
            // ** set some default values
            currentSensorId = 2;
            currentFrequency = "daily";

            // ** put content to the frequency listbox
            frequencyListBox.Items.Add("hourly");
            frequencyListBox.Items.Add("daily");
            frequencyListBox.Items.Add("monthly");
            frequencyListBox.SelectedItem = currentFrequency;
            
            // ** get default dates for datepickers
            endDatePicker.Value = currentTime = DateTime.Now;
            startDatePicker.Value = currentTime - TimeSpan.FromDays(7);

            // ** get a reference to the model
            model = controller.getModel();

            // ** get sensors from model
            foreach (Sensor s in model.currentSensors)
            {
                sensorsListBox.Items.Add(s.longName);   // ** fill the sensors listbox
                if (currentSensorId == s.sensorId)      // ** set the current sensor
                {
                    currentSensorName.Text = s.longName;
                    unitTextBlock.Text = s.unit;
                    sensorsListBox.SelectedItem = s.longName;
                }
            }
            // ** get the default graph that is, power consumption from last week
            controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI.History: History page loaded.");
        }

        // ** this function is called by the controller when history infromation for current sensor is successfully downloaded
        public void SetGraphDataContext()
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


        private void sensorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ** change the current sensor!
            foreach (Sensor s in model.currentSensors)
            {
                if (s.longName == sensorsListBox.SelectedItem.ToString()) 
                {
                    unitTextBlock.Text = s.unit;
                    currentSensorId = s.sensorId;
                }
            }
            System.Diagnostics.Debug.WriteLine("UI.History: current sensor id is now: " + currentSensorId);
        }

        private void frequencyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ** change the current frequency!
            currentFrequency = frequencyListBox.SelectedItem.ToString();
            System.Diagnostics.Debug.WriteLine("UI.History: current frequency is now: " + currentFrequency);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI.Hisroty: getting history time scale for a single sensor");
            if (currentSensorId == 1) // ** sensor with id 2 is "power consumed"
            {
                controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "diffscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
            }
            else
            {
                controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
            }
        }
    }
}