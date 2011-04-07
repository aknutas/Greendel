class CreateWeathers < ActiveRecord::Migration
  def self.up
    create_table :weathers do |t|
      t.float :temp
      t.float :high
      t.float :low
      t.string :desc
      t.string :source
      t.string :woeid
      t.binary :yweather

      t.integer :location_id

      t.timestamps
    end

    add_index :weathers, :location_id

  end

  def self.down
    drop_table :weathers
  end
end
