xml.instruct!
xml.user do
  xml.name @user.name
  xml.realname @user.realname
  xml.device do
    xml.id @user.device.id
    xml.name @user.device.name
    xml.location do
      xml.address @user.device.location.address
      xml.town @user.device.location.town
      xml.weather do
        xml.temp @user.device.location.weather.temp
        xml.unit "C"
        xml.desc @user.device.location.weather.desc
      end
    end
    xml.sensors do
      @user.device.sensors.each do |sensor|
        xml.sensor do
          xml.name sensor.name
          xml.vartype sensor.vartype
          xml.latestreading sensor.latestreading
        end
      end
    end
        xml.outputs do
      @user.device.outputs.each do |output|
        xml.output do
          xml.name output.name
          xml.state output.state
          xml.haschanged output.haschanged
        end
      end
    end
  end
end