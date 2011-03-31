class CreateSocialmedias < ActiveRecord::Migration
  def self.up
    create_table :socialmedias do |t|
      t.bool :status
      t.bool :facebook
      t.bool :twitter
      t.string :fbun
      t.string :twitterun

      t.integer :user_id

      t.timestamps
    end

    add_index :socialmedias, :user_id

  end

  def self.down
    drop_table :socialmedias
  end
end
