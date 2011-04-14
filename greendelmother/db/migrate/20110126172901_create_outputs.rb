class CreateOutputs < ActiveRecord::Migration
  def self.up
    create_table :outputs do |t|
      t.string :name
      t.string :longname
      t.boolean :state
      t.boolean :haschanged
      t.integer :priority, :default => 0 # Power adjustment priority (zero to five)
      t.integer :estcost, :default => 0 # Estimated power cost/benefit
      t.boolean :cost # True for cost, false for benefit
      t.boolean :autoswitchable, :default => false # Machine switchable?

      t.integer :device_id

      t.timestamps
    end

    add_index :outputs, :device_id
    
  end


  def self.down
    drop_table :outputs
  end
end
