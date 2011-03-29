class AddLatestreadingToSensor < ActiveRecord::Migration
  def self.up
    add_column :sensors, :latestreading, :integer
  end

  def self.down
    remove_column :sensors, :latestreading
  end
end
