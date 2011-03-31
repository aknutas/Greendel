class CreateSensors < ActiveRecord::Migration
  def self.up
    create_table :sensors do |t|
      t.string :name
      t.string :vartype
      t.integer :device_id
      t.float :latestreading

      t.timestamps
    end
  end

  def self.down
    drop_table :sensors
  end
end