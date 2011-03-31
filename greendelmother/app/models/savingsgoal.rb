class Savingsgoal < ActiveRecord::Base
  belongs_to :device
  belongs_to :sensor

end
