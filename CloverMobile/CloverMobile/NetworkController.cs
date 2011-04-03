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
        private WebClient wcDown = null;
        private WebClient wcUp = null;
        private XDocument dataDoc;
        private bool xmlRetrieved = false;
        private string xmlDocumentName;
        private string xmlMessage;

        public NetworkController()
        {
            //wc = new WebClient();
        }
        public void sendValues(bool lightning, bool heating)
        {
            // ** send values
            wcUp = new WebClient();
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
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (xmlDocumentName == "hash")
            {
                    // ** parse and display XML
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    
                    var historyValues = from historyValue in dataDoc.Descendants("hash")
                    select new HistoryData
                    {
                        powerConsumption0 = int.Parse(historyValue.Element("today").Value),
                        powerConsumption1 = int.Parse(historyValue.Element("yesterday").Value),
                        powerConsumption2 = int.Parse(historyValue.Element("daysago2").Value),
                        powerConsumption3 = int.Parse(historyValue.Element("daysago3").Value),
                   };

                    foreach (HistoryData hd in historyValues)
                    {
                        /*
                        string historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now) + "     " + hd.powerConsumption0.ToString() + "kW";
                        listBox1.Items.Add(historyUnit);
                        historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1)) + "     " + hd.powerConsumption1.ToString() + "kW";
                        listBox1.Items.Add(historyUnit);
                        historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-2)) + "     " + hd.powerConsumption2.ToString() + "kW";
                        listBox1.Items.Add(historyUnit);
                        historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-3)) + "     " + hd.powerConsumption3.ToString() + "kW";
                        listBox1.Items.Add(historyUnit);
                        */
                    }
            }
            if (xmlDocumentName == "output")
            {
                // ** parse and display XML
                dataDoc = XDocument.Load(new StringReader(e.Result));
                var dataUnitoutputs = from output in dataDoc.Descendants("output")
                select new Output
                {
                    created = DateTime.Parse(output.Element("created-at").Value),
                    deviceId = int.Parse(output.Element("device-id").Value),
                    id = int.Parse(output.Element("id").Value),
                    name = output.Element("name").Value.ToString(),
                    state = bool.Parse(output.Element("state").Value),
                    updatedAt = DateTime.Parse(output.Element("updated-at").Value),
               };

                foreach (Output o in dataUnitoutputs)
                {
                    /*
                    // ** find the states of solar and wind and set them to the screen
                    if (o.name == "solar")
                    {
                        if (o.state == true)
                        {
                            Uri uri = new Uri("Sun_enabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            solarImage.Source = imgSource;
                        }
                        else
                        {
                            Uri uri = new Uri("Sun_disabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            solarImage.Source = imgSource;
                        }
                    }
                    if (o.name == "wind")
                    {
                        if (o.state == true)
                        {
                            Uri uri = new Uri("Wind_enabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            windImage.Source = imgSource;
                        }
                        else
                        {
                            Uri uri = new Uri("Wind_disabled.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            windImage.Source = imgSource;
                        }
                    }
                    */
                }
            }
            if (xmlDocumentName == "sensor")
            {
                    // ** parse XML
                    dataDoc = XDocument.Load(new StringReader(e.Result));
                    var sensorOutputs = from sensor in dataDoc.Descendants("sensor")
                    select new Sensor
                    {
                        createdAt = DateTime.Parse(sensor.Element("created-at").Value),
                        deviceId = int.Parse(sensor.Element("device-id").Value),
                        id = int.Parse(sensor.Element("id").Value),
                        latestReading = int.Parse(sensor.Element("latestreading").Value),
                        name = sensor.Element("name").Value.ToString(),
                        updatedAt = DateTime.Parse(sensor.Element("updated-at").Value),
                    };
                foreach (Sensor s in sensorOutputs)
                {
                /*
                if (s.name == "poweruse")
                    {
                        if (s.latestReading == 0)
                        {
                            rotateClover.Pause();
                            textBlock2.Text = "0 kW";
                        }
                        else if (s.latestReading < 0) // oikealle vihreä
                        {
                            rotateClover.Pause();

                            Uri uri = new Uri("Clover.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            image.Source = imgSource;
                            myAnimation.To = 360;
                            if (s.latestReading == -1000)
                            {
                                textBlock2.Text = "-1000 kW";
                                rotateClover.SpeedRatio = 0.2;
                            }
                            else if (s.latestReading == -2000)
                            {
                                textBlock2.Text = "-2000 kW";
                                rotateClover.SpeedRatio = 2.0;
                            }
                            rotateClover.Resume();
                        }
                        else if (s.latestReading > 0) // punainen vasen
                        {
                            rotateClover.Pause();
                            Uri uri = new Uri("CloverRed.png", UriKind.Relative);
                            ImageSource imgSource = new BitmapImage(uri);
                            image.Source = imgSource;
                            myAnimation.To = -360;
                            if (s.latestReading == 1000)
                            {
                                textBlock2.Text = "1000 kW";
                                rotateClover.SpeedRatio = 0.2;
                            }
                            else if (s.latestReading == 2000)
                            {
                                textBlock2.Text = "2000 kW";
                                rotateClover.SpeedRatio = 2.0;
                            }
                            rotateClover.Resume();
                        }
                    }
                */
                }      
            } 
        }

        void wc_DownloadOutputsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

        }
        void wcUpload_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {

        }

    }
}
