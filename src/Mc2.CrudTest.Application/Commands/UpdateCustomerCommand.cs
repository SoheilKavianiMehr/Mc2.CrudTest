using MediatR;
using Mc2.CrudTest.Application.DTOs;

namespace Mc2.CrudTest.Application.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
    }
}