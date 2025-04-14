Feature: Loggin as csrep or admin

    Scenario Outline: loggin though form
        Given I am on the loggin page
        When I input "<email>" in the email textbox
        And I input "<password>" in the password textbox
        And I submit the form
        Then I should see the relevant page based on my "<role>"
    
        Examples: 
          | email                               | password | role  |
          | Testcompanyadmin1@legitemail.xyzzz  | 123      | admin |
          | Testcompanyadmin2@legitemail.xyzzz  | 123      | admin |
          | Testcompany1csrep1@legitemail.xyzzz | 123      | csrep |
          | Testcompany1csrep2@legitemail.xyzzz | 123      | csrep |
          | Testcompany2csrep1@legitemail.xyzzz | 123      | csrep |
          | Testcompany2csrep2@legitemail.xyzzz | 123      | csrep |