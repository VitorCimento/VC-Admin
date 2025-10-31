namespace VC_Admin.Application.DTO.Auth;

public record AuthResponseDTO(string Token, DateTime ExpiresAt);
