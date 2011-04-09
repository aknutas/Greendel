class CreateSavingsgoals < ActiveRecord::Migration
  def self.up
    create_table :savingsgoals do |t|
      t.float :amount
      t.string :type
      t.bool :completed
      t.bool :successful
      t.date :timestart
      t.date :timeend

      t.integer :device_id
      t.integer :sensor_id

      t.timestamps
    end

    add_index :savingsgoals, :device_id
    add_index :savingsgoals, :sensor_id

  end

  def self.down
    drop_table :savingsgoals
  end
end
