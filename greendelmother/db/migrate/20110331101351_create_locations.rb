class CreateLocations < ActiveRecord::Migration
  def self.up
    create_table :locations do |t|
      t.string :name
      t.string :address
      t.string :town
      t.string :extaddress
      t.float :powerprice # eur / kWh
      t.float :xcoord
      t.float :ycoord

      t.integer :device_id

      t.timestamps
    end

    add_index :locations, :device_id

  end

  def self.down
    drop_table :locations
  end
end
