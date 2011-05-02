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
    public partial class Settings : PhoneApplicationPage
    {
        private DataMaster myMaster;
        private Controller controller;
        private PowerPrice currentpowerPrice;
        public Settings()
        {
            InitializeComponent();
            controller = Controller.getInstance;
            //controller.setActivePage(this);
            controller.setImageSource(this);
            myMaster = controller.getModel();
            currentpowerPrice = myMaster.currentPowerPrise;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            settingsScreenAnimation.Begin();
            ppTextBox.Text = currentpowerPrice.powerPrice.ToString() + " €";
            monthlyUseTextBox.Text = currentpowerPrice.lmuse.ToString() + " kW/h";
            monthlyPriceTextBox.Text = currentpowerPrice.lmprice.ToString() + " €";
            weeklyUseTextBox.Text = currentpowerPrice.lwuse.ToString() + " kW/h";
            weeklyPriceTextBox.Text = currentpowerPrice.lmprice.ToString() + " €";


        }



        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            System.Diagnostics.Debug.WriteLine("Reverse animation");
            settingsScreenAnimationReverse.Begin();


        }

        private void settingsScreenAnimationReverse_Completed(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

      


    }
}