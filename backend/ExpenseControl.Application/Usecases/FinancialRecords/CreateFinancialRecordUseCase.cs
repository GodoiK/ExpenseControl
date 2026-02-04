using ExpenseControl.Application.Dtos.FinancialRecord;
using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Exceptions;
using ExpenseControl.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ExpenseControl.Application.UseCases.FinancialRecord;

public class CreateFinancialRecordUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IFinancialRecordRepository _recordRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateFinancialRecordUseCase(
        IUserRepository userRepository,
        IFinancialRecordRepository recordRepository,
        ICategoryRepository categoryRepository)
    {
        _userRepository = userRepository;
        _recordRepository = recordRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ExecuteAsync(CreateFinancialRecordRequest request)
    {
        if (request.Type == 2) request.Type = 0;
        if (!Enum.IsDefined(typeof(FinancialRecordType), request.Type))
            throw new ArgumentException("Invalid financial record type.");

        var recordType = (FinancialRecordType)request.Type;

        if (request.Amount <= 0)
            throw new BusinessException("Valor deve ser um número positivo.");

        if (request.UserId == Guid.Empty)
            throw new BusinessException("UserId is required.");

        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new BusinessException("User not found.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new BusinessException("Category not found.");

        if (recordType == FinancialRecordType.Expense && category.Purpose == CategoryPurpose.Income)
            throw new BusinessException("Categoria informada é apenas para Receita.");

        if (recordType == FinancialRecordType.Income && category.Purpose == CategoryPurpose.Expense)
            throw new BusinessException("Categoria informada é apenas para Despesa.");

        if (user.IsMinor() && recordType == FinancialRecordType.Income)
            throw new BusinessException("Usuários menores de idade não podem registrar receitas.");

        var description = string.IsNullOrWhiteSpace(request.Description)
            ? "Sem descrição"
            : request.Description.Trim();

        var record = new ExpenseControl.Domain.Entities.FinancialRecord(
            user,
            category,
            recordType,
            request.Amount,
            description
        );

        await _recordRepository.AddAsync(record);
    }
}
