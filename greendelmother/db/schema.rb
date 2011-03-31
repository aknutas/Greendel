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

ActiveRecord::Schema.define(:version => 20110331104212) do

  create_table "devices", :force => true do |t|
    t.string   "name"
    t.integer  "user_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "devices", ["user_id"], :name => "index_devices_on_user_id"

  create_table "locations", :force => true do |t|
    t.string   "name"
    t.string   "address"
    t.string   "town"
    t.string   "extaddress"
    t.float    "xcoord"
    t.float    "ycoord"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "locations", ["device_id"], :name => "index_locations_on_device_id"

  create_table "outputs", :force => true do |t|
    t.string   "name"
    t.boolean  "state"
    t.boolean  "haschanged"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "outputs", ["device_id"], :name => "index_outputs_on_device_id"

  create_table "readings", :force => true do |t|
    t.string   "name"
    t.float    "value"
    t.integer  "sensor_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "readings", ["sensor_id"], :name => "index_readings_on_sensor_id"

  create_table "savingsgoals", :force => true do |t|
    t.float    "amount"
    t.string   "type"
    t.date     "timestart"
    t.date     "timeend"
    t.integer  "device_id"
    t.integer  "sensor_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "savingsgoals", ["device_id"], :name => "index_savingsgoals_on_device_id"
  add_index "savingsgoals", ["sensor_id"], :name => "index_savingsgoals_on_sensor_id"

  create_table "sensors", :force => true do |t|
    t.string   "name"
    t.string   "vartype"
    t.float    "latestreading"
    t.integer  "device_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "sensors", ["device_id"], :name => "index_sensors_on_device_id"

  create_table "socialmedias", :force => true do |t|
    t.string   "fbun"
    t.string   "twitterun"
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
    t.datetime "lastlogin"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  create_table "weathers", :force => true do |t|
    t.float    "temp"
    t.string   "desc"
    t.string   "source"
    t.integer  "location_id"
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "weathers", ["location_id"], :name => "index_weathers_on_location_id"

end
