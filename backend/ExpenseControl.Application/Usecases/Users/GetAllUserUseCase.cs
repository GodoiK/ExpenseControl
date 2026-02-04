using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Application.Dtos;
using ExpenseControl.Application.Dtos.User;

namespace ExpenseControl.Application.UseCases.Users;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u =>
            new UserResponse(u.Id, u.Name, u.Age)
        );
    }
}