using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);

    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
}