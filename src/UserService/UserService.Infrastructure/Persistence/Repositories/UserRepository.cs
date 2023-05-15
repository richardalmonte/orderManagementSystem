using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserServiceDbContext _context;

    public UserRepository(UserServiceDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var userEntry = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return userEntry.Entity;
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (user.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(user.Id));
        }

        var updatedUser = _context.Users.Update(user);
        await _context.SaveChangesAsync();

        if (updatedUser?.Entity is null)
        {
            throw new Exception("User not found");
        }

        return updatedUser.Entity;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        _context.Users.Remove(user);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}