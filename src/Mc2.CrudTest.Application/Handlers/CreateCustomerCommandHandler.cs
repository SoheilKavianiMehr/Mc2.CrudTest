using MediatR;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;

namespace Mc2.CrudTest.Application.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.PhoneNumber,
            request.BankAccountNumber);

        if (await _customerRepository.IsExist(customer))
        {
            throw new ArgumentException("User sould be unique", customer.UniquenessKey);
        }

        var createdCustomer = await _customerRepository.CreateAsync(customer);

        return new CustomerDto
        {
            Id = createdCustomer.Id,
            FirstName = createdCustomer.FirstName,
            LastName = createdCustomer.LastName,
            DateOfBirth = createdCustomer.DateOfBirth,
            Email = createdCustomer.Email.Value,
            PhoneNumber = createdCustomer.PhoneNumber.Value,
            BankAccountNumber = createdCustomer.BankAccountNumber,
            CreatedAt = createdCustomer.CreatedAt,
            UpdatedAt = createdCustomer.UpdatedAt,
            IsDeleted = createdCustomer.IsDeleted
        };
    }
}