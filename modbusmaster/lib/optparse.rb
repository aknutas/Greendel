#Reading CLI parameters
$options = {}

optparse = OptionParser.new do|opts|
  # Set a banner, displayed at the top
  # of the help screen.
  opts.banner = "Usage: main.rb [options]"

  # Define the options, and what they do
  $options[:simulate] = false
  opts.on( '-s', '--simulate', 'Simulated output' ) do
    $options[:simulate] = true
  end

  $options[:logfile] = nil
  opts.on( '-l', '--logfile FILE', 'Write log to FILE' ) do |file|
    $options[:logfile] = file
  end

  # This displays the help screen, all programs are
  # assumed to have this option.
  opts.on( '-h', '--help', 'Display this screen' ) do
    puts opts
    exit
  end
end

# Parse the command-line.
optparse.parse!