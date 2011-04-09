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
        private bool xmlRetrieved = false;
        private string documentType;
        private string xmlMessage;
        private string username = "testipaavo";
        private string password = "testi";
        private Thread downloader;
        private Thread uploader;



        public void setDataMaster(DataMaster mstr)
        {
            master = mstr;
            serviceAddress = "http://localhost:3000";
        }
        public void setMasterController(Controller ctrl)
        {
            controller = ctrl;
        }
        public NetworkController()
        {
            wcDown = new WebClient();
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wcUp = new WebClient();
        }
        public void authenticate(string username,string password)
        {
            
            //WebClient req = new WebClient();
            //req.Credentials = new NetworkCredential(username, password);
        }
        public void downloadXML(string documentName)
        {
            if (documentName == "userInfo")
            {
                downloader = new Thread(getUserInformationXML); // Kick off a new thread
                downloader.Start();
            }
            else if (documentName == "sensors")
            {
                downloader = new Thread(getSensorsXML); // Kick off a new thread
                downloader.Start();
            }
            else if (documentName == "outputs")
            {
                downloader = new Thread(getOutputsXML); // Kick off a new thread
                downloader.Start();
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
            System.Diagnostics.Debug.WriteLine("CALLING EVENT HANDLER");
            if (documentType == "userInfo")
            {

                System.Diagnostics.Debug.WriteLine("EVENT HANDLER FOR USER INFO");
                try
                {
                    // ** xml document received fully, give it to the master
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseUserInformation(dataDoc);
                }
                catch (WebException we)
                {
                    // print message to somewhere
                }
                documentType = "";
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
                try
                {
                    // ** xml document received fully, give it to the master
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    master.parseOutpus(dataDoc);
                }
                catch (WebException we)
                {
                    // print message to somewhere
                }
                documentType = "";
            }
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            // ** when uploaded, this happens
        }
    }
}
