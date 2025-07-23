using MediatR;
using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Application.Common;

namespace Mc2.CrudTest.Application.Queries
{
    public class GetCustomersPagedQuery : IRequest<PagedResult<CustomerDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}