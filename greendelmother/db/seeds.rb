# encoding: UTF-8

# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#   
#   cities = City.create([{ :name => 'Chicago' }, { :name => 'Copenhagen' }])
#   Major.create(:name => 'Daley', :city => cities.first)

#Creating first test user
user = User.create(:name => 'testipaavo', :realname => 'Veli-Ensiö Järvinen', :email => 'velipoika@example.com', :password => 'testi')

cloverpower = Device.create(:name => 'Cloverpower')
user.device = cloverpower
user.save

sm = Socialmedia.create(:twitteron => false, :facebookon => true, :status => true)
sm.fbauth = "***REMOVED***|411b0321d0327ebd24583973.1-100002348930065|jsDNib6501byb1NYcSeQ7v5PP4M"
sm.save
user.socialmedia = sm
user.save

testlocation = Location.create(:address => 'Nollakatu 2', :name => 'Oma Koti', :town => 'Lappeenranta')
cloverpower.location = testlocation
cloverpower.save

tweather = Weather.create(:source => 'yahoo')
testlocation.weather = tweather
testlocation.save

s = Sensor.create(:name => 'poweruse', :longname => 'Power Consumption', :vartype => 'integer', :unit => 'W', :counter => true)
cloverpower.sensors << s
s = Sensor.create(:name => 'powerconsumed', :longname => 'Consumed Power', :vartype => 'integer', :unit => 'kWh')
cloverpower.sensors << s
s = Sensor.create(:name => 'powerprice', :longname => 'Price of Electricity', :vartype => 'float', :unit => 'e/kWh')
cloverpower.sensors << s
s = Sensor.create(:name => 'insidetemp', :longname => 'Inside Temperature', :vartype => 'float', :unit => 'C')
cloverpower.sensors << s
s = Sensor.create(:name => 'outsidetemp', :longname => 'Outside Temperature', :vartype => 'float', :unit => 'C')
cloverpower.sensors << s
cloverpower.save

o = Output.create(:name => 'wall_socket', :longname => 'Kitchen Wall Socket', :cost => true)
cloverpower.outputs << o
o = Output.create(:name => 'solar', :longname => 'Solar', :cost => false)
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