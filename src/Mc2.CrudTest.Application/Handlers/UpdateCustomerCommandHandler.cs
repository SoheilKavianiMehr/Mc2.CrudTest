using MediatR;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Domain.Repositories;

namespace Mc2.CrudTest.Application.Handlers;

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
            BankAccountNumber = updatedCustomer.BankAccountNumber,
            CreatedAt = updatedCustomer.CreatedAt,
            UpdatedAt = updatedCustomer.UpdatedAt,
            IsDeleted = updatedCustomer.IsDeleted
        };
    }
}