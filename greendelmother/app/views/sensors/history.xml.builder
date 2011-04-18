xml.instruct!
xml.sensor do
  xml.id @sensor.id
  xml.name @sensor.name
  xml.longname @sensor.longname
  xml.vartype @sensor.vartype
  xml.latestreading @sensor.latestreading
  xml.updated_at @sensor.updated_at
  xml.unit @sensor.unit
  xml.readings do
    @readings.each do |reading|
      xml.reading do
        xml.time reading.time
        xml.value reading.value
      end
    end
  end
end