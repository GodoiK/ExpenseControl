using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Dtos.User
{
    public record UserResponse
    (
        Guid Id,
        string Name,
        int Age
    );
}
