using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Dtos.FinancialRecord
{
    public class CreateFinancialRecordRequest
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }

        public int Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
