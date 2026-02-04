using ExpenseControl.Application.Dtos;
using ExpenseControl.Application.Dtos.FinancialRecord;
using ExpenseControl.Application.Usecases.FinancialRecords;
using ExpenseControl.Application.UseCases;
using ExpenseControl.Application.UseCases.FinancialRecord;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/financial-records")]
public class FinancialRecordController : ControllerBase
{
    [HttpPost] 
    public async Task<IActionResult> Create(
        [FromServices] CreateFinancialRecordUseCase useCase,
        [FromBody] CreateFinancialRecordRequest request)
    {
        await useCase.ExecuteAsync(request);
        return StatusCode(201);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
            [FromServices] GetAllFinancialRecordsUseCase useCase)
    {
        var result = await useCase.ExecuteAsync();
        return Ok(result);
    }
}