using AutoMapper;
using VC_Admin.Application.DTO.User;
using VC_Admin.Application.Interfaces.Repository;
using VC_Admin.Application.Interfaces.Services;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Services;

public class UserService : IUserService<UserResponseDTO, UserCreateDTO, UserUpdateDTO>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserResponseDTO> CreateAsync(UserCreateDTO entity)
    {
        var existing = await _repository.GetByEmailAsync(entity.Email);

        if (existing != null) throw new InvalidOperationException("Já existe um usuário com o e-mail informado!");

        var user = _mapper.Map<User>(entity);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password);

        await _repository.AddAsync(user);
        
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        await _repository.DeleteAsync(user);
        return true;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync(int skip = 0, int take = 10)
    {
        var users = await _repository.GetAllAsync(skip, take);
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task<UserResponseDTO?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return _mapper.Map<UserResponseDTO?>(user);
    }

    public Task<bool> ResetPassword()
    {
        throw new NotImplementedException();
    }

    public async Task<UserResponseDTO?> UpdateAsync(Guid id, UserUpdateDTO entity)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        _mapper.Map(entity, user);

        await _repository.UpdateAsync(user);

        return _mapper.Map<UserResponseDTO>(user);
    }
}
