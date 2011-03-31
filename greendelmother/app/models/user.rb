class User < ActiveRecord::Base
  has_many :devices
  has_one :socialmedia

end
