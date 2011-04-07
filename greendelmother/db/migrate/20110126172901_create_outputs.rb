class CreateOutputs < ActiveRecord::Migration
  def self.up
    create_table :outputs do |t|
      t.string :name
      t.string :longname
      t.boolean :state
      t.boolean :haschanged

      t.integer :device_id

      t.timestamps
    end

    add_index :outputs, :device_id
    
  end


  def self.down
    drop_table :outputs
  end
end
