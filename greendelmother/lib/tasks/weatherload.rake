task :weatherload => :environment do

  #Enabling logger
  #log_to STDOUT

  puts "Starting to refresh weathers"
  client = YahooWeather::Client.new
  fc = nil

  puts "Loading weather objects"
  weathers = Weather.find(:all, :include => [:histories, :location, {:location => :device}])

  puts "Refreshing objects"
  weathers.each do |weather|
    if weather.woeid
      puts Time.now.asctime + ":" + " Refreshing the weather of " + weather.location.device.name + " at " + weather.location.town
      response = client.lookup_by_woeid(weather.woeid, YahooWeather::Units::CELSIUS)
      weather.yweather = Marshal.dump(response)
      response.forecasts.each do |ofc|
        puts "Examining forecast at " + ofc.date.to_s
        if (Date.parse(ofc.date.to_s) == Date.today)
          puts "Match found!"
          fc = ofc
        end
      end
      weather.high = fc.high
      weather.low = fc.low
      weather.temp = response.condition.temp
      weather.desc = response.condition.text
      unless (weather.histories.last.try(:fday) == Date.today)
        his = History.new
        his.fday = fc.date
        his.desc = fc.text
        his.high = fc.high
        his.low = fc.low
        his.temp = (his.high + his.low) / 2
        his.yweather = weather.yweather
        weather.histories << his
        his.save
        puts "Recording new weather history from day " + his.fday.to_s
        puts "Day high: " + his.high.to_s
        puts "Day low: " + his.low.to_s
        puts "Day avg: " + his.temp.to_s
        puts "Day fc: " + his.desc.to_s
      end
      puts "Finished: Temperature is " + response.condition.temp.to_s + " and weather looks like " + response.condition.text.to_s
    else
      weather.temp = nil
      weather.desc = nil
      weather.yweather = nil
    end
    weather.save
  end
end