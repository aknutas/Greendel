def logtext(message)
  message = Time.now.to_s + ": " + message
  puts message
  if $options[:logfile]
    begin
      open('modbusmaster.log', 'a') do |f|
        f.puts message
      end
    rescue
    end
  end
end