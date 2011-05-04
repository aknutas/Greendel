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
using System.Windows.Media.Imaging;

namespace CloverMobile
{
    public partial class Control : PhoneApplicationPage
    {
        private Controller controller;
        private DataMaster model;
        private WeatherForecast forecast;
        private bool Socket_Toggle;
        private string weatherForecastSource;
        
        
        public Control()
        {
            InitializeComponent();
            controller = Controller.getInstance;  
            model = controller.getModel();
            //controller.setActivePage(this);
            controller.setImageSource(this);
            
            //disable heating
            Update_Button.IsEnabled = false;
            Heating_TextBlock.IsEnabled = false;

            // ** get weather forecast
            forecast = model.currentForecast;
            SetCurrentWeather(forecast.code);

            // ** get outputs
            controller.getOutputsXML();


        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            controlScreenAnimation.Begin();
            
            
            
        }
        private void Socket_Click(object sender, RoutedEventArgs e)
        {
            if (Socket_Toggle == true)
            {
                System.Diagnostics.Debug.WriteLine("ui: changing socket output to false");
                controller.sendOutputs(1, false);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ui: changing socket output to true");
                controller.sendOutputs(1, true);     
            }
        }

        // Hijack Back button event for reverse animation
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            System.Diagnostics.Debug.WriteLine("Reverse animation");
            controlScreenAnimationReverse.Begin();
        }
        
        private void controlScreenAnimationReverse_Completed(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        // ** this is called by the controller when the outuputs are received!
        public void OutputsReceived()
        {
            System.Diagnostics.Debug.WriteLine("ui.control: outputs received.");
            // ** when outputs received, set the current value for outputs
            foreach (Output o in model.currentOutputs)
            {
                if (o.name == "wall_socket")
                {
                    if (o.state == "true")
                    {
                        Socket_Toggle = true;

                    }
                    else
                    {
                        Socket_Toggle = false;

                    }
                    Socket_FadeOut.Begin();
                }
            }
        }

        private void Socket_FadeOut_Completed(object sender, EventArgs e)
        {
            if (Socket_Toggle == true)
            {
                Socket.Content = "ON";
                //Socket.Background = Resources["Socket_Color_ON"] as Brush;

            

            }
            else
            {
                Socket.Content = "OFF";
                //Socket.Background = Resources["Socket_Color_OFF"] as Brush;
                
            }
            Socket_FadeIn.Begin();
        }
        public void SetCurrentWeather(int code)
        {

            // get the time of day and determine if it is day or night
            // DateTime now = DateTime.Now;
            if (code != 0)
            {
                weatherForecastSource = "http://l.yimg.com/a/i/us/nws/weather/gr/" + code.ToString() + "d.png"; //34d.png
                System.Diagnostics.Debug.WriteLine("UI: weathersource is:  " + weatherForecastSource);
                Uri uri = new Uri(weatherForecastSource, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                weatherForecastImage.Source = imgSource;
                weatherForecastImage.Visibility = System.Windows.Visibility.Visible;
                forecastTextBlock.Text += forecast.temp.ToString() + " °C";
            }

        }
    }
}
