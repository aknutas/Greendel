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
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace CloverMobile
{
    public class NetworkController
    {
        private string serviceAddress;
        private DataMaster master;
        private Controller controller;
        private WebClient wcDown = null;
        private WebClient wcUp = null;
        private XDocument dataDoc;
        private bool downloading = false;
        private bool uploading = false;
        private string documentType = "";
        private string xmlMessage;
        private string username = "testipaavo";
        private string password = "testi2";
        private Thread downloader;
        private Thread uploader;
        private List<WorkItem> downloadWorkQueue;
        private List<WorkItem> uploadWorkQueue;
        private WorkItem currentWorkItem;


        public NetworkController()
        {
            downloadWorkQueue = new List<WorkItem>();
            wcDown = new WebClient();
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wcUp = new WebClient();

            downloader = new Thread(doDownloading);
            downloader.Name = "Downloader";
            downloader.Start();       
            //uploader = new Thread(doUploading);
            //uploader.Name = "Uploader";
            //uploader.Start();
            
        }
        public void setDataMaster(DataMaster mstr)
        {
            master = mstr;
            serviceAddress = "http://localhost:3000";
        }
        public void setMasterController(Controller ctrl)
        {
            controller = ctrl;
        }
        public void doDownloading()
        {
            while (true)
            {
                // ** there is some downloading to do
                if (downloadWorkQueue.Count > 0 && !downloading)
                {
                    downloading = true;

                        lock (downloadWorkQueue)
                        {
                            currentWorkItem = downloadWorkQueue[downloadWorkQueue.Count - 1];
                        }
                        switch (currentWorkItem.documentName)
                        {
                            case "userInfo":
                                documentType = "userInfo";
                                try
                                {
                                    wcDown.Credentials = new NetworkCredential(username, password);
                                    wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                                }
                                catch
                                {
                                    System.Diagnostics.Debug.WriteLine("!!!");
                                }
                                break;

                            case "sensors":
                                documentType = "sensors";
                                try
                                {
                                    wcDown.Credentials = new NetworkCredential(username, password);
                                    wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));
                                }
                                catch (WebException we)
                                {
                                    System.Diagnostics.Debug.WriteLine("!!!" + we.Message.ToString());
                                }
                                break;
                            case "outputs":
                                documentType = "outputs";
                                try
                                {
                                    wcDown.Credentials = new NetworkCredential(username, password);
                                    wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));
                                }
                                catch (WebException we)
                                {
                                    System.Diagnostics.Debug.WriteLine("!!!" + we.Message.ToString());
                                }
                                break;
                            case "sensor":
                                documentType = "sensor";
                                                                
                                try
                                {
                                    wcDown.Credentials = new NetworkCredential(username, password);
                                    wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/" + currentWorkItem.sensorId.ToString()));
                                }
                                catch (WebException we)
                                {
                                    System.Diagnostics.Debug.WriteLine("!!!" + we.Message.ToString());
                                }

                                break;
                            default:
                                break;
                        }
                        // ** delete workunit from the list
                        lock (downloadWorkQueue)
                        {
                            downloadWorkQueue.RemoveAt(downloadWorkQueue.Count - 1);
                        }
                }
                // ** work queue is empty
                else
                {
                    Thread.Sleep(50);               
                }           
            }       
        }
        public void doUploading()
        {
            // ** there is some downloading to do
            if (uploadWorkQueue.Count > 0 && !uploading)
            {
             uploading = true;
                    lock (uploadWorkQueue)
                    {
                        currentWorkItem = uploadWorkQueue[uploadWorkQueue.Count - 1];
                    }
                    switch (currentWorkItem.documentName)
                    { 
                        case "userInfo":

                                break;
                        case "sensors":

                                break;
                        case "outputs":

                                break;
                        default:
                        break;                                    
                    }
                    // ** delete workunit from the list
                    lock (uploadWorkQueue)
                    {
                        uploadWorkQueue.RemoveAt(uploadWorkQueue.Count - 1);
                    }
                }
                // ** work queue is empty
                else
                {
                    Thread.Sleep(50);               
                }           
        }

        // ** authorize the user
        public void authenticate(string name,string pass)
        {
            username = name;
            password = pass;
            wcDown.Credentials = new NetworkCredential(username, password);
            wcUp.Credentials = new NetworkCredential(username, password);
        }

        // ** Safely add new work unit to the list !
        public void addNewDownloadWorkUnit(WorkItem newItem)
        {
            lock (downloadWorkQueue)
            {        
                downloadWorkQueue.Add(newItem);           
            }
        }
        // ** Safely add new work unit to the list !
        public void addNewUploadWorkUnit(WorkItem newItem)
        {
            lock (uploadWorkQueue)
            {
                uploadWorkQueue.Add(newItem);
            }
        }
        public void getUserInformationXML()
        {
            documentType = "userInfo";
            try
            {
                wcDown.Credentials = new NetworkCredential(username, password);
                wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                //wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            }
            catch (WebException we)
            { 
                // print message
            }
        }
        public void getSensorsXML()
        {
            documentType = "sensors";
            try
            {
                wcDown.Credentials = new NetworkCredential(username, password);
                
                /*
                wcDown.Credentials = new NetworkCredential(username, password);
                wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                //wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                 * */
            }
            catch (WebException we)
            {
                System.Diagnostics.Debug.WriteLine("!!!");
            }
        }
        public void getOutputsXML()
        {
            documentType = "outputs";
            try
            {
                wcDown.Credentials = new NetworkCredential(username, password);
                wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                //wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            }
            catch (WebException we)
            {
                // print message
            }
        }


        public void sendValues(bool lightning, bool heating)
        {
            // ** this is old function
            wcUp.Headers[HttpRequestHeader.ContentType] = "application/xml";
            xmlMessage = "";
            
            if (heating)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/1"), "PUT", xmlMessage);
                wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }
            else if (!heating)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/1"), "PUT", xmlMessage);
                wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }

            else if (lightning)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/2"), "PUT", xmlMessage);
                wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }

            else if (lightning)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/2"), "PUT", xmlMessage);
                wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            downloading = false;

            if (e.Error != null) // ** connection error
            {

                System.Diagnostics.Debug.WriteLine("CONNECTION ERROR! " + e.Error.ToString());
                sendErrorMessage(e.Error.ToString());
                //downloader.Abort();
                // stop threads
                
            }
            System.Diagnostics.Debug.WriteLine("CALLING EVENT HANDLER");
            if (documentType == "userInfo")
            {
                System.Diagnostics.Debug.WriteLine("EVENT HANDLER FOR USER INFO");
                try
                {
                    // ** xml document received fully, give it to the master
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseUserInformation(dataDoc);
                    documentType = "";
                    sendAuthenticationOk();
                }
                catch (WebException we)
                {
                    // print message to somewhere
                }
            }
            else if (documentType == "sensors")
            {
                System.Diagnostics.Debug.WriteLine("EVENT HANDLER FOR SENSORS");
                try
                {
                    // ** xml document received fully, give it to the master
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseSensors(dataDoc);
                }
                catch (WebException we)
                {
                    // print message to somewhere
                }
                documentType = "";
            }
            else if (documentType == "outputs")
            {
                System.Diagnostics.Debug.WriteLine("EVENT HANDLER FOR OUTPUTS");
            
                 // ** xml document received fully, give it to the master
                 dataDoc = XDocument.Load(new StringReader(e.Result));
                 master.parseOutpus(dataDoc);
                documentType = "";
            }
            else if (documentType == "sensor")
            {
                System.Diagnostics.Debug.WriteLine("EVENT HANDLER FOR ONE SENSOR");
                // ** xml document received fully, give it to the master
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseOutpus(dataDoc);
          
                // print message to somewhere
              
                documentType = ""; 
            
            }
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            uploading = false;
        }
        public void sendErrorMessage(string errorMessage)
        {
            controller.printErrorMessage(errorMessage);
           
        }
        public void sendAuthenticationOk()
        {
            controller.authenticationOk();
        }
    }
}
