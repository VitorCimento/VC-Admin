using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VC_Admin.Application.DTO.Auth;
using VC_Admin.Application.DTO.User;
using VC_Admin.Application.Interfaces.Repository;
using VC_Admin.Application.Interfaces.Services;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly byte[] _secret;

    public AuthService(IUserRepository userRepository, IConfiguration config, IMapper mapper)
    {
        _userRepository = userRepository;
        _config = config;

        var key = _config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret não configurado");

        _secret = Encoding.UTF8.GetBytes(key);
        _mapper = mapper;
    }

    public string GenerateToken(User user)
    {
        var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 15;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username ?? string.Empty)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

        var token = GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return new AuthResponseDTO(token, jwt.ValidTo);
    }

    public async Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO request)
    {
        var exists = await _userRepository.GetByEmailAsync(request.Email);

        if (exists != null) throw new InvalidOperationException("E-Mail já cadastrado!");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);

        return _mapper.Map<UserResponseDTO>(user);
    }
}
