Feature: Users

Scenario: User is created
	When a user is created with first name "first" and last name "last"
	Then the user should be created with the following properties
    | Id | FirstName | LastName |
    | 1  | first     | last     |

Scenario: User is created with validation error
	When a user is created with first name " " and last name "last"
    Then an error with type "Validation" should be returned