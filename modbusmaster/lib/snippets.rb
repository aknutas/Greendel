# Serial IO (old method, don't use)
# cl = ModBus::RTUClient.new('/dev/ttyUSB0', 9600, 1, {:data_bits => 8, :stop_bits => 1, :parity => SerialPort::NONE})

# Establishing the Modbus over TCP connection
# cl = ModBus::TCPClient.new('192.168.1.224', 502, 255)


#Read four registers from address 0
#The right spot for the address data
=begin
puts "Reading registers"
puts cl.read_holding_registers(34, 4)
=end

=begin
while(!ended) do
  puts "MENU"
  puts "1. Start simulating"
  puts "2. Read from actual source"
  puts "3. Quit"
  puts "X. Stop loops"
  puts "Y. Get list of remote devices"
  puts "Z. Start reading and submitting"
  puts "S. Start simulating and submitting"
  choice = gets.chomp
  case choice
  when "1"
    simulator = Simulator.new
    simulate = true
    Thread.new() {
      while(simulate) do
        devicearray.each do |onedevice|
          onedevice.sensors.each do |onesensor|
            reading = Reading.new
            reading.value = simulator.gimmerandom
            reading.time = Time.now
            onesensor.readings << reading
            onesensor.save
          end
        end
        sleep 10
      end
    }
  when "S"
    simulator = Simulator.new
    simulate = true
    Thread.new() {
      while(simulate) do
        devicearray.each do |onedevice|
          onedevice.sensors.each do |onesensor|
            reading = Reading.new
            reading.value = simulator.gimmerandom
            reading.time = Time.now
            onesensor.readings << reading
            onesensor.save
          end
        end
        sleep 10
      end
    }
  when "2"
    read = true
    Thread.new() {
      puts "Connecting.."
      cl = ModBus::TCPClient.new('192.168.1.224', 502, 255)
      puts "Connected!"

      while read do
        puts "Reading registers"
        values = cl.read_holding_registers(34, 4)
        puts values
        puts "Register 1:" + values[0].to_s
        puts "Register 2:" + values[1].to_s
        puts "Register 3:" + values[2].to_s
        puts "Register 4:" + values[3].to_s
        sleep(1)
      end
      cl.close
      cl = nil
    }
  when "3"
    simulate = false
    ended = true
  when "X"
    simulate = false
    read = false
  when "Y"
    resourcer = Resourcer.new()
    resourcer.printDevices
  when "Z"
    read = true
    res = Resourcer.new()

    Thread.new() {
      puts "Connecting.."
      cl = ModBus::TCPClient.new('192.168.1.224', 502, 255)
      puts "Connected!"

      while read do
        puts "Reading registers"
        values = cl.read_holding_registers(34, 4)
        puts "Register 1:" + values[0].to_s
        puts "Register 2:" + values[1].to_s
        puts "Register 3:" + values[2].to_s
        puts "Register 4:" + values[3].to_s
        puts "Submitting register..."
        res.submitReading(values[1])
        puts "Submitted!"
        sleep(1)
      end
      cl.close
      cl = nil
    }
  end
end
=end