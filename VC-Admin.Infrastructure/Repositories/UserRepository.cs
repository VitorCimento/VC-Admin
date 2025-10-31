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

    public async Task<User?> GetByEmailAsync(string email)
    {
        var normalized = email?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(normalized)) return null;

        return await _db.Users.SingleOrDefaultAsync(u => u.Email == normalized);
    }
}
