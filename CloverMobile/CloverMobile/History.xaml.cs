using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;

namespace CloverMobile
{

    public partial class History : PhoneApplicationPage
    {
        public History()
        {
            InitializeComponent();
            // ** connect the web service here   
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            historyScreenLoad.Begin();
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.GB.serviceAddress+"readings/history/1.xml"));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }   

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // ** parse and display XML
                XDocument dataDoc = XDocument.Load(new StringReader(e.Result));
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
                    string historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now) + "     " + hd.powerConsumption0.ToString() + "kW";
                    listBox1.Items.Add(historyUnit);
                    historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1)) + "     " + hd.powerConsumption1.ToString() + "kW";
                    listBox1.Items.Add(historyUnit);
                    historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-2)) + "     " + hd.powerConsumption2.ToString() + "kW";
                    listBox1.Items.Add(historyUnit);
                    historyUnit = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-3)) + "     " + hd.powerConsumption3.ToString() + "kW";
                    listBox1.Items.Add(historyUnit);
                }
            }
            catch
            {

                listBox1.Items.Add("ERROR READING XML");
            }
        }
    
 
    }
}