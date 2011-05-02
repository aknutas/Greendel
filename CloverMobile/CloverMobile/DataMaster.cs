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
        public WeatherForecast currentForecast {get; set;}
        public PowerPrice currentPowerPrise {get; set;}
        public List<Sensor> currentSensors { get; set; }
        public List<Output> currentOutputs { get; set; }
        public List<HistoryData> currentReadings { get; set; }
        public DataMaster()
        {
            currentUser = new User();
            currentDevice = new Device();
            currentLocation = new Location();
            currentWeather = new Weather();
            currentForecast = new WeatherForecast();
            currentPowerPrise = new PowerPrice();
            currentOutputs = new List<Output>();
            currentSensors = new List<Sensor>();
            currentReadings = new List<HistoryData>();
            dataDoc = new XDocument();
        }
        public DataMaster getReference()
        {
            return this;
        }
        public int GetSensorIdByShorName(string shortname)
        {
            foreach (Sensor s in currentSensors)
            {
                if (s.sensorName == shortname)
                {
                    return s.sensorId;
                }
            }
            return 0;
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
                currentUser.id = u.id;
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
            System.Diagnostics.Debug.WriteLine("WEATHER:" + " " + currentWeather.temp.ToString() + " " + currentWeather.high.ToString() + " " + currentWeather.low.ToString() + " " + currentWeather.unit + " " + currentWeather.description + " " + currentWeather.code.ToString());
            
            var forecast = from weatherForecastValue in xmlDoc.Descendants("forecast")
            select new WeatherForecast
            {
                temp = float.Parse(weatherForecastValue.Element("temp").Value),
                high = float.Parse(weatherForecastValue.Element("high").Value),
                low = float.Parse(weatherForecastValue.Element("low").Value),
                //unit = weatherForecastValue.Element("unit").Value.ToString(),
                description = weatherForecastValue.Element("desc").Value.ToString(),
                code = int.Parse(weatherForecastValue.Element("code").Value),
            };
            foreach (WeatherForecast wf in forecast)
            {
                currentForecast.temp = wf.temp;
                //currentForecast.unit = wf.unit;
                currentForecast.description = wf.description;
                currentForecast.code = wf.code;
            }

            var prises = from powerPriseValue in xmlDoc.Descendants("powerprices")
            select new PowerPrice
            {
                powerPrice = float.Parse(powerPriseValue.Element("powerprice").Value),
                lwuse = float.Parse(powerPriseValue.Element("lwuse").Value),
                lwprice = float.Parse(powerPriseValue.Element("lmprice").Value),
                lmuse = float.Parse(powerPriseValue.Element("lmuse").Value),
                lmprice = float.Parse(powerPriseValue.Element("lmprice").Value),
            };

            foreach (PowerPrice pp in prises)
            {
                currentPowerPrise.powerPrice = pp.powerPrice;
                currentPowerPrise.lwuse = pp.lwuse;
                currentPowerPrise.lwprice = pp.lwprice;
                currentPowerPrise.lmuse = pp.lmuse;
                currentPowerPrise.lmprice = pp.lmprice;
            }
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
                        latestReading = s.Element("latestreading") != null ? s.Element("latestreading").Value : string.Empty,
                        unit = s.Element("unit").Value.ToString(),
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
                            if (s.DataUnit.Count > 0)
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
        // gets the latest value for a sensor
        public void parseSingleOutput(int outputId, XDocument xmlDoc)
        {
            System.Diagnostics.Debug.WriteLine("Model: getting current value for an output.");
            var output = from outputValue in xmlDoc.Descendants("output")
                         select new Output
                         {
                             state = outputValue.Element("state").Value,
                             // ** get latestreading and updated-at values from xml
                             //latestReading = outputValue.Element("latestreading").Value,
                             //updatedAt = Convert.ToDateTime(sensorValue.Element("updated-at").Value),
                         };
            lock (currentOutputs) // ** lock the list of outputss before updating!
            {
                foreach (Output o in currentOutputs) // ** this is our outputss list
                {
                    if (o.id == outputId) // ** find a output with given id
                    {
                        foreach (Output outp in output) // ** this is the temp "list" of 1 output
                        {
                            o.state = outp.state;
                            System.Diagnostics.Debug.WriteLine("Model: Current output state: " + o.state.ToString());
                        }
                    }
                }
            }
        }
        public void parseSavingGoals(int userId, XDocument xmlDoc)
        { 
        
        
        
        
        
        
        }
    }
}
