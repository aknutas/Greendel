class CreateUsers < ActiveRecord::Migration
  def self.up
    create_table :users do |t|
      t.string :name
      t.string :hashed_password
      t.string :salt
      t.string :email
      t.string :realname

      t.timestamp :lastlogin

      t.timestamps
    end

    add_index :users, :name

  end

  def self.down
    drop_table :users
  end
end
