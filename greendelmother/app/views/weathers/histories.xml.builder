xml.instruct!
xml.weather do
  xml.id @weather.id
  xml.updated_at @weather.updated_at
  xml.desc @weather.desc
  xml.source @weather.source
  xml.temp @weather.temp
  xml.low @weather.low
  xml.high @weather.high
  xml.forecast do
    xml.date @wtomorrow.date
    xml.day @wtomorrow.day
    xml.high @wtomorrow.high
    xml.low @wtomorrow.low
    xml.desc @wtomorrow.text
    xml.code @wtomorrow.code
  end
  xml.histories do
    @weather.histories.each do |history|
      xml.history do
        xml.id history.id
        xml.fday history.fday
        xml.temp history.temp
        xml.low history.low
        xml.high history.high
        xml.desc history.desc
        xml.code history.code
      end
    end
  end
end