# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#   
#   cities = City.create([{ :name => 'Chicago' }, { :name => 'Copenhagen' }])
#   Major.create(:name => 'Daley', :city => cities.first)
cloverpower = Device.create(:name => 'Cloverpower')

Sensor.create(:name => 'poweruse', :vartype => 'integer', :device_id => cloverpower.id)

Sensor.create(:name => 'insidetemp', :vartype => 'integer', :device_id => cloverpower.id)

Sensor.create(:name => 'outsidetemp', :vartype => 'integer', :device_id => cloverpower.id)

Output.create(:name => 'heating', :device_id => cloverpower.id)

Output.create(:name => 'lights', :device_id => cloverpower.id)

Output.create(:name => 'solar', :device_id => cloverpower.id)

Output.create(:name => 'wind', :device_id => cloverpower.id)
