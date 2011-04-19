xml.instruct!
xml.device do
  xml.id @device.id
  xml.name @device.name
  xml.weather do
    xml.temp @device.location.weather.temp
    xml.high @device.location.weather.high
    xml.low @device.location.weather.low
    xml.unit "C"
    xml.desc @device.location.weather.desc
    xml.code @device.location.weather.code
    xml.updated_at @device.location.weather.updated_at
  end
  xml.sensors do
    @device.sensors.each do |sensor|
      xml.sensor do
        xml.id sensor.id
        xml.name sensor.name
        xml.longname sensor.longname
        xml.vartype sensor.vartype
        xml.counter sensor.counter
        xml.latestreading sensor.latestreading
        xml.unit sensor.unit
        xml.updated_at sensor.updated_at
      end
    end
  end
  xml.outputs do
    @device.outputs.each do |output|
      xml.output do
        xml.id output.id
        xml.name output.name
        xml.longname output.longname
        xml.state output.state
        xml.haschanged output.haschanged
        xml.updated_at output.updated_at
        xml.priority output.priority
        xml.estcost output.estcost
        xml.cost output.cost
        xml.machineswitchable output.autoswitchable
      end
    end
  end
end