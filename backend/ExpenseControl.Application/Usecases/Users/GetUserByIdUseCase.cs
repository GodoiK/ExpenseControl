using ExpenseControl.Application.Dtos.User;
using ExpenseControl.Domain.Exceptions;
using ExpenseControl.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Usecases.Users
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> ExecuteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                throw new BusinessException("User not found.");

            return new UserResponse(user.Id, user.Name, user.Age);
        }
    }
}
