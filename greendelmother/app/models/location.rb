class Location < ActiveRecord::Base
    belongs_to :device

    has_one :weather
end
