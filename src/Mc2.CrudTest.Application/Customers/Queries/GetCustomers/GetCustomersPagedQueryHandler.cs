using MediatR;
using Mc2.CrudTest.Application.Basic.Common;
using Mc2.CrudTest.Domain.Customers;

namespace Mc2.CrudTest.Application.Customers.Queries.GetCustomers;

public class GetCustomersPagedQueryHandler : IRequestHandler<GetCustomersPagedQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersPagedQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<CustomerDto>> Handle(GetCustomersPagedQuery request, CancellationToken cancellationToken)
    {
        var (customers, totalCount) = await _customerRepository.GetPagedAsync(request.Page, request.PageSize);

        var customerDtos = customers.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            DateOfBirth = customer.DateOfBirth,
            Email = customer.Email.Value,
            PhoneNumber = customer.PhoneNumber.Value,
            BankAccountNumber = customer.BankAccountNumber
        });

        return new PagedResult<CustomerDto>
        {
            Items = customerDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}