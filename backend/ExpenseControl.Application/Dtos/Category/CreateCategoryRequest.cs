using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Dtos.Category
{
    public record CreateCategoryRequest(
        string Description,
        int Purpose
    )
    {
        public Guid UserId { get; internal set; }
    }
}
