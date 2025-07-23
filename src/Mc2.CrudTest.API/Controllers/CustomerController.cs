using Mc2.CrudTest.Application.Basic.Common;
using Mc2.CrudTest.Application.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Application.Customers.Commands.DeleteCustomer;
using Mc2.CrudTest.Application.Customers.Commands.UpdateCustomer;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomerByEmail;
using Mc2.CrudTest.Application.Customers.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerDto>>> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetCustomersPagedQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
    {
        try
        {
            var query = new GetCustomerByEmailQuery { Email = email };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = $"Customer with email '{email}' not found." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetCustomerByEmail),
                new { email = result.Email },
                result);
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerCommand command)
    {
        try
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        try
        {
            var command = new DeleteCustomerCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}