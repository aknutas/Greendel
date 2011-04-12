class SessionsController < ApplicationController

  def create
    puts 'sessions/create'
    p params

    code = params['code'] # Facebooks verification string
    if code
      access_token_hash = MiniFB.oauth_access_token(FB_APP_ID, HOST + "sessions/create", FB_SECRET, code)
      p access_token_hash
      @access_token = access_token_hash["access_token"]
      cookies[:access_token] = @access_token
      flash[:success] = "Authentication successful."

      @user = current_user

      @user.socialmedia.fbauth = @access_token
      @user.socialmedia.facebookon = true
      @user.save

    end
    redirect_to :controller => "socialmedias"
  end

end
