#Initializing variables and creating new objects
puts "Initializing database"

DataMapper::Logger.new($stdout, :debug)
DataMapper.setup(:default, 'sqlite:database.sqlite3')
DataMapper::Model.raise_on_save_failure = true

class Device
  include DataMapper::Resource
  has n, :sensors
  has n, :outputs

  property :id, Integer, :key => true
  property :name, String
  property :ip, String
  property :port, Integer
  property :slaveaddr, Integer
  property :interval, Integer
end

class Sensor
  include DataMapper::Resource
  has n, :readings
  belongs_to :device

  property :id, Integer, :key => true
  property :register, Integer
  property :name, String
  property :ntype, String
  property :scalefactor, Decimal, :precision => 10, :scale => 5
end

class Output
  include DataMapper::Resource
  belongs_to :device

  property :id, Integer, :key => true
  property :register, Integer
  property :name, String
  property :onoff, Boolean
end

class Reading
  include DataMapper::Resource
  belongs_to :sensor

  property :id, Serial
  property :value, Float
  property :time, DateTime
end

DataMapper.finalize
DataMapper.auto_upgrade!

#Sample class
=begin
class Post
  include DataMapper::Resource

  property :id,         Serial   # An auto-increment integer key
  property :title,      String   # A varchar type string, for short strings
  property :body,       Text     # A text block, for longer string data.
  property :created_at, DateTime # A DateTime, for any date you might like.
end
=end