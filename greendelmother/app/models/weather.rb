class Weather < ActiveRecord::Base
  belongs_to :location

  has_many :histories

  def get_forecasts
    forecasts = Hash.new
    yw = Marshal.load(self.yweather)
    yw.forecasts.each do |ofc|
      if (Date.parse(ofc.date.to_s) == Date.today)
        forecasts[:today] = ofc
      elsif(Date.parse(ofc.date.to_s) == Date.tomorrow)
        forecasts[:tomorrow] = ofc
      end
    end
    return forecasts
  end

  def get_image_link
    return "http://l.yimg.com/a/i/us/nws/weather/gr/" + self.code.to_s + "d.png"
  end

  def get_forecast_link(fcSymbol)
    fcs = get_forecasts
    fc = fcs[fcSymbol]
    return "http://l.yimg.com/a/i/us/nws/weather/gr/" + fc.code.to_s + "d.png"
  end

end
