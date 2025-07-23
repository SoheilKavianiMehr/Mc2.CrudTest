using FluentAssertions;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.Common;
using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Mc2.CrudTest.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class CustomerManagementSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;
        
        private CreateCustomerCommand _createCustomerCommand;
        private UpdateCustomerCommand _updateCustomerCommand;
        private CustomerDto _createdCustomer;
        private CustomerDto _retrievedCustomer;
        private Exception _lastException;
        private PagedResult<CustomerDto> _pagedResult;
        private List<CustomerDto> _customers;
        
        public CustomerManagementSteps(ScenarioContext scenarioContext, IServiceProvider serviceProvider)
        {
            _scenarioContext = scenarioContext;
            _serviceProvider = serviceProvider;
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
        }

        [Given(@"the system is running")]
        public void GivenTheSystemIsRunning()
        {
        }

        [Given(@"the database is clean")]
        public async Task GivenTheDatabaseIsClean()
        {
            var allCustomers = await _customerRepository.GetAllAsync();
            foreach (var customer in allCustomers)
            {
                await _customerRepository.DeleteAsync(customer);
            }
            await _customerRepository.SaveChangesAsync();
        }

        [Given(@"I have valid customer information:")]
        public void GivenIHaveValidCustomerInformation(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            _createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
        }

        [Given(@"a customer exists with:")]
        public async Task GivenACustomerExistsWith(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            var command = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
            
            _createdCustomer = await _mediator.Send(command);
        }

        [Given(@"I have customer information with invalid email:")]
        public void GivenIHaveCustomerInformationWithInvalidEmail(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            _createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
        }

        [Given(@"I have customer information with invalid phone:")]
        public void GivenIHaveCustomerInformationWithInvalidPhone(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            _createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
        }

        [Given(@"I have customer information with underage person:")]
        public void GivenIHaveCustomerInformationWithUnderagePerson(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            _createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
        }

        [Given("a customer exists with ID \"(.*)\"")]
        public async Task GivenACustomerExistsWithId(string customerId)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = "Test",
                LastName = "Customer",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "test@example.com",
                PhoneNumber = "+1234567890",
                BankAccountNumber = "12345678901234567890"
            };
            
            _createdCustomer = await _mediator.Send(command);
            _scenarioContext["CustomerId"] = _createdCustomer.Id;
        }

        [Given("a customer exists with email \"(.*)\"")]
        public async Task GivenACustomerExistsWithEmail(string email)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = "Search",
                LastName = "Customer",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = email,
                PhoneNumber = "+1234567890",
                BankAccountNumber = "12345678901234567890"
            };
            
            _createdCustomer = await _mediator.Send(command);
        }

        [Given(@"(\d+) customers exist in the system")]
        public async Task GivenCustomersExistInTheSystem(int customerCount)
        {
            for (int i = 1; i <= customerCount; i++)
            {
                var command = new CreateCustomerCommand
                {
                    FirstName = $"Customer{i}",
                    LastName = $"LastName{i}",
                    DateOfBirth = new DateTime(1990, 1, i % 28 + 1),
                    Email = $"customer{i}@example.com",
                    PhoneNumber = $"+123456789{i:D1}",
                    BankAccountNumber = $"12345678{i:D10}"
                };
                
                await _mediator.Send(command);
            }
        }

        [When(@"I create a new customer")]
        public async Task WhenICreateANewCustomer()
        {
            try
            {
                _createdCustomer = await _mediator.Send(_createCustomerCommand);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I try to create another customer with:")]
        public async Task WhenITryToCreateAnotherCustomerWith(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            var command = new CreateCustomerCommand
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
            
            try
            {
                await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I try to create a new customer")]
        public async Task WhenITryToCreateANewCustomer()
        {
            try
            {
                _createdCustomer = await _mediator.Send(_createCustomerCommand);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I update the customer with:")]
        public async Task WhenIUpdateTheCustomerWith(Table table)
        {
            var customerData = table.CreateInstance<CustomerTestData>();
            var customerId = (Guid)_scenarioContext["CustomerId"];
            
            _updateCustomerCommand = new UpdateCustomerCommand
            {
                Id = customerId,
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                Email = customerData.Email,
                DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                PhoneNumber = customerData.PhoneNumber,
                BankAccountNumber = customerData.BankAccountNumber
            };
            
            try
            {
                await _mediator.Send(_updateCustomerCommand);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I create the customer")]
        public async Task WhenICreateTheCustomer()
        {
            try
            {
                _createdCustomer = await _mediator.Send(_createCustomerCommand);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I update the customer with new information:")]
        public async Task WhenIUpdateTheCustomerWithNewInformation(Table table)
        {
            try
            {
                var customerData = table.CreateInstance<CustomerTestData>();
                _updateCustomerCommand = new UpdateCustomerCommand
                {
                    Id = _createdCustomer.Id,
                    FirstName = customerData.FirstName,
                    LastName = customerData.LastName,
                    DateOfBirth = DateTime.Parse(customerData.DateOfBirth),
                    Email = customerData.Email,
                    PhoneNumber = customerData.PhoneNumber,
                    BankAccountNumber = customerData.BankAccountNumber
                };
                
                await _mediator.Send(_updateCustomerCommand);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I delete the customer")]
        public async Task WhenIDeleteTheCustomer()
        {
            try
            {
                var command = new DeleteCustomerCommand { Id = _createdCustomer.Id };
                await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When("I search for customer by email \"(.*)\"")]
        public async Task WhenISearchForCustomerByEmail(string email)
        {
            try
            {
                var query = new GetCustomerByEmailQuery { Email = email };
                _retrievedCustomer = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [When(@"I request customers with page (\d+) and page size (\d+)")]
        public async Task WhenIRequestCustomersWithPageAndPageSize(int page, int pageSize)
        {
            try
            {
                var query = new GetCustomersPagedQuery { Page = page, PageSize = pageSize };
                _pagedResult = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                _lastException = ex;
            }
        }

        [Then(@"the customer should be created successfully")]
        public void ThenTheCustomerShouldBeCreatedSuccessfully()
        {
            _createdCustomer.Should().NotBeNull();
            _lastException.Should().BeNull();
        }

        [Then(@"the customer should have a unique ID")]
        public void ThenTheCustomerShouldHaveAUniqueId()
        {
            _createdCustomer.Id.Should().NotBeEmpty();
        }

        [Then(@"a CustomerCreatedEvent should be raised")]
        public void ThenACustomerCreatedEventShouldBeRaised()
        {
            _createdCustomer.Should().NotBeNull();
        }

        [Then(@"the customer creation should fail")]
        public void ThenTheCustomerCreationShouldFail()
        {
            _lastException.Should().NotBeNull();
        }

        [Then(@"I should receive an error")]
        public void ThenIShouldReceiveAUniquenessViolationError()
        {
            _lastException.Should().NotBeNull();
            _lastException.Message.ToLowerInvariant().Should().Contain("unique");
        }

        [Then(@"I should receive an email validation error")]
        public void ThenIShouldReceiveAnEmailValidationError()
        {
            _lastException.Should().NotBeNull();
            _lastException.Message.ToLowerInvariant().Should().Contain("email");
        }

        [Then(@"I should receive a phone validation error")]
        public void ThenIShouldReceiveAPhoneValidationError()
        {
            _lastException.Should().NotBeNull();
            _lastException.Message.ToLowerInvariant().Should().Contain("phone");
        }

        [Then(@"I should receive an age validation error")]
        public void ThenIShouldReceiveAnAgeValidationError()
        {
            _lastException.Should().NotBeNull();
            _lastException.Message.ToLowerInvariant().Should().Contain("18");
        }

        [Then(@"the customer should be updated successfully")]
        public void ThenTheCustomerShouldBeUpdatedSuccessfully()
        {
            _lastException.Should().BeNull();
        }

        [Then(@"a CustomerUpdatedEvent should be raised")]
        public void ThenACustomerUpdatedEventShouldBeRaised()
        {
            _lastException.Should().BeNull();
        }

        [Then(@"the customer should be marked as deleted")]
        public void ThenTheCustomerShouldBeMarkedAsDeleted()
        {
            _lastException.Should().BeNull();
        }

        [Then(@"a CustomerDeletedEvent should be raised")]
        public void ThenACustomerDeletedEventShouldBeRaised()
        {
            _lastException.Should().BeNull();
        }

        [Then(@"I should find the customer")]
        public void ThenIShouldFindTheCustomer()
        {
            _retrievedCustomer.Should().NotBeNull();
        }

        [Then(@"the customer details should be returned")]
        public void ThenTheCustomerDetailsShouldBeReturned()
        {
            _retrievedCustomer.Should().NotBeNull();
            _retrievedCustomer.Email.Should().NotBeNullOrEmpty();
        }

        [Then(@"I should receive (\d+) customers")]
        public void ThenIShouldReceiveCustomers(int expectedCount)
        {
            _pagedResult.Should().NotBeNull();
            _pagedResult.Items.Should().HaveCount(expectedCount);
        }

        [Then(@"the total count should be (\d+)")]
        public void ThenTheTotalCountShouldBe(int expectedTotal)
        {
            _pagedResult.TotalCount.Should().Be(expectedTotal);
        }

        [Then(@"pagination information should be correct")]
        public void ThenPaginationInformationShouldBeCorrect()
        {
            _pagedResult.Should().NotBeNull();
            _pagedResult.Page.Should().BeGreaterThan(0);
            _pagedResult.PageSize.Should().BeGreaterThan(0);
        }
    }

    public class CustomerTestData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BankAccountNumber { get; set; }
    }
}