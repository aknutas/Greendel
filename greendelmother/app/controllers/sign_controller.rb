class SignController < ApplicationController
  def login
    if request.post?
      user = User.authenticate(params[:name], params[:password])
      if user
        reset_session
        session[:login_id] = login.id
        session[:login_name] = login.name
        session[:user_name] = login.user.name
        session[:user_id] = login.user.id
        session[:lastlogin] = login.lastlogin.ctime if login.lastlogin
        login.lastlogin = Time.now
        login.save
        redirect_to(:controller => "users", :action => "index")
      else
        reset_session
        session[:user_id] = :logged_out
        flash.now[:notice] = "Invalid login/password combination"
      end
    end
  end

  def logout
    session[:user_id] = nil
    reset_session
    flash[:notice] = "Logged out"
    redirect_to(:action => "login")
  end

  def index
    render :layout => 'application'
  end

end
