xml.instruct!
xml.device do
  xml.id @device.id
  xml.name @device.name
  xml.weather do
    xml.temp @device.location.weather.temp
    xml.unit "C"
    xml.desc @device.location.weather.desc
    xml.updated_at @device.location.weather.updated_at
  end
  xml.sensors do
    @device.sensors.each do |sensor|
      xml.sensor do
        xml.id sensor.id
        xml.name sensor.name
        xml.longname sensor.longname
        xml.vartype sensor.vartype
        xml.latestreading sensor.latestreading
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
      end
    end
  end
end