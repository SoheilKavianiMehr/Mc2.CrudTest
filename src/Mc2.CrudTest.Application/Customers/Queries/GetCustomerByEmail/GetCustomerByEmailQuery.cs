using MediatR;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;

namespace Mc2.CrudTest.Application.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQuery : IRequest<CustomerDto>
    {
        public string Email { get; set; } = string.Empty;
    }
}