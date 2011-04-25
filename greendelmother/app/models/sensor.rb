class Sensor < ActiveRecord::Base
  belongs_to :device

  has_many :savingsgoals
  has_many :readings

  def get_avg_readings(startdate, enddate, avgscale)

    avgreadings = Array.new

    if (avgscale == "hourly")
      interval = 1.hours
    elsif (avgscale == "daily")
      interval = 1.days
    elsif (avgscale == "monthly")
      interval = 1.months
    elsif (avgscale == "yearly")
      interval = 1.years
    else
      return nil
    end
    avgstart = startdate
    avgend = startdate + interval

    begin
      avgpart = self.readings.find(:all, :order => "time ASC", :conditions => {:time => avgstart..avgend})
      count = avgpart.size
      sum = 0

      if (count > 0)
        avgpart.each do |reading|
          sum = reading.value + sum
        end
        avg = sum / count
        avgreading = Reading.new(:value => avg, :time => avgpart.first.time)

        tt = avgreading.time
        if (avgscale == "hourly")
          avgreading.time = Time.local(tt.year, tt.month, tt.day, tt.hour)
        elsif (avgscale == "daily")
          avgreading.time = Time.local(tt.year, tt.month, tt.day)
        elsif (avgscale == "monthly")
          avgreading.time = Time.local(tt.year, tt.month)
        elsif (avgscale == "yearly")
          avgreading.time = Time.local(tt.year)
        end

        avgreadings << avgreading
      end

      avgstart = avgstart + interval
      avgend = avgend + interval
    end while (avgend <= enddate)

    return avgreadings

  end

  def get_diff(startdate, enddate, scale)

    avgreadings = Array.new

    if (scale == "hourly")
      interval = 1.hours
    elsif (scale == "daily")
      interval = 1.days
    elsif (scale == "monthly")
      interval = 1.months
    elsif (scale == "yearly")
      interval = 1.years
    else
      return nil
    end
    avgstart = startdate
    avgend = startdate + interval

    begin
      diffpart = self.readings.find(:all, :order => "time ASC", :conditions => {:time => avgstart..avgend})
      first = 0
      last = 0
      count = diffpart.size
      diff = 0

      if (count > 0)
        first = diffpart.first.value
        last = diffpart.last.value

        diff = last - first

        avgreading = Reading.new(:value => diff, :time => diffpart.first.time)

        tt = avgreading.time
        if (scale == "hourly")
          avgreading.time = Time.local(tt.year, tt.month, tt.day, tt.hour)
        elsif (scale == "daily")
          avgreading.time = Time.local(tt.year, tt.month, tt.day)
        elsif (scale == "monthly")
          avgreading.time = Time.local(tt.year, tt.month)
        elsif (scale == "yearly")
          avgreading.time = Time.local(tt.year)
        end

        avgreadings << avgreading
      end

      avgstart = avgstart + interval
      avgend = avgend + interval
    end while (avgend <= enddate)

    return avgreadings

  end

  def get_readings(startdate, enddate, limit)
    if (limit)
      readings = self.readings.find(:all, :limit => limit, :order => "time DESC", :conditions => {:time => startdate..enddate})
      readings.reverse!
    else
      readings = self.readings.find(:all, :order => "time ASC", :conditions => {:time => startdate..enddate})
    end
    return readings
  end
end
