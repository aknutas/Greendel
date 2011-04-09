class Sensor < ActiveRecord::Base
  belongs_to :device

  has_many :savingsgoals
  has_many :readings

  def get_avg_readings(startdate, enddate, avgscale)
    avgreadings = nil

    if (avgscale == "hourly" || avgscale == "daily")
      avgreadings = Array.new
      if (avgscale == "hourly")
        interval = 1.hours
      elsif (avgscale == "daily")
        interval = 1.days
      end
      avgstart = startdate
      avgend = startdate + interval

      while (avgend <= enddate) do
        avgpart = self.readings.find(:all, :order => "time ASC", :conditions => {:time => avgstart..avgend})
        count = avgpart.size
        sum = 0

        if (count > 0)
          avgpart.each do |reading|
            sum = reading.value + sum
          end
          avg = sum / count
          avgreading = 
          avgreadings << Reading.new(:value => avg, :time => avgpart.first.time)
        end

        avgstart = avgstart + interval
        avgend = avgend + interval
      end
      return avgreadings
    else
      return nil
    end
  end

  def get_readings(startdate, enddate)
    readings = self.readings.find(:all, :order => "time ASC", :conditions => {:time => startdate..enddate})
    return readings
  end

end
