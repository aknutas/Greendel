class SocialmediasController < ApplicationController
  # GET /socialmedias
  # GET /socialmedias.xml
  def index
    @socialmedias = Socialmedia.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @socialmedias }
    end
  end

  # GET /socialmedias/1
  # GET /socialmedias/1.xml
  def show
    @socialmedia = Socialmedia.find(params[:id])

    respond_to do |format|
      format.html # show.html.erb
      format.xml  { render :xml => @socialmedia }
    end
  end

  # GET /socialmedias/new
  # GET /socialmedias/new.xml
  def new
    @socialmedia = Socialmedia.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml  { render :xml => @socialmedia }
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
        format.xml  { render :xml => @socialmedia, :status => :created, :location => @socialmedia }
      else
        format.html { render :action => "new" }
        format.xml  { render :xml => @socialmedia.errors, :status => :unprocessable_entity }
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
        format.xml  { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @socialmedia.errors, :status => :unprocessable_entity }
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
      format.xml  { head :ok }
    end
  end
end
