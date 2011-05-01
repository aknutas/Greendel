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
        private List<string> sensorNames;
        private List<string> frequencies;
        public string currentFrequency { get; set; }
        public string newFrequency { get; set; }
        public int currentSensorId { get; set; }
        public string currentSensorShortName { get; set; }
        public int i;
        public History()
        {
            
            InitializeComponent();
            i = 0;
            sensorNames = new List<string>();
            frequencies = new List<string>();
            
            frequencies.Add("daily");
            frequencies.Add("hourly");
            frequencies.Add("monthly");

            System.Diagnostics.Debug.WriteLine("UI.History: History page constructor called");
            // ** get controller instance and give our reference
            controller = Controller.getInstance;
            //controller.setActivePage(this);
            controller.setImageSource(this);
            // ** set some default values

            currentSensorShortName = "poweruse";
            currentFrequency = "daily";

            // ** put content to the frequency listbox
            //frequencyListBox.SelectedItem = currentFrequency;

            // ** get default dates for datepickers
            endDatePicker.Value = currentTime = DateTime.Now;
            startDatePicker.Value = currentTime - TimeSpan.FromDays(7);

            // ** get a reference to the model
            model = controller.getModel();


            // ** get sensors from model
            foreach (Sensor s in model.currentSensors)
            {
                sensorNames.Add(s.longName); // ** set the current sensor
                if (currentSensorShortName == s.sensorName)      
                {
                    currentSensorId = s.sensorId;
                    currentSensorName.Text = s.longName + "(" + s.unit + ")";
                    //unitTextBlock.Text = s.unit;
                    
                }
            }
            // ** get the default graph that is, power consumption from last week
            //controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
            
            sensorsListPicker.ItemsSource = sensorNames;
            frequenciesListPicker.ItemsSource = frequencies;
            AskNewValues();
            //sensorsListPicker.SelectedIndex
            
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            historyScreenAnimation.Begin();
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
                    currentSensorName.Text = s.longName + " ( " + s.unit + " )";

                    this.DataContext = s;
                }
            }
            StopLoadingAnimation();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AskNewValues();

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            System.Diagnostics.Debug.WriteLine("Reverse animation");
            historyScreenAnimationReverse.Begin();



        }

        private void historyScreenAnimationReverse_Completed(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }



        private void sensorsListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (i < 4)
            {
                i++;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("UI.History CALLING SENSORS SELECTION CHANGED");
                System.Diagnostics.Debug.WriteLine(sender.ToString(), e.ToString());
                // ** change the current sensor!
                foreach (Sensor s in model.currentSensors)
                {
                    if (s.longName == sensorsListPicker.SelectedItem.ToString())
                    {
                        currentSensorId = s.sensorId;
                        currentSensorShortName = s.sensorName;
                    }
                }
                System.Diagnostics.Debug.WriteLine("UI.History: current sensor id is now: " + currentSensorId);
                AskNewValues();
                
            }

        }

        private void frequenciesListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (i < 4)
            {
                i++;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("UI.History CALLING FREQS SELECTION CHANGED");
                System.Diagnostics.Debug.WriteLine(sender.ToString(), e.ToString());
                currentFrequency = frequenciesListPicker.SelectedItem.ToString();
                AskNewValues();
                
            }
        }

        public void AskNewValues()
        {
            StartLoadingAnimation();
            
            System.Diagnostics.Debug.WriteLine("UI.Hisroty: getting history time scale for a single sensor");
            if (currentSensorShortName == "powerconsumed") // ** sensor with id 2 is "power consumed"
            {
                controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "diffscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
            }
            else
            {
                controller.getSensorHistoryFromSpecifiedTimeScale(currentSensorId, "avgscale", currentFrequency.ToString(), String.Format("{0:yyyy-MM-dd}", startDatePicker.Value), String.Format("{0:yyyy-MM-dd}", endDatePicker.Value));
            }       
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (i < 4)
            {
                i++;
            }
            else
            {
                AskNewValues();
            }
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI.History PAGE UNLOADED!");
        }
        public void StartLoadingAnimation()
        {
       
            loadingSplashscreen.Visibility = System.Windows.Visibility.Visible;
            LoadScreenIn.Begin();
            RotateGreendelIcon.Begin();
        }
        public void StopLoadingAnimation()
        {
            RotateGreendelIcon.Pause();
            LoadScreenOut.Begin();
            loadingSplashscreen.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}