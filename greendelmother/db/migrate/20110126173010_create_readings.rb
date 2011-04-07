class CreateReadings < ActiveRecord::Migration
  def self.up
    create_table :readings do |t|
      t.float :value
      t.timestamp :time

      t.integer :sensor_id

      t.timestamps
    end

    add_index :readings, :sensor_id
    add_index :readings, :time

  end

  def self.down
    drop_table :readings
  end
end
