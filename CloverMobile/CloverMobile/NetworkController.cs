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
        private string username;
        private string password;
        private Thread downloader;
        private Thread uploader;
        private List<WorkItem> downloadWorkQueue;
        private List<WorkItem> uploadWorkQueue;
        private WorkItem currentWorkItem;
        private int currentSensorId;


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
            //serviceAddress = "http://greendel.heroku.com:80";
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
                        currentWorkItem = downloadWorkQueue.First(); //(downloadWorkQueue.Count - downloadWorkQueue.Count-1)
                    }
                    switch (currentWorkItem.documentName)
                    {
                        case "userInfo":
                            documentType = "userInfo";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/users/datastatus/1"));
                            break;

                        case "sensors":
                            documentType = "sensors";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));                 
                            break;

                        case "outputs":
                            documentType = "outputs";
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/devices/datastatus/" + currentWorkItem.deviceId.ToString()));                    
                            break;

                        case "sensor":
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentSensorId.ToString()));                   
                            break;

                        case "sensorUpdate":
                            documentType = "sensorUpdate";
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/" + currentSensorId.ToString() + ".xml"));
                            break;

                        case "historyFromTimeScale":
                            documentType = "historyFromTimeScale";
                            currentSensorId = currentWorkItem.sensorId;
                            // /sensors/history/<sensorid>.xml?avgscale=<hourly|daily|monthly>&startdate=<yyyy-mm-dd>&enddate=<yyyy-mm-dd>
                            // /sensors/history/<sensorid>.xml?diffscale=<hourly|daily|monthly>&startdate=<yyyy-mm-dd>&enddate=<yyyy-mm-dd>
                            string uri = serviceAddress + "/sensors/history/" + currentWorkItem.sensorId.ToString() + ".xml?" + currentWorkItem.historyInfoType + "=" + currentWorkItem.frequency + "&startdate=" + currentWorkItem.start + "&enddate=" + currentWorkItem.end;
                            System.Diagnostics.Debug.WriteLine("nwc: current uri is:" + uri);
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentWorkItem.sensorId.ToString() + ".xml?" + currentWorkItem.historyInfoType + "="  + currentWorkItem.frequency + "&startdate=" + currentWorkItem.start + "&enddate=" + currentWorkItem.end));
                            break;

                        case "latestSensorValues":
                            documentType = "latestSensorValues";
                            currentSensorId = currentWorkItem.sensorId;
                            wcDown.DownloadStringAsync(new Uri(serviceAddress + "/sensors/history/" + currentWorkItem.sensorId.ToString() + ".xml?limit=" + currentWorkItem.pointsToGet.ToString()));
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
                // ** there is some downloading to do
                if (uploadWorkQueue.Count > 0 && !uploading)
                {
                    uploading = true;
                    lock (uploadWorkQueue)
                    {
                        System.Diagnostics.Debug.WriteLine("nwc: getting first upload queue member");
                        currentWorkItem = uploadWorkQueue.First();
                    }
                    switch (currentWorkItem.documentName)
                    {
                        case "userInfo":

                            break;
                        case "sensors":

                            break;
                        case "sendOutput":
                            System.Diagnostics.Debug.WriteLine("nwc: sending output state.");
                            wcUp.Headers[HttpRequestHeader.ContentType] = "application/xml";
                            
                            documentType = "sendOutput";
                            if (currentWorkItem.output_state == true) {
                                System.Diagnostics.Debug.WriteLine("SENDOUTPUT: sending output state true.");
                                xmlMessage = "<output><haschanged>true</haschanged><state>true</state></output>";
                            }
                            else{
                                System.Diagnostics.Debug.WriteLine("SENDOUTPUT: sending output state false.");
                                xmlMessage = "<output><haschanged>true</haschanged><state>false</state></output>";
                            }
                            
                            wcUp.UploadStringAsync(new Uri("http://localhost:3000/outputs/1.xml"), "PUT", xmlMessage);

                            //wcUp.UploadStringAsync(new Uri(serviceAddress + "outputs/" + currentWorkItem.sensorId.ToString()), "PUT", xmlMessage);
                            
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
                    controller.parseSensorsOk();
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
                    controller.sensorHistoryDownloaded();
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
                    controller.sensorHistoryDownloaded();
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
                if (documentType == "sendOutput")
                {
                    System.Diagnostics.Debug.WriteLine("nwc: asking model for userinfo..");
                    // Return HTTP message
                    System.Diagnostics.Debug.WriteLine(e.Result.ToString());
                    documentType = "";
                 
                }
                
            }
            catch (WebException we)
            {
                we.Message.ToString();
            }

        }
    }
}
