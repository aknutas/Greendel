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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace CloverMobile
{
    public class DataMaster
    {
        private bool isUpdated;
        private XDocument dataDoc;

        private User currentUser;
        private Device currentDevice;
        private Location currentLocation;
        private Weather currentWeather;
        private List<Sensor> allSensors;
        private List<Output> allOutputs; 
        
       // private List<HistoryData> allHistoryData;

        public DataMaster()
        {      
            currentUser = new User();
            currentDevice = new Device();
            currentLocation = new Location();
            currentWeather = new Weather();
            allOutputs = new List<Output>();
            allSensors = new List<Sensor>();
            dataDoc = new XDocument();
            //allHistoryData = new List<HistoryData>();
        }
        public List<Sensor> getSensors()
        {
            return allSensors;
        }
        public List<Output> getOutputs()
        {
            return allOutputs;
        }
        public void parseXML(XDocument xmlDoc)
        {

            System.Diagnostics.Debug.WriteLine("Parsing XML...");
            var user = from userValue in xmlDoc.Descendants("user")
                       select new User
                       {
                           id = int.Parse(userValue.Element("id").Value),
                           name = userValue.Element("name").Value.ToString(),
                           realName = userValue.Element("realname").Value.ToString(),
                       };
            foreach (User u in user)
            {
                System.Diagnostics.Debug.WriteLine("USER:" + " " + u.id.ToString() + " " + u.name + " " + u.realName);
            }

            var device = from deviceValue in xmlDoc.Descendants("device")
                         select new Device
                         {
                             deviceId = int.Parse(deviceValue.Element("id").Value),
                             deviceName = deviceValue.Element("name").Value.ToString(),
                         };
            foreach (Device d in device)
            {
                System.Diagnostics.Debug.WriteLine("DEVICE:" + " " + d.deviceId.ToString() + " " + d.deviceName);
            }

            var location = from locationValue in xmlDoc.Descendants("location")
                           select new Location
                           {
                               address = locationValue.Element("address").Value.ToString(),
                               town = locationValue.Element("town").Value.ToString(),

                           };
            foreach (Location l in location)
            {
                System.Diagnostics.Debug.WriteLine("LOCATION:" + " " + l.address + " " + l.town);
            }

            var weather = from weatherValue in xmlDoc.Descendants("weather")
                          select new Weather
                          {
                              temp = float.Parse(weatherValue.Element("temp").Value),
                              unit = weatherValue.Element("unit").Value.ToString(),
                              description = weatherValue.Element("desc").Value.ToString(),
                          };
            foreach (Weather w in weather)
            {
                System.Diagnostics.Debug.WriteLine("WEATHER:" + " " + w.temp.ToString() + " " + w.unit + " " + w.description);
            }
        }
            //allSensors.Clear();
        public SensorInformation parseSensors(XDocument xmlDoc)
        {
                //System.Diagnostics.Debug.WriteLine(xmlDoc.ToString());

                var allSensors = new SensorInformation();
                /*
                XElement generalElement = xmlDoc
                        .Element("user")
                        .Element("device")
                        .Element("location")
                        .Element("sensors")                     
                        ;
                */
                //students.School = generalElement.Element("School").Value;
                //students.Department = generalElement.Element("Department").Value;
                try
                {
                    allSensors.mySensors = (from s in xmlDoc.Descendants("sensor")
                                            select new Sensor()
                                            {
                                                sensorId = Convert.ToInt16(s.Element("id").Value),
                                                sensorName = s.Element("name").Value.ToString(),
                                                // add long name
                                                sensorVarType = s.Element("vartype").Value.ToString(),
                                                latestReading = s.Element("latestreading") != null ? s.Element("latestreading").Value : string.Empty
                                            }).ToList<Sensor>();
                }
                catch (FormatException f)
                {
                    System.Diagnostics.Debug.WriteLine(f.Message.ToString());
                }
                foreach (Sensor si in allSensors.mySensors)
                {
                   System.Diagnostics.Debug.WriteLine("SENSORS: " + si.sensorId.ToString() + " " + si.sensorName + " " + si.sensorVarType.ToString()  ); 
                }

                return allSensors;
        }
        /*
        public OutputInformation parseOutpus(XDocument xmlDoc)
        {
        
        
        }
         */ 
                /*
                System.Diagnostics.Debug.WriteLine("Parsing outputs and sensors: ");



                var sensorOutputs = from sensors in dataDoc.Descendants("sensors")
                //select new Sensor
                select new Sensor
                {
             
                   sensorId = int.Parse(sensors.Element("id").Value),
                   sensorName = sensors.Element("name").Value.ToString(),
                   sensorVarType = sensors.Element("vartype").Value,
                   latestReading = int.Parse(sensors.Element("latestreading").Value), 
                    
                    
                                              
                };
            
                foreach (Sensor s in sensorOutputs)
                {
                   System.Diagnostics.Debug.WriteLine(s.sensorId.ToString() + " " + s.sensorName + " " + s.sensorVarType);
                   allSensors.Add(s);
                   System.Diagnostics.Debug.WriteLine("FOR EACH ");
                }
         
                //allOutputs.Clear();
                var dataUnitoutputs = from outputs in dataDoc.Descendants("output")
                select new Output
                {
                    id = int.Parse(outputs.Element("id").Value),
                    name = outputs.Element("name").Value.ToString(),
                    state = bool.Parse(outputs.Element("state").Value),
                    hasChanged = bool.Parse(outputs.Element("haschanged").Value),
                };
                
                foreach (Output o in dataUnitoutputs)
                {
                    System.Diagnostics.Debug.WriteLine(o.id.ToString() + " " + o.name + " " + o.state.ToString() + " " + o.hasChanged.ToString());
                    allOutputs.Add(o);
                    System.Diagnostics.Debug.WriteLine("FOR EACH ");
                    
                }
        public void WriteHistory(XDocument xmlDoc)
        {
            /*
            allHistoryData.Clear();
            var historyValues = from historyValue in xmlDoc.Descendants("hash")
            select new HistoryData
            {
                powerConsumption0 = int.Parse(historyValue.Element("today").Value),
                powerConsumption1 = int.Parse(historyValue.Element("yesterday").Value),
                powerConsumption2 = int.Parse(historyValue.Element("daysago2").Value),
                powerConsumption3 = int.Parse(historyValue.Element("daysago3").Value),
            };
            foreach (HistoryData hd in historyValues)
            {
                allHistoryData.Add(hd);
            }
            isUpdated = true;
           */
        //}
        public void WriteOutputs(XDocument xmlDoc)
        {
            allOutputs.Clear();
            var dataUnitoutputs = from output in dataDoc.Descendants("output")
            select new Output
            {
                id = int.Parse(output.Element("id").Value),
                name = output.Element("name").Value.ToString(),
                state = bool.Parse(output.Element("state").Value),
                hasChanged = bool.Parse(output.Element("haschanged").Value),
            };
            foreach (Output o in dataUnitoutputs)
            {
                allOutputs.Add(o);
            }
            isUpdated = true;        
        }
        public void WriteSensors(XDocument xmlDoc)
        {
            allSensors.Clear();
            var sensorOutputs = from sensor in dataDoc.Descendants("sensor")
            select new Sensor
            {
                sensorId = int.Parse(sensor.Element("id").Value),
                sensorName = sensor.Element("name").Value.ToString(),
                sensorVarType = sensor.Element("name").Value.ToString(),
                //latestReading = int.Parse(sensor.Element("latestreading").Value),
                //updatedAt = DateTime.Parse(sensor.Element("updated-at").Value),
            };
            foreach (Sensor s in sensorOutputs)
            {
                allSensors.Add(s);
            }
            isUpdated = true;
        }
    }
}
