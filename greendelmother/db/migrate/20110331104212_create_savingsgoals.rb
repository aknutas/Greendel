class CreateSavingsgoals < ActiveRecord::Migration
  def self.up
    create_table :savingsgoals do |t|
      t.float :amount
      t.string :type
      t.boolean :completed, :default => false
      t.boolean :successful
      t.date :timestart
      t.date :timeend

      t.integer :device_id
      t.integer :sensor_id

      t.timestamps
    end

    add_index :savingsgoals, :device_id
    add_index :savingsgoals, :sensor_id
    add_index :savingsgoals, :completed

  end

  def self.down
    drop_table :savingsgoals
  end
end
