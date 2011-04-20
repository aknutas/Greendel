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
using System.Collections.ObjectModel;

namespace CloverMobile
{
    public class DataMaster
    {   
        private XDocument dataDoc;
        public User currentUser { get; set; }
        public Device currentDevice { get; set; }
        public Location currentLocation { get; set; }
        public Weather currentWeather { get; set; }
        public List<Sensor> currentSensors { get; set; }
        public List<Output> currentOutputs { get; set; }
        public List<HistoryData> currentReadings { get; set; }
        public DataMaster()
        {
            currentUser = new User();
            currentDevice = new Device();
            currentLocation = new Location();
            currentWeather = new Weather();
            currentOutputs = new List<Output>();
            currentSensors = new List<Sensor>();
            currentReadings = new List<HistoryData>();
            dataDoc = new XDocument();
        }
        public DataMaster getReference()
        {
            return this;
        }

        public void parseUserInformation(XDocument xmlDoc)
        {
            // ** USER
            System.Diagnostics.Debug.WriteLine("Model: Parsing User Info XML...");
            var user = from userValue in xmlDoc.Descendants("user")
            select new User
            {
                id = int.Parse(userValue.Element("id").Value),
                name = userValue.Element("name").Value.ToString(),
                realName = userValue.Element("realname").Value.ToString(),
            };
            foreach (User u in user)
            {
                currentUser.name = u.name;
                currentUser.realName = u.realName;     
            }
            System.Diagnostics.Debug.WriteLine("USER:" + " " + currentUser.id.ToString() + " " + currentUser.name + " " + currentUser.realName);
            
            // ** DEVICE
            var device = from deviceValue in xmlDoc.Descendants("device")
            select new Device
            {
                deviceId = int.Parse(deviceValue.Element("id").Value),
                deviceName = deviceValue.Element("name").Value.ToString(),
            };
            foreach (Device d in device)
            {
                currentDevice.deviceId = d.deviceId;
                currentDevice.deviceName = d.deviceName;
                
            }
            System.Diagnostics.Debug.WriteLine("DEVICE:" + " " + currentDevice.deviceId.ToString() + " " + currentDevice.deviceName);

            // ** LOCATION
            var location = from locationValue in xmlDoc.Descendants("location")
            select new Location
            {
                address = locationValue.Element("address").Value.ToString(),
                town = locationValue.Element("town").Value.ToString(),

            };
            foreach (Location l in location)
            {
                currentLocation.address = l.address;
                currentLocation.town = l.town;                
            }
            System.Diagnostics.Debug.WriteLine("LOCATION:" + " " + currentLocation.address + " " + currentLocation.town);
            
            // ** WEATHER
            var weather = from weatherValue in xmlDoc.Descendants("weather")
            select new Weather
            {
                temp = float.Parse(weatherValue.Element("temp").Value),
                high = float.Parse(weatherValue.Element("high").Value),
                low = float.Parse(weatherValue.Element("low").Value),
                unit = weatherValue.Element("unit").Value.ToString(),
                description = weatherValue.Element("desc").Value.ToString(),
                code = int.Parse(weatherValue.Element("code").Value),
            };
            foreach (Weather w in weather)
            {
                currentWeather.temp = w.temp;
                currentWeather.unit = w.unit;
                currentWeather.description = w.description;
                currentWeather.code = w.code;
            }
            System.Diagnostics.Debug.WriteLine("WEATHER:" + " " + currentWeather.temp.ToString() + " " + currentWeather.unit + " " + currentWeather.description + " " + currentWeather.code.ToString());
        }
        
        // ** get all sensors only
        public void parseSensors(XDocument xmlDoc)
        {
            System.Diagnostics.Debug.WriteLine("Model: parsing sensors.");
            var allSensors = new List<Sensor>();
            try
            {
                allSensors = (from s in xmlDoc.Descendants("sensor")
                    select new Sensor()
                    {
                        sensorId = Convert.ToInt16(s.Element("id").Value),
                        sensorName = s.Element("name").Value.ToString(),
                        longName = s.Element("longname").Value.ToString(),
                        sensorVarType = s.Element("vartype").Value.ToString(),
                        latestReading = s.Element("latestreading") != null ? s.Element("latestreading").Value : string.Empty
                    }).ToList<Sensor>();
            }              
            catch (FormatException f)
            {
                System.Diagnostics.Debug.WriteLine(f.Message.ToString());
            }
            currentSensors = allSensors;  
            foreach (Sensor s in currentSensors)
            {
                System.Diagnostics.Debug.WriteLine("SENSORS: " + s.sensorId.ToString() + " " + s.sensorName + " " + s.sensorVarType.ToString());
            }
            //return allSensors;
        }
        // ** parse all outputs only
        public void parseOutpus(XDocument xmlDoc)
        {
            System.Diagnostics.Debug.WriteLine("Model: parsing outputs");
            //System.Diagnostics.Debug.WriteLine(xmlDoc.ToString());
            var allOutputs = new List<Output>();

            allOutputs = (from o in xmlDoc.Descendants("output")
            select new Output()
            {
                id = Convert.ToInt16(o.Element("id").Value),
                name = o.Element("name").Value.ToString(),
                // add long name
                state = o.Element("state") != null ? o.Element("state").Value : string.Empty
                //state = o.Element("state").Value.ToString(),
                //hasChanged = o.Element("haschanged") != null ? o.Element("latestreading").Value : string.Empty
            }).ToList<Output>();

            currentOutputs = allOutputs;
            foreach (Output o in currentOutputs)
            {
                System.Diagnostics.Debug.WriteLine("OUTPUTS: " + o.id.ToString() + " " + o.name + " " + o.state + " " + o.hasChanged);
            }
            //return allOutputs;
        }
        // ** gets the history values for a specific sensor
        public void parseSensorHistory(int sensorId, XDocument xmlDoc)
        { 
            System.Diagnostics.Debug.WriteLine("Model: parsing history values for a single sensor");
            //System.Diagnostics.Debug.WriteLine(xmlDoc.ToString());
            var allReadings = new List<HistoryData>();

            allReadings = (from r in xmlDoc.Descendants("reading")
            select new HistoryData()
            {
                time =  r.Element("time").Value,
                value = Convert.ToDouble(r.Element("value").Value),

            }).ToList<HistoryData>();


            //currentReadings = allReadings;
            lock (currentSensors)
            {
                // ** find the right sensor from the list of sensors!
                foreach (Sensor s in currentSensors)
                {
                    if (s.sensorId == sensorId) // found it
                    {
                        foreach (HistoryData hd in allReadings)
                        {
                            s.addNewHistoryValue(Convert.ToDateTime(hd.time), hd.value);
                            //System.Diagnostics.Debug.WriteLine("Current Readings: " + hd.time.ToString() + " " + hd.value.ToString());
                        }
                    }
                }
            }
        }
        // gets the latest value for a sensor
        public void parseSingleSensorForNewHistoryDatapoint(int sensorId, XDocument xmlDoc)
        {
            System.Diagnostics.Debug.WriteLine("Model: getting current value for a sensor.");
            var sensor = from sensorValue in xmlDoc.Descendants("sensor")
            select new Sensor
            {     
                // ** get latestreading and updated-at values from xml
                latestReading = sensorValue.Element("latestreading").Value,
                updatedAt = Convert.ToDateTime(sensorValue.Element("updated-at").Value),
            };
            lock (currentSensors) // ** lock the list of sensors before updating!
            {
                foreach (Sensor s in currentSensors) // ** this is our sensors list
                {
                    if (s.sensorId == sensorId) // ** find a sensor with given id
                    {
                        foreach (Sensor sens in sensor) // ** this is the temp "list" of 1 sensors
                        {
                            s.latestReading = sens.latestReading; // ** update the current values
                            s.updatedAt = sens.updatedAt;
                            // ** remove the first value and add a new value
                            s.DataUnit.RemoveAt(0);
                            s.addNewHistoryValue(sens.updatedAt, double.Parse(sens.latestReading));
                        }
                    }
                }
            }
        }
        // this function gets the sensor history from specified timescale, historydatalist is cleared and new values are added
        public void parseSensorTimeScaleHistory(int sensorId, XDocument xmlDoc)
        {
            System.Diagnostics.Debug.WriteLine("Model: parsing timescale history values for a single sensor.");
            //System.Diagnostics.Debug.WriteLine(xmlDoc);
            //System.Diagnostics.Debug.WriteLine(xmlDoc.ToString());
            var allReadings = new List<HistoryData>();

            allReadings = (from r in xmlDoc.Descendants("reading")
                           select new HistoryData()
                           {
                               time = r.Element("time").Value,
                               value = Convert.ToDouble(r.Element("value").Value),

                           }).ToList<HistoryData>();

            //currentReadings = allReadings;
            lock (currentSensors)
            {
                // ** find the right sensor from the list of sensors!
                foreach (Sensor s in currentSensors)
                {
                    if (s.sensorId == sensorId) // ** found it
                    {
                        s.DataUnit.Clear(); // ** clear the existing values! 
                        
                        foreach (HistoryData hd in allReadings)
                        {
                            s.addNewHistoryValue(Convert.ToDateTime(hd.time), hd.value);
                            System.Diagnostics.Debug.WriteLine("Current Readings: " + hd.time.ToString() + " " + hd.value.ToString());
                        }
                    }
                }
            }
        }
        
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
    }
}
