# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#   
#   cities = City.create([{ :name => 'Chicago' }, { :name => 'Copenhagen' }])
#   Major.create(:name => 'Daley', :city => cities.first)

#Creating first test user
user = User.create(:name => 'testipaavo', :realname => 'Paavo Paavoke', :email => 'paavo@example.com', :password => 'testi')

cloverpower = Device.create(:name => 'Cloverpower')
user.device = cloverpower
user.save

sm = Socialmedia.create(:twitteron => false, :facebookon => false)
user.socialmedia = sm
user.save

testlocation = Location.create(:address => 'Nollakatu 0', :name => 'TestLocation', :town => 'Lappeenranta')
cloverpower.location = testlocation
cloverpower.save

tweather = Weather.create(:source => 'yahoo')
testlocation.weather = tweather
testlocation.save

s = Sensor.create(:name => 'poweruse', :longname => 'Power Consumption', :vartype => 'integer')
cloverpower.sensors << s
s = Sensor.create(:name => 'powerconsumed', :longname => 'Consumed Power', :vartype => 'integer')
cloverpower.sensors << s
s = Sensor.create(:name => 'insidetemp', :longname => 'Inside Temperature', :vartype => 'integer')
cloverpower.sensors << s
s = Sensor.create(:name => 'outsidetemp', :longname => 'Outside Temperature', :vartype => 'integer')
cloverpower.sensors << s
cloverpower.save

o = Output.create(:name => 'heating', :longname => 'Heating')
cloverpower.outputs << o
o = Output.create(:name => 'lights', :longname => 'Lights')
cloverpower.outputs << o
o = Output.create(:name => 'solar', :longname => 'Solar')
cloverpower.outputs << o
o = Output.create(:name => 'wind', :longname => 'Wind')
cloverpower.outputs << o
cloverpower.save

#Populating woeid db
Woeid.create(:location => 'Lappeenranta', :woeid => '568782')
Woeid.create(:location => 'Helsinki', :woeid => '565346')

#Populating woeids
weathers = Weather.find(:all)
weathers.each do |weather|
  woeidobj = Woeid.find_by_location(weather.location.town)
  weather.woeid = woeidobj.woeid
  weather.save
end