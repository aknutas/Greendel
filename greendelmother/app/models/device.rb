class Device < ActiveRecord::Base
  has_one :user

  has_many :sensors
  has_many :outputs
  has_many :savingsgoals
  has_one :location
  
end
