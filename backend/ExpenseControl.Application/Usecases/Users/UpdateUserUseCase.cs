using ExpenseControl.Application.Dtos.User;
using ExpenseControl.Domain.Exceptions;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> ExecuteAsync(Guid id, UpdateUserRequest request)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new BusinessException("User not found.");

        if ((user.Name != name || user.Age != request.Age) && await _userRepository.ExistsAsync(name, request.Age))
            throw new BusinessException("Já existe um usuário cadastrado com o mesmo Nome e Idade.");

        user.Update(name, request.Age);
        await _userRepository.UpdateAsync(user);

        return new UserResponse(user.Id, user.Name, user.Age);
    }
}
