class DevicesController < ApplicationController
  # GET /devices
  # GET /devices.xml
  def index
    @devices = Device.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml { render :xml => @devices }
    end
  end

  # GET /devices/1
  # GET /devices/1.xml
  def show

    @device = Device.find(params[:id], :include => [:sensors, :location])
    @sensors = Array.new

    @device.sensors.each do |sensor|
      @sensors << sensor
    end

    @startdate = Time.zone.today.to_time - 1.days
    @enddate = Time.zone.today.to_time + 1.days

    @startdate = params[:startdate].to_date.to_time if params[:startdate]
    @enddate = params[:enddate].to_date.to_time if params[:enddate]

    avgscale = params[:avgscale]
    diffscale = params[:diffscale]
    limit = params[:limit]

    # Setting defaults
    unless (avgscale)
      avgscale = "hourly"
    end
    unless (limit)
      limit = 500
    end

    @chartreadings = Hash.new
    @powercons = nil

    @sensors.each do |sensor|
      if (avgscale == "none")
        reading = sensor.get_readings(@startdate, @enddate, nil)
      elsif (avgscale == "hourly" || avgscale == "daily" || avgscale == "monthly" || avgscale == "yearly")
        if (sensor.name == 'powerconsumed')
          reading = sensor.get_diff(@startdate, @enddate, avgscale)
          @powercons = sensor
        else
          reading = sensor.get_avg_readings(@startdate, @enddate, avgscale)
        end
      else
        reading = sensor.get_readings(@startdate, @enddate, limit)
      end
      @chartreadings[sensor] = reading
    end

    if (avgscale && @powercons)
      vsensor = Sensor.new(:name => 'moneycost', :longname => 'Price of Consumption', :unit => 'e')

      powerconsread = @chartreadings[@powercons]
      powerprice = @device.sensors.find_by_name('powerprice').latestreading

      vreadings = Array.new

      powerconsread.each do |pcs|
        reading = Reading.new(:value => (pcs.value * powerprice), :time => pcs.time)
        vreadings << reading
      end

      @chartreadings[vsensor] = vreadings
      @sensors << vsensor
    end

    respond_to do |format|
      format.html # show.html.erb
      format.xml { render :xml => @device }
    end
  end

  def datastatus
    @device = Device.find(params[:id], :include => [:sensors, :outputs, :location, {:location => :weather}])

    respond_to do |format|
      format.xml # datastatus.xml.builder
    end
  end

  # GET /devices/new
  # GET /devices/new.xml
  def new
    @device = Device.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml { render :xml => @device }
    end
  end

  # GET /devices/1/edit
  def edit
    @device = Device.find(params[:id])
  end

  # POST /devices
  # POST /devices.xml
  def create
    @device = Device.new(params[:device])

    respond_to do |format|
      if @device.save
        flash[:notice] = 'Device was successfully created.'
        format.html { redirect_to(@device) }
        format.xml { render :xml => @device, :status => :created, :location => @device }
      else
        format.html { render :action => "new" }
        format.xml { render :xml => @device.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /devices/1
  # PUT /devices/1.xml
  def update
    @device = Device.find(params[:id])

    respond_to do |format|
      if @device.update_attributes(params[:device])
        flash[:notice] = 'Device was successfully updated.'
        format.html { redirect_to(@device) }
        format.xml { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml { render :xml => @device.errors, :status => :unprocessable_entity }
      end
    end
  end

  # DELETE /devices/1
  # DELETE /devices/1.xml
  def destroy
    @device = Device.find(params[:id])
    @device.destroy

    respond_to do |format|
      format.html { redirect_to(devices_url) }
      format.xml { head :ok }
    end
  end
end
