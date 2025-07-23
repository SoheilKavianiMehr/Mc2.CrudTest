using MediatR;
using Mc2.CrudTest.Application.DTOs;

namespace Mc2.CrudTest.Application.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
    }
}