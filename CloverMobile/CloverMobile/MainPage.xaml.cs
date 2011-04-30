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
using System.IO;
using CloverMobile;
using System.IO.IsolatedStorage;

namespace CloverMobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool sensorsReceived = false;
        private Weather weatherNow;
        private DispatcherTimer timer;
        private Controller controller;
        private DataMaster myMaster;
        //private DateTime now;
        bool settingsFileExists = false;
        private string fileName = "settings";

        public MainPage()
        {       
            InitializeComponent();
            // ** get the controller instance and give the reference of current page to it
            controller = Controller.getInstance;
            //controller.setActivePage(this);
            controller.setImageSource(this);
            //background.SetBinding(Image.SourceProperty, controller.setImageSource(this));
            // ** disable the ui-elements on the background        
            hideMainScreenElements();

            // ** check if the settings file exists
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (appStorage.FileExists(fileName) == true) // file is found
            {
                settingsFileExists = true;
                // ** no need for splashscreen (user settings exist)
                splashScreen.Visibility = System.Windows.Visibility.Collapsed;
                  
                // ** get the serialized settings file
                var settingsData = XmlSerilizierHelper.Deserialize(fileName, typeof(SettingsFile));
                SettingsFile mySerSettings = new SettingsFile();
                mySerSettings = (SettingsFile)settingsData;
             
                // ** send the usenrame and password
                System.Diagnostics.Debug.WriteLine("ui: authenticating, file exists.");
                controller.authenticate(mySerSettings.username.ToString(), mySerSettings.password.ToString(), mySerSettings.serviceAddress.ToString());
                
                // ** authenticate by sending basic information 
                controller.getUserXML();

            }
            else // ** there are no file, user has not logged in yet, display the splash screen
            {
                PageTitle.Text = "Login";
                splashScreen.Visibility = System.Windows.Visibility.Visible;
                errorMessageTextBlock.Text = "";
            }

            // ** start the timer that polls current power consumption
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            timer.Tick += new EventHandler(Timer_tick);

        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("UI: mainpage loaded, timer started.");
            timer.Start();
            // ** start the timer again when user navigates to the page, and possibly get new values for power consumption graph
        }

        // ** functions for navigating away from mainpage
        private void ApplicationBarIconButtonSettings_Click(object sender, EventArgs e)
        {
            // ** before navigating, stop the timer!
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));    
        }
        private void ApplicationBarIconButtonHistory_Click(object sender, EventArgs e)
        {
            // ** before navigating, stop the timer!
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.RelativeOrAbsolute));
        }
        private void ApplicationBarControlIconButtonControl_Click(object sender, EventArgs e)
        {
            // ** before navigating, stop the timer!
            NavigationService.Navigate(new Uri("/Control.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButtonSocial_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Social.xaml", UriKind.RelativeOrAbsolute));
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
                // ** try to authenticate
                System.Diagnostics.Debug.WriteLine("ui: authenticating.");
                controller.authenticate(userNameTextBox.Text, passwordTextBox.Password, serverAddressTextBox.Text);
                controller.getUserXML();        
        }
        public void authenticationOk() // ** this function is called by controller if the autentication is successfull
        {
            // ** get sensors
            controller.getSensorsXML();  
            System.Diagnostics.Debug.WriteLine("UI: authentication OK.");

            // ** remove the splashscreen and show the UI elements
            splashScreen.Visibility = System.Windows.Visibility.Collapsed;
            showMainScreenElements();
            PageTitle.Text = "Main";

            // ** get model reference and get the weather
            myMaster = controller.getModel();
            weatherNow = myMaster.currentWeather;
            SetCurrentWeather(weatherNow.code);  

            // ** save current settings to the file
            if (settingsFileExists == false) // ** if autentication was successfull and this is the first time when logging in, create new file
            {               
                // ** create the settings object, serialize it and write it to phone's memory
                SettingsFile mySettings = new SettingsFile();
                mySettings.username = userNameTextBox.Text;
                mySettings.password = passwordTextBox.Password;
                mySettings.serviceAddress = serverAddressTextBox.Text;
                XmlSerilizierHelper.Serialize(fileName, mySettings);
                settingsFileExists = true;
            }
        }

        // ** this is called from controller if authentication fails
        public void printError()
        {
            hideMainScreenElements();
            splashScreen.Visibility = System.Windows.Visibility.Visible;
            errorMessageTextBlock.Text = "Connection Error.";
        }

        public void SetCurrentWeather(int code)
        {
            
            // get the time of day and determine if it is day or night
            // DateTime now = DateTime.Now;

            string weatherSource = "http://l.yimg.com/a/i/us/nws/weather/gr/" + code.ToString() + "d.png"; //34d.png
            System.Diagnostics.Debug.WriteLine("UI: weathersource is:  " + weatherSource);
            Uri uri = new Uri(weatherSource, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            currentWeather.Source = imgSource;
            currentWeather.Visibility = System.Windows.Visibility.Visible;

            currentTemperatureTextBlock.Text = weatherNow.temp.ToString() + " °C";
            
        }
        // ** this function is called by the controller when we have received sensors
        public void GetPowerUsage()
        {
            ApplicationBar.IsVisible = true;
            sensorsReceived = true;
            // ** get latest 20 values for power consumption graph
            System.Diagnostics.Debug.WriteLine("ui: getting latest 20 points for power use sensor.");
            controller.getLatestNpoints(1, 20);
                      
            // ** Set the data context of the graph
            foreach (Sensor s in myMaster.currentSensors)
            {
                if (s.sensorName == "poweruse")
                {
                    System.Diagnostics.Debug.WriteLine("ui: binding datacontext.");
                    this.DataContext = s;
                    currentPowerConsumptionTextBlock.Text = s.latestReading.ToString() + " W";
                }
            }           
        }
        // ** timer function, updates value of power consumption sensor after 3 seconds
        private void Timer_tick(object sender, EventArgs e)
        {
            if (sensorsReceived == true)
            {
                System.Diagnostics.Debug.WriteLine("UI: timer.");
                controller.updateValueOfThisSensor(1);

                foreach (Sensor s in myMaster.currentSensors)
                {
                    if (s.sensorName == "poweruse")
                    {
                        GlobalData.currentConsumption = Convert.ToDouble(s.latestReading);
                        currentPowerConsumptionTextBlock.Text = s.latestReading.ToString() + " W";
                        controller.setImageSource(this);
                        
                    }
                }                  
            }
        }
        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            // ** stop the timer when navigating away from mainpage
            System.Diagnostics.Debug.WriteLine("UI: navigated away from the mainpage.");
            timer.Stop();
        }
        public void hideMainScreenElements()
        {
            currentTemperatureTextBlock.Visibility = Visibility.Collapsed;
            currentWeather.Visibility = Visibility.Collapsed;
            PageTitle.Text = "Login";
            Chart_Grid.Visibility = Visibility.Collapsed;
            Powervalue_Grid.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = false;
        }
        public void showMainScreenElements()
        {
            currentTemperatureTextBlock.Visibility = Visibility.Visible;
            currentWeather.Visibility = Visibility.Visible;
            PageTitle.Text = "Main";
            Chart_Grid.Visibility = Visibility.Visible;
            Powervalue_Grid.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = true;
        
        }
    }
}