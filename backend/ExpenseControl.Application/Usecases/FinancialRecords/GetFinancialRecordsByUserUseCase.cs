using ExpenseControl.Application.Dtos.FinancialRecord;
using ExpenseControl.Application.Interfaces.Repositories;

namespace ExpenseControl.Application.UseCases.FinancialRecords;

public class GetFinancialRecordsByUserUseCase
{
    private readonly IFinancialRecordRepository _repository;

    public GetFinancialRecordsByUserUseCase(IFinancialRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FinancialRecordResponse>> ExecuteAsync(Guid userId)
    {
        var records = await _repository.GetAllByUserIdAsync(userId);
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
