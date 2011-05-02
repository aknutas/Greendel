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

namespace CloverMobile
{
    public partial class Social : PhoneApplicationPage
    {
        private Controller controller;
        private DataMaster myMaster;
        private int sensor1Id;
        private int sensor2Id;
        public Social()
        {
            InitializeComponent();
            controller = Controller.getInstance;
            //controller.setActivePage(this);
            controller.setImageSource(this);
            myMaster = controller.getModel();
            // ** get sensors from model

            foreach (Sensor s in myMaster.currentSensors)
            {
                if (s.sensorName == "poweruse")
                {

                    sensor1CheckBox.Content = s.longName + " : " + String.Format("{0:0.#}", double.Parse(s.latestReading)) + " : " + s.unit;
                    sensor1Id = s.sensorId;
                    
                    //currentSensorId = s.sensorId;
                    //currentSensorName.Text = s.longName + "(" + s.unit + ")";
                    //unitTextBlock.Text = s.unit;
                }
                else if (s.sensorName == "powerconsumed")
                {
                    sensor2CheckBox.Content = s.longName + " : " + String.Format("{0:0.#}", double.Parse(s.latestReading)) +" : " + s.unit;
                    sensor2Id = s.sensorId;
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            socialScreenAnimation.Begin();
            System.Diagnostics.Debug.WriteLine("Animation");
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            System.Diagnostics.Debug.WriteLine("Reverse animation");
            socialScreenAnimationReverse.Begin();

        }

        private void socialScreenAnimationReverse_Completed(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int sensor1value = 0;
            int sensor2value = 0;
            // ** check checkboxes, sensor id is 0 if not sent
            if (sensor1CheckBox.IsChecked == true)
            {
                sensor1value = sensor1Id;

            }
            if (sensor2CheckBox.IsChecked == true)
            {
                sensor2value = sensor2Id;
            }
            if (sensor1value != 0 || sensor2value != 0)
            {
                System.Diagnostics.Debug.WriteLine("ui.social: current user id is " + myMaster.currentUser.id);
                controller.SendToFaceBook(myMaster.currentUser.id, sensor1value, sensor2value);
            }           
        }
        // ** when user clicks either of the checkboxes, clear the text
        private void sensor1CheckBox_Click(object sender, RoutedEventArgs e)
        {
            statusMessageTextBlock.Text = "";
        }

        private void sensor2CheckBox_Click(object sender, RoutedEventArgs e)
        {
            statusMessageTextBlock.Text = "";
        }
    }
}