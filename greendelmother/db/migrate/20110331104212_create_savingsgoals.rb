class CreateSavingsgoals < ActiveRecord::Migration
  def self.up
    create_table :savingsgoals do |t|
      t.float :amount
      t.string :type
      t.date :timestart
      t.date :timeend
      t.integer :device_id
      t.integer :sensor_id

      t.timestamps
    end
  end

  def self.down
    drop_table :savingsgoals
  end
end
