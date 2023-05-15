using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
}