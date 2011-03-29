class Device < ActiveRecord::Base
  has_many :sensors
  has_many :outputs
  
end
