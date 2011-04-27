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
        public Social()
        {
            InitializeComponent();
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


    }
}