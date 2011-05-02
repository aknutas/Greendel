class Savingsgoal < ActiveRecord::Base
  belongs_to :device
  belongs_to :sensor

  def duration
    return (self.timeend - self.timestart)
  end

end
