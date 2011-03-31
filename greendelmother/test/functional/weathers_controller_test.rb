require 'test_helper'

class WeathersControllerTest < ActionController::TestCase
  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:weathers)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create weather" do
    assert_difference('Weather.count') do
      post :create, :weather => { }
    end

    assert_redirected_to weather_path(assigns(:weather))
  end

  test "should show weather" do
    get :show, :id => weathers(:one).to_param
    assert_response :success
  end

  test "should get edit" do
    get :edit, :id => weathers(:one).to_param
    assert_response :success
  end

  test "should update weather" do
    put :update, :id => weathers(:one).to_param, :weather => { }
    assert_redirected_to weather_path(assigns(:weather))
  end

  test "should destroy weather" do
    assert_difference('Weather.count', -1) do
      delete :destroy, :id => weathers(:one).to_param
    end

    assert_redirected_to weathers_path
  end
end
