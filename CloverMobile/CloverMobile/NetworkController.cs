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
        //private bool uploading = false;
        private string documentType = "";
        private string xmlMessage;
        private string username;
        private string password;
        private Thread downloader;
        //private Thread uploader;
        private List<WorkItem> downloadWorkQueue;
        //private List<WorkItem> uploadWorkQueue;
        private WorkItem currentWorkItem;
        private int currentSensorId;


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
                            //wcDown.Credentials = new NetworkCredential(username, password);
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                            break;

                        case "sensors":
                            documentType = "sensors";
                            //wcDown.Credentials = new NetworkCredential(username, password);
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));                 
                            break;

                        case "outputs":
                            documentType = "outputs";
                            //wcDown.Credentials = new NetworkCredential(username, password);
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));                    
                            break;

                        case "sensor":
                            documentType = "sensor";
                            //wcDown.Credentials = new NetworkCredential(username, password);
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentSensorId.ToString()));                   
                            break;
                        case "sensorUpdate":
                            documentType = "sensorUpdate";
                            //wcDown.Credentials = new NetworkCredential(username, password);
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/" + currentSensorId.ToString() + ".xml"));
                            break;
                        case "historyFromTimeScale":
                            documentType = "historyFromTimeScale";
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentWorkItem.sensorId.ToString() + ".xml?avgscale="  + currentWorkItem.frequency + "&startdate=" + currentWorkItem.start + "&enddate=" + currentWorkItem.end));
                            // view-source/sensors/history/1.xml?avgscale=daily&startdate=2011-03-01&enddate=2011-04-01
                            break;
                        case "latestSensorValues":
                            documentType = "latestSensorValues";
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentWorkItem.sensorId.ToString() + ".xml?limit=" + currentWorkItem.pointsToGet.ToString()));
                            break;
                            // /sensors/history/<sensorid>.xml?limit=<n>
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
        /*
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
        */
        // ** authorize the user
        public void authenticate(string name, string pass)
        {
            username = name;
            password = pass;
            wcDown.Credentials = new NetworkCredential(username, password);
            wcUp.Credentials = new NetworkCredential(username, password);
        }

        // ** Safely add new work unit to the download list !
        public void addNewDownloadWorkUnit(WorkItem newItem)
        {
            lock (downloadWorkQueue)
            {
                downloadWorkQueue.Add(newItem);
            }
        }
        // ** Safely add new work unit to the upload list !
        /*
        public void addNewUploadWorkUnit(WorkItem newItem)
        {
            lock (uploadWorkQueue)
            {
                uploadWorkQueue.Add(newItem);
            }
        }
        */
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
                controller.printErrorMessage(e.Error.ToString());
            }
            System.Diagnostics.Debug.WriteLine("nwc: finished downloading xml.");
            if (documentType == "userInfo")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for userinfo..");
                // ** xml document received fully, give it to the master for parsing
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseUserInformation(dataDoc);
                documentType = "";
                // ** successfully downloaded basic document, inform about successfull autetication 
                controller.authenticationOk();
            }
            else if (documentType == "sensors")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for sensors..");
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseSensors(dataDoc);
                documentType = "";
            }
            else if (documentType == "outputs")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for outputs..");
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseOutpus(dataDoc);
                documentType = "";
            }
            else if (documentType == "sensor")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor history..");
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseSensorHistory(currentSensorId, dataDoc);
                documentType = "";
            }
            else if (documentType == "sensorUpdate")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor update..");
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseSingleSensorForNewHistoryDatapoint(currentSensorId, dataDoc);
                documentType = "";
            }
            else if (documentType == "historyFromTimeScale" || documentType == "latestSensorValues")
            {
                System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor timescale history information..");
                dataDoc = XDocument.Load(new StringReader(e.Result));
                master.parseSensorTimeScaleHistory(currentSensorId, dataDoc);
                documentType = "";    
            }
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //uploading = false;
        }
    }
}
