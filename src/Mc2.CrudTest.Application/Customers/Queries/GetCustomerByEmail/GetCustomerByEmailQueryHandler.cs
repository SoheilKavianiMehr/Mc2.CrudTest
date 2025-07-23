using MediatR;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;
using Mc2.CrudTest.Domain.Customers;

namespace Mc2.CrudTest.Application.Customers.Queries.GetCustomerByEmail;

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
            BankAccountNumber = customer.BankAccountNumber
        };
    }
}