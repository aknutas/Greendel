class Device < ActiveRecord::Base
  belongs_to :user

  has_many :sensors
  has_many :outputs
  
end
