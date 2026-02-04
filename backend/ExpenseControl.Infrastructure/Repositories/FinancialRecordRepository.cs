using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Infrastructure.Repositories
{
    public class FinancialRecordRepository : IFinancialRecordRepository
    {
        private readonly AppDbContext _context;

        public FinancialRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FinancialRecord record)
        {
            await _context.FinancialRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FinancialRecord>> GetAllAsync()
        {
            return await _context.FinancialRecords
                .Include(r => r.User)
                .Include(r => r.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinancialRecord>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.FinancialRecords
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.Category)
                .ToListAsync();
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var records = await _context.FinancialRecords
                .Where(r => r.UserId == userId)
                .ToListAsync();

            _context.FinancialRecords.RemoveRange(records);
            await _context.SaveChangesAsync();
        }
    }
}
