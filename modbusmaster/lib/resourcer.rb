#Resourcer manages remote device access

#Extending a Rails base class (for reading a login conf file)
module Getlogins
  def self.included(base)
    base.extend ClassMethods
  end

  module ClassMethods
    def getlogins
      @logins = YAML.load_file './conf/logins.yml'
      @user = @logins['user']
      @passwd = @logins['passwd']
      @caddress = @logins['caddress']
      @port = @logins['port']
    end
  end
end

class Resourcer

  def printdevices
    devices = RemoteDevice.find(:all)
    puts devices.map(&:name)
  end

  def submitreading(sensorid, reading, time)
    RemoteSensorReading.create(
      :sensor_id => sensorid,
      :value => reading
    )
    remotesensor = RemoteSensor.find(sensorid)
    remotesensor.latestreading = reading
    remotesensor.save
  end

  def checkoutput(oneoutput, connection, rvalue)
    remoteoutput = RemoteOutput.find(oneoutput.id)
    if(remoteoutput.haschanged) then
      logtext("Output " + remoteoutput.name + ":" + remoteoutput.id.to_s + " changed")
      if(remoteoutput.state) then
        connection.write_single_coil(oneoutput.register, 1)
        logtext("Updating coil to 1")
      else
        connection.write_single_coil(oneoutput.register, 0)
        logtext("Updating coil to 0")
      end
      remoteoutput.haschanged = false
      remoteoutput.save
    else
      changed = false
      if(rvalue == 1) then
        if(remoteoutput != true) then
          remoteoutput.state = true
          changed = true
        end
      elsif(rvalue == 0)
        if(remoteoutput != false) then
          remoteoutput.state = false
          changed = true
        end
      end
      if(changed) then
        remoteoutput.save
      end
    end
  end

  def emptycache()
    readings = Reading.all
    readings.each do |onereading|
      submitreading(onereading.sensor_id, onereading.value, onereading.time)
      onereading.destroy
    end
  end

end

#Remote machine classes

class RemoteDevice < ActiveResource::Base
  include Getlogins
  getlogins
  self.site = "http://#{@caddress}:#{@port}"
  self.element_name = "device"
  self.timeout = 10
end

class RemoteSensor < ActiveResource::Base
  include Getlogins
  getlogins
  self.site = "http://#{@user}:#{@passwd}@#{@caddress}:#{@port}"
  self.element_name = "sensor"
  self.timeout = 10
end

class RemoteOutput < ActiveResource::Base
  include Getlogins
  getlogins
  self.site = "http://#{@user}:#{@passwd}@#{@caddress}:#{@port}"
  self.element_name = "output"
  self.timeout = 10
end

class RemoteSensorReading < ActiveResource::Base
  include Getlogins
  getlogins
  self.site = "http://#{@user}:#{@passwd}@#{@caddress}:#{@port}"
  self.element_name = "reading"
  self.timeout = 10
end
