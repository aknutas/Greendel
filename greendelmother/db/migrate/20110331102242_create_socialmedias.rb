class CreateSocialmedias < ActiveRecord::Migration
  def self.up
    create_table :socialmedias do |t|
      t.boolean :status, :default => false
      t.boolean :facebookon, :default => false
      t.boolean :twitteron, :default => false
      t.string :fb_un
      t.string :twitter_un
      t.string :fbauth
      t.string :access_token
      t.string :access_secret

      t.integer :user_id

      t.timestamps
    end

    add_index :socialmedias, :user_id

  end

  def self.down
    drop_table :socialmedias
  end
end
