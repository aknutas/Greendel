# Methods added to this helper will be available to all templates in the application.
module ApplicationHelper

  def try_print(testObject)
    unless testObject
      return h("-")
    else
      return h(testObject)
    end
  end

end
