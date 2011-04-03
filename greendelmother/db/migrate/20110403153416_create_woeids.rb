class CreateWoeids < ActiveRecord::Migration
  def self.up
    create_table :woeids do |t|
      t.string :location
      t.string :woeid

      t.timestamps
    end

    add_index :woeids, :location

  end

  def self.down
    drop_table :woeids
  end
end
