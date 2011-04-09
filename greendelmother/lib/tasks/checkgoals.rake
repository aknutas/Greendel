task :checkgoals => :environment do
  puts "Starting to refresh savings goals"

  @savings = Savingsgoal.find(:all, :conditions => {:completed => false}, :include => [:device, :sensor])

  @savings.each do |saving|
    startsave = saving.timestart
    endsave = saving.timeend
    duration = endsave - startsave

    startcomp = startsave - duration
    endcomp = endsave - duration

    startreading = saving.sensor.readings.find(:limit => 1, :order => "time ASC", :conditions => {:time => startsave..endsave})
    endreading = saving.sensor.readings.find(:limit => 1, :order => "time DESC", :conditions => {:time => startsave..endsave})

    startcompreading = saving.sensor.readings.find(:limit => 1, :order => "time ASC", :conditions => {:time => startcomp..endcomp})
    endcompreading = saving.sensor.readings.find(:limit => 1, :order => "time DESC", :conditions => {:time => startcomp..endcomp})

    if ((endcompreading.value - startcompreading.value) - (endreading.value - startreading.value) >= saving.amount)
      saving.success = true
      saving.completed = true
    else
      saving.success = false
      saving.completed = true
    end

    saving.save

  end

end