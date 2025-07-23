using MediatR;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Domain.Repositories;

namespace Mc2.CrudTest.Application.Handlers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {request.Id} not found.");
        }

        customer.Delete();
        await _customerRepository.UpdateAsync(customer);
    }
}