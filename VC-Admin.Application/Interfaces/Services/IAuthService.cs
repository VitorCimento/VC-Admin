using VC_Admin.Application.DTO.Auth;
using VC_Admin.Application.DTO.User;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Services;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO request);
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    string GenerateToken(User user);
}
