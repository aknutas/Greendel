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
            currentSensorId = 1;
            currentFrequency = "Hourly";
            InitializeComponent();
            controller = Controller.getInstance;
            
            model = controller.getModel();
            //this.DataContext = model;

            frequencyListBox.Items.Add("Hourly");
            frequencyListBox.Items.Add("Daily");
            frequencyListBox.Items.Add("Monthly");
            frequencyListBox.SelectedItem = currentFrequency;
            foreach (Sensor s in model.currentSensors)
            {
                sensorsListBox.Items.Add(s.longName);
                if (currentSensorId == s.sensorId)
                {
                    sensorsListBox.SelectedItem = s.longName;
                    this.DataContext = s;
                }
            
            }
            //sensorsListBox.SelectedItem = ""
            //this.DataContext = model.currentSensors[currenSensorId - 1];

            // ** connect the web service here   
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            chart1.FontSize = 10;
            //historyScreenLoad.Begin();
        }
        /*
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SENSOR UPDATE");
            controller.updateValueOfThisSensor(currentSensorId);
            //model.currentSensors[currenSensorId - 1].addNewDataUnit();
        }
        */
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            // check if selected new sensor is same than the current
            // check if newFrequency is the same than the frequency already displayed
            
            sensorsListBox.SelectedItem.ToString();
            frequencyListBox.SelectedItem.ToString();
            System.Diagnostics.Debug.WriteLine("SENSOR UPDATE");
            controller.updateValueOfThisSensor(currentSensorId);
        }
        
        
    }
}