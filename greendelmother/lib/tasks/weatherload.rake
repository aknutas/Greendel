task :weatherload => :environment do
  puts "Starting to refresh weathers"
  client = YahooWeather::Client.new

  puts "Loading weather objects"
  weathers = Weather.find(:all, 'c')

  puts "Refreshing objects"
  weathers.each do |weather|
    if weather.woeid
      puts Time.now.asctime + ":" + " Refreshing the weather of " + weather.location.device.name + " at " + weather.location.town
      response = client.lookup_by_woeid(weather.woeid, YahooWeather::Units::CELSIUS)
      weather.yweather = Marshal.dump(response)
      weather.temp = response.condition.temp
      weather.desc = response.condition.text
      puts "Finished: Temperature is " + response.condition.temp.to_s + " and weather looks like " + response.condition.text.to_s
    else
      weather.temp = nil
      weather.desc = nil
      weather.yweather = nil
    end
    weather.save
  end
end