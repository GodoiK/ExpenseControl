namespace ExpenseControl.Application.Dtos.FinancialRecord
{
    public record FinancialRecordResponse(
        Guid Id,
        string Description,
        decimal Amount,
        int Type,
        Guid CategoryId,
        string CategoryDescription,
        Guid UserId,
        string UserName,
        DateTime CreatedAt
    );
}
