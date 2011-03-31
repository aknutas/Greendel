class CreateWeathers < ActiveRecord::Migration
  def self.up
    create_table :weathers do |t|
      t.float :temp
      t.string :desc
      t.string :source

      t.integer :location_id

      t.timestamps
    end

    add_index :weathers, :location_id

  end

  def self.down
    drop_table :weathers
  end
end
