require 'singleton'
require 'monitor'

# This Singleton class holds all devices and other data.
class DataMaster
  include Singleton
  @deviceMap
  def initialize
    @deviceMap = Hash.new
    extend(MonitorMixin)
  end
  def adddevice(id, deviceName, ip)
    synchronize do
      @deviceMap.store(deviceName, Device.new(id, deviceName, ip))
    end
  end
  def getdevice(deviceName)
    synchronize do
      return @deviceMap[deviceName]
    end
    def storedatabase()
     synchronize do

     end
    end
    def loaddatabase()
      synchronize do

      end
    end
  end
end

# This class represents a single device. It can contain several sensors.
class Device
  @sensorMap
  @id
  @deviceName
  @ip
  #Creates a new device with the name given as an argument.
  def initialize(id, deviceName, ip)
    @sensorMap = Hash.new
    @id = id
    @deviceName = deviceName
    @ip = ip
    @dm = DataMaster.instance
  end
  # Add a new sensor into the sensor hash.
  # Parameter is the name of the new sensor.
  def addsensor(sensorName)
    @dm.synchronize do
      @sensorMap.store(sensorName, Sensor.new(sensorName))
    end
  end
  #Add a new reading to the specified sensor
  #Raises an ArgumentError if trying to use an unexisting sensor
  def addreading(sensorName, value)
    @dm.synchronize do
      raise ArgumentError, 'No such sensor exists!' unless sensorMap.has_key?(sensorName)
      @sensorMap[sensorName].newreading(value)
    end
  end

  attr_accessor :sensorMap, :deviceName
end

# This class represents a single sensor. It has a name, and then an array of timestamped readings.
class Sensor
  @sensorName
  @readingsArray
  # Constructor sets the sensor name variable, and automatically creates a new array of sensor readings.
  def initialize(sensorName)
    @sensorName = sensorName
    @readingsArray = Array.new
    @dm = DataMaster.instance
  end
  #Adds a new sensor reading with the argument given into the sensor queue.
  def newreading(value)
    @dm.synchronize do
      @readingsArray.push(SensorReading.new(value))
    end
  end

  attr_accessor :sensorName, :readingsArray
end

# Reading is a timestamped value from a single sensor.
class SensorReading
  @value
  @time
  # Constructor takes a single value when the reading is created, and automatically timestamps it.
  def initialize(value)
    @value = value
    @time = Time.now
  end

  attr_accessor :value, :time
end