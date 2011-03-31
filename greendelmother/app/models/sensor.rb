class Sensor < ActiveRecord::Base
  belongs_to :device

  has_many :savingsgoals
  has_many :readings
end
