using ExpenseControl.Application.Dtos.User;
using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.UseCases.FinancialRecord;

public class GetUserSummaryUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IFinancialRecordRepository _financialRepository;

    public GetUserSummaryUseCase(
        IUserRepository userRepository,
        IFinancialRecordRepository financialRepository)
    {
        _userRepository = userRepository;
        _financialRepository = financialRepository;
    }

    public async Task<UserSummaryListResponse> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var records = await _financialRepository.GetAllAsync();

        var items = users.Select(u =>
        {
            var userRecords = records.Where(r => r.UserId == u.Id);

            var totalIncome = userRecords
                .Where(r => r.Type == FinancialRecordType.Income)
                .Sum(r => r.Amount);

            var totalExpense = userRecords
                .Where(r => r.Type == FinancialRecordType.Expense)
                .Sum(r => r.Amount);

            return new UserSummaryResponse(
                u.Id,
                u.Name,
                totalIncome,
                totalExpense,
                totalIncome - totalExpense
            );
        }).ToList();

        var totalIncomeAll = items.Sum(i => i.TotalIncome);
        var totalExpenseAll = items.Sum(i => i.TotalExpense);

        return new UserSummaryListResponse(
            items,
            new UserSummaryTotals(
                totalIncomeAll,
                totalExpenseAll,
                totalIncomeAll - totalExpenseAll
            )
        );
    }
}