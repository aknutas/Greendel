class UsersController < ApplicationController
  layout 'application', :except => [:current_consumption, :power_prices]

  # GET /users
  # GET /users.xml
  def index
    @users = User.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml { render :xml => @users }
    end
  end

  # GET /users/1
  # GET /users/1.xml
  def show
    @user = User.find(params[:id], :include => [:device, {:device => [:sensors, :outputs, :location, {:location => :weather}]}])

    @csensor = @user.device.sensors.find(:first, :conditions => {:name => "poweruse"})
    @readings = @csensor.readings.find(:all, :order => "time DESC", :limit => 40)

    begin
      @wnow = @user.device.location.weather
      fcs = @wnow.get_forecasts
      @wtoday = fcs[:today]
      @wtomorrow = fcs[:tomorrow]
    rescue
      @wnow = nil
      @wtoday = nil
      @wtomorrow = nil
    end

    @sensors = @user.device.sensors

    @sensorhash = Hash.new

    @sensors.each do |sensor|
      @sensorhash[sensor.name] = sensor
    end

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")

    @wdiff = enduse.value - startuse.value
    @wyprice = @wdiff * @pprice.value

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")

    @mdiff = enduse.value - startuse.value
    @myprice = @mdiff * @pprice.value

    respond_to do |format|
      format.html # show.html.erb
      format.xml # show.xml.builder
    end
  end

  # AJAX update method
  def current_consumption
    @user = current_user()
    @csensor = @user.device.sensors.find(:first, :conditions => {:name => "poweruse"})
    @reading = @csensor.readings.last
  end

  # AJAX update method
  def power_prices
    @sensors = current_user.device.sensors

    @sensorhash = Hash.new

    @sensors.each do |sensor|
      @sensorhash[sensor.name] = sensor
    end

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")

    @wdiff = enduse.value - startuse.value
    @wyprice = @wdiff * @pprice.value

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")

    @mdiff = enduse.value - startuse.value
    @myprice = @mdiff * @pprice.value

    respond_to do |format|
      format.html # show.html.erb
      format.xml # show.xml.builder
    end
  end

  # AJAX update method for Highcharts
  def current_consumption_chart
    @user = current_user()
    @csensor = @user.device.sensors.find(:first, :conditions => {:name => "poweruse"})

    value = @csensor.readings.last.value
    returnarray = Array.new
    returnarray[0] = (@csensor.readings.last.time.to_i + 3.hours) * 1000
    returnarray[1] = value

    response.headers['Content-Type'] = 'text/json'
    render :json => returnarray
  end

  #Custom for mobile
  def datastatus
    @user = User.find(session[:user_id], :include => [:device, {:device => [:sensors, :outputs, :location, {:location => :weather}]}])

    begin
      @wnow = @user.device.location.weather
      fcs = @wnow.get_forecasts
      @wtoday = fcs[:today]
      @wtomorrow = fcs[:tomorrow]
    rescue
      @wnow = nil
      @wtoday = nil
      @wtomorrow = nil
    end

    @sensors = @user.device.sensors

    @sensorhash = Hash.new

    @sensors.each do |sensor|
      @sensorhash[sensor.name] = sensor
    end

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")

    @wdiff = enduse.value - startuse.value
    @wyprice = @wdiff * @pprice.value

    startuse = @sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    enduse = @sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    @pprice = @sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")

    @mdiff = enduse.value - startuse.value
    @myprice = @mdiff * @pprice.value

    respond_to do |format|
      format.xml # datastatus.xml.builder
    end
  end

  # GET /users/new
  # GET /users/new.xml
  def new
    @user = User.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml { render :xml => @user }
    end
  end

  # GET /users/1/edit
  def edit
    @user = User.find(params[:id])
  end

  # POST /users
  # POST /users.xml
  def create
    @user = User.new(params[:user])

    respond_to do |format|
      if @user.save
        format.html { redirect_to(@user, :notice => 'User was successfully created.') }
        format.xml { render :xml => @user, :status => :created, :location => @user }
      else
        format.html { render :action => "new" }
        format.xml { render :xml => @user.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /users/1
  # PUT /users/1.xml
  def update
    @user = User.find(params[:id])

    respond_to do |format|
      if @user.update_attributes(params[:user])
        format.html { redirect_to(@user, :notice => 'User was successfully updated.') }
        format.xml { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml { render :xml => @user.errors, :status => :unprocessable_entity }
      end
    end
  end

  # DELETE /users/1
  # DELETE /users/1.xml
  def destroy
    @user = User.find(params[:id])
    @user.destroy

    respond_to do |format|
      format.html { redirect_to(users_url) }
      format.xml { head :ok }
    end
  end
end
