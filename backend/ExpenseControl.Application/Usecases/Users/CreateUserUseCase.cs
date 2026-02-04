using ExpenseControl.Application.Dtos.User;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Exceptions;
using ExpenseControl.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ExpenseControl.Application.UseCase.Users
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> ExecuteAsync(CreateUserRequest request)
        {
            var name = (request.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (await _userRepository.ExistsAsync(name, request.Age))
                throw new BusinessException("Já existe um usuário cadastrado com o mesmo Nome e Idade.");

            var user = new User(name, request.Age);

            await _userRepository.AddAsync(user);

            return new UserResponse(
                user.Id,
                user.Name,
                user.Age
            );
        }
    }
}
