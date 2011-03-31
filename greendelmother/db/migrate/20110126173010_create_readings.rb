class CreateReadings < ActiveRecord::Migration
  def self.up
    create_table :readings do |t|
      t.string :name
      t.float :value

      t.integer :sensor_id

      t.timestamps
    end

    add_index :readings, :sensor_id

  end

  def self.down
    drop_table :readings
  end
end
