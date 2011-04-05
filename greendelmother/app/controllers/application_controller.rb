# Filters added to this controller apply to all controllers in the application.
# Likewise, all the methods added will be available for all controllers.

class ApplicationController < ActionController::Base
  before_filter :authorize, :except => :login
  helper :all # include all helpers, all the time
  protect_from_forgery # See ActionController::RequestForgeryProtection for details

  protected
  def authorize
    unless User.find_by_id(session[:user_id])
      if session[:user_id] != :logged_out

        authenticate_or_request_with_http_basic('greendel') do |username, password|
          user = User.authenticate(username, password)

          # Sparing the DB
          # session[:lastlogin] = user.lastlogin.ctime if user.lastlogin
          # user.lastlogin = Time.now
          # user.save

          if user
            session[:user_id] = user.id
            session[:user_name] = user.name
          end
        end

      else
        flash[:notice] = "Please log in"
        redirect_to :controller => 'sign', :action => 'login'
      end
    end
    @current_user ||= User.find_by_id(session[:user_id])
  end

  def current_user
    @current_user ||= User.find_by_id(session[:user_id])
    return @current_user
  end

  # Scrub sensitive parameters from your log
  # filter_parameter_logging :password
end
