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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using System.IO;
using CloverMobile;

namespace CloverAnimation
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer;
        WebClient wc = null;
        bool xmlRetrieved = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            rotateClover.Begin();
            rotateClover.Pause();
            App.GB.serviceAddress = "http://anttitek.net:3000/";
            xmlRetrieved = false;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            timer.Tick += new EventHandler(Timer_tick);
            timer.Start();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainScreenLoad.Begin();
            wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.GB.serviceAddress+"outputs.xml"));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // ** parse and display XML
                XDocument dataDoc = XDocument.Load(new StringReader(e.Result));
                var dataUnitoutputs = from output in dataDoc.Descendants("output")
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
                    // ** find the states of solar and wind and set them to the screen
                    if (o.name == "solar")
                    {
                        if (o.state == true)
                        {
                            Uri uri = new Uri("Sun_enabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            solarImage.Source = imgSource;
                        }
                        else
                        {
                            Uri uri = new Uri("Sun_disabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            solarImage.Source = imgSource;
                        }
                    }
                    if (o.name == "wind")
                    {
                        if (o.state == true)
                        {
                            Uri uri = new Uri("Wind_enabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            windImage.Source = imgSource;
                        }
                        else
                        {
                            Uri uri = new Uri("Wind_disabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            windImage.Source = imgSource;
                        }
                    }
                }
            }
            catch
            {
                textBlock2.Text = "XML Error";
            }
            // ** get the sensors.xml
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.GB.serviceAddress + "sensors.xml"));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted2);
        }
        void wc_DownloadStringCompleted2(object sender, DownloadStringCompletedEventArgs e)
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
                    if (s.name == "poweruse")
                    {
                        if (s.latestReading == 0)
                        {
                            rotateClover.Pause();
                            textBlock2.Text = "0 kW";
                        }
                        else if (s.latestReading < 0) // oikealle vihreä
                        {
                            rotateClover.Pause();

                            Uri uri = new Uri("Clover.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            image.Source = imgSource;
                            myAnimation.To = 360;
                            if (s.latestReading == -1000)
                            {
                                textBlock2.Text = "-1000 kW";
                                rotateClover.SpeedRatio = 0.2;
                            }
                            else if (s.latestReading == -2000)
                            {
                                textBlock2.Text = "-2000 kW";
                                rotateClover.SpeedRatio = 2.0;
                            }
                            rotateClover.Resume();
                        }
                        else if (s.latestReading > 0) // punainen vasen
                        {
                            rotateClover.Pause();
                            Uri uri = new Uri("CloverRed.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            image.Source = imgSource;
                            myAnimation.To = -360;
                            if (s.latestReading == 1000)
                            {
                                textBlock2.Text = "1000 kW";
                                rotateClover.SpeedRatio = 0.2;
                            }
                            else if (s.latestReading == 2000)
                            {
                                textBlock2.Text = "2000 kW";
                                rotateClover.SpeedRatio = 2.0;
                            }
                            rotateClover.Resume();
                        }
                    }

                }
                xmlRetrieved = true;
            }
            catch
            {
                textBlock2.Text = "XML Error";
            }
        }
        private void ApplicationBarIconButtonSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButtonHistory_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarControlIconButtonControl_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Control.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            if (xmlRetrieved == true)
            {
                wc.DownloadStringAsync(new Uri(App.GB.serviceAddress + "outputs.xml"));
                xmlRetrieved = false;
            }
            //timer.Tick += new EventHandler(Timer_tick);
            //timer.Start();
            
        }
    }
}