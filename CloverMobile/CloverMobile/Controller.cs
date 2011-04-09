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
using Microsoft.Phone.Controls;

namespace CloverMobile
{
    public sealed class Controller
    {
        // ** controller is singleton
        static Controller instance=null;
        Device device;
        static readonly object padlock = new object();
        string address = "http://localhost:3000";
        string xmlType;

        // ** references to network controller, model (datamaster) and current ui page 
        private NetworkController nwc;
        private DataMaster model;
        private PhoneApplicationPage activePage;

        public void setActivePage(PhoneApplicationPage currentPage)
        {
            activePage = currentPage;
            activePage.GetType().ToString();
            System.Diagnostics.Debug.WriteLine("ACTIVE PAGE IS: " + activePage.GetType().ToString() );
            if (activePage.GetType().ToString() == "CloverMobile.MainPage")
            {
                CloverMobile.MainPage mainPageRef = (CloverMobile.MainPage)currentPage;
                mainPageRef.printError();                
            }
            //activePage = 
        }
        public DataMaster getModel()
        {
            return model;
        }
        private Controller()
        {
            device = new Device();
            nwc = new NetworkController();
            model = new DataMaster();
            nwc.setDataMaster(model);
            nwc.setMasterController(this);
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
        public bool authenticate(string userName, string password)
        {
            nwc.authenticate(userName, password);
            return true;       
        }
        public void getUserXML()
        {
            WorkItem newItem = new WorkItem("userInfo", 0);
            nwc.addNewDownloadWorkUnit(newItem);      
        }
        public void getSensorsXML()
        {
            
            device = model.getDevice();
            WorkItem newItem = new WorkItem("sensors", device.deviceId);
            nwc.addNewDownloadWorkUnit(newItem);
        }
        public void getOutputsXML()
        {
            device = model.getDevice();
            WorkItem newItem = new WorkItem("outputs", device.deviceId);
            nwc.addNewDownloadWorkUnit(newItem);
        }
        public void sendHeatingAndLightning(bool heating, bool lightning)
        { 

            //nwc.    
        }
        public void printErrorMessage(string message)
        { 
            
            //activePage
        }


    }
}
