using MediatR;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;
using Mc2.CrudTest.Domain.Customers;

namespace Mc2.CrudTest.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {request.Id} not found.");
        }

        var existingCustomerByEmail = await _customerRepository.GetByEmailAsync(request.Email);
        if (existingCustomerByEmail != null && existingCustomerByEmail.Id != request.Id)
        {
            throw new ArgumentException("Email must be unique", nameof(request.Email));
        }

        var existingCustomerByNameAndDob = await _customerRepository.GetByNameAndDateOfBirthAsync(
            request.FirstName, request.LastName, request.DateOfBirth);
        if (existingCustomerByNameAndDob != null && existingCustomerByNameAndDob.Id != request.Id)
        {
            throw new ArgumentException("Customer with the same first name, last name, and date of birth already exists");
        }

        customer.Update(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.PhoneNumber,
            request.BankAccountNumber);

        var updatedCustomer = await _customerRepository.UpdateAsync(customer);

        return new CustomerDto
        {
            Id = updatedCustomer.Id,
            FirstName = updatedCustomer.FirstName,
            LastName = updatedCustomer.LastName,
            DateOfBirth = updatedCustomer.DateOfBirth,
            Email = updatedCustomer.Email.Value,
            PhoneNumber = updatedCustomer.PhoneNumber.Value,
            BankAccountNumber = updatedCustomer.BankAccountNumber
        };
    }
}