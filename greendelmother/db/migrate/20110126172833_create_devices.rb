class CreateDevices < ActiveRecord::Migration
  def self.up
    create_table :devices do |t|
      t.string :name

      t.integer :user_id

      t.timestamps
    end

    add_index :devices, :user_id

  end

  def self.down
    drop_table :devices
  end
end
