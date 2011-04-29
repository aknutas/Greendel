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
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Data;

namespace CloverMobile
{
    public sealed class Controller
    {
        // ** controller is singleton
        static Controller instance=null;
        Device device;
        static readonly object padlock = new object();        
        private CloverMobile.MainPage mainPageRef;
        private CloverMobile.History historyRef;
        private CloverMobile.Social socialRef;
        private CloverMobile.Settings settingsRef;
        private CloverMobile.Control controlRef;
        //private Binding myBinding;
        
        private Uri uri;
        //private picturePath;
        private ImageSource imgSource;


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
            //pageReferences.Add(mainPageRef);
            //pageReferences.Add(historyRef);
            //pageReferences.Add(socialRef);
            //pageReferences.Add(controlRef);
            //pageReferences.Add(settingsRef);
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
        public void setImageSource(PhoneApplicationPage currentPage)
        {           

            activePage = currentPage;

            System.Diagnostics.Debug.WriteLine("Controller: Current power consumption is: " + GlobalData.currentConsumption.ToString());
            if ( GlobalData.currentConsumption < 200)
            {
                System.Diagnostics.Debug.WriteLine("Controller: binding1");
                uri = new Uri("Backgrounds/greendel_100pros.png", UriKind.Relative);
                    
            }
            else if (GlobalData.currentConsumption >= 200 && GlobalData.currentConsumption < 400)
            {
                System.Diagnostics.Debug.WriteLine("Controller: binding2");
                uri = new Uri("Backgrounds/greendel_80pros.png", UriKind.Relative);
            }
            else if (GlobalData.currentConsumption >= 400 && GlobalData.currentConsumption < 600)
            {
                System.Diagnostics.Debug.WriteLine("Controller: binding3");
                uri = new Uri("Backgrounds/greendel_60pros.png", UriKind.Relative);
            }
            else if (GlobalData.currentConsumption >= 600 && GlobalData.currentConsumption < 800)
            {
                System.Diagnostics.Debug.WriteLine("Controller: binding4");
                uri = new Uri("Backgrounds/greendel_40pros.png", UriKind.Relative);
            }
            else if (GlobalData.currentConsumption >= 800 && GlobalData.currentConsumption < 1000)
            {
                System.Diagnostics.Debug.WriteLine("Controller: binding5");
                uri = new Uri("Backgrounds/greendel_20pros.png", UriKind.Relative);
            }
            // determine, which page is active
            
            imgSource = new BitmapImage(uri);
       
            if (activePage.GetType().ToString() == "CloverMobile.MainPage")
            {

                mainPageRef = (CloverMobile.MainPage)currentPage;
                mainPageRef.FadeOut.Begin();
                mainPageRef.background.Source = imgSource;
                mainPageRef.FadeIn.Begin();
                historyRef = null;
                socialRef = null;
                settingsRef = null;
                controlRef = null;
            }
            else if (activePage.GetType().ToString() == "CloverMobile.History")
            {
                historyRef = (CloverMobile.History)currentPage;
                historyRef.background.Source = imgSource;
                mainPageRef = null;
                socialRef = null;
                settingsRef = null;
                controlRef = null;

            }
            else if (activePage.GetType().ToString() == "CloverMobile.Social")
            {
                socialRef = (CloverMobile.Social)currentPage;
                socialRef.background.Source = imgSource;
                mainPageRef = null;
                historyRef = null;
                settingsRef = null;
                controlRef = null;
            }
            else if (activePage.GetType().ToString() == "CloverMobile.Settings")
            {
                settingsRef = (CloverMobile.Settings)currentPage;
                settingsRef.background.Source = imgSource;
                mainPageRef = null;
                historyRef = null;
                socialRef = null;
                controlRef = null;

            }
            else if (activePage.GetType().ToString() == "CloverMobile.Control")
            {
                controlRef = (CloverMobile.Control)currentPage;
                controlRef.background.Source = imgSource;              
                settingsRef = null; ;
                mainPageRef = null;
                historyRef = null;
                socialRef = null;
            }
            
            //mainPageRef.UpdateLayout();
        }
        public DataMaster getModelInstance()
        {
           return model.getReference();        
        }
        public void setActivePage(PhoneApplicationPage currentPage)
        {
            activePage = currentPage;
            System.Diagnostics.Debug.WriteLine("Controller's active page is: " + activePage.GetType().ToString() );
            
            if (activePage.GetType().ToString() == "CloverMobile.MainPage")
            {
  
                mainPageRef = (CloverMobile.MainPage)currentPage;
                historyRef = null;
                socialRef = null;
                settingsRef = null;
                controlRef = null;
            }
            else if (activePage.GetType().ToString() == "CloverMobile.History")
            {
                historyRef = (CloverMobile.History)currentPage;
                mainPageRef = null;
                socialRef = null;
                settingsRef = null;
                controlRef = null;

            }
            else if (activePage.GetType().ToString() == "CloverMobile.Social")
            {
                socialRef = (CloverMobile.Social)currentPage;
                mainPageRef = null;
                historyRef = null;
                settingsRef = null;
                controlRef = null;
            }
            else if (activePage.GetType().ToString() == "CloverMobile.Settings")
            {
                settingsRef = (CloverMobile.Settings)currentPage;
                mainPageRef = null;
                historyRef = null;
                socialRef = null;
                controlRef = null;

            }
            else if (activePage.GetType().ToString() == "CloverMobile.Control")
            {
                controlRef = (CloverMobile.Control)currentPage;
                settingsRef = null; ;
                mainPageRef = null;
                historyRef = null;
                socialRef = null;
            }
        }
        public DataMaster getModel()
        {
            return model;
        }

        public void authenticate(string userName, string password, string serviceAddress)
        {
            // ** pass the autentication request from UI to NWC
            nwc.authenticate(userName, password, serviceAddress);
        }
        public void getUserXML()
        {
            System.Diagnostics.Debug.WriteLine("controller: getting user xml");
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
        public void getSensorHistoryFromSpecifiedTimeScale(int sensorId, string type, string frequency, string start, string end)
        {
            WorkItem newItem = new WorkItem("historyFromTimeScale", 0, sensorId, type, frequency, start, end);
            nwc.addNewDownloadWorkUnit(newItem);         
        }
        public void getLatestNpoints(int sensorId, int points)
        {
            WorkItem newItem = new WorkItem("latestSensorValues", 0, sensorId, points);
            nwc.addNewDownloadWorkUnit(newItem); 
        }

        public void sendOutputs(int sensorId, bool output1)
        {
            System.Diagnostics.Debug.WriteLine("controller: sending output state");
            WorkItem newItem = new WorkItem("sendOutput", 0, sensorId, output1);
            nwc.addNewUploadWorkUnit(newItem);
        }
        public void printErrorMessage(string message)
        {
            if (mainPageRef != null)
            {
                mainPageRef.printError();
            }
        }
        public void authenticationOk()
        {
            if (mainPageRef != null)
            {
                mainPageRef.authenticationOk();
            }
        }
        public void parseSensorsOk()
        {
            if (mainPageRef != null)
            {
                mainPageRef.GetPowerUsage();
            }
        }
        public void sensorHistoryDownloaded()
        {
            if (historyRef != null)
            {
                historyRef.SetGraphDataContext();
            }
        }
    }
}
