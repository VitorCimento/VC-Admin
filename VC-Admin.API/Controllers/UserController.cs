using Microsoft.AspNetCore.Mvc;
using VC_Admin.Application.DTO.User;
using VC_Admin.Application.Interfaces.Services;

namespace VC_Admin.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService<UserResponseDTO, UserCreateDTO, UserUpdateDTO> _userService;

    public UserController(IUserService<UserResponseDTO, UserCreateDTO, UserUpdateDTO> userService) => _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var users = await _userService.GetAllAsync(skip, take);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "Usuário não encontrado!" });
        return Ok(user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDTO userUpdateDto)
    {
        var updatedUser = await _userService.UpdateAsync(id, userUpdateDto);
        if (updatedUser == null) return NotFound(new { message = "Usuário não encontrado!" });
        return Ok(updatedUser);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Usuário não encontrado!" });
        return NoContent();
    }
}
