using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Interfaces.Services;

public interface IUserService<ReturnEntity, CreateEntity, UpdateEntity> : IServiceBase<ReturnEntity, CreateEntity, UpdateEntity>
{
    Task<bool> ResetPassword();
}
