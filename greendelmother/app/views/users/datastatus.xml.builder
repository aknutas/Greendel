xml.instruct!
xml.user do
  xml.name @user.name
  xml.realname @user.realname
  xml.devices do
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
  end
end