require 'test_helper'

class SocialmediasControllerTest < ActionController::TestCase
  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:socialmedias)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create socialmedia" do
    assert_difference('Socialmedia.count') do
      post :create, :socialmedia => { }
    end

    assert_redirected_to socialmedia_path(assigns(:socialmedia))
  end

  test "should show socialmedia" do
    get :show, :id => socialmedias(:one).to_param
    assert_response :success
  end

  test "should get edit" do
    get :edit, :id => socialmedias(:one).to_param
    assert_response :success
  end

  test "should update socialmedia" do
    put :update, :id => socialmedias(:one).to_param, :socialmedia => { }
    assert_redirected_to socialmedia_path(assigns(:socialmedia))
  end

  test "should destroy socialmedia" do
    assert_difference('Socialmedia.count', -1) do
      delete :destroy, :id => socialmedias(:one).to_param
    end

    assert_redirected_to socialmedias_path
  end
end
