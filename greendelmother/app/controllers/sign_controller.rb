class SignController < ApplicationController
  def login
    if request.post?
      user = User.authenticate(params[:name], params[:password])
      if user
        reset_session
        session[:login_id] = user.id
        session[:login_name] = user.name
        session[:user_name] = user.name
        session[:user_id] = user.id

        session[:lastlogin] = user.lastlogin.ctime if user.lastlogin
        user.lastlogin = Time.now
        user.save

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
