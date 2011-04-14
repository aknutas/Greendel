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
  end

end
