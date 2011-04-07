#Custom functions
def round_to(f, d)
  (f * 10**d).round.to_f / 10**d
end

#Parsing modbus values
def parse(sensor, value)
  if(sensor.ntype == "ushort")
    value = (value * sensor.scalefactor)
    value = round_to(value, 2)
    return value
  elsif(sensor.ntype == "short")
    if value > 32767
      return (value - 65536)
    else
      return value
    end
  end
end