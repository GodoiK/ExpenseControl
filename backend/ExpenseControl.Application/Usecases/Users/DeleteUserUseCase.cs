using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFinancialRecordRepository _financialRepository;

        public DeleteUserUseCase(
            IUserRepository userRepository,
            IFinancialRecordRepository financialRepository)
        {
            _userRepository = userRepository;
            _financialRepository = financialRepository;
        }

        public async Task ExecuteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("Usuário não encontrado");

            await _financialRepository.DeleteByUserIdAsync(userId);

            await _userRepository.DeleteAsync(user);
        }
    }
}
