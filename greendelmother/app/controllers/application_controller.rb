# Filters added to this controller apply to all controllers in the application.
# Likewise, all the methods added will be available for all controllers.

class ApplicationController < ActionController::Base
  before_filter :authorize, :except => :login
  helper :all # include all helpers, all the time
  protect_from_forgery # See ActionController::RequestForgeryProtection for details

  protected
    def authorize
    unless Login.find_by_id(session[:login_id])
      if session[:login_id] != :logged_out

        authenticate_or_request_with_http_basic('logiikkaweb') do |username, password|
          login = Login.authenticate(username, password)

          unless login.is_device?
            session[:user_name] = login.user.name
            session[:user_id] = login.user.id
            session[:lastlogin] = login.lastlogin.ctime if login.lastlogin
            login.lastlogin = Time.now
            login.save
          end
          if login
            session[:login_id] = login.id
            session[:login_name] = login.name
          end
        end

      else
        flash[:notice] = "Please log in"
        redirect_to :controller => 'sign', :action => 'login'
      end
    end
  end

  # Scrub sensitive parameters from your log
  # filter_parameter_logging :password
end
