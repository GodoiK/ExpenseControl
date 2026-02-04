using ExpenseControl.Application.Dtos.FinancialRecord;
using ExpenseControl.Application.Interfaces.Repositories;

namespace ExpenseControl.Application.Usecases.FinancialRecords;

public class GetAllFinancialRecordsUseCase
{
    private readonly IFinancialRecordRepository _repository;

    public GetAllFinancialRecordsUseCase(IFinancialRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FinancialRecordResponse>> ExecuteAsync()
    {
        var records = await _repository.GetAllAsync();
        return records.Select(r => new FinancialRecordResponse(
            r.Id,
            r.Description,
            r.Amount,
            (int)r.Type,
            r.CategoryId,
            r.Category.Description,
            r.UserId,
            r.User.Name,
            r.CreatedAt
        ));
    }
}
