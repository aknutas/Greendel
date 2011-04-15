class SavingsgoalsController < ApplicationController
  # GET /savingsgoals
  # GET /savingsgoals.xml
  def index
    @savingsgoals = Savingsgoal.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml { render :xml => @savingsgoals }
    end
  end

  # GET /savingsgoals/1
  # GET /savingsgoals/1.xml
  def show
    @savingsgoal = Savingsgoal.find(params[:id])

    respond_to do |format|
      format.html # show.html.erb
      format.xml { render :xml => @savingsgoal }
    end
  end

  def datastatus
    @user = current_user()
    @savingsgoals = @user.device.savingsgoals.find(:all, :order => "timestart ASC", :include => [:device, :sensor])

    respond_to do |format|
      format.xml # datastatus.xml.builder
    end
  end

  # GET /savingsgoals/new
  # GET /savingsgoals/new.xml
  def new
    @savingsgoal = Savingsgoal.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml { render :xml => @savingsgoal }
    end
  end

  # GET /savingsgoals/1/edit
  def edit
    @savingsgoal = Savingsgoal.find(params[:id])
  end

  # POST /savingsgoals
  # POST /savingsgoals.xml
  def create
    @savingsgoal = Savingsgoal.new(params[:savingsgoal])

    respond_to do |format|
      if @savingsgoal.save
        format.html { redirect_to(@savingsgoal, :notice => 'Savingsgoal was successfully created.') }
        format.xml { render :xml => @savingsgoal, :status => :created, :location => @savingsgoal }
      else
        format.html { render :action => "new" }
        format.xml { render :xml => @savingsgoal.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /savingsgoals/1
  # PUT /savingsgoals/1.xml
  def update
    @savingsgoal = Savingsgoal.find(params[:id])

    respond_to do |format|
      if @savingsgoal.update_attributes(params[:savingsgoal])
        format.html { redirect_to(@savingsgoal, :notice => 'Savingsgoal was successfully updated.') }
        format.xml { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml { render :xml => @savingsgoal.errors, :status => :unprocessable_entity }
      end
    end
  end

  # DELETE /savingsgoals/1
  # DELETE /savingsgoals/1.xml
  def destroy
    @savingsgoal = Savingsgoal.find(params[:id])
    @savingsgoal.destroy

    respond_to do |format|
      format.html { redirect_to(savingsgoals_url) }
      format.xml { head :ok }
    end
  end
end
