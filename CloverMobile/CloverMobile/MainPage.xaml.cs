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

namespace CloverMobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        Controller controller; 
        
        // ** Constructor
        public MainPage()
        {
            InitializeComponent();
            controller = Controller.getInstance;
            rotateClover.Begin();
            rotateClover.Pause();

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
    }
}