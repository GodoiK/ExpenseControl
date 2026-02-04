using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Dtos.User
{
    public record UserSummaryResponse(
        Guid Id,
        string Name,
        Decimal TotalIncome,
        Decimal TotalExpense,
        decimal Balance
    );
}
