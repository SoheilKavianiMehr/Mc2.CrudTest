using MediatR;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;
using Mc2.CrudTest.Domain.Customers.Entities;
using Mc2.CrudTest.Domain.Customers;

namespace Mc2.CrudTest.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingCustomerByEmail = await _customerRepository.GetByEmailAsync(request.Email);
        if (existingCustomerByEmail != null)
        {
            throw new ArgumentException("Email must be unique", nameof(request.Email));
        }

        var existingCustomerByNameAndDob = await _customerRepository.GetByNameAndDateOfBirthAsync(
            request.FirstName, request.LastName, request.DateOfBirth);
        if (existingCustomerByNameAndDob != null)
        {
            throw new ArgumentException("Customer with the same first name, last name, and date of birth already exists");
        }

        var customer = Customer.Create(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.PhoneNumber,
            request.BankAccountNumber);

        var createdCustomer = await _customerRepository.CreateAsync(customer);

        return new CustomerDto
        {
            Id = createdCustomer.Id,
            FirstName = createdCustomer.FirstName,
            LastName = createdCustomer.LastName,
            DateOfBirth = createdCustomer.DateOfBirth,
            Email = createdCustomer.Email.Value,
            PhoneNumber = createdCustomer.PhoneNumber.Value,
            BankAccountNumber = createdCustomer.BankAccountNumber
        };
    }
}