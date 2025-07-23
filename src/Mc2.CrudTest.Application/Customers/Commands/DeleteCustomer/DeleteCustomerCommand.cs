using MediatR;

namespace Mc2.CrudTest.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}