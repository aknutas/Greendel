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
        Controller controller;
        DataMaster model;
        private bool Socket_Toggle;

        public Control()
        {
            InitializeComponent();
            controller = Controller.getInstance;  
            model = controller.getModel();
            controller.setActivePage(this);
            controller.setImageSource(this);

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
        public void UpdateView() // this is called by controller when the outputs are updated
        {
            
            
            /*
            foreach (Output o in model.currentOutputs)
            {
                if (o.name == "wall_socket")
                {
                    if (o.state == "true")
                    {
                        Socket.Content = "ON";
                        Socket_Toggle = true;
                        //SocketAnimation_ON.Begin();

                    }
                    else if (o.state == "false")
                    {
                        Socket.Content = "OFF";
                        Socket_Toggle = false;
                        //SocketAnimation_OFF.Begin();
                    }

                }
            }
             */
            
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
            // ** when outputs received, set the current value for outputs
            foreach (Output o in model.currentOutputs)
            {
                if (o.name == "wall_socket")
                {
                    if (o.state == "true")
                    {
                        Socket_Toggle = true;
                        Socket.Content = "ON";
                        //SocketAnimation_ON.Begin();

                    }
                    else
                    {
                        Socket_Toggle = false;
                        Socket.Content = "OFF";
                        //SocketAnimation_OFF.Begin();
                    }
                }
            }
        
        }
    }
}
