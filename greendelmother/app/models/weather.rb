class Weather < ActiveRecord::Base
  belongs_to :location

  serialize :yweather

end
