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
        private string xmlDocumentName;
        private string xmlMessage;

        public void setDataMaster(DataMaster mstr)
        {
            master = mstr;
        }
        public void setMasterController(Controller ctrl)
        {
            controller = ctrl; 
        }
        public NetworkController()
        {
            wcDown = new WebClient();
            wcUp = new WebClient();
        }
        public void sendValues(bool lightning, bool heating)
        {
            // ** send values
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
        public void GetSensorsXML(string serviceAddress)
        {
            wcDown.DownloadStringAsync(new Uri(serviceAddress + "outputs.xml"));
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);   
        }
        public void GetOutputsXML(string serviceAddress)
        {
            wcDown.DownloadStringAsync(new Uri(serviceAddress + "sensors.xml"));
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }
        public void getHistory(string serviceAddress)
        {
            wcDown.DownloadStringAsync(new Uri(serviceAddress + "hash.xml"));
            wcDown.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // ** xml document received fully
            dataDoc = XDocument.Load(new StringReader(e.Result));
            if (xmlDocumentName == "hash")
            {
                master.WriteHistory(dataDoc);
                // kerro controllerille että uutta dataa tuli
            }
            if (xmlDocumentName == "output")
            {
                master.WriteOutputs(dataDoc);
            }
            if (xmlDocumentName == "sensor")
            {
                master.WriteSensors(dataDoc);

            }
        }
        void wc_DownloadOutputsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // ** when downloaded, this happens
        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            // ** when uploaded, this happens
        }
    }
}
