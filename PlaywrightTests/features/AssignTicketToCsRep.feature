Feature: Assigning ticket to Csrep
    
Scenario: Assign ticket
    Given I am logged in with my credentials, "Testcompany1csrep1@legitemail.xyzzz", "123"
    And I have submitted a form, with relevant info, "Testcompany", "TestCustomer1@email", "9" and "issue customer 1, company 1, ticket to assign to csrep"
    When I click on the chat I want to assign myself and I'm sent to the chatpage
    And I go back to the arbetarsida
    Then I should not see my assigned chat in the list
    And I should see my assigned chat with my email, "Testcompany1csrep1@legitemail.xyzzz", if i check the show all chats checkbox
    