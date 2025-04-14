Feature: Submit form to company
    
Scenario Outline: Submit form
    Given I am on "<company>" form page
    When I input "<email>" in the email textbox
    And I select "<issue>" from the issue dropdown
    And I input "<text>" in the description textbox
    And I submit the form
    Then I should see the confirmation page
    
    Examples: 
   | company      | email               | issue      | text                        |
   | Testcompany  | TestCustomer1@email | Testcase 1 | issue customer 1, company 1 |
   | Testcompany2 | TestCustomer1@email | Testcase 3 | issue customer 1, company 2 |
   | Testcompany  | TestCustomer2@email | Testcase 2 | issue customer 2, company 1 |
   | Testcompany2 | TestCustomer3@email | Testcase 3 | issue customer 3, company 2 |

