class Sensor < ActiveRecord::Base
  belongs_to :device
  has_many :readings
  
end
