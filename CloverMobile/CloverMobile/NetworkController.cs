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
        private string downloaDocumentType = "";
        private string uploadDocumentType = "";
        private string xmlMessage;
        private string username;
        private string password;
        private Thread downloader;
        private Thread uploader;
        private List<WorkItem> downloadWorkQueue;
        private List<WorkItem> uploadWorkQueue;
        private WorkItem currentDownloadWorkItem;
        private WorkItem currentUploadWorkItem;
        private int currentSensorId;
        private int currentOutputId;


        public NetworkController()
        {
            downloadWorkQueue = new List<WorkItem>();
            uploadWorkQueue = new List<WorkItem>();

            wcDown = new WebClient();
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wcUp = new WebClient();
            wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);

            downloader = new Thread(doDownloading);
            downloader.Name = "Downloader";
            downloader.Start();

            uploader = new Thread(doUploading);
            uploader.Name = "Uploader";
            uploader.Start();
        }
        public void setDataMaster(DataMaster mstr)
        {
            master = mstr;
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
                        currentDownloadWorkItem = downloadWorkQueue.First(); //(downloadWorkQueue.Count - downloadWorkQueue.Count-1)
                    }
                    switch (currentDownloadWorkItem.documentName)
                    {
                        case "userInfo":
                            downloaDocumentType = "userInfo";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                            break;

                        case "sensors":
                            downloaDocumentType = "sensors";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentDownloadWorkItem.deviceId.ToString()));                 
                            break;

                        case "outputs":
                            downloaDocumentType = "outputs";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentDownloadWorkItem.deviceId.ToString()));                    
                            break;

                        case "sensor":
                            currentSensorId = currentDownloadWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentSensorId.ToString()));                   
                            break;

                        case "sensorUpdate":
                            downloaDocumentType = "sensorUpdate";
                            currentSensorId = currentDownloadWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/" + currentSensorId.ToString() + ".xml"));
                            break;

                        case "historyFromTimeScale":
                            downloaDocumentType = "historyFromTimeScale";
                            currentSensorId = currentDownloadWorkItem.sensorId;
                            // /sensors/history/<sensorid>.xml?avgscale=<hourly|daily|monthly>&startdate=<yyyy-mm-dd>&enddate=<yyyy-mm-dd>
                            // /sensors/history/<sensorid>.xml?diffscale=<hourly|daily|monthly>&startdate=<yyyy-mm-dd>&enddate=<yyyy-mm-dd>
                            string uri = serviceAddress + "/sensors/history/" + currentDownloadWorkItem.sensorId.ToString() + ".xml?" + currentDownloadWorkItem.historyInfoType + "=" + currentDownloadWorkItem.frequency + "&startdate=" + currentDownloadWorkItem.start + "&enddate=" + currentDownloadWorkItem.end;
                            System.Diagnostics.Debug.WriteLine("nwc: current uri is:" + uri);
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentDownloadWorkItem.sensorId.ToString() + ".xml?" + currentDownloadWorkItem.historyInfoType + "="  + currentDownloadWorkItem.frequency + "&startdate=" + currentDownloadWorkItem.start + "&enddate=" + currentDownloadWorkItem.end));
                            break;

                        case "latestSensorValues":
                            downloaDocumentType = "latestSensorValues";
                            currentSensorId = currentDownloadWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentDownloadWorkItem.sensorId.ToString() + ".xml?limit=" + currentDownloadWorkItem.pointsToGet.ToString()));
                            break;

                        case "outputUpdate":
                            downloaDocumentType = "outputUpdate";
                            currentOutputId = currentDownloadWorkItem.outputId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/outputs/" + currentOutputId.ToString() + ".xml"));
                            break;
                                
                        default:
                            break;
                    }
                    // ** delete workunit from the list
                    lock (downloadWorkQueue)
                    {
                        downloadWorkQueue.RemoveAt(0);
                    }
                }
                // ** work queue is empty
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        
        public void doUploading()
        {
            while (true)
            {
                // ** there is some uploading to do
                if (uploadWorkQueue.Count > 0 && !uploading)
                {
                    uploading = true;
                    lock (uploadWorkQueue)
                    {
                        System.Diagnostics.Debug.WriteLine("nwc: getting first upload queue member");
                        currentUploadWorkItem = uploadWorkQueue.First();
                    }
                    switch (currentUploadWorkItem.documentName)
                    {
                        case "sendOutput":
                            //System.Diagnostics.Debug.WriteLine("nwc: sending output state."); 
                            uploadDocumentType = "sendOutput";
                            currentOutputId = currentUploadWorkItem.outputId;

                            if (currentUploadWorkItem.outputState == true) 
                            {
                                
                                System.Diagnostics.Debug.WriteLine("nwc: sending output state true.");
                                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("nwc: sending output state false.");
                                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                            }
                            wcUp.Headers[HttpRequestHeader.ContentType] = "application/xml";
                            wcUp.UploadStringAsync(new Uri(serviceAddress + "/outputs/" + currentOutputId + ".xml"), "PUT", xmlMessage);
                            System.Diagnostics.Debug.WriteLine(xmlMessage); 
                            
                            break;

                        default:
                            break;
                    }
                    // ** delete workunit from the list
                    lock (uploadWorkQueue)
                    {
                        System.Diagnostics.Debug.WriteLine("nwc: deleting first member of the upload queue");
                        uploadWorkQueue.RemoveAt(0);
                    }
                }
                // ** work queue is empty
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        
        // ** authorize the user
        public void authenticate(string name, string pass, string address)
        {
            username = name;
            password = pass;
            serviceAddress = address;
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
        
        public void addNewUploadWorkUnit(WorkItem newItem)
        {
            lock (uploadWorkQueue)
            {
                System.Diagnostics.Debug.WriteLine("nwc: adding workitem to upload que");
                uploadWorkQueue.Add(newItem);
            }
        }
        
        public void sendOutput(bool output)
        {
            // ** this is old function
            wcUp.Headers[HttpRequestHeader.ContentType] = "application/xml";
            xmlMessage = "";

            if (output)
            {
                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/2"), "PUT", xmlMessage);
                wcUp.UploadStringCompleted += new UploadStringCompletedEventHandler(wcUpload_UploadStringCompleted);
            }

            else if (!output)
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
            try
            {
                System.Diagnostics.Debug.WriteLine("nwc: finished downloading xml.");
                if (downloaDocumentType == "userInfo")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for userinfo..");
                    // ** xml document received fully, give it to the master for parsing
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseUserInformation(dataDoc);
                    downloaDocumentType = "";
                    // ** successfully downloaded basic document, inform about successfull autetication 
                    controller.authenticationOk();
                }
                else if (downloaDocumentType == "sensors")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for sensors..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseSensors(dataDoc);
                    downloaDocumentType = "";
                    controller.parseSensorsOk();
                }
                else if (downloaDocumentType == "outputs")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for outputs..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseOutpus(dataDoc);
                    downloaDocumentType = "";
                    controller.Outputsdownloaded();
                }
                else if (downloaDocumentType == "sensor")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor history..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseSensorHistory(currentSensorId, dataDoc);
                    downloaDocumentType = "";
                    controller.sensorHistoryDownloaded();
                }
                else if (downloaDocumentType == "sensorUpdate")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor update..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseSingleSensorForNewHistoryDatapoint(currentSensorId, dataDoc);
                    downloaDocumentType = "";
                    
                }
                else if (downloaDocumentType == "historyFromTimeScale" || downloaDocumentType == "latestSensorValues")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for sensor timescale history information..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseSensorTimeScaleHistory(currentSensorId, dataDoc);
                    downloaDocumentType = "";
                    controller.sensorHistoryDownloaded();
                }
                else if (downloaDocumentType == "outputUpdate")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for single output information..");
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    System.Diagnostics.Debug.WriteLine(e.Result.ToString());
                    master.parseSingleOutput(currentOutputId, dataDoc);
                    downloaDocumentType = "";
                    controller.outputUpdated();
                }
            }
            catch (WebException we)
            {
                we.Message.ToString();
            }
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //sendOutput
            uploading = false;
            if (e.Error != null) // ** connection error
            {
                System.Diagnostics.Debug.WriteLine("CONNECTION ERROR! " + e.Error.ToString());
                controller.printErrorMessage(e.Error.ToString());
            }
            try
            {
                System.Diagnostics.Debug.WriteLine("nwc: finished uploading xml.");
                if (uploadDocumentType == "sendOutput")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: event handler output changes sent");             
                    uploadDocumentType = "";
                    controller.updateValueForThisOutput(currentOutputId);
                    
                    //controller.getOutputsXML();
                    //controller.updateControlPageView();  
                }              
            }
            catch (WebException we)
            {
                we.Message.ToString();
            }

        }
    }
}
