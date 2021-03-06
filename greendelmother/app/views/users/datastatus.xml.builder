xml.instruct!
xml.user do
  xml.id @user.id
  xml.name @user.name
  xml.realname @user.realname
  xml.device do
    xml.id @user.device.id
    xml.name @user.device.name
    xml.location do
      xml.address @user.device.location.address
      xml.town @user.device.location.town
      xml.weather do
        xml.id @user.device.location.weather.id
        xml.temp @user.device.location.weather.temp
        xml.high @user.device.location.weather.high
        xml.low @user.device.location.weather.low
        xml.unit "C"
        xml.desc @user.device.location.weather.desc
        xml.code @user.device.location.weather.code
        xml.updated_at @user.device.location.weather.updated_at
      end
      if @wtomorrow
        xml.forecast do
          xml.date @wtomorrow.date
          xml.day @wtomorrow.day
          xml.temp (@wtomorrow.high + @wtomorrow.low) / 2
          xml.high @wtomorrow.high
          xml.low @wtomorrow.low
          xml.desc @wtomorrow.text
          xml.code @wtomorrow.code
        end
      end
    end
    xml.powerprices do
      xml.powerprice @sensorhash['powerprice'].latestreading
      xml.lwuse @wdiff.round(2).to_s
      xml.lwprice @wyprice.round(2).to_s
      xml.lmuse @mdiff.round(2).to_s
      xml.lmprice @myprice.round(2).to_s
    end
    xml.sensors do
      @user.device.sensors.each do |sensor|
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
      @user.device.outputs.each do |output|
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
end