namespace VC_Admin.Application.Interfaces.Services;

public interface IServiceBase<ReturnEntity, CreateEntity, UpdateEntity>
{
    Task<ReturnEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<ReturnEntity>> GetAllAsync(int skip = 0, int take = 10);
    Task<bool> DeleteAsync(Guid id);
    Task<ReturnEntity> CreateAsync(CreateEntity entity);
    Task<ReturnEntity?> UpdateAsync(Guid id, UpdateEntity entity);
}
