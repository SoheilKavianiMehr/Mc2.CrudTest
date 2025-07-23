using MediatR;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;

namespace Mc2.CrudTest.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
    }
}