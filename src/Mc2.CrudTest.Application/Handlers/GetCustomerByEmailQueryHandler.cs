using MediatR;
using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Domain.Repositories;

namespace Mc2.CrudTest.Application.Handlers;

public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(request.Email);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with email {request.Email} not found.");
        }

        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            DateOfBirth = customer.DateOfBirth,
            Email = customer.Email.Value,
            PhoneNumber = customer.PhoneNumber.Value,
            BankAccountNumber = customer.BankAccountNumber,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            IsDeleted = customer.IsDeleted
        };
    }
}