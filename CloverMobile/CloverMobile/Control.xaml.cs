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
        DispatcherTimer timer;
        Controller controller;
        DataMaster model;
        private bool Socket_Toggle = true;
        public Control()
        {
            InitializeComponent();
            controller = Controller.getInstance;  
            model = controller.getModel();
            controller.setActivePage(this);
            controller.setImageSource(this);
            // ** after 10 seconds, ask the model for new information
            model = controller.getModel();
            UpdateView();
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


        public void UpdateView()
        {
            System.Diagnostics.Debug.WriteLine("UI: timer.");
            controller.updateValueOfThisSensor(1);

            foreach (Output o in model.currentOutputs)
            {
                if (o.name == "heating")
                {
                    if (o.state == "true")
                    {
                        SocketAnimation_ON.Begin();
                        Socket_Toggle = true;
                    }
                    else
                    {
                        SocketAnimation_OFF.Begin();
                        Socket_Toggle = false;
                    }

                }
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
    }
}
