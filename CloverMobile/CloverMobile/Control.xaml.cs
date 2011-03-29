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
        bool lightsValueFromServer;
        bool heatingValueFromServer;
        DispatcherTimer timer;
        WebClient wc = null;
        bool xmlRetrieved = false;

        public Control()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            timer.Tick += new EventHandler(Timer_tick);
            timer.Start();
            // ** get current values
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            controlScreenAnimation.Begin();
            wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.GB.serviceAddress+"sensors.xml"));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // ** parse and display XML
                XDocument dataDoc = XDocument.Load(new StringReader(e.Result));
                var sensorOutputs = from sensor in dataDoc.Descendants("sensor")
                                    select new Sensor
                                    {
                                        createdAt = DateTime.Parse(sensor.Element("created-at").Value),
                                        deviceId = int.Parse(sensor.Element("device-id").Value),
                                        id = int.Parse(sensor.Element("id").Value),
                                        latestReading = int.Parse(sensor.Element("latestreading").Value),
                                        //latestValue = int.Parse(sensor.Element("latestvalue").Value),
                                        name = sensor.Element("name").Value.ToString(),
                                        updatedAt = DateTime.Parse(sensor.Element("updated-at").Value),
                                    };

                foreach (Sensor s in sensorOutputs)
                {
                    if (s.name == "outsidetemp")
                    {
                        tempInsideTextBox.Text = s.latestReading.ToString();
                    }
                    if (s.name == "insidetemp")
                    {
                        tempOutsideTextBox.Text = s.latestReading.ToString();
                    }
                }
            }
            catch
            {
                textBlockTest.Text = "XML Error";
            }
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.GB.serviceAddress+"outputs.xml"));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted2);
        }
        void wc_DownloadStringCompleted2(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // ** parse and display XML
                XDocument dataDoc = XDocument.Load(new StringReader(e.Result));

                var dataUnitoutputs = from output in dataDoc.Descendants("output")
                                      // let created = DateTime.Parse(output.Element("created").Value)
                                      // orderby created ascending
                                      select new Output
                                      {
                                          created = DateTime.Parse(output.Element("created-at").Value),
                                          deviceId = int.Parse(output.Element("device-id").Value),
                                          id = int.Parse(output.Element("id").Value),
                                          name = output.Element("name").Value.ToString(),
                                          state = bool.Parse(output.Element("state").Value),
                                          updatedAt = DateTime.Parse(output.Element("updated-at").Value),
                                      };

                foreach (Output o in dataUnitoutputs)
                {
                    if (o.name == "heating")
                    {
                        if (o.state == true)
                        {
                            heatingValueFromServer = true;
                            radioButtonHeatingOn.IsChecked = true;
                        }
                        else
                        {
                            heatingValueFromServer = false;
                            radioButtonHeatingOff.IsChecked = true;
                        }
                    }
                    if (o.name == "lights")
                    {
                        if (o.state == true)
                        {
                            lightsValueFromServer = true;
                            radioButtonLightsOn.IsChecked = true;
                        }
                        else
                        {
                            lightsValueFromServer = false;
                            radioButtonLightsOff.IsChecked = true;
                        }
                    }
                }
                xmlRetrieved = true;
            }
            catch
            {
                textBlock1.Text = "Xml Error";
            }
        }
        private void sendChanges_Click(object sender, RoutedEventArgs e)
        {
            // ** send values
            WebClient wcUpload = new WebClient();
            wcUpload.Headers[HttpRequestHeader.ContentType] = "application/xml";
            string xmlMessage;
            if (radioButtonHeatingOn.IsChecked == true && heatingValueFromServer == false)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                wcUpload.UploadStringAsync(new Uri(App.GB.serviceAddress + "outputs/1"), "PUT", xmlMessage);
                wcUpload.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);

            }
            else if (radioButtonHeatingOff.IsChecked == true && heatingValueFromServer == true)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                wcUpload.UploadStringAsync(new Uri(App.GB.serviceAddress + "outputs/1"), "PUT", xmlMessage);
                wcUpload.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }

            else if (radioButtonLightsOn.IsChecked == true && lightsValueFromServer == false)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                wcUpload.UploadStringAsync(new Uri(App.GB.serviceAddress + "outputs/2"), "PUT", xmlMessage);
                wcUpload.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }

            else if (radioButtonLightsOff.IsChecked == true && lightsValueFromServer == true)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                wcUpload.UploadStringAsync(new Uri(App.GB.serviceAddress + "outputs/2"), "PUT", xmlMessage);
                wcUpload.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);

            }
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            textBlockTest.Text = "Committed!";
        
        }
        private void Timer_tick(object sender, EventArgs e)
        {
            if (xmlRetrieved == true)
            {
                wc.DownloadStringAsync(new Uri(App.GB.serviceAddress + "sensors.xml"));
                xmlRetrieved = false;
                textBlockTest.Text = "";
            }
        }
        /*
        void wcUpload_UploadStringCompleted2(object sender, UploadStringCompletedEventArgs e)
        {
            textBlockTest.Text = "Lighting committed!";

        }
         * */
    }
}

