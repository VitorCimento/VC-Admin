using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}
