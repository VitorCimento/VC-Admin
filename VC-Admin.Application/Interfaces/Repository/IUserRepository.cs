using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync(int skip = 0, int take = 10);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task AddAsync(User user);
}
