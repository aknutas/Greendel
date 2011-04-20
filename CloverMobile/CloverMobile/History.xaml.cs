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
        DataMaster model;
        DateTime currentTime;
        DateTime sevenDaysAgo;
        //string format = "yyyy:MM:dd";
        string currentTimeString = "";
        string sevenDaysAgoString = "";
        public string currentFrequency { get; set; }
        public string newFrequency { get; set; }
        public int currentSensorId { get; set; }
        public History()
        {
            
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("UI.History: History page constructor called");
            controller = Controller.getInstance;
            controller.setActivePage(this);
            // ** set some default values
            currentSensorId = 2;
            currentFrequency = "daily";
            frequencyListBox.Items.Add("hourly");
            frequencyListBox.Items.Add("daily");
            frequencyListBox.Items.Add("monthly");
            frequencyListBox.SelectedItem = currentFrequency;
            
            // ** get dates for datepickers
            endDatePicker.Value = currentTime = DateTime.Now;
            startDatePicker.Value = currentTime - TimeSpan.FromDays(7);

            //currentTimeString = String.Format("{0:yyyy-MM-dd}", currentTime);
            //sevenDaysAgoString = String.Format("{0:yyyy-MM-dd}", sevenDaysAgo);
            //startDatePicker.Value = ("{0:yyyy-MM-dd}", currentTime);
            model = controller.getModel();

            foreach (Sensor s in model.currentSensors)
            {
                sensorsListBox.Items.Add(s.longName);
                if (currentSensorId == s.sensorId) // ** set the current sensor
                {
                    currentSensorName.Text = s.longName;
                    sensorsListBox.SelectedItem = s.longName;
                }
            }
            //System.Diagnostics.Debug.WriteLine("UI.Histoyu: " + sevenDaysAgoString + currentTimeString);
            controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI.History: History page loaded.");

            //SetGraphDataContext(currentSensorId);
        }

        // ** controller calls this function to tell the page that 
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
            controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
        }

    }
}