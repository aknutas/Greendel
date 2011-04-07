class CreateHistories < ActiveRecord::Migration
  def self.up
    create_table :histories do |t|
      t.string :desc
      t.float :temp
      t.float :low
      t.float :high
      t.date :fday
      t.blob :yweather
      t.integer :weather_id

      t.timestamps
    end

    add_index :histories, :weather_id
    add_index :histories, :fday

  end

  def self.down
    drop_table :histories
  end
end
