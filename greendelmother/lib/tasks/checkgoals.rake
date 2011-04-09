task :checkgoals => :environment do
  puts "Starting to refresh savings goals"

  @savings = Savingsgoal.find(:all, :where, :include => [:device, :sensor])

  @savings.each do |saving|
    startsave = saving.timestart
    endsave = saving.timeend


  end

end