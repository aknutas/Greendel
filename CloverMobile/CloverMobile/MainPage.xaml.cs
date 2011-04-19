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
        private Controller controller;
        private DataMaster myMaster;
        bool settingsFileExists = false;
        private string fileName = "settings";

        public MainPage()
        {
            // ** get the controller instance and give the reference of current page to it
            InitializeComponent();
            controller = Controller.getInstance;
            controller.setActivePage(this);
            currentWeather.Visibility = System.Windows.Visibility.Collapsed;

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
             
                // ** autheticate
                controller.authenticate(mySerSettings.username.ToString(), mySerSettings.password.ToString());
                System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR USER INFO");
                
                // ** get basic information
                controller.getUserXML();
                //controller.getSensorsXML();
                //myMaster = controller.getModel();
                //SetCurrentWeather(myMaster.currentWeather.code);
            }
            else // ** there are no file, user has not logged in yet, display the splash screen
            {
                PageTitle.Text = "Log In";
                splashScreen.Visibility = System.Windows.Visibility.Visible;
                errorMessageTextBlock.Text = "";
            }       
            //rotateClover.Begin();
            //rotateClover.Pause();

        }
        public void printError()
        {
            splashScreen.Visibility = System.Windows.Visibility.Visible;
            errorMessageTextBlock.Text = "Connection Error.";
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // ** this loads the animation
            //MainScreenLoad.Begin();
            
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

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
                // ** try to authenticate
                controller.authenticate(userNameTextBox.Text, passwordTextBox.Password);
                System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR USER INFO");
                controller.getUserXML();             
        }

        public void authenticationOk() // ** this function is called  if the autentication is successfull
        {
            // ** remove the splashscreen
            splashScreen.Visibility = System.Windows.Visibility.Collapsed;
            PageTitle.Text = "Main Page";
            myMaster = controller.getModel();
            SetCurrentWeather(myMaster.currentWeather.code);
            controller.getSensorsXML();
            GetPowerUsage();


            if (settingsFileExists == false) // ** if autentication was successfull and this is the first time when logging in, create new file
            {               
                // ** create the settings object, serialize it and write it to phone's memory
                SettingsFile mySettings = new SettingsFile();
                mySettings.username = userNameTextBox.Text;
                mySettings.password = passwordTextBox.Password;
                mySettings.serviceAddress = "http://localhost:3000";
                XmlSerilizierHelper.Serialize(fileName, mySettings);
                settingsFileExists = true;
            }
        }
        public void SetCurrentWeather(int code)
        {
            // get the time of day and determine if it is day or night
            //DateTime now = DateTime.Now;

            string weatherSource = "http://l.yimg.com/a/i/us/nws/weather/gr/" + code.ToString() + "d.png"; //34d.png
            System.Diagnostics.Debug.WriteLine("UI: weathersource is:  " + weatherSource);
            Uri uri = new Uri(weatherSource, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            currentWeather.Source = imgSource;
            currentWeather.Visibility = System.Windows.Visibility.Visible;
        }
        public void GetPowerUsage()
        {
            //currentPowerConsumptionTextBlock.Text = myMaster.currentSensors[1].latestReading.ToString();
            controller.getLatestNpoints(1, 20);
        }
    }
}