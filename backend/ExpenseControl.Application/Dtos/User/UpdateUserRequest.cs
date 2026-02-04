namespace ExpenseControl.Application.Dtos.User
{
    public record UpdateUserRequest(
        string Name,
        int Age
    );
}
