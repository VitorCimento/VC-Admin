using VC_Admin.Application.DTO.Auth;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterRequestDTO request);
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    string GenerateToken(User user);
}
