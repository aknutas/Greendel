xml.instruct!
xml.savingsgoals do
  @savingsgoals.each do |goal|
    xml.id goal.id
    xml.sensor do
      xml.id goal.sensor_id
      xml.name goal.sensor.name
      xml.longname goal.sensor.longname
    end
    xml.timestart goal.timestart
    xml.timeend goal.timeend
    xml.completed goal.completed
    xml.successful goal.successful
  end
end