class User < ActiveRecord::Base
  has_one :device
  has_one :socialmedia

end
