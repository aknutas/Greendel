# This file is auto-generated from the current state of the database. Instead of editing this file, 
# please use the migrations feature of Active Record to incrementally modify your database, and
# then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your database schema. If you need
# to create the application database on another system, you should be using db:schema:load, not running
# all the migrations from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended to check this file into your version control system.

ActiveRecord::Schema.define(:version => 20110407082126) do

  create_table "devices", :force => true do |t|
    t.string   "name"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  create_table "histories", :force => true do |t|
    t.string   "desc"
    t.float    "temp"
    t.float    "low"
    t.float    "high"
    t.date     "fday"
    t.binary   "yweather"
    t.integer  "weather_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "histories", ["fday"], :name => "index_histories_on_fday"
  add_index "histories", ["weather_id"], :name => "index_histories_on_weather_id"

  create_table "locations", :force => true do |t|
    t.string   "name"
    t.string   "address"
    t.string   "town"
    t.string   "extaddress"
    t.float    "powerprice"
    t.float    "xcoord"
    t.float    "ycoord"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "locations", ["device_id"], :name => "index_locations_on_device_id"

  create_table "outputs", :force => true do |t|
    t.string   "name"
    t.string   "longname"
    t.boolean  "state"
    t.boolean  "haschanged"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "outputs", ["device_id"], :name => "index_outputs_on_device_id"

  create_table "readings", :force => true do |t|
    t.float    "value"
    t.datetime "time"
    t.integer  "sensor_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "readings", ["sensor_id"], :name => "index_readings_on_sensor_id"
  add_index "readings", ["time"], :name => "index_readings_on_time"

  create_table "savingsgoals", :force => true do |t|
    t.float    "amount"
    t.string   "type"
    t.boolean  "completed",     :default => false
    t.boolean  "successful"
    t.boolean  "publishsocial"
    t.date     "timestart"
    t.date     "timeend"
    t.integer  "device_id"
    t.integer  "sensor_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "savingsgoals", ["completed"], :name => "index_savingsgoals_on_completed"
  add_index "savingsgoals", ["device_id"], :name => "index_savingsgoals_on_device_id"
  add_index "savingsgoals", ["sensor_id"], :name => "index_savingsgoals_on_sensor_id"

  create_table "sensors", :force => true do |t|
    t.string   "name"
    t.string   "longname"
    t.string   "vartype"
    t.float    "latestreading"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "sensors", ["device_id"], :name => "index_sensors_on_device_id"

  create_table "socialmedias", :force => true do |t|
    t.boolean  "status",        :default => false
    t.boolean  "facebookon",    :default => false
    t.boolean  "twitteron",     :default => false
    t.string   "fb_un"
    t.string   "twitter_un"
    t.string   "fbauth"
    t.string   "access_token"
    t.string   "access_secret"
    t.integer  "user_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "socialmedias", ["user_id"], :name => "index_socialmedias_on_user_id"

  create_table "users", :force => true do |t|
    t.string   "name"
    t.string   "hashed_password"
    t.string   "salt"
    t.string   "email"
    t.string   "realname"
    t.integer  "device_id"
    t.datetime "lastlogin"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "users", ["device_id"], :name => "index_users_on_device_id"
  add_index "users", ["name"], :name => "index_users_on_name"

  create_table "weathers", :force => true do |t|
    t.float    "temp"
    t.float    "high"
    t.float    "low"
    t.string   "desc"
    t.string   "source"
    t.string   "woeid"
    t.binary   "yweather"
    t.integer  "location_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "weathers", ["location_id"], :name => "index_weathers_on_location_id"

  create_table "woeids", :force => true do |t|
    t.string   "location"
    t.string   "woeid"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "woeids", ["location"], :name => "index_woeids_on_location"

end
