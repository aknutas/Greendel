class Weather < ActiveRecord::Base
  belongs_to :location

  has_many :histories

end
