using ExpenseControl.Application.Dtos.Category;
using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.Usecases.Category;

public class GetCategorySummaryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFinancialRecordRepository _financialRepository;

    public GetCategorySummaryUseCase(
        ICategoryRepository categoryRepository,
        IFinancialRecordRepository financialRepository)
    {
        _categoryRepository = categoryRepository;
        _financialRepository = financialRepository;
    }

    public async Task<CategorySummaryListResponse> ExecuteAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var records = await _financialRepository.GetAllAsync();

        var items = categories.Select(c =>
        {
            var catRecords = records.Where(r => r.CategoryId == c.Id);

            var totalIncome = catRecords
                .Where(r => r.Type == FinancialRecordType.Income)
                .Sum(r => r.Amount);

            var totalExpense = catRecords
                .Where(r => r.Type == FinancialRecordType.Expense)
                .Sum(r => r.Amount);

            return new CategorySummaryResponse(
                c.Id,
                c.Description,
                totalIncome,
                totalExpense,
                totalIncome - totalExpense
            );
        }).ToList();

        var totalIncomeAll = items.Sum(i => i.TotalIncome);
        var totalExpenseAll = items.Sum(i => i.TotalExpense);

        return new CategorySummaryListResponse(
            items,
            new CategorySummaryTotals(
                totalIncomeAll,
                totalExpenseAll,
                totalIncomeAll - totalExpenseAll
            )
        );
    }
}
