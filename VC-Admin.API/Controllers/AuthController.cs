using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VC_Admin.Application.DTO.Auth;
using VC_Admin.Application.Interfaces.Services;

namespace VC_Admin.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IConfiguration _config;

    public AuthController(IAuthService auth, IConfiguration config)
    {
        _auth = auth;
        _config = config;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        try
        {
            var user = await _auth.RegisterAsync(request);
            return CreatedAtAction(null, new { id = user.Id, email = user.Email, username = user.Username });
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var auth = await _auth.LoginAsync(request);
        if (auth == null) return Unauthorized(new { message = "Credenciais inválidas!" });

        #region Exemplo escrita no Cookie
        //var cookieOpts = new CookieOptions
        //{
        //    HttpOnly = true,
        //    Secure = false,
        //    SameSite = SameSiteMode.Lax,
        //    Expires = auth.ExpiresAt
        //};

        //Response.Cookies.Append("jwt", auth.Token, cookieOpts);
        #endregion

        return Ok(new { token = auth.Token, expiresAt = auth.ExpiresAt });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return NoContent();
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new { message = "autenticado", claims });
    }
}
