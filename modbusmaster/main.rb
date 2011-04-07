# encoding: UTF-8

#Libraries
require 'rubygems'
require 'rmodbus'
require 'thread'
require 'dm-core'
require 'dm-migrations'
require 'yaml'
require 'active_resource'
require 'optparse'
#Other source files
require './optparse.rb'
require './logger.rb'
require './dataclasses.rb'
require './simulator.rb'
require './resourcer.rb'
require './mbparser.rb'

##
## Optional section starts
## (use YAML to load up configuration)
##

settings = YAML.load_file './conf/settings.yml'
devset = YAML.load_file './conf/devices.yml'

logtext "Initializing settings"

interval = settings['interval']
startupdelay = settings['startupdelay']

logtext("Read frequency: " + interval.to_s)
logtext "Initializing devices"

devicearray = Array.new

devset.each { |ydevice|
  device = Device.first_or_create(
    :id => ydevice['id'],
    :name => ydevice['name'],
    :ip => ydevice['ip'],
    :port => ydevice['port'],
    :slaveaddr => ydevice['slaveaddr']
  )
  sensors = ydevice['sensors']
  sensors.each { |ysensor|
    device.sensors.first_or_create(
      :id => ysensor['id'],
      :name => ysensor['name'],
      :register => ysensor['register'],
      :ntype => ysensor['ntype'],
      :scalefactor => ysensor['scalefactor']
    )
  }
  outputs = ydevice['outputs']
  outputs.each { |youtput|
    device.outputs.first_or_create(
      :id => youtput['id'],
      :name => youtput['name'],
      :register => youtput['register']
    )
  }
  devicearray.push(device)
}

##
## End optional section
##

#Reading sensor names for log output and debugging
devicearray.each do |onedevice|
  logtext("The device " + onedevice.name + ' ' + onedevice.ip + " has the following sensors:")
  onedevice.sensors.each do |sensor|
    logtext("Name: " + sensor.name)
    logtext("Register: " + sensor.register.to_s)
    logtext ""
  end
  logtext ""
    logtext("The device " + onedevice.name + ' ' + onedevice.ip + " has the following outputs:")
  onedevice.outputs.each do |output|
    logtext("Name: " + output.name)
    logtext("Register: " + output.register.to_s)
    logtext ""
  end
  logtext ""
end
logtext "Startup finished"

#Catching interrupts
interrupted = false
trap("INT") { interrupted = true }

logtext "Simulating instead of real output" if $options[:simulate]
logtext "Logging to file #{options[:logfile]}" if $options[:logfile]

logtext("Sleeping for start delay: " + startupdelay.to_s)
sleep startupdelay

#Creating helper classes
resourcer = Resourcer.new

if $options[:simulate]
  # Main program loop (SIMULATED)
  while(interrupted == false)
    logtext("New iteration loop at " + Time.now.to_s)
    #Starting the timer
    timer = Time.now
    devicearray.each do |onedevice|
      #Opening device
      begin
        logtext("Reading from " + onedevice.name)
        simulator = Simulator.new
        readingarray = Hash.new
        onedevice.sensors.each do |onesensor|
          begin
            #Reading values from device
            reading = Reading.new
            modbusreading = simulator.gimmerandom
            reading.value = parse(onesensor, modbusreading)
            reading.time = Time.now
            logtext("Read " + reading.value.to_s + " from " + onesensor.name)
            readingarray[reading] = onesensor
          rescue
            #Handle sensor read error
            logtext($!.to_s + $@.to_s)
            logtext("Sensor " + onesensor.id.to_s + " read error!")
          ensure
            reading = nil
          end
        end
        #Submitting
        readingarray.each_pair do |onereading, onesensor|
          begin
            #Submitting
            resourcer.submitreading(onesensor.id, onereading.value, onereading.time)
          rescue
            logtext($!.to_s + $@.to_s)
            #Storing if fails
            onesensor.readings << onereading
            onesensor.save
          end
        end
        #Cleanup
        readingarray = nil
      rescue
        logtext($!.to_s + $@.to_s)
        logtext("Device " + onedevice.id.to_s + " read error!")
        #Close connections, skip to next device
      ensure
        #if simulator.connection.open?
        #simulator.close
        simulator = nil
      end
    end
    #Sleeping, hopefully not too long
    delay = Time.now - timer
    sleep (interval-delay) if delay < interval
  end
  logtext("Closing")
  #Cleanup here
  #if simulator.connection.open?
  #simulator.close
  simulator = nil
  exit
else
  # Main program loop (ACTUAL)
  while(interrupted == false)
    logtext("New iteration loop at " + Time.now.to_s)
    #Starting the timer
    timer = Time.now
    devicearray.each do |onedevice|
      #Opening device
      begin
        connection = ModBus::TCPClient.new(onedevice.ip, onedevice.port, onedevice.slaveaddr)
        readingarray = Hash.new
        onedevice.sensors.each do |onesensor|
          begin
            #Reading values from device
            reading = Reading.new
            rvalues = connection.read_holding_registers(onesensor.register, 1)
            modbusreading = rvalues[0]
            reading.value = parse(onesensor, modbusreading)
            reading.time = Time.now
            logtext("Read " + reading.value.to_s + " from " + onesensor.name)
            readingarray[reading] = onesensor
          rescue
            #Handle sensor read error
            logtext($!.to_s + $@.to_s)
            logtext("Sensor " + onesensor.id.to_s + " read error!")
          ensure
            reading = nil
          end
        end
        #Handling outputs
        onedevice.outputs.each do |oneoutput|
          begin
            rvalues = connection.read_coils(oneoutput.register, 1)
            modbusreading = rvalues[0]
            logtext("Read " + modbusreading.to_s + " from " + oneoutput.name)
            resourcer.checkoutput(oneoutput, connection, modbusreading)
          rescue
            #Handle output read error
            logtext($!.to_s + $@.to_s)
            logtext("Output " + oneoutput.id.to_s + " read error!")
          ensure
          end
        end
        #Closing connection
        begin
          connection.close unless connection.closed?
        rescue
        end
        connection = nil
        #Submitting
        readingarray.each_pair do |onereading, onesensor|
          begin
            #Submitting
            resourcer.submitreading(onesensor.id, onereading.value, onereading.time)
          rescue
            logtext($!.to_s + $@.to_s)
            #Storing if fails
            #onesensor.readings << onereading
            #onesensor.save
          end
        end
      rescue
        logtext($!.to_s + $@.to_s)
        logtext("Device " + onedevice.id.to_s + " read error!")
        #Close connections, skip to next device
      ensure
        #Cleanup
        begin
          connection.close unless connection.closed?
        rescue
        end
        connection = nil
        readingarray = nil
      end
    end
    #Sleeping, hopefully not too long
    delay = Time.now - timer
    sleep (interval-delay) if delay < interval
  end
  logtext("Closing")
  simulator = nil
end