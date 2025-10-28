using VC_Admin.Application.DTO;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    string GenerateToken(User user);
}
