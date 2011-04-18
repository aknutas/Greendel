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

namespace CloverMobile
{

    public partial class History : PhoneApplicationPage
    {
        private Controller controller;
        DataMaster model;
        public string currentFrequency { get; set; }
        public string newFrequency { get; set; }
        public int currentSensorId { get; set; }
        public History()
        {
            System.Diagnostics.Debug.WriteLine("CALLING HISTORY CONTROLLER!");
            InitializeComponent();
            currentSensorId = 1;
            //this.DataContext = model;
            // ** write stuff to check boxes
            //sensorsListBox.SelectedItem = ""
            //this.DataContext = model.currentSensors[currenSensorId - 1];

            // ** connect the web service here   
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR SENSORS");
            // ** ask the controller to get all sensors
            controller = Controller.getInstance;

            controller.getSensorHistory(currentSensorId);
            // ** get a reference to model's sensorlist
            model = controller.getModel();

            // ** set some default values
            //currentSensorId = 1;
            currentFrequency = "Hourly";

            //historyScreenLoad.Begin();
            frequencyListBox.Items.Add("Hourly");
            frequencyListBox.Items.Add("Daily");
            frequencyListBox.Items.Add("Monthly");
            frequencyListBox.SelectedItem = currentFrequency;
            foreach (Sensor s in model.currentSensors)
            {               
                sensorsListBox.Items.Add(s.longName);
                controller.getSensorHistory(s.sensorId);
                if (currentSensorId == s.sensorId)
                {     
                    sensorsListBox.SelectedItem = s.longName;
                    this.DataContext = s;
                }
            }
        }
        /*
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SENSOR UPDATE");
            controller.updateValueOfThisSensor(currentSensorId);
            //model.currentSensors[currenSensorId - 1].addNewDataUnit();
        }
        */

        // ** this buttone is for testing
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            // check if selected new sensor is same than the current
            // check if newFrequency is the same than the frequency already displayed
            
            sensorsListBox.SelectedItem.ToString();
            frequencyListBox.SelectedItem.ToString();
            System.Diagnostics.Debug.WriteLine("SENSOR UPDATE");
            controller.updateValueOfThisSensor(currentSensorId);
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
                    sensorsListBox.SelectedItem = s.longName;
                    this.DataContext = s;
                }
            }
        
        }
    }
}