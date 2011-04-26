def powerfluctuate(reading)
  time = reading.time
  if (time.month == 1)
    reading.value = reading.value * 2
  elsif (time.month == 2)
    reading.value = reading.value * 1.5
  end
  if (time.hour >= 8 && time.hour < 16)
    reading.value = reading.value * 2
  else
    reading.value = reading.value / 2
  end
  if(time.wday > 5)
    reading.value = reading.value * 1.25
  end
end

task :genhistory => :environment do

  #Enabling logger
  #log_to STDOUT

  puts "Starting to generate artificial history."
  puts "Loading sensors..."
  sensors = Sensor.find(:all)
  random = Random.new

  timestart = Time.local(2011, 01, 01)
  timeend = Time.zone.now

  sensors.each do |sensor|
    timenow = timestart
    puts Time.zone.now.to_s + ": Processing " + sensor.name
    while (timenow <= timeend)
      if (sensor.name == "powerconsumed" && sensor.readings.last)
        reading = Reading.create(:value => (sensor.readings.last.value + (random.rand(500) + random.rand(500) + random.rand(500) + random.rand(500))/1000), :time => timenow)
      elsif (sensor.name == "powerconsumed" && sensor.readings.last == nil)
        reading = Reading.create(:value => (random.rand(500) + random.rand(500) + random.rand(500) + random.rand(500))/1000, :time => timenow)
      elsif (sensor.name == "powerprice")
        reading = Reading.create(:value => ((random.rand(21) + random.rand(21) + random.rand(21) + random.rand(21))*0.00018), :time => timenow)
      elsif (sensor.name == "poweruse")
        reading = Reading.new(:value => (random.rand(500) + random.rand(500) + random.rand(500) + random.rand(500)), :time => timenow)
        powerfluctuate(reading)
        reading.save
      else
        reading = Reading.create(:value => (random.rand(11) + random.rand(11) + random.rand(11)), :time => timenow)
      end
      sensor.readings << reading
      timenow = timenow + 1.hours
    end
    sensor.latestreading = sensor.readings.last.value
    sensor.save
  end

end