using Microsoft.EntityFrameworkCore;
using VC_Admin.Application.Interfaces.Repository;
using VC_Admin.Domain.Entities;
using VC_Admin.Infrastructure.Contexts;

namespace VC_Admin.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(User user)
    {
        _db.Add(user); 
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _db.Remove(user);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync(int skip = 0, int take = 10) => 
        await _db.Users
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<User?> GetByEmailAsync(string email)
    {
        var normalized = email?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(normalized)) return null;

        return await _db.Users.SingleOrDefaultAsync(u => u.Email == normalized);
    }

    public async Task<User?> GetByIdAsync(Guid id) => 
        await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

    public async Task UpdateAsync(User user)
    {
        _db.Update(user);
        await _db.SaveChangesAsync();
    }
}
