class SocialmediasController < ApplicationController

  # GET /socialmedias
  # GET /socialmedias.xml
  def index
    @socialmedias = Socialmedia.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml { render :xml => @socialmedias }
    end
  end

  # GET /socialmedias/1
  # GET /socialmedias/1.xml
  def show
    @socialmedia = Socialmedia.find(params[:id], :include => [:user, {:user => [:device, {:device => :sensors}]}])

    @sensors = @socialmedia.user.device.sensors

    @oauth_url = MiniFB.oauth_url(FB_APP_ID, HOST + "sessions/create",
                                  :scope=>MiniFB.scopes.join(","))

    respond_to do |format|
      format.html # show.html.erb
      format.xml { render :xml => @socialmedia }
    end
  end

  def fbpost
    @socialmedia = Socialmedia.find(params[:id], :include => [:user, {:user => [:device, {:device => :sensors}]}])

    @sensors = @socialmedia.user.device.sensors

    sensorhash = Hash.new

    @sensors.each do |sensor|
      sensorhash[sensor.name] = sensor
    end

    startuse = sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    enduse = sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    pprice = sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")

    wdiff = enduse.value - startuse.value
    wyprice = wdiff * pprice.value

    startuse = sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    enduse = sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    pprice = sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")

    mdiff = enduse.value - startuse.value
    myprice = mdiff * pprice.value

    poststring = "Greendel status report! \n\n"

    @sensors.each do |sensor|
      if (params[sensor.name] == "1")
        if (sensor.name == 'poweruse')
          poststring << "My house is currently consuming " + sensor.latestreading.round(1).to_s + " W\n"
          poststring << "A full week of use at this rate would cost me " + (sensorhash['poweruse'].latestreading * 24 * 7 * sensorhash['powerprice'].latestreading / 1000).round(2).to_s + " euros. \n"
        elsif (sensor.name == 'powerconsumed')
          poststring << "Last week my electricity use was " + wdiff.round(2).to_s + " kWh and it cost me " + wyprice.round(2).to_s + " euros. \n"
          poststring << "Last month my electricity use was " + mdiff.round(2).to_s + " kWh and it cost me " + myprice.round(2).to_s + " euros. \n"
        else
          poststring << sensor.longname.downcase.capitalize + " is currently at: " + sensor.latestreading.to_s + " " + sensor.unit + "\n"
        end
      end
    end

    @id = "me"
    @type = "feed"
    @message = poststring

    if (@socialmedia.facebookon)
      begin
        MiniFB.post(@socialmedia.fbauth, @id, :type=>@type, :metadata=>true, :params => {:message => @message, :link => @link})
        redirect_to(@socialmedia, :notice => 'Post was successful!')
      rescue
        redirect_to(@socialmedia, :notice => 'Facebook post error!')
      end
    else
      redirect_to(@socialmedia, :notice => 'Post failed: You do not have facebook configured.')
    end
  end

  def postuse
    @socialmedia = Socialmedia.find(params[:id], :include => [:user, {:user => [:device, {:device => :sensors}]}])

    sensorids = Array.new

    params['sensorid'].split(',').each do |s|
      sensorids << s.to_i
    end

    @sensorsall = @socialmedia.user.device.sensors
    @sensors = Sensor.find_all_by_id(sensorids)

    sensorhash = Hash.new

    @sensorsall.each do |sensor|
      sensorhash[sensor.name] = sensor
    end

    startuse = sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    enduse = sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")
    pprice = sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 14.days.ago .. 6.days.ago}, :order => "time")

    wdiff = enduse.value - startuse.value
    wyprice = wdiff * pprice.value

    startuse = sensorhash['powerconsumed'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    enduse = sensorhash['powerconsumed'].readings.find(:last, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")
    pprice = sensorhash['powerprice'].readings.find(:first, :conditions => {:time => 2.months.ago .. 1.months.ago}, :order => "time")

    mdiff = enduse.value - startuse.value
    myprice = mdiff * pprice.value

    poststring = "Greendel status report! \n\n"

    @sensors.each do |sensor|
      if (sensor.name == 'poweruse')
        poststring << "My house is currently consuming " + sensor.latestreading.round(1).to_s + " W\n"
        poststring << "A full week of use at this rate would cost me " + (sensorhash['poweruse'].latestreading * 24 * 7 * sensorhash['powerprice'].latestreading / 1000).round(2).to_s + " euros. \n"
      elsif (sensor.name == 'powerconsumed')
        poststring << "Last week my electricity use was " + wdiff.round(2).to_s + " kWh and it cost me " + wyprice.round(2).to_s + " euros. \n"
        poststring << "Last month my electricity use was " + mdiff.round(2).to_s + " kWh and it cost me " + myprice.round(2).to_s + " euros. \n"
      else
        poststring << sensor.longname.downcase.capitalize + " is currently at: " + sensor.latestreading.to_s + " " + sensor.unit + "\n"
      end
    end

    @id = "me"
    @type = "feed"
    @message = poststring

    if (@socialmedia.facebookon)
      MiniFB.post(@socialmedia.fbauth, @id, :type=>@type, :metadata=>true, :params => {:message => @message})
      success = true
    else
      success = false
    end
    render :xml => {:status => success}
  end

# GET /socialmedias/new
# GET /socialmedias/new.xml
  def new
    @socialmedia = Socialmedia.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml { render :xml => @socialmedia }
    end
  end

# GET /socialmedias/1/edit
  def edit
    @socialmedia = Socialmedia.find(params[:id])
  end

# POST /socialmedias
# POST /socialmedias.xml
  def create
    @socialmedia = Socialmedia.new(params[:socialmedia])

    respond_to do |format|
      if @socialmedia.save
        format.html { redirect_to(@socialmedia, :notice => 'Socialmedia was successfully created.') }
        format.xml { render :xml => @socialmedia, :status => :created, :location => @socialmedia }
      else
        format.html { render :action => "new" }
        format.xml { render :xml => @socialmedia.errors, :status => :unprocessable_entity }
      end
    end
  end

# PUT /socialmedias/1
# PUT /socialmedias/1.xml
  def update
    @socialmedia = Socialmedia.find(params[:id])

    respond_to do |format|
      if @socialmedia.update_attributes(params[:socialmedia])
        format.html { redirect_to(@socialmedia, :notice => 'Socialmedia was successfully updated.') }
        format.xml { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml { render :xml => @socialmedia.errors, :status => :unprocessable_entity }
      end
    end
  end

# DELETE /socialmedias/1
# DELETE /socialmedias/1.xml
  def destroy
    @socialmedia = Socialmedia.find(params[:id])
    @socialmedia.destroy

    respond_to do |format|
      format.html { redirect_to(socialmedias_url) }
      format.xml { head :ok }
    end
  end

end
