Feature: Manage employees
    
Scenario: Add employee
    Given I am logged in with my credentials, "Testcompanyadmin1@legitemail.xyzzz", "123"
    And I click on the employees button
    When I see the employee site
    And I click the add employee button and see the popup
    And I enter their email "TestemployeeToAdd@legitemail.xyzzz"
    And I submit the form
    Then I should see their email "TestemployeeToAdd@legitemail.xyzzz" as a new employee
    #Possible to add several employees with same mail
    
Scenario: Remove employee
    Given I am logged in with my credentials, "Testcompanyadmin1@legitemail.xyzzz", "123"
    And I click on the employees button
    When I see the employee site
    And I click the add employee button and see the popup
    And I enter their email "TestemployeeToRemove@legitemail.xyzzz"
    And I submit the form
    And I see their email "TestemployeeToRemove@legitemail.xyzzz" as a new employee
    And I click on the remove coworker button for "TestemployeeToRemove@legitemail.xyzzz"
    Then I should not see the coworkers email "TestemployeeToRemove@legitemail.xyzzz"
    
    
    