class CreateSocialmedias < ActiveRecord::Migration
  def self.up
    create_table :socialmedias do |t|
      t.boolean :status
      t.boolean :facebookon
      t.boolean :twitteron
      t.string :fbun
      t.string :twitterun
      t.string :fbauth
      t.string :twitterauth

      t.integer :user_id

      t.timestamps
    end

    add_index :socialmedias, :user_id

  end

  def self.down
    drop_table :socialmedias
  end
end
