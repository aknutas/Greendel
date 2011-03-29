# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_greendelmother2_session',
  :secret      => '654ffeccb0f242b65274e3b20c0e78bdaee45dde5bd83b82bb72fbb93b6e3e2b3e7be5d7c984cc95b888be79a07f19705c9f0df66570cc3ba3e8d79ab831a81c'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
