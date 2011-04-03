# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#   
#   cities = City.create([{ :name => 'Chicago' }, { :name => 'Copenhagen' }])
#   Major.create(:name => 'Daley', :city => cities.first)
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

tweather = Weather.create(:source => 'yahoo', :woeid => '568782')
testlocation.weather = tweather
testlocation.save

s = Sensor.create(:name => 'poweruse', :vartype => 'integer')
cloverpower.sensors << s
s = Sensor.create(:name => 'insidetemp', :vartype => 'integer')
cloverpower.sensors << s
s = Sensor.create(:name => 'outsidetemp', :vartype => 'integer')
cloverpower.sensors << s
cloverpower.save

o = Output.create(:name => 'heating')
cloverpower.outputs << o
o = Output.create(:name => 'lights')
cloverpower.outputs << o
o = Output.create(:name => 'solar')
cloverpower.outputs << o
o = Output.create(:name => 'wind')
cloverpower.outputs << o
cloverpower.save