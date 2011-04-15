# Methods added to this helper will be available to all templates in the application.
module ApplicationHelper

  def try_print(testObject)
    unless testObject
      return h("-")
    else
      return h(testObject)
    end
  end

  def plot_helper(readings)
    string = ""
    readings.each do |reading|
      string = string + "[Date.UTC(" + reading.time.strftime("%Y,%m,%d,%H,%M,%S") + "), " + reading.value.to_s + "],"
    end
    return string
  end

end
