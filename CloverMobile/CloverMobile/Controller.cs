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
        private CloverMobile.MainPage mainPageRef;

        // ** references to network controller, model (datamaster) and current ui page 
        private NetworkController nwc;
        private DataMaster model;
        private PhoneApplicationPage activePage;

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
                    if (instance == null)
                    {
                        instance = new Controller();
                    }
                    return instance;
                }
            }
        }
        public DataMaster getModelInstance()
        {
           return model.getReference();        
        }
        public void setActivePage(PhoneApplicationPage currentPage)
        {
            activePage = currentPage;
            activePage.GetType().ToString();
            System.Diagnostics.Debug.WriteLine("ACTIVE PAGE IS: " + activePage.GetType().ToString() );
            if (activePage.GetType().ToString() == "CloverMobile.MainPage")
            {
                mainPageRef = (CloverMobile.MainPage)currentPage;            
            }
            //activePage = 
        }
        public DataMaster getModel()
        {
            return model;
        }

        public void authenticate(string userName, string password)
        {
            // ** pass the autentication request from UI to NWC
            nwc.authenticate(userName, password);
        }
        public void getUserXML()
        {
            WorkItem newItem = new WorkItem("userInfo");
            nwc.addNewDownloadWorkUnit(newItem);      
        }
        public void getSensorsXML()
        {
            device = model.currentDevice;
            WorkItem newItem = new WorkItem("sensors", device.deviceId);
            nwc.addNewDownloadWorkUnit(newItem);
        }
        public void getOutputsXML()
        {
            device = model.currentDevice;
            WorkItem newItem = new WorkItem("outputs", device.deviceId);
            nwc.addNewDownloadWorkUnit(newItem);
        }
        public void getSensorHistory(int sensorId)
        {
            WorkItem newItem = new WorkItem("sensor", 0, sensorId);
            nwc.addNewDownloadWorkUnit(newItem);
        }
        public void updateValueOfThisSensor(int sensorId)
        {
            WorkItem newItem = new WorkItem("sensorUpdate", 0, sensorId);
            nwc.addNewDownloadWorkUnit(newItem);      
        }
        public void getSensorHistoryFromSpecifiedTimeScale(int sensorId, string frequency, string start, string end)
        {
            WorkItem newItem = new WorkItem("historyFromTimeScale", 0, sensorId, frequency, start, end);
            nwc.addNewDownloadWorkUnit(newItem);         
        }
        public void getLatestNpoints(int sensorId, int points)
        {
            WorkItem newItem = new WorkItem("latestSensorValues", 0, sensorId, points);
            nwc.addNewDownloadWorkUnit(newItem); 
        }

        public void sendHeatingAndLightning(bool heating, bool lightning)
        { 
    
        }
        public void printErrorMessage(string message)
        {
            mainPageRef.printError();
        }
        public void authenticationOk()
        {
            mainPageRef.authenticationOk();       
        }
    }
}
