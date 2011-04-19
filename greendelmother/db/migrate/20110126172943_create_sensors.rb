class CreateSensors < ActiveRecord::Migration
  def self.up
    create_table :sensors do |t|
      t.string :name
      t.string :longname
      t.string :vartype
      t.string :unit
      t.boolean :counter, :default => false
      t.float :latestreading

      t.integer :device_id

      t.timestamps
    end

    add_index :sensors, :device_id
    add_index :sensors, :name

  end


  def self.down
    drop_table :sensors
  end
end