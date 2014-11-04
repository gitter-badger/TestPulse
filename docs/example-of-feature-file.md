Feature: Login
In order to secure my website,
as a business owner,
I want to implement login functionality
 
Background: Given I am on login page
 
@wip @smoke @browser @ci
Scenario Outline: Check user's login credentials
Given I filled <username> <email> and <password>
When I clicked on Login
Then I should see <success or failure> message a
And I should get an email saying: "Dear User, You have just tried to login to our system. If your login is successful then you will get another email to verify you details. Thanks"

Examples: Valid login
|username    |email          |password |success or failure |
|Andy        |andy@gmail.com |abcd1234 |success            |
|Mike        |mike@gmail.com |abcd1234 |success            |

Examples: InValid login
|username    |email          |password         |success or failure |
|Andy        |andy@gmail.com|                  |failure            |

References:
http://shashikantjagtap.net/take-good-care-of-given-when-then-in-the-bdd/