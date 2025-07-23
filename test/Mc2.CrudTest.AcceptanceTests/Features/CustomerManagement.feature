Feature: Customer Management
  As a system administrator
  I want to manage customer information

  Background:
    Given the system is running
    And the database is clean

  Scenario: Create a new customer with valid information
    Given I have valid customer information:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789     |
    When I create a new customer
    Then the customer should be created successfully
    And the customer should have a unique ID
    And a CustomerCreatedEvent should be raised

  Scenario: Prevent duplicate customers based on uniqueness rule
    Given a customer exists with:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789      |
    When I try to create another customer with:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789      |
    Then the customer creation should fail
    And I should receive an error

  Scenario: Prevent duplicate customers based on Email
    Given a customer exists with:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789     |
    When I try to create another customer with:
      | Field            | Value                    |
      | FirstName        | ali                      |
      | LastName         | kmm                      |
      | DateOfBirth      | 2001-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789      |
    Then the customer creation should fail
    And I should receive an error

  Scenario: Validate email format
    Given I have customer information with invalid email:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | invalid-email            |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 123456789012345678       |
    When I try to create a new customer
    Then the customer creation should fail
    And I should receive an email validation error

  Scenario: Validate mobile phone number
    Given I have customer information with invalid phone:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2000-01-15               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | 123                      |
      | BankAccountNumber| 1234567890123456789      |
    When I try to create a new customer
    Then the customer creation should fail
    And I should receive a phone validation error

  Scenario: Validate minimum age requirement
    Given I have customer information with underage person:
      | Field            | Value                    |
      | FirstName        | soheil                   |
      | LastName         | km                       |
      | DateOfBirth      | 2010-01-01               |
      | Email            | soheil79km@hotmail.com   |
      | PhoneNumber      | +989383623312            | 
      | BankAccountNumber| 1234567890123456789      |
    When I try to create a new customer
    Then the customer creation should fail
    And I should receive an age validation error

  Scenario: Update existing customer
    Given a customer exists with ID "550e8400-e29b-41d4-a716-446655440000"
    When I update the customer with:
      | Field            | Value                    |
      | FirstName        | Updatedsoheil            |
      | LastName         | Updatedkm                |
      | DateOfBirth      | 2000-01-01               |
      | Email            | updated@hotmail.com      |
      | PhoneNumber      | +989383623312            |
      | BankAccountNumber| 1234567890123456789      |
    Then the customer should be updated successfully
    And a CustomerUpdatedEvent should be raised

  Scenario: Delete existing customer
    Given a customer exists with ID "550e8400-e29b-41d4-a716-446655440000"
    When I delete the customer
    Then the customer should be marked as deleted
    And a CustomerDeletedEvent should be raised

  Scenario: Retrieve customer by email
    Given a customer exists with email "soheil79km@hotmail.com"
    When I search for customer by email "soheil79km@hotmail.com"
    Then I should find the customer
    And the customer details should be returned

  Scenario: Get all customers with pagination
    Given 15 customers exist in the system
    When I request customers with page 1 and page size 10
    Then I should receive 10 customers
    And the total count should be 15
    And pagination information should be correct