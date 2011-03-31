class CreateWeathers < ActiveRecord::Migration
  def self.up
    create_table :weathers do |t|
      t.float :temp
      t.string :desc
      t.string :source

      t.timestamps
    end
  end

  def self.down
    drop_table :weathers
  end
end
