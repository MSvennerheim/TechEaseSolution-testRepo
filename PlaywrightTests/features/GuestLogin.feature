
Feature: access chat as customer
    
Scenario Outline: login and access chat
    Given I have submitted a form, with relevant info, "<company>", "<email>", "<issue>" and "<text>" 
    When I click on the loggin link
    Then I should see my chat with my submitted "<text>"
    
    Examples: 
      | company      | email               | issue      | text                                |
      | Testcompany  | TestCustomer1@email | 9 | issue customer 1, company 1, login |
      | Testcompany2 | TestCustomer1@email | 11 | issue customer 1, company 2, login |
      | Testcompany  | TestCustomer2@email | 10 | issue customer 2, company 1, login |
      | Testcompany2 | TestCustomer3@email | 11 | issue customer 3, company 2, login |
