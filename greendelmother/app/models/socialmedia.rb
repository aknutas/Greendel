class Socialmedia < ActiveRecord::Base
  belongs_to :user

  def poststring(string)
    MiniFB.post(self.fbauth, @id, :type=>@type, :metadata=>true, :params => {:message => string})
  end

  def postlink(string, link)
    MiniFB.post(self.fbauth, @id, :type=>@type, :metadata=>true, :params => {:message => string, :link => link})
  end

end
