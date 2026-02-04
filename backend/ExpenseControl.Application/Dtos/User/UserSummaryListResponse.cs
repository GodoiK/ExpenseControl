namespace ExpenseControl.Application.Dtos.User
{
    public record UserSummaryTotals(
        decimal TotalIncome,
        decimal TotalExpense,
        decimal Balance
    );

    public record UserSummaryListResponse(
        IEnumerable<UserSummaryResponse> Items,
        UserSummaryTotals Totals
    );
}
