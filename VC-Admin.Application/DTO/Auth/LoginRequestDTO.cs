namespace VC_Admin.Application.DTO.Auth;

public record LoginRequestDTO
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
