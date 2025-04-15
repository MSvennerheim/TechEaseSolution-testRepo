Feature: send answer to a chat

Scenario: Answer as customer
    Given I have submitted a form, with relevant info, "Testcompany", "TestCustomer1@email", "9" and "issue customer 1, company 1, answer my own chat" 
    And I am on my chatpage 
    When I input "here's the answer from customer" in the textbox
    And I submit the form
    Then I should see my chat with my submitted "here's the answer from customer"
    
Scenario: Answer as csRep with Send button
    Given I have submitted a form, with relevant info, "Testcompany", "TestCustomer1@email", "9" and "issue customer 1, company 1, answer from csrep"
    And I am logged in with my credentials, "Testcompany1csrep1@legitemail.xyzzz", "123"
    And clicked the button to get to their chat
    When I input "here's the answer from csrep" in the textbox
    And I submit the form with the send button
    Then I should see my chat with my submitted "here's the answer from csrep"

Scenario: Answer as csRep with Send and take next open ticket
    Given I have submitted a form, with relevant info, "Testcompany", "TestCustomer1@email", "9" and "issue customer 1, company 1, answer from csrep skip to next ticket"
    And I am logged in with my credentials, "Testcompany1csrep2@legitemail.xyzzz", "123"
    And clicked the button to get to their chat
    When I input "here's the answer from csrep, i will leave now" in the textbox
    And I submit the form with the Send and take next open ticket button
    Then I should be redirected to a new chat or to the arbetarsida