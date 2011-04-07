xml.instruct!
xml.weather do
  xml.id @weather.id
  xml.desc @weather.desc
  xml.source @weather.source
  xml.low @weather.low
  xml.high @weather.high
  xml.histories do
    @weather.histories.each do |history|
      xml.history do
        xml.id history.id
        xml.fday history.fday
        xml.temp history.temp
        xml.low history.low
        xml.high history.high
        xml.desc history.desc
      end
    end
  end
end