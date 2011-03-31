require 'test_helper'

class SavingsgoalsControllerTest < ActionController::TestCase
  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:savingsgoals)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create savingsgoal" do
    assert_difference('Savingsgoal.count') do
      post :create, :savingsgoal => { }
    end

    assert_redirected_to savingsgoal_path(assigns(:savingsgoal))
  end

  test "should show savingsgoal" do
    get :show, :id => savingsgoals(:one).to_param
    assert_response :success
  end

  test "should get edit" do
    get :edit, :id => savingsgoals(:one).to_param
    assert_response :success
  end

  test "should update savingsgoal" do
    put :update, :id => savingsgoals(:one).to_param, :savingsgoal => { }
    assert_redirected_to savingsgoal_path(assigns(:savingsgoal))
  end

  test "should destroy savingsgoal" do
    assert_difference('Savingsgoal.count', -1) do
      delete :destroy, :id => savingsgoals(:one).to_param
    end

    assert_redirected_to savingsgoals_path
  end
end
