namespace ExpenseControl.Application.Dtos.Category
{
    public record CategorySummaryResponse(
        Guid Id,
        string Description,
        decimal TotalIncome,
        decimal TotalExpense,
        decimal Balance
    );

    public record CategorySummaryTotals(
        decimal TotalIncome,
        decimal TotalExpense,
        decimal Balance
    );

    public record CategorySummaryListResponse(
        IEnumerable<CategorySummaryResponse> Items,
        CategorySummaryTotals Totals
    );
}
