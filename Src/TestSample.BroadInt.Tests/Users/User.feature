Feature: Users

Scenario: User is created
	When a user is created with first name "first" and last name "last"
	Then the a user is returned with the following properties
    | FirstName | LastName |
    | first     | last     |

Scenario: Get a user by first and last name
    Given a user exists with first name "first" and last name "last"
	When a user is retrieved with first name "first" and last name "last"
	Then the a user is returned with the following properties
    | FirstName | LastName |
    | first     | last     |

Scenario: User is created with validation error
	When a user is created with first name " " and last name "last"
    Then an error with status code "InvalidArgument" should be returned
    And an error with message "First and Last name cannot be empty." should be returned