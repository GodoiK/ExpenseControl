using ExpenseControl.Application.Dtos.User;
using ExpenseControl.Application.UseCase.Users;
using ExpenseControl.Application.Usecases.Users;
using ExpenseControl.Application.UseCases.FinancialRecord;
using ExpenseControl.Application.UseCases.FinancialRecords;
using ExpenseControl.Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateUserUseCase useCase,
        [FromBody] CreateUserRequest request)
    {
        var result = await useCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllUsersUseCase useCase)
    {
        return Ok(await useCase.ExecuteAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, GetUserByIdUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteUserUseCase useCase)
    {
        await useCase.ExecuteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromServices] UpdateUserUseCase useCase,
        [FromBody] UpdateUserRequest request)
    {
        var result = await useCase.ExecuteAsync(id, request);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromServices] GetUserSummaryUseCase useCase)
    {
        return Ok(await useCase.ExecuteAsync());
    }

    [HttpGet("{id}/financial-records")]
    public async Task<IActionResult> GetFinancialRecordsByUser(
        Guid id,
        [FromServices] GetFinancialRecordsByUserUseCase useCase)
    {
        return Ok(await useCase.ExecuteAsync(id));
    }

    [HttpPost("{id}/financial-records")]
    public async Task<IActionResult> CreateFinancialRecordForUser(
        Guid id,
        [FromServices] CreateFinancialRecordUseCase useCase,
        [FromBody] ExpenseControl.Application.Dtos.FinancialRecord.CreateFinancialRecordRequest request)
    {
        request.UserId = id;
        await useCase.ExecuteAsync(request);
        return StatusCode(201);
    }
}