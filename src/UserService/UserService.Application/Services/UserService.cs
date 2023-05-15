using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> CreateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return _userRepository.CreateUserAsync(user);
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        return await _userRepository.GetUserByIdAsync(userId);
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (user.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(user.Id));
        }

        return await _userRepository.UpdateUserAsync(user);
    }

    public Task DeleteUserAsync(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        return _userRepository.DeleteUserAsync(userId);
    }
}