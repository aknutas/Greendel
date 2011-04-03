using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CloverMobile
{
    public sealed class Controller
    {
        static Controller instance=null;
        static readonly object padlock = new object();
        private NetworkController nwc;
        private DataMaster model;  
        private Controller()
        {
            nwc = new NetworkController();
            model = new DataMaster();
        }
        public static Controller getInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new Controller();
                    }
                    return instance;
                }
            }
        }
        public void sendHeatingAndLightning(bool heating, bool lightning)
        { 

            //nwc.
        
        }

    }
}
