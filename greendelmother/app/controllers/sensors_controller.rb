class SensorsController < ApplicationController
  # GET /sensors
  # GET /sensors.xml
  def index
    @sensors = Sensor.find(params[:id])

    respond_to do |format|
      format.html # index.html.erb
      format.xml # index.xml.builder
    end
  end

  # GET /sensors/1
  # GET /sensors/1.xml
  def show
    @sensor = Sensor.find(params[:id])

    @startdate = Time.zone.today.to_time - 1.days
    @enddate = Time.zone.today.to_time + 1.days

    @startdate = params[:startdate].to_date.to_time if params[:startdate]
    @enddate = params[:enddate].to_date.to_time if params[:enddate]

    @sensor = Sensor.find(params[:id], :include => [:device])
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

    if (avgscale == "none")
      @readings = @sensor.get_readings(@startdate, @enddate, nil)
    elsif (avgscale == "hourly" || avgscale == "daily" || avgscale == "monthly" || avgscale == "yearly")
      @readings = @sensor.get_avg_readings(@startdate, @enddate, avgscale)
      #elsif (diffscale == "hourly" || diffscale == "daily" || diffscale == "monthly" || diffscale == "yearly")
      #  @readings = @sensor.get_diff(@startdate, @enddate, diffscale)
    else
      @readings = @sensor.get_readings(@startdate, @enddate, limit)
    end

    # Obsolete
    # @readings = @sensor.readings.find(:all, :order => "time ASC", :conditions => {:time => @startdate..@enddate})
    @firstreadings = @sensor.readings.find(:all, :order => "time ASC", :limit => 10)

    respond_to do |format|
      format.html # show.html.erb
      format.xml { render :xml => @sensor }
    end
  end

  def history

    @startdate = Time.zone.today.to_time - 1.days
    @enddate = Time.zone.today.to_time + 1.days

    @startdate = params[:startdate].to_date.to_time if params[:startdate]
    @enddate = params[:enddate].to_date.to_time if params[:enddate]

    @sensor = Sensor.find(params[:id], :include => [:device])
    avgscale = params[:avgscale]
    diffscale = params[:diffscale]
    limit = params[:limit]

    if (avgscale == "hourly" || avgscale == "daily" || avgscale == "monthly" || avgscale == "yearly")
      @readings = @sensor.get_avg_readings(@startdate, @enddate, avgscale)
    elsif (diffscale == "hourly" || diffscale == "daily" || diffscale == "monthly" || diffscale == "yearly")
      @readings = @sensor.get_diff(@startdate, @enddate, diffscale)
    else
      @readings = @sensor.get_readings(@startdate, @enddate, limit)
    end

    respond_to do |format|
      format.xml # datastatus.xml.builder
    end

  end

  # GET /sensors/new
  # GET /sensors/new.xml
  def new
    @sensor = Sensor.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml { render :xml => @sensor }
    end
  end

  # GET /sensors/1/edit
  def edit
    @sensor = Sensor.find(params[:id])
  end

  # POST /sensors
  # POST /sensors.xml
  def create
    @sensor = Sensor.new(params[:sensor])

    respond_to do |format|
      if @sensor.save
        flash[:notice] = 'Sensor was successfully created.'
        format.html { redirect_to(@sensor) }
        format.xml { render :xml => @sensor, :status => :created, :location => @sensor }
      else
        format.html { render :action => "new" }
        format.xml { render :xml => @sensor.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /sensors/1
  # PUT /sensors/1.xml
  def update
    @sensor = Sensor.find(params[:id])

    respond_to do |format|
      if @sensor.update_attributes(params[:sensor])
        flash[:notice] = 'Sensor was successfully updated.'
        format.html { redirect_to(@sensor) }
        format.xml { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml { render :xml => @sensor.errors, :status => :unprocessable_entity }
      end
    end
  end

  # DELETE /sensors/1
  # DELETE /sensors/1.xml
  def destroy
    @sensor = Sensor.find(params[:id])
    @sensor.destroy

    respond_to do |format|
      format.html { redirect_to(sensors_url) }
      format.xml { head :ok }
    end
  end
end
