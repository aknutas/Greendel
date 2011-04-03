class User < ActiveRecord::Base
  has_one :device
  has_one :socialmedia

  validates_presence_of     :name
  validates_uniqueness_of   :name

  attr_accessor :password_confirmation
  validates_confirmation_of :password

  validate :password_non_blank

  after_create :default_values

  def self.authenticate(name, password)
    user = self.find_by_name(name)
    if user
      expected_password = encrypted_password(password, user.salt)
      if user.hashed_password != expected_password
        user = nil
      end
    end
    user
  end

  # 'password' is a virtual attribute

  def password
    @password
  end

  def password=(pwd)
    @password = pwd
    return if pwd.blank?
    create_new_salt
    self.hashed_password = Login.encrypted_password(self.password, self.salt)
  end

  private

  def default_values
    self.lastlogin = Time.now unless self.lastlogin
  end

  def password_non_blank
    errors.add(:password, "Missing password") if hashed_password.blank?
  end

  def create_new_salt
    self.salt = self.object_id.to_s + rand.to_s
  end

  def self.encrypted_password(password, salt)
    string_to_hash = password + "dabble" + salt
    Digest::SHA1.hexdigest(string_to_hash)
  end

end
