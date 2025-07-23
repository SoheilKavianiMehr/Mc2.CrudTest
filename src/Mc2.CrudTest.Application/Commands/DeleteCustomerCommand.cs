using MediatR;

namespace Mc2.CrudTest.Application.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}