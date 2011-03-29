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

ActiveRecord::Schema.define(:version => 20110126184141) do

  create_table "devices", :force => true do |t|
    t.string   "name",       :default => ""
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  create_table "outputs", :force => true do |t|
    t.string   "name",       :default => ""
    t.boolean  "state"
    t.boolean  "haschanged"
    t.integer  "device_id",  :default => 0
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  create_table "readings", :force => true do |t|
    t.string   "name",       :default => ""
    t.integer  "value",      :default => 0
    t.integer  "sensor_id",  :default => 0
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  create_table "sensors", :force => true do |t|
    t.string   "name",          :default => ""
    t.string   "vartype",       :default => ""
    t.integer  "device_id",     :default => 0
    t.datetime "created_at"
    t.datetime "updated_at"
    t.integer  "latestreading", :default => 0
  end

end
