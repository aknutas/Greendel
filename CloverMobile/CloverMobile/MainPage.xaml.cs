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
        private string fileName = "settings.txt";
        // ** Constructor
        public MainPage()
        {
            InitializeComponent();
            controller = Controller.getInstance;
            controller.setActivePage(this);
            // ** Check the setting here, if the setting file exists, don't display the splashscreen, otherwise, display it
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (appStorage.FileExists(fileName) == true)
            {
                splashScreen.Visibility = System.Windows.Visibility.Collapsed;
  
                //logIn("testipaavo", "testi2");
                // ** dont display the splashscreen
            }
            else
            {
                PageTitle.Text = "Log In";
                splashScreen.Visibility = System.Windows.Visibility.Visible;
                errorMessageTextBlock.Text = "";
            }
            // splashScreen.visibility = System.Windows.Visibility.Visible/Collabsed
            // get the username and password from memory card          
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
            MainScreenLoad.Begin();
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

        private void windImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
                // ** if splashscreen is displayed, user has not logged in 
                controller.authenticate(userNameTextBox.Text, passwordTextBox.Password);
                System.Diagnostics.Debug.WriteLine("CALLING CONTROLLER FOR USER INFO");
                controller.getUserXML();
                
                   
        }
        public void authenticationOk()
        {
            // save the username and password to phone's flash memory to keep the user logged in
            splashScreen.Visibility = System.Windows.Visibility.Collapsed;
            PageTitle.Text = "Main Page";
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            // ** authenticate first, if successfull, then create the file

            if (appStorage.FileExists("settings.txt") == false)
            {

                // create a new file
                using (var file = appStorage.OpenFile(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.Write(userNameTextBox.Text + " " + passwordTextBox.Password);
                    }
                }
            }  
        
        }
        private void passwordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // delete this
        }

        private void userNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // delete this
        }
    }
}