using MediatR;
using Mc2.CrudTest.Application.Basic.Common;

namespace Mc2.CrudTest.Application.Customers.Queries.GetCustomers
{
    public class GetCustomersPagedQuery : IRequest<PagedResult<CustomerDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}