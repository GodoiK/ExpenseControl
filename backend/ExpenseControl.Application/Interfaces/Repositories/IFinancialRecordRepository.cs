using ExpenseControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Interfaces.Repositories
{
    public interface IFinancialRecordRepository
    {
        Task AddAsync(FinancialRecord record);
        Task<IEnumerable<FinancialRecord>> GetAllAsync();
        Task<IEnumerable<FinancialRecord>> GetAllByUserIdAsync(Guid userId);
        Task DeleteByUserIdAsync(Guid userId);
    }
}
